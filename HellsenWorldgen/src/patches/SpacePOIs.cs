using HarmonyLib;
using RexLib;
using System.Collections.Generic;

namespace HellsenWorldgen
{
	public static partial class Patches
	{
		public static class HarvestablePOIConfig_GenerateConfigs_Patch
		{
			private static float SubtractAmountFromPOI(HarvestablePOIConfig.HarvestablePOIParams harvestable, SimHashes element, float amountToRemove)
			{
				if (harvestable.poiType.harvestableElements.TryGetValue(element, out float currentAmount)) {
					if (currentAmount > amountToRemove) {
						harvestable.poiType.harvestableElements[element] -= amountToRemove;
						return amountToRemove;
					} else {
						harvestable.poiType.harvestableElements.Remove(element);
						return currentAmount;
					}
				} else {
					Element elementObj = ElementLoader.FindElementByHash(element);
					RexLogger.LogWarning($"Could not find element '{elementObj.name}' in POI '{harvestable.poiType.id}'");
					return 0;
				}
			}

			private static void TransferAmountFromPOI(HarvestablePOIConfig.HarvestablePOIParams harvestable, SimHashes fromElement, SimHashes toElement, float amountToTransfer)
			{
				float amount = SubtractAmountFromPOI(harvestable, fromElement, amountToTransfer);
				if (harvestable.poiType.harvestableElements.ContainsKey(toElement)) {
					harvestable.poiType.harvestableElements[toElement] += amount;
				} else {
					harvestable.poiType.harvestableElements[toElement] = amount;
				}
			}

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
					switch (harvestable.poiType.id) {
					case HarvestablePOIConfig.GlimmeringAsteroidField:
						TransferAmountFromPOI(harvestable, SimHashes.CarbonDioxide, SimHashes.Katairite, 1f);
						break;
					case HarvestablePOIConfig.MetallicAsteroidField:
						TransferAmountFromPOI(harvestable, SimHashes.Obsidian, SimHashes.Katairite, 1f);
						break;
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
