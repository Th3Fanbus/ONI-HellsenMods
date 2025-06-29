using HarmonyLib;

namespace TeleStorage
{
	public class GeneratedBuildingsPatch
	{
		public static void AddBuildingToPlanScreenBehindNext(HashedString category, string building_id, string relativeBuildingId)
		{
			if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(relativeBuildingId)) {
				string subcategoryID = TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[relativeBuildingId];
#if true
				TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[building_id] = subcategoryID;
#else
				if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(building_id)) {
					TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[building_id] = subcategoryID;
				} else {
					TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.Add(building_id, subcategoryID);
				}
#endif
				ModUtil.AddBuildingToPlanScreen(category, building_id, subcategoryID, relativeBuildingId);
			} else {
				ModUtil.AddBuildingToPlanScreen(category, building_id);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Postfix()
			{

				AddBuildingToPlanScreenBehindNext("Base", TeleStorageLiquidConfig.Id, LiquidReservoirConfig.ID);
				AddBuildingToPlanScreenBehindNext("Base", TeleStorageGasConfig.Id, GasReservoirConfig.ID);
			}
		}
	}
}
