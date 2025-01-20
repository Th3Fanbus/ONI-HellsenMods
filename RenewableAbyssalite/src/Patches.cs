using HarmonyLib;
using RexLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RenewableAbyssalite
{
    public static class Patches
    {
        [HarmonyPatch(typeof(BaseHatchConfig), nameof(BaseHatchConfig.HardRockDiet))]
        public static class BaseHatchConfig_HardRockDiet_Patch
        {
            public static void Postfix(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced, ref List<Diet.Info> __result)
            {
                __result.Add(new Diet.Info([
                    SimHashes.Tungsten.CreateTag()
                ], SimHashes.Katairite.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced));
            }
        }

        [HarmonyPatch(typeof(HarvestablePOIConfigurator), nameof(HarvestablePOIConfigurator.FindType))]
        public static class HarvestablePOIConfigurator_FindType_Patch
        {
            public static void Postfix(ref HarvestablePOIConfigurator.HarvestablePOIType? __result)
            {
                if (__result is not null && __result.id.Equals(HarvestablePOIConfig.GlimmeringAsteroidField)) {
                    if (!__result.harvestableElements.ContainsKey(SimHashes.Katairite)) {
                        Debug.Log("[RenewableAbyssalite]: Starmap patch succesful");
                        __result.harvestableElements.Clear();
                        __result.harvestableElements[SimHashes.MoltenTungsten] = 2f;    // 2
                        __result.harvestableElements[SimHashes.Wolframite] = 6f;        // 6
                        __result.harvestableElements[SimHashes.Carbon] = 0.9f;          // 1
                        __result.harvestableElements[SimHashes.CarbonDioxide] = 0.9f;   // 1
                        __result.harvestableElements[SimHashes.Katairite] = 0.2f;       // 0
                    }
                }
            }
        }
    }
}
