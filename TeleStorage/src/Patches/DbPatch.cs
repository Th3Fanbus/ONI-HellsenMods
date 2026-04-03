using HarmonyLib;

namespace TeleStorage.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				Db.Get().Techs.Get("Catalytics").unlockedItemIDs.Add(TeleStorageLiquidConfig.ID);
				Db.Get().Techs.Get("Catalytics").unlockedItemIDs.Add(TeleStorageGasConfig.ID);
			}
		}
	}
}
