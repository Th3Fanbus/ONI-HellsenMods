using HarmonyLib;
using UnityEngine;

namespace TeleStorage.Patches
{
	public class FilterSideScreenPatches
	{
		[HarmonyPatch(typeof(FilterSideScreen), nameof(FilterSideScreen.IsValidForTarget))]
		public static class FilterSideScreen_IsValidForTarget
		{
			public static void Postfix(FilterSideScreen __instance, GameObject target, ref bool __result)
			{
				if (__result || __instance.isLogicFilter) {
					return;
				}
				if (target == null) {
					return;
				}
				if (target.TryGetComponent<TeleStorageFlowControl>(out _) && target.TryGetComponent<Filterable>(out _)) {
					__result = true;
				}
			}
		}
	}
}
