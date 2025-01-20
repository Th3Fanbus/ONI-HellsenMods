using HarmonyLib;

namespace HellsenPowerTweaks
{
    public static partial class Patches
    {
        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                //RexUtils.RegisterSprite("biomeIconFerricCore");
            }
        }
    }
}
