using HarmonyLib;
using RexLib;
using UnityEngine;

namespace TemperatureResistantPips
{
	public static class Patches
	{
		[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
		public static class Localization_Initialize_Patch
		{
			public static void Postfix() => LocalisationUtil.Translate(typeof(MOD_STRINGS), true);
		}

		[HarmonyPatch(typeof(BaseSquirrelConfig), nameof(BaseSquirrelConfig.BaseSquirrel))]
		public class BaseSquirrelConfig_BaseSquirrel_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				if (__result.TryGetDef(out CritterTemperatureMonitor.Def? mon)) {
					mon.temperatureHotDeadly = Config.Instance.PipMaxTemperature;
					mon.temperatureHotUncomfortable = Config.Instance.PipMaxTemperature - 1f;
					mon.temperatureColdDeadly = Config.Instance.PipMinTemperature;
					mon.temperatureColdUncomfortable = Config.Instance.PipMinTemperature + 1f;
				}
			}
		}
	}
}
