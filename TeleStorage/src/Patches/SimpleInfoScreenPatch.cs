using HarmonyLib;
using UnityEngine;

namespace TeleStorage
{
	public class SimpleInfoScreenPatch
	{
		[HarmonyPatch(typeof(SimpleInfoScreen), nameof(SimpleInfoScreen.RefreshStoragePanel))]
		public static class SimpleInfoScreen_RefreshStoragePanel_Patch
		{
			public static bool Prefix(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
			{
				if (targetPanel == null || targetEntity == null) {
					Debug.LogWarning($"HELL: NULL? {targetPanel == null} {targetEntity == null}");
					return false;
				}
				TeleStorage? teleStorage = targetEntity.GetComponent<TeleStorage>();
				if (teleStorage == null) {
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
	}
}
