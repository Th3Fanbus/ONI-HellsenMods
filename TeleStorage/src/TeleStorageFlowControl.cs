namespace TeleStorage
{
	public class TeleStorageFlowControl : KMonoBehaviour, IIntSliderControl
	{
		public const int GramsPerKilogram = 1000;
		public const int Multiplier = 10;

		public string SliderTitleKey => nameof(MOD_STRINGS.UI.UISIDESCREENS.TELESTORAGE.FLOW.TITLE);

		public string SliderUnits => STRINGS.UI.UNITSUFFIXES.MASS.GRAM + "/" + STRINGS.UI.UNITSUFFIXES.SECOND;

		public int SliderDecimalPlaces(int index) => 0;

		public float GetSliderMin(int index) => 0;
		public float GetSliderMax(int index)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(GetComponent<TeleStorage>().Type);
			return (flowManager?.MaxMass ?? 1) * GramsPerKilogram * Multiplier;
			//return Traverse.Create(flowManager).Field("MaxMass").GetValue<float>() * GramsPerKilogram * Multiplier;
		}

		public float GetSliderValue(int index) => GetComponent<TeleStorage>().Flow;

		public void SetSliderValue(float percent, int index) => GetComponent<TeleStorage>().Flow = percent;

		public string GetSliderTooltipKey(int index) => nameof(MOD_STRINGS.UI.UISIDESCREENS.TELESTORAGE.FLOW.TOOLTIP);

		public string GetSliderTooltip(int index) => Strings.Get(GetSliderTooltipKey(index));
	}
}
