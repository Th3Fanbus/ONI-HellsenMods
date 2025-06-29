using HarmonyLib;

namespace TeleStorage
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				Db.Get().Techs.Get("Catalytics").unlockedItemIDs.Add(TeleStorageLiquidConfig.Id);
				Db.Get().Techs.Get("Catalytics").unlockedItemIDs.Add(TeleStorageGasConfig.Id);
			}
		}
	}
}
