using HarmonyLib;
using System;
using UnityEngine;

namespace TeleStorage
{
	public class TeleStoragePatches
	{
		[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
		public static class Localization_Initialize_Patch
		{
			public static void Postfix() => RexLib.LocalisationUtil.Translate(typeof(MOD_STRINGS), true);
		}

		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		internal static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			internal static void Postfix()
			{
				ModUtil.AddBuildingToPlanScreen("Base", TeleStorageLiquidConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Base", TeleStorageGasConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		internal static class Db_Initialize_Patch
		{
			internal static void Postfix()
			{
				Db.Get().Techs.Get("Catalytics").unlockedItemIDs.Add(TeleStorageLiquidConfig.Id);
				Db.Get().Techs.Get("Catalytics").unlockedItemIDs.Add(TeleStorageGasConfig.Id);
			}
		}

		[HarmonyPatch(typeof(SimpleInfoScreen), nameof(SimpleInfoScreen.RefreshStoragePanel))]
		internal static class SimpleInfoScreen_RefreshStoragePanel_Patch
		{
			internal static bool Prefix(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
			{
				if (targetPanel is null || targetEntity is null) {
					Debug.LogWarning($"HELL: NULL? {targetPanel is null} {targetEntity is null}");
					return false;
				}
				TeleStorage? teleStorage = targetEntity.GetComponent<TeleStorage>();
				if (teleStorage is null) {
					return true;
				}
				targetPanel.gameObject.SetActive(true);
				targetPanel.SetTitle(STRINGS.UI.DETAILTABS.DETAILS.GROUPNAME_CONTENTS);
				int num = teleStorage.AddStorageItems(targetPanel, 0);
				if (num == 0) {
					targetPanel.SetLabel("storage_empty", STRINGS.UI.DETAILTABS.DETAILS.STORAGE_EMPTY, "");
				}
				targetPanel.Commit();
				return false;
			}
		}

		[HarmonyPatch(typeof(FilterSideScreen), nameof(FilterSideScreen.IsValidForTarget))]
		internal static class FilterSideScreen_IsValidForTarget
		{
			internal static bool Prefix(GameObject target, FilterSideScreen __instance, ref bool __result)
			{
				if (target.TryGetComponent<TeleStorageFlowControl>(out _)) {
					__result = !__instance.isLogicFilter;
					return false;
				}
				return true;
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager), nameof(KSerialization.Manager.GetType), [typeof(string)])]
		internal static class TeleStorageSerializationPatch
		{
			internal static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "TeleStorage.TeleStorage") {
					__result = typeof(TeleStorage);
				}
			}
		}

		[HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.FinaliseElementsTable))]
		internal static class ElementLoader_FinaliseElementsTable_Patch
		{
			internal static void Postfix()
			{
				foreach (Element el in ElementLoader.elements) {
					if (el == null) {
						continue;
					}

					Debug.Log($"HELL: el {el.name} is {el.GetStateString()} ({el.state})");
					if (el.IsLiquid || el.IsGas) {
						//el.state &= ~Element.State.Unbreakable;
					}
				}
			}
		}

		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), [typeof(string)])]
		internal static class SaveLoader_Load_Patch
		{
			internal static void Postfix(string filename) => TeleStorageData.Load(filename);
		}

		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save), [typeof(string), typeof(bool), typeof(bool)])]
		internal static class SaveLoader_Save_Patch
		{
			internal static void Postfix(string filename) => TeleStorageData.Save(filename);
		}
	}
}
