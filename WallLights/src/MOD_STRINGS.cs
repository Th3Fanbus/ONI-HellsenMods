using STRINGS;

namespace WallPowerLights
{
    public static class MOD_STRINGS
    {
        public static class BUILDINGS
        {
            public static class PREFABS
            {
                public static class SMOLBATTERYSMART
                {
                    public static LocString NAME = UI.FormatAsLink("Smol Smart Battery", nameof(SMOLBATTERYSMART));
                    public static LocString DESC = "Smart batteries send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when they require charging.";
                    public static LocString EFFECT =
                        "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nSends a " +
                        UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) +
                        " based on the configuration of the Logic Activation Parameters.\n\nVery slightly loses charge over time.";
                }
            }
        }
    }
}