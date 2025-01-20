using STRINGS;

namespace HellsenWorldgen
{
    public static class MOD_STRINGS
    {
        public static class SUBWORLDS
        {
            public static class FERRICCORE
            {
                public static LocString NAME = "Ferric Core";
                public static LocString DESC = ("The Ferric Core Biome consists of extremely hot " + UI.FormatAsLink("Iron", "MOLTENIRON") + ", making it a reliable source of extreme heat that can be exploited for the purposes of producing " + UI.FormatAsLink("Power", "POWER") + " and fuel.");
                public static LocString UTILITY = (UI.FormatAsLink("Iron", "MOLTENIRON") + " is source of extreme " + UI.FormatAsLink("Heat", "HEAT") + " which can be used to transform " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Steam", "STEAM") + " or " + UI.FormatAsLink("Crude Oil", "CRUDEOIL") + " into " + UI.FormatAsLink("Petroleum", "PETROLEUM") + ". In order to prevent the extreme temperatures of this biome invading other parts of my base, suitable insulation must be constructed using materials with high melting points like " + UI.FormatAsLink("Ceramic", "CERAMIC") + " or " + UI.FormatAsLink("Obsidian", "OBSIDIAN") + ".\n\nThough " + UI.FormatAsLink("Atmo Suits", "ATMO_SUIT") + " will provide some protection for my Duplicants, there is still a danger they will overheat if spending an extended amount of time in this Biome. I should ensure that suitable medical facilities have been constructed nearby to take care of any medical emergencies.");
            }
        }
        public static class CREATURES
        {
            public static class SPECIES
            {
                public static class GEYSER
                {
                    public static class MOLTEN_LEAD
                    {
                        public static LocString NAME = UI.FormatAsLink("Lead Volcano", "GeyserGeneric_MOLTEN_LEAD");
                        public static LocString DESC = ("A large volcano that periodically erupts with molten " + UI.FormatAsLink("Lead", "MOLTENLEAD") + ".");
                    }
                    public static class LIQUID_CHLORINE
                    {
                        public static LocString NAME = UI.FormatAsLink("Liquid Chlorine Geyser", "GeyserGeneric_LIQUID_CHLORINE");
                        public static LocString DESC = ("A highly pressurized geyser that periodically erupts with " + UI.FormatAsLink("Liquid Chlorine", "CHLORINE") + ".");
                    }
                    public static class LIQUID_ETHANOL
                    {
                        public static LocString NAME = UI.FormatAsLink("Liquid Ethanol Geyser", "GeyserGeneric_LIQUID_ETHANOL");
                        public static LocString DESC = ("A highly pressurized geyser that periodically erupts with " + UI.FormatAsLink("Liquid Ethanol", "ETHANOL") + ".");
                    }
                    public static class GAS_NUCLEAR_FALLOUT
                    {
                        public static LocString NAME = UI.FormatAsLink("Nuclear Fallout Vent", "GeyserGeneric_GAS_NUCLEAR_FALLOUT");
                        public static LocString DESC = ("A highly pressurized vent that periodically erupts with scalding " + UI.FormatAsLink("Nuclear Fallout", "FALLOUT") + ".");
                    }
                }
            }
        }
        public static class HELLSEN
        {
            public static class BUILDINGS
            {
                public static class PREFABS
                {
                    public static class WARPPORTAL
                    {
                        public static LocString NAME = "Hellsen Teleporter Transmitter";
                        public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the outgoing side, and has one pre-programmed destination.";
                    }
                    public static class WARPRECEIVER
                    {
                        public static LocString NAME = "Hellsen Teleporter Receiver";
                        public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the incoming side.";
                    }
                }
            }
            public static class WORLD_TRAITS
            {
                public static class DEEP_CRASHED_SATELLITES
                {
                    public static LocString NAME = "Deep Crashed Satellites";
                    public static LocString DESC = "This world contains crashed radioactive satellites buried deep underground";
                }
                public static class GEOHYPERACTIVE
                {
                    public static LocString NAME = "Geohyperactive";
                    public static LocString DESC = "This world has waaay more <style=\"KKeyword\">Geysers</style> and <style=\"KKeyword\">Vents</style> than usual";
                }
                public static class TELEPORTER_PAIR
                {
                    public static LocString NAME = "Teleporter Pair";
                    public static LocString DESC = "This world has a teleporter pair";
                }
            }
            public static class CLUSTER_NAMES
            {
                public static class MINICLUSTER_FLIPPEDSTART
                {
                    public static LocString NAME = "Moonlet Cluster - Hellsen Flipped";
                    public static LocString DESC = "A cluster of visitable planetoids with an inverted starting world.";
                }
                public static class VANILLA_ARIDIO_CLUSTER
                {
                    public static LocString NAME = "Hellsen Aridio Cluster";
                    public static LocString DESC = "A cluster of planets with a hot foresty planetoid to start on.";
                }
                public static class VANILLA_RIME_CLUSTER
                {
                    public static LocString NAME = "Hellsen Rime Cluster";
                    public static LocString DESC = "A cluster of planets with a frozen planetoid to start on.";
                }
                public static class CERES_SPACED_OUT_CLUSTER
                {
                    public static LocString NAME = "Hellsen Ceres Minor Cluster";
                    public static LocString DESC = "A cluster of visitable planetoids with an Ice Cave starting world.";
                }
            }
            public static class WORLDS
            {
                public static class MINI_METALLIC_SWAMPY
                {
                    public static LocString NAME = "Hellsen Metallic Swampy Moonlet";
                    public static LocString DESC = "A small swampy world with an abundance of renewable metal.\n\n<smallcaps>Metallic Swampy Moonlet have a high concentration of metal and a swampy, polluted atmosphere.</smallcaps>";
                }
                public static class MINI_BADLANDS
                {
                    public static LocString NAME = "The Hellsen Desolands Moonlet";
                    public static LocString DESC = "A barren location with an overabundance of mineral resources and oil.\n\n<smallcaps>The rocky terrain of The Desolands poses no immediate threats, but the limited variety of resources will make expansion and technological progress challenging.</smallcaps>";
                }
                public static class MINI_FLIPPED
                {
                    public static LocString NAME = "Hellsen Flipped Moonlet";
                    public static LocString DESC = "A moonlet in which the surface is molten hot lava and the core is livable.\n\n<smallcaps>The lava surface of the Flipped Moonlet will make it challenging to start a rocketry program.</smallcaps>";
                }
                public static class FORESTY_WASTELAND
                {
                    public static LocString NAME = "Hellsen Glowood Wasteland Moonlet";
                    public static LocString DESC = "A small forested moonlet with a frozen radioactive core.\n\n<smallcaps>While Glowood Wasteland Moonlets are largely foresty, they also contain a great deal of rust.</smallcaps>";
                }
                public static class RADIOACTIVE_TERRA
                {
                    public static LocString NAME = "Hellsen Radioactive Terra Moonlet";
                    public static LocString DESC = "A small terra world with a radioactive core.\n\n<smallcaps>The Radioactive Terra Moonlet is fairly dry and sandy.</smallcaps>";
                }
                public static class TUNDRA_MOONLET
                {
                    public static LocString NAME = "Hellsen Tundra Moonlet";
                    public static LocString DESC = "A small frozen planetoid with sub-zero temperatures.\n\n<smallcaps>Duplicants that travel to Tundra Moonlets will need to wear protective gear due to its harsh environment.</smallcaps>";
                }
                public static class REGOLITH_MOONLET
                {
                    public static LocString NAME = "Hellsen Regolith Moonlet";
                    public static LocString DESC = "A tiny, cold world with plenty of craters.\n\n<smallcaps>Regolith Moonlets have frequent meteor showers and are chock-full of Regolith, an incredibly useful filtration material.</smallcaps>";
                }
                public static class NIOBIUM_MOONLET
                {
                    public static LocString NAME = "Hellsen Superconductive Moonlet";
                    public static LocString DESC = "A small location with an abundance of Niobium.\n\n<smallcaps>Superconductive Moonlets offer rich veins of Niobium to Duplicants that can withstand its molten rivers of magma.</smallcaps>";
                }
                public static class MARSHY_MOONLET
                {
                    public static LocString NAME = "Hellsen Marshy Moonlet";
                    public static LocString DESC = "A small location with an abundance of marshland.\n\n<smallcaps>While Marshy Moonlets offer an abundance of organic resources like Slime Mold and Algae, their air quality poses a significant disease risk for Duplicants.</smallcaps>";
                }
                public static class RIME_MOONLET
                {
                    public static LocString NAME = "Hellsen Rime Moonlet";
                    public static LocString DESC = "A frigid location marked by inhospitably low temperatures throughout.\n\n<smallcaps>Rime's low temperatures will make finding water and establishing farms or ranches problematic. However, the wide spectrum of minerals and cool environment could lead to thriving industry.</smallcaps>";
                }
                public static class MOO_MOONLET
                {
                    public static LocString NAME = "Hellsen Moo Moonlet";
                    public static LocString DESC = "A small world with unique local lifeforms.\n\n<smallcaps>Moo Moonlets are the natural breeding ground of Gassy Moos due to their abundance of Gas Grass and Methane.</smallcaps>";
                }
                public static class VANILLA_ARIDIO
                {
                    public static LocString NAME = "Hellsen Aridio Asteroid";
                    public static LocString DESC = "A location with oppressively hot temperatures.\n\n<smallcaps>Temperatures on Aridio are much higher than expected. While resources are abundant, maintaining food and infrastructure could be difficult as our colony warms.</smallcaps>";
                }
                public static class VANILLA_RIME
                {
                    public static LocString NAME = "Hellsen Rime Asteroid";
                    public static LocString DESC = "A frigid location marked by inhospitably low temperatures throughout.\n\n<smallcaps>Rime's low temperatures will make finding water and establishing farms or ranches problematic. However, the wide spectrum of minerals and cool environment could lead to thriving industry.</smallcaps>";
                }
                public static class MEDIUM_SANDY_SWAMP
                {
                    public static LocString NAME = "Hellsen Radioactive Terrabog Asteroid";
                    public static LocString DESC = "A mid-sized terra world with a radioactive core.\n\n<smallcaps>The Radioactive Terrabog Asteroid is fairly dry and sandy, but does contain swampy areas with a partially frozen surface.</smallcaps>";
                }
                public static class MEDIUM_SWAMPY
                {
                    public static LocString NAME = "Hellsen Stinko Swamp Asteroid";
                    public static LocString DESC = "A large-ish, polluted swamp world.\n\n<smallcaps>Stinko Swamp Asteroids are full of swampy and marshy areas, but also contain a variety of other biomes diverse enough to keep a determined colony supplied with essentials.</smallcaps>";
                }
                public static class SWAMPY_LANDING_SITE
                {
                    public static LocString NAME = "Hellsen Irradiated Swampy Asteroid";
                    public static LocString DESC = "A small swampy world with a radioactive core.\n\n<smallcaps>Irradiated Swamps have a high concentration of rare swampy and radioactive resources but are also chocked full of dangers.</smallcaps>";
                }
                public class OIL_RICH_WARP_TARGET
                {
                    public static LocString NAME = "Hellsen Rusty Oil Asteroid";
                    public static LocString DESC = "A rusty mid-sized world with an oily core.\n\n<smallcaps>Duplicants must sift through a great deal of rust and ocean to reach the prize of this planetoid's oil.</smallcaps>";
                }
                public static class CERES_SPACED_OUT
                {
                    public static LocString NAME = "Hellsen Ceres Minor";
                    public static LocString DESC = "A smaller frosty starting world with sub-zero temperatures throughout.\n\n<smallcaps>Duplicants who work in Ceres's sub-zero temperatures will need to wear or build warming equipment. Abundant natural fuel sources could lead to a well-powered colony.</smallcaps>";
                }
            }
        }
    }
}