using HarmonyLib;

namespace TeleStorage
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
		public static class Localization_Initialize_Patch
		{
			public static void Postfix() => RexLib.LocalisationUtil.Translate(typeof(MOD_STRINGS), true);
		}
	}
}
