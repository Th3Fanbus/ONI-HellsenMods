using HarmonyLib;
using RexLib;
using System.Collections.Generic;

using static GeyserConfigurator;
using static GeyserGenericConfig;

namespace HellsenWorldgen
{
    public static partial class Extensions
    {
        public static GeyserPrefabParams MakeGeyserParams(this GeyserType geyserType, GeoTunerConfig.Category category, bool isGenericGeyser)
        {
            int width, height;
            switch (geyserType.shape) {
            case GeyserShape.Gas:
                width = 2;
                height = 4;
                break;
            case GeyserShape.Liquid:
                width = 4;
                height = 2;
                break;
            case GeyserShape.Molten:
                width = 3;
                height = 3;
                break;
            default:
                RexLogger.LogError($"Unexpected geyser shape for {geyserType.id}!");
                width = 4;
                height = 2;
                break;
            }
            GeoTunerConfig.geotunerGeyserSettings.Add(geyserType.id, GeoTunerConfig.CategorySettings[category]);
            return new GeyserPrefabParams(
                anim: $"geyser_{geyserType.id}_kanim",
                width: width,
                height: height,
                geyserType: geyserType,
                isGenericGeyser: isGenericGeyser
            );
        }
    }

    public static partial class Patches
    {
        [HarmonyPatch(typeof(GeyserGenericConfig), nameof(GeyserGenericConfig.GenerateConfigs))]
        public static class GeyserGenericConfig_GenerateConfigs_Patch
        {
            public static void Postfix(List<GeyserPrefabParams> __result)
            {
                __result.Add(new GeyserType(
                    id: "molten_lead",
                    element: SimHashes.MoltenLead,
                    shape: GeyserShape.Molten,
                    temperature: 2000f,
                    minRatePerCycle: RATES.MOLTEN_NORMAL_MIN,
                    maxRatePerCycle: RATES.MOLTEN_NORMAL_MAX,
                    maxPressure: MAX_PRESSURES.MOLTEN,
                    requiredDlcIds: null,
                    forbiddenDlcIds: null,
                    minIterationLength: ITERATIONS.FREQUENT_MOLTEN.LEN_MIN,
                    maxIterationLength: ITERATIONS.FREQUENT_MOLTEN.LEN_MAX,
                    minIterationPercent: ITERATIONS.FREQUENT_MOLTEN.PCT_MIN,
                    maxIterationPercent: ITERATIONS.FREQUENT_MOLTEN.PCT_MAX
                ).MakeGeyserParams(category: GeoTunerConfig.Category.METALS_CATEGORY, isGenericGeyser: true));

                __result.Add(new GeyserType(
                    id: "liquid_chlorine",
                    element: SimHashes.Chlorine,
                    shape: GeyserShape.Liquid,
                    temperature: 203.15f,
                    minRatePerCycle: RATES.LIQUID_SMALL_MIN,
                    maxRatePerCycle: RATES.LIQUID_SMALL_MAX,
                    maxPressure: MAX_PRESSURES.LIQUID,
                    requiredDlcIds: null
                ).MakeGeyserParams(category: GeoTunerConfig.Category.ORGANIC_CATEGORY, isGenericGeyser: true));

                __result.Add(new GeyserType(
                    id: "liquid_ethanol",
                    element: SimHashes.Ethanol,
                    shape: GeyserShape.Liquid,
                    temperature: 263.15f,
                    minRatePerCycle: RATES.LIQUID_SMALL_MIN,
                    maxRatePerCycle: RATES.LIQUID_SMALL_MAX,
                    maxPressure: MAX_PRESSURES.LIQUID,
                    requiredDlcIds: null
                ).MakeGeyserParams(category: GeoTunerConfig.Category.HYDROCARBON_CATEGORY, isGenericGeyser: false));

                __result.Add(new GeyserType(
                    id: "gas_nuclear_fallout",
                    element: SimHashes.Fallout,
                    shape: GeyserShape.Gas,
                    temperature: 1773.15f,
                    minRatePerCycle: RATES.GAS_SMALL_MIN,
                    maxRatePerCycle: RATES.GAS_SMALL_MAX,
                    maxPressure: MAX_PRESSURES.GAS_HIGH,
                    requiredDlcIds: DlcManager.EXPANSION1
                ).MakeGeyserParams(category: GeoTunerConfig.Category.ORGANIC_CATEGORY, isGenericGeyser: false));

                __result.Add(new GeyserType(
                    id: "molten_nickel",
                    element: SimHashes.MoltenNickel,
                    shape: GeyserShape.Molten,
                    temperature: 2500f,
                    minRatePerCycle: RATES.MOLTEN_NORMAL_MIN,
                    maxRatePerCycle: RATES.MOLTEN_NORMAL_MAX,
                    maxPressure: MAX_PRESSURES.MOLTEN,
                    requiredDlcIds: DlcManager.DLC4,
                    forbiddenDlcIds: null,
                    minIterationLength: ITERATIONS.FREQUENT_MOLTEN.LEN_MIN,
                    maxIterationLength: ITERATIONS.FREQUENT_MOLTEN.LEN_MAX,
                    minIterationPercent: ITERATIONS.FREQUENT_MOLTEN.PCT_MIN,
                    maxIterationPercent: ITERATIONS.FREQUENT_MOLTEN.PCT_MAX
                ).MakeGeyserParams(category: GeoTunerConfig.Category.METALS_CATEGORY, isGenericGeyser: true));

                __result.RemoveAll(geyser => !DlcManager.IsCorrectDlcSubscribed(geyser.geyserType));
            }
        }
    }
}
