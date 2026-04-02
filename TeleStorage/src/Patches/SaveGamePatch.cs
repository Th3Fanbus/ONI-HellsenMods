using HarmonyLib;

namespace TeleStorage.Patches
{
	public class SaveGamePatch
	{
		[HarmonyPatch(typeof(SaveGame), nameof(SaveGame.OnPrefabInit))]
		public class SaveGame_OnPrefabInit_Patch
		{
			public static void Postfix(SaveGame __instance)
			{
				__instance.gameObject.AddOrGet<TeleStorageData>();
			}
		}
	}
}
