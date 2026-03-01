using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace HellsenWorldgen
{
	public sealed class HellsenWorldgenMod : UserMod2
	{
		public static Harmony? harmonyInstance = null;

		public override void OnLoad(Harmony harmony)
		{
			PUtil.InitLibrary(false);
			new POptions().RegisterOptions(this, typeof(ModOptions));
			base.OnLoad(harmony);
			harmonyInstance = harmony;
			Debug.Log($"{mod.staticID} - Mod Version: {mod.packagedModInfo.version}");
		}
	}
}
