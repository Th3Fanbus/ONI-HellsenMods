using HarmonyLib;
using UnityEngine;

namespace TeleStorage
{
	public class FilterSideScreenPatches
	{
		[HarmonyPatch(typeof(FilterSideScreen), nameof(FilterSideScreen.IsValidForTarget))]
		public static class FilterSideScreen_IsValidForTarget
		{
			public static bool Prefix(GameObject target, FilterSideScreen __instance, ref bool __result)
			{
				if (target.TryGetComponent<TeleStorageFlowControl>(out _)) {
					__result = !__instance.isLogicFilter;
					return false;
				}
				return true;
			}
		}
	}
}
