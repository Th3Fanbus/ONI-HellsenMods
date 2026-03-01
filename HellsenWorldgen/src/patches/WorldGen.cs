using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using static ProcGen.World;
using static ProcGen.World.AllowedCellsFilter;
using static ProcGen.World.TemplateSpawnRules;

namespace HellsenWorldgen
{
	public static partial class Extensions
	{
		public static bool ShouldSkip(this ProcGen.World world) => world.moduleInterior;

		public static void InjectTemplateRule(this ProcGen.World world, TemplateSpawnRules template)
		{
			if (template.names.Count == 0) {
				return;
			}
			string name = template.names.FirstOrDefault();
			if (!world.worldTemplateRules.Any(x => x.names.Contains(name))) {
				world.worldTemplateRules.Add(template);
			}
		}

		public static void InjectCleanNeutroniumEdges(this ProcGen.World world)
		{
			world.defaultsOverrides.data["WorldBorderThickness"] = 2;
			world.defaultsOverrides.data["WorldBorderRange"] = 0;
		}
	}

	public static partial class Patches
	{
		private static readonly TemplateSpawnRules EthanolGeyserTemplate = new() {
			names = ["expansion1::geysers/ethanol_geyser_full"],
			listRule = ListRule.TryOne,
			times = 2,
			priority = 75,
			allowDuplicates = true,
			allowExtremeTemperatureOverlap = false,
			useRelaxedFiltering = true,
			allowedCellsFilter = [
				new() {
					command = Command.Replace,
					tagcommand = TagCommand.DistanceFromTag,
					tag = WorldGenTags.AtSurface.name,
					minDistance = 2,
					maxDistance = 99,
				},
				new() {
					command = Command.IntersectWith,
					tagcommand = TagCommand.Default,
					zoneTypes = [
						SubWorld.ZoneType.FrozenWastes,
					],
				},
				new() {
					command = Command.ExceptWith,
					tagcommand = TagCommand.AtTag,
					tag = WorldGenTags.NoGlobalFeatureSpawning.name,
				},
				new() {
					command = Command.ExceptWith,
					zoneTypes = [
						SubWorld.ZoneType.Space,
					],
				},
			],
		};

		private static readonly TemplateSpawnRules HellsenTeleporterTemplate = new() {
			names = ["expansion1::poi/warp/hellsen_teleporter_mini"],
			listRule = ListRule.GuaranteeAll,
			times = 1,
			priority = 240,
			allowDuplicates = true,
			allowExtremeTemperatureOverlap = false,
			useRelaxedFiltering = true,
			allowedCellsFilter = [
				new() {
					command = Command.Replace,
					tagcommand = TagCommand.DistanceFromTag,
					tag = nameof(WorldGenTags.AtSurface),
					minDistance = 2,
					maxDistance = 99,
				},
				new() {
					command = Command.ExceptWith,
					zoneTypes = [
						SubWorld.ZoneType.Space,
					],
				},
				new() {
					command = Command.ExceptWith,
					tagcommand = TagCommand.AtTag,
					tag = nameof(WorldGenTags.NoGlobalFeatureSpawning),
				},
			],
		};

		private static void InjectWorldTemplateRules(Worlds? __instance)
		{
			if (__instance?.worldCache is null) {
				return;
			}
			foreach (ProcGen.World world in __instance.worldCache.Values) {
				if (world.ShouldSkip()) {
					continue;
				}
				if (ModOptions.Instance.CleanNeutroniumEdges) {
					world.InjectCleanNeutroniumEdges();
				}
				if (ModOptions.Instance.InjectEthanolGeysers) {
					world.InjectTemplateRule(EthanolGeyserTemplate);
				}
				//world.InjectTemplateRule(HellsenTeleporterTemplate);
			}
		}

		[HarmonyPatch(typeof(Worlds), nameof(Worlds.UpdateWorldCache))]
		public static class Worlds_UpdateWorldCache_Patch
		{
			public static void Postfix(Worlds __instance) => InjectWorldTemplateRules(__instance);
		}

		[HarmonyPatch(typeof(Worlds), nameof(Worlds.LoadFiles))]
		public static class Worlds_LoadFiles_Patch
		{
			public static void Postfix(Worlds __instance) => InjectWorldTemplateRules(__instance);
		}
	}
}
