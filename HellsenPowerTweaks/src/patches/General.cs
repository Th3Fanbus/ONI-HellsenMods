using HarmonyLib;
using RexLib;

namespace HellsenPowerTweaks
{
    public static partial class Patches
    {
        [HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
        public static class Localization_Initialize_Patch
        {
            public static void Postfix() => LocalisationUtil.Translate(typeof(MOD_STRINGS), true);
        }
    }
}
