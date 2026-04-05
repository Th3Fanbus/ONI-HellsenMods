using HarmonyLib;
using RexLib;

namespace TeleStorage.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			/* Run after other mods which may move the reservoirs to another category */
			[HarmonyPriority(Priority.Low)]
			public static void Postfix()
			{
				RexUtils.AddBuildingToPlanScreenBehindNext("Base", TeleStorageLiquidConfig.ID, LiquidReservoirConfig.ID);
				RexUtils.AddBuildingToPlanScreenBehindNext("Base", TeleStorageGasConfig.ID, GasReservoirConfig.ID);
			}
		}
	}
}
