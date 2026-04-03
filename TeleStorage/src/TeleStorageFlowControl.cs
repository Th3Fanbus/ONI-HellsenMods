namespace TeleStorage
{
	public class TeleStorageFlowControl : KMonoBehaviour, IIntSliderControl
	{
		public const int GramsPerKilogram = 1000;
		public const int Multiplier = 10;

		public const string TITLE_KEY = "STRINGS.UI.UISIDESCREENS.TELESTORAGESIDESCREEN.TITLE";
		public const string TOOLTIP_KEY = "STRINGS.UI.UISIDESCREENS.TELESTORAGESIDESCREEN.TOOLTIP";

		public string SliderTitleKey => TITLE_KEY;

		public string SliderUnits => STRINGS.UI.UNITSUFFIXES.MASS.GRAM + "/" + STRINGS.UI.UNITSUFFIXES.SECOND;

		public int SliderDecimalPlaces(int index) => 0;

		public float GetSliderMin(int index) => 0;
		public float GetSliderMax(int index)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(GetComponent<TeleStorage>().Type);
			return (flowManager?.MaxMass ?? 1) * GramsPerKilogram * Multiplier;
		}

		public float GetSliderValue(int index) => GetComponent<TeleStorage>().Flow;

		public void SetSliderValue(float percent, int index) => GetComponent<TeleStorage>().Flow = percent;

		public string GetSliderTooltipKey(int index) => TOOLTIP_KEY;

		public string GetSliderTooltip(int index) => Strings.Get(TOOLTIP_KEY);
	}
}
