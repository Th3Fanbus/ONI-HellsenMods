using HarmonyLib;
using System.Collections.Generic;

namespace HellsenWorldgen
{
    public static partial class Patches
    {
        //[HarmonyPatch(typeof(HarvestablePOIConfig), nameof(HarvestablePOIConfig.GenerateConfigs))]
        //[HarmonyPatch(typeof(AutoMinerConfig), nameof(AutoMinerConfig.CreateBuildingDef))]
        public static class HarvestablePOIConfig_GenerateConfigs_Patch
        {
            public static void StarMapPostfix(List<HarvestablePOIConfig.HarvestablePOIParams> __result)
            {
#if false
                __result.Add(new HarvestablePOIConfig.HarvestablePOIParams("glimmering_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType(
                    id: "GlimmeringAsteroidField",
                    harvestableElements: new() {
                        [SimHashes.MoltenTungsten] = 2f,
                        [SimHashes.Wolframite] = 6f,
                        [SimHashes.Carbon] = 1f,
                        [SimHashes.CarbonDioxide] = 1f
                    },
                    poiCapacityMin: 30000f,
                    poiCapacityMax: 45000f,
                    poiRechargeMin: 30000f,
                    poiRechargeMax: 60000f,
                    canProvideArtifacts: true,
                    orbitalObject: HarvestablePOIConfig.AsteroidFieldOrbit,
                    maxNumOrbitingObjects: 20,
                    requiredDlcIds: DlcManager.EXPANSION1)));
#endif
                foreach (HarvestablePOIConfig.HarvestablePOIParams harvestable in __result) {
                    if (harvestable.poiType.id.Equals(HarvestablePOIConfig.GlimmeringAsteroidField)) {
#if !false
                        harvestable.poiType.harvestableElements.Clear();
                        harvestable.poiType.harvestableElements[SimHashes.MoltenTungsten] = 2f;
                        harvestable.poiType.harvestableElements[SimHashes.Wolframite] = 6f;
                        harvestable.poiType.harvestableElements[SimHashes.Carbon] = 1f;
                        harvestable.poiType.harvestableElements[SimHashes.CarbonDioxide] = 0f;
                        harvestable.poiType.harvestableElements[SimHashes.Katairite] = 1f;
#else
                        harvestable.poiType.harvestableElements[SimHashes.CarbonDioxide] = 0f;
                        harvestable.poiType.harvestableElements[SimHashes.Katairite] = 1f;
#endif
                    } else if (harvestable.poiType.id.Equals(HarvestablePOIConfig.MetallicAsteroidField)) {
                        harvestable.poiType.harvestableElements.Clear();
                        harvestable.poiType.harvestableElements[SimHashes.Obsidian] = 6f;
                        harvestable.poiType.harvestableElements[SimHashes.Katairite] = 1f;
                    }
                }

                __result.RemoveAll(poi => !DlcManager.IsCorrectDlcSubscribed(poi.poiType));
            }

            public static bool patched = false;
            public static bool Prefix()
            {
                if (patched) {
                    return true;
                }
                patched = true;
                var m_TargetMethod = AccessTools.Method(typeof(HarvestablePOIConfig), nameof(HarvestablePOIConfig.GenerateConfigs));
                var m_Postfix = AccessTools.Method(typeof(HarvestablePOIConfig_GenerateConfigs_Patch), nameof(StarMapPostfix));

                Debug.Assert(HellsenWorldgenMod.harmonyInstance != null, "HellsenWorldgenMod.harmonyInstance is null");
                HellsenWorldgenMod.harmonyInstance?.Patch(m_TargetMethod, null, new HarmonyMethod(m_Postfix));

                return true;
            }
        }
    }
}
