namespace TeleStorage
{
    public static class MOD_STRINGS
    {
        public static class BUILDINGS
        {
            public static class PREFABS
            {
                public static class TELESTORAGELIQUID
                {
                    public static LocString NAME = STRINGS.UI.FormatAsLink("Advanced Liquid Storage", nameof(TELESTORAGELIQUID));
                    public static LocString DESC = "Stores liquid inside an alternate dimension.";
                    public static LocString EFFECT = "Compresses liquids into an alternate dimension for more effective storage.";
                }
                public static class TELESTORAGEGAS
                {
                    public static LocString NAME = STRINGS.UI.FormatAsLink("Advanced Gas Storage", nameof(TELESTORAGEGAS));
                    public static LocString DESC = "Stores gases inside an alternate dimension.";
                    public static LocString EFFECT = "Compresses gases into an alternate dimension for more effective storage.";
                }
            }
        }
        public static class UI
        {
            public static class UISIDESCREENS
            {
                public static class TELESTORAGE
                {
                    public static class FLOW
                    {
                        public static LocString TITLE = "Flow rate";
                        public static LocString TOOLTIP = "Flow rate";
                    }
                }
            }
        }
    }
}