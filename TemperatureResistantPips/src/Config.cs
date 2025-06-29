using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;

namespace TemperatureResistantPips
{
	[Serializable]
	[ConfigFile(SharedConfigLocation: true)]
	[ModInfo("Temperature Resistant Pips")]
	public class Config : SingletonOptions<Config>
	{
		[Option("STRINGS.MODCONFIG.PIP.MINTEMP.NAME", "STRINGS.MODCONFIG.PIP.MINTEMP.TOOLTIP")]
		[JsonProperty]
		public float PipMinTemperature { get; set; }

		[Option("STRINGS.MODCONFIG.PIP.MAXTEMP.NAME", "STRINGS.MODCONFIG.PIP.MAXTEMP.TOOLTIP")]
		[JsonProperty]
		public float PipMaxTemperature { get; set; }

		public Config()
		{
			PipMinTemperature = 0.0f;
			PipMaxTemperature = 9999.0f;
		}
	}
}
