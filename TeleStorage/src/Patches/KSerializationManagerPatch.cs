using HarmonyLib;
using System;

namespace TeleStorage
{
	public class KSerializationManagerPatch
	{
		[HarmonyPatch(typeof(KSerialization.Manager), nameof(KSerialization.Manager.GetType), [typeof(string)])]
		public static class TeleStorageSerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "TeleStorage.TeleStorage") {
					__result = typeof(TeleStorage);
				}
			}
		}
	}
}
