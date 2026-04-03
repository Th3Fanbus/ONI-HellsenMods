using HarmonyLib;

namespace TeleStorage.Patches
{
	public class GeneratedBuildingsPatch
	{
		public static void AddBuildingToPlanScreenBehindNext(HashedString category, string building_id, string relative_building_id)
		{
			if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(relative_building_id)) {
				string subcategory_id = TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[relative_building_id];
				TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[building_id] = subcategory_id;
				ModUtil.AddBuildingToPlanScreen(category, building_id, subcategory_id, relative_building_id);
			} else {
				ModUtil.AddBuildingToPlanScreen(category, building_id);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			[HarmonyPriority(Priority.Low)]
			public static void Postfix()
			{
				AddBuildingToPlanScreenBehindNext("Base", TeleStorageLiquidConfig.ID, LiquidReservoirConfig.ID);
				AddBuildingToPlanScreenBehindNext("Base", TeleStorageGasConfig.ID, GasReservoirConfig.ID);
			}
		}
	}
}
