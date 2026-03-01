using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;

namespace HellsenWorldgen
{
	[Serializable]
	[RestartRequired]
	[ConfigFile(SharedConfigLocation: true)]
	[ModInfo("Hellsen Worldgen")]
	public class ModOptions : SingletonOptions<ModOptions>
	{
		[JsonProperty]
		[Option("STRINGS.MOD_OPTIONS.WORLDGEN.CLEAN_NEUTRONIUM_EDGES.NAME", "STRINGS.MOD_OPTIONS.WORLDGEN.CLEAN_NEUTRONIUM_EDGES.TOOLTIP")]
		public bool CleanNeutroniumEdges { get; set; }

		[JsonProperty]
		[Option("STRINGS.MOD_OPTIONS.WORLDGEN.INJECT_ETHANOL_GEYSERS.NAME", "STRINGS.MOD_OPTIONS.WORLDGEN.INJECT_ETHANOL_GEYSERS.TOOLTIP")]
		public bool InjectEthanolGeysers { get; set; }

		public ModOptions()
		{
			CleanNeutroniumEdges = true;
			InjectEthanolGeysers = true;
		}
	}
}
