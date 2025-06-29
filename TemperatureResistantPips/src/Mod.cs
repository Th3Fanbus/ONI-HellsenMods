using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace TemperatureResistantPips
{
	public sealed class TemperatureResistantPipsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			PUtil.InitLibrary(false);
			new POptions().RegisterOptions(this, typeof(Config));
			base.OnLoad(harmony);
			Debug.Log($"{mod.staticID} - Mod Version: {mod.packagedModInfo.version}");
		}
	}
}
