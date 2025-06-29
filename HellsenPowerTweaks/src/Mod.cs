using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace HellsenPowerTweaks
{
	public sealed class HellsenPowerTweaksMod : UserMod2
	{
		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);
			Patches.MicroTransformerPatches.ExecutePatches(harmony);
		}
	}
}
