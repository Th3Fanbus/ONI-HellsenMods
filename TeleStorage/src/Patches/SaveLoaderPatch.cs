using HarmonyLib;

namespace TeleStorage
{
	public class SaveLoaderPatch
	{
		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), [typeof(string)])]
		public static class SaveLoader_Load_Patch
		{
			public static void Postfix(string filename) => TeleStorageData.Load(filename);
		}

		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save), [typeof(string), typeof(bool), typeof(bool)])]
		public static class SaveLoader_Save_Patch
		{
			public static void Postfix(string filename) => TeleStorageData.Save(filename);
		}
	}
}
