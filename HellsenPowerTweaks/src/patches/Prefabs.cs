using HarmonyLib;
using PeterHan.PLib.Core;
using RexLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HellsenPowerTweaks
{
	public static partial class Patches
	{
		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Postfix()
			{
				ModUtil.AddBuildingToPlanScreen("Power", SmolBatterySmartConfig.ID);
				ModUtil.AddBuildingToPlanScreen("Power", HugeBatterySmartConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				Db.Get().Techs.Get("AdvancedPowerRegulation").unlockedItemIDs.Add(SmolBatterySmartConfig.ID);
				Db.Get().Techs.Get("SpacePower").unlockedItemIDs.Add(HugeBatterySmartConfig.ID);
			}
		}

		public static void RelaxBuildingConstraints(ref BuildingDef __result)
		{
			__result.ContinuouslyCheckFoundation = false;
			__result.BuildLocationRule = BuildLocationRule.Anywhere;
			__result.Floodable = false;
			__result.Entombable = false;
		}

		public static void RemoveIndustrialMachinery(GameObject go)
			=> go.GetComponent<KPrefabID>().RemoveTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

		/* Patch the overloaded non-virtual helper method */
		[HarmonyPatch(typeof(BaseBatteryConfig), nameof(BaseBatteryConfig.CreateBuildingDef))]
		public static class BaseBatteryConfig_CreateBuildingDef_Patch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				RelaxBuildingConstraints(ref __result);
				__result.PermittedRotations = PermittedRotations.R360;
			}
		}

		/* Patch the overloaded non-virtual helper method */
		[HarmonyPatch(typeof(ElectrobankChargerConfig), nameof(ElectrobankChargerConfig.CreateBuildingDef))]
		public static class ElectrobankChargerConfig_CreateBuildingDef_Patch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				RelaxBuildingConstraints(ref __result);
				__result.PermittedRotations = PermittedRotations.R360;
			}
		}

		[HarmonyPatch]
		public static class VariousPowerPrefabs_CreateBuildingDef_Patch
		{
			public static IEnumerable<MethodBase> TargetMethods()
				=> RexUtils.MethodFromTypes("CreateBuildingDef",
					typeof(PowerTransformerConfig),
					typeof(PowerTransformerSmallConfig),
					typeof(HydrogenGeneratorConfig)
				);

			public static void Postfix(ref BuildingDef __result) => RelaxBuildingConstraints(ref __result);
		}

#if false
        /* Patch out all "KPrefabID.AddTag" calls */
        public static IEnumerable<CodeInstruction> RemoveIndustrialMachinery(IEnumerable<CodeInstruction> method)
            => PatchTools.RemoveMethodCall(method, typeof(KPrefabID).GetMethodSafeAnyArgs(nameof(KPrefabID.AddTag), false));
#endif

		[HarmonyPatch]
		public static class VariousPrefabs_RemoveIndustrialMachinery_Patch
		{
			public static IEnumerable<MethodBase> TargetMethods()
				=> RexUtils.MethodFromTypes("DoPostConfigureComplete",
					typeof(GasFilterConfig),
					typeof(GasMiniPumpConfig),
					typeof(GasPumpConfig),
					typeof(HydrogenGeneratorConfig),
					typeof(LiquidFilterConfig),
					typeof(LiquidMiniPumpConfig),
					typeof(LiquidPumpConfig),
					typeof(ManualGeneratorConfig),
					typeof(PowerTransformerConfig),
					typeof(PowerTransformerSmallConfig),
					typeof(ElectrobankChargerConfig),
					typeof(LargeElectrobankDischargerConfig));

			public static void Postfix(GameObject go) => RemoveIndustrialMachinery(go);
		}

		public static class MicroTransformerPatches
		{
			private static void PatchPostfix(Harmony harmony, Type? tgt, string targetName, string postfixName)
			{
				MethodInfo? m_TargetMethod = AccessTools.Method(tgt, targetName);
				MethodInfo? m_Postfix = AccessTools.Method(typeof(MicroTransformerPatches), postfixName);
				if (m_TargetMethod is not null) {
					harmony.Patch(m_TargetMethod, postfix: new HarmonyMethod(m_Postfix));
				}
			}

			private static void Postfix_CreateBuildingDef(ref BuildingDef __result) => RelaxBuildingConstraints(ref __result);

			private static void Postfix_DoPostConfigureComplete(GameObject go) => RemoveIndustrialMachinery(go);

			public static void ExecutePatches(Harmony harmony)
			{
				Type? tgt = PPatchTools.GetTypeSafe("MicroTransformer.SmallTransformerConfig", "MicroTransformer");

				PatchPostfix(harmony, tgt, "CreateBuildingDef", nameof(Postfix_CreateBuildingDef));
				PatchPostfix(harmony, tgt, "DoPostConfigureComplete", nameof(Postfix_DoPostConfigureComplete));
			}
		}
	}
}
