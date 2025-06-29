using HarmonyLib;
using RexLib;

namespace HellsenWorldgen
{
    public static partial class Patches
    {
        [HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
        public class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                RexUtils.RegisterSprite("biomeIconFerricCore");
                RexUtils.RegisterSprite("HellsenGeoHyperActive");
                RexUtils.RegisterSprite("HellsenDeepCrashedSatellites");
            }
        }
    }
}
