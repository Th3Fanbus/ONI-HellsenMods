using HarmonyLib;
using KMod;

namespace HellsenWorldgen
{
    public sealed class HellsenWorldgenMod : UserMod2
    {
        public static Harmony? harmonyInstance = null;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            harmonyInstance = harmony;
        }
    }
}
