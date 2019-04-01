using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2R_Mapper.Palace_Routing;

namespace Z2R_Mapper
{
    class Z2R_Mapper
    {
        private readonly Z2R_Reader _z2rReader;

        private enum ConnectionInfoDrawingInstructions
        {
            IgnoreThisConnection,
            LookupItem,
            LookupTown,
            LookupPalace,
            TagWithSpecificImage,
        }

        private readonly string[] OverworldAreaNames = new string[]
        {
            "Western Hyrule",
            "Death Mountain",
            "Eastern Hyrule",
            "Maze Island",
        };

        private readonly Bitmap[] TownNameBitmaps = new Bitmap[]
        {
            Properties.Resources.Rauru,
            Properties.Resources.Ruto,
            Properties.Resources.Saria_Right,
            Properties.Resources.Mido,
            Properties.Resources.Nabooru,
            Properties.Resources.Darunia,
            Properties.Resources.NewKasuto,
            Properties.Resources.OldKasuto,
            Properties.Resources.Saria_Left,
            Properties.Resources.NorthPalace,
        };

        private readonly string[] TownNameStrings = new string[]
        {
            "Rauru",
            "Ruto",
            "Saria",
            "Mido",
            "Nabooru",
            "Darunia",
            "New Kasuto",
            "Old Kasuto",
            "Saria (Left)",
        };

        private readonly string[] NewKasutoItemAreaNames = new string[]
        {
            "Magic Container Lady",
            "Magic Key Basement",
        };
        
        private readonly string[] CollectibleItemNames = new string[]
        {
            "Candle",
            "Glove",
            "Raft",
            "Boots",
            "Flute",
            "Cross",
            "Hammer",
            "Magic Key",
            "Normal Key",
            "---",
            "PBag",
            "PBag",
            "PBag",
            "PBag",
            "Magic Container",
            "Heart Container",
            "Blue Jar",
            "Red Jar",
            "1Up Doll",
            "Child",
            "Trophy",
            "Medicine",
        };

        private readonly string[] SpellNames = new string[]
        {
            "Shield",
            "Jump",
            "Life",
            "Fairy",
            "Fire",
            "Reflect",
            "Spell",
            "Thunder",
            ""              // Invalid
        };

        private readonly string[] PalaceRouteTypeNames = new string[]
        {
            "Entrance to Item",
            "Item to Boss",
            "Entrance to Boss",
        };

        private readonly string[] DirectionNames = new string[]
        {
            "left",
            "down",
            "up",
            "right",
            "drop",         // Shortened from "drop down pit" to save space in summary TextBox
            "xx"
        };

        private readonly string[] RequirementNames = new string[]
        {
            "",
            "glove required",
            "jump or fairy required",
            "fairy required",
            "downstab required",
            "upstab required",
            "Rebonack required",
            "Thunderbird required",
        };

        // How to crop bitmap for each overworld area, specified in tiles.
        // These dimensions are specific to the Zelda II Randomizer.
        // Applying these cropping dimensions to the normal Zelda II ROM will look funny.
        private readonly Size[] OverworldAreaBitmapCroppingDimensions = new Size[]
        {
            new Size(64, 75),   // Western Hyrule
            new Size(30, 46),   // Death Mountain
            new Size(64, 75),   // Eastern Hyrule
            new Size(22, 21),   // Maze Island
        };

        private class ConnectionInfo
        {
            public ConnectionInfoDrawingInstructions drawingInstructions;
            public Bitmap imageToUse;
            public bool includeInItemSummary = false;
            public int itemSummaryIndex;
            public string locationName;
        }

        // Instructions on how to treat each connection.
        // Indexed by connection id.
        private readonly ConnectionInfo[] WesternHyruleConnectionInfoTable =
        {
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },      // Not really a town
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                // 01
                includeInItemSummary = true, itemSummaryIndex = 2, locationName = "Trophy Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 02
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                // 03
                includeInItemSummary = true, itemSummaryIndex = 1, locationName = "Macic Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 04
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                // 05
                includeInItemSummary = true, itemSummaryIndex = 0, locationName = "Grass Tile" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 08
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 09
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.ParapaCave_Left },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.ParapaCave_Right },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.JumpCave_Left },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.JumpCave_Right },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                // 14
                includeInItemSummary = true, itemSummaryIndex = 4, locationName = "PBag Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                // 15
                includeInItemSummary = true, itemSummaryIndex = 3, locationName = "Medicine Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                // 16
                includeInItemSummary = true, itemSummaryIndex = 5, locationName = "Heart Container Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.FairyCave_Left },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.FairyCave_Right },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 23
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 24
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 25
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 30
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 31
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },              // 32
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 33
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 34
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 35
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 36
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 37
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 38
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 39
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 40
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 41 - Raft
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.DeathMountain_Entrance1 },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.DeathMountain_Entrance2 },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.KingsTomb },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 45 - Rauru
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 46
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 47 - Ruto
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 48 - Saria (left)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 49 - Saria (right)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Bagu },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 51 - Mido
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupPalace },            // 52 - (Normally Palace 1)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupPalace },            // 53 - (Normally Palace 2)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupPalace },            // 54 - (Normally Palace 3)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 55
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 56
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 57
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 58
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 59
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 60
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 61
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 62
        };
        private readonly ConnectionInfo[] DeathMountainAndMazeIslandConnectionInfoTable =
        {
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveA },   // 00
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveA },   // 01
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveB },   // 02
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveB },   // 03
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveC },   // 04
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveC },   // 05
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveD },   // 06
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveD },   // 07
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveE },   // 08
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveE },   // 09
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveF },   // 10
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveF },   // 11
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveG },   // 12
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveG },   // 13
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveJ },   // 14
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveJ },   // 15
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveK },   // 16
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveK },   // 17
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveL },   // 18
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveL },   // 19
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveM },   // 20
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveM },   // 21
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveN },   // 22
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveN },   // 23
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveO },   // 24
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveO },   // 25
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveP },   // 26
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveP },   // 27
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                                                        // 28
                includeInItemSummary = true, itemSummaryIndex = 0, locationName = "Hammer Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveH3 },  // 29
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveH4 },  // 30
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveH1 },  // 31
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveH2 },  // 32
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveI1 },  // 33
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveI2 },  // 34
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveI3 },  // 35
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveI4 },  // 36
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                                                        // 39
                includeInItemSummary = true, itemSummaryIndex = 1, locationName = "Magic Container Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 40 - Raft
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 41 - Bridge to EH
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.DeathMountain_Exit1 },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.DeathMountain_Exit2 },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 44 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 45 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 46 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 47 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 48 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 49 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 50 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 51 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupPalace },                                                    // 52 - (Normally Palace 4)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 53 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 54 - Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                                                        // 55
                includeInItemSummary = true, itemSummaryIndex = 0, locationName = "Child Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,                                                        // 56
                includeInItemSummary = true, itemSummaryIndex = 1, locationName = "Spectacle Rock" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },                                            // 62
        };
        private readonly ConnectionInfo[] EasternHyruleConnectionInfoTable =
        {
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 00
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 01
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },    // Fence Encounters
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },    // Bago Bago Bridges
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },    // Darunia Path
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,        // 10 - Heart Container Water Tile
                includeInItemSummary = true, itemSummaryIndex = 0, locationName = "Water Tile" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveA },               // Nabooru Cave
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveA },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,        // 13 - PBag Cave 1
                includeInItemSummary = true, itemSummaryIndex = 2, locationName = "PBag Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,        // 14 - PBag Cave 2
                includeInItemSummary = true, itemSummaryIndex = 3, locationName = "PBag Cave" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveB },               // Tektite Cave
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveB },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveD },               // 2nd VoD Cave
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveD },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveC },               // 1st VoD Cave
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.CaveC },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 21 - Swamp 1Up
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // Unused
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 23 - Desert PBag Mountain
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 24 - Desert Red Jar
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 25 - Desert 1Up (Dazzle)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem,        // 26 - Heart Container Desert Tile
                includeInItemSummary = true, itemSummaryIndex = 1, locationName = "Desert Tile" },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 27 - Forest Fairy
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 28 - VoD PBag
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupItem },      // 29 - VoD Red Jar
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter2 },   // VoD Encounters
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter2 },   // Use the light blue box since red
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.TagWithSpecificImage, imageToUse = Properties.Resources.Forced_Encounter2 },   // doesn't show up well on red.
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 40 - Long Bridge to Maze Island
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 41 - Raft
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 42 - (Reserved for DM entrance/exit)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 43 - (Reserved for DM entrance/exit)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 45 - Nabooru
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 47 - Darunia
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 49 - New Kasuto
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupTown },              // 51 - Old Kasuto
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupPalace },            // 52 - (Normally Palace 5)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupPalace },            // 53 - (Noramlly Palace 6)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.LookupPalace },            // 54 - (Normally Great Palace)
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 55
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 56
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 57
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 58
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 59
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 60
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 61
            new ConnectionInfo { drawingInstructions = ConnectionInfoDrawingInstructions.IgnoreThisConnection },    // 62
        };

        private class ItemLocationInfo
        {
            public OverworldArea overworldArea;
            public Point locationCoordinates;
            public bool isHidden = false;
            public string locationName;
            public CollectibleItem item;
        }

        private readonly ItemLocationInfo[,] _itemLocationTable = new ItemLocationInfo[6, 7];

        private readonly ItemLocationInfo[] _townLocationTable = new ItemLocationInfo[8];

        private RoutingSolution[][][] _allPalaceRoutingSolutions;

        public Z2R_Mapper(String romFileName)
        {
            _z2rReader = new Z2R_Reader(romFileName);
            GeneratePalaceRoutingSolutionsTable();
        }

        public Bitmap GetOverworldBitmap(OverworldArea overworldArea)
        {
            Bitmap overworldBitmap = new Bitmap(2048, 2400);

            TerrainType[,] terrainMap = _z2rReader.GetOverworldMap(overworldArea);
            using (Graphics grD = Graphics.FromImage(overworldBitmap))
            {
                for(int row = 0; row < 75; row++)
                {
                    for(int col = 0; col < 64; col++)
                    {
                        Bitmap thisTile = _z2rReader.GetTerrainTypeBitmap(terrainMap[row, col]);
                        grD.DrawImage(thisTile, col * 32, row * 32);
                    }
                }
            }

            AddConnectionAndItemInfo(ref overworldBitmap, overworldArea);

            Bitmap retVal = GetCroppedBitmap(overworldBitmap, overworldArea);

            return retVal;
        }

        public string GetItemSummary()
        {
            // Make sure this has been done, since this section of the table will not be
            // prebuilt by the calls to GetOverworldBitmap().
            PopulateNewKasutoItemLocationInfo();

            StringBuilder itemSummary = new StringBuilder(1024);
            ItemLocationInfo locationInfo;

            foreach (OverworldArea overworldArea in (OverworldArea[])Enum.GetValues(typeof(OverworldArea)))
            {
                itemSummary.Append(OverworldAreaNames[(int)overworldArea]);
                itemSummary.AppendLine(":");
                for (int itemIndex = 0; itemIndex < 6; itemIndex++)
                {
                    if ((locationInfo = _itemLocationTable[(int)overworldArea, itemIndex]) != null)
                    {
                        if(overworldArea == OverworldArea.WesternHyrule || overworldArea == OverworldArea.EasternHyrule)
                        {
                            AddItemToItemSummary(ref itemSummary, locationInfo, false, true, 2);
                        } else
                        {
                            AddItemToItemSummary(ref itemSummary, locationInfo, false, false, 2);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                itemSummary.AppendLine();   // Add space before next group
            }

            itemSummary.Append("New Kasuto");
            if(_z2rReader.CheckIsNewKasutoHidden())
            {
                itemSummary.AppendLine(" (hidden):");
            } else
            {
                itemSummary.AppendLine(":");
            }
            foreach (NewKasutoItemArea itemArea in (NewKasutoItemArea[])Enum.GetValues(typeof(NewKasutoItemArea)))
            {
                if ((locationInfo = _itemLocationTable[4, (int)itemArea]) != null)
                {
                    AddItemToItemSummary(ref itemSummary, locationInfo, false, false, 2);
                }
                else
                {
                    break;
                }
            }
            itemSummary.AppendLine();

            itemSummary.AppendLine("Palaces:");
            for(int palaceIndex = 0; palaceIndex < 7; palaceIndex++)
            {
                if ((locationInfo = _itemLocationTable[5, palaceIndex]) != null)
                {
                    AddItemToItemSummary(ref itemSummary, locationInfo, true, true, 2);
                }
                else
                {
                    break;
                }
            }

            return itemSummary.ToString();
        }

        private void AddItemToItemSummary(ref StringBuilder itemSummary, ItemLocationInfo locationInfo,
            bool includeOverworldArea, bool includeRelativeLocation, int indentation)
        {
            bool includeOverworldLocationInfo = (includeOverworldArea || includeRelativeLocation);

            itemSummary.Append(' ', indentation);
            itemSummary.Append(locationInfo.locationName);
            if(includeOverworldLocationInfo)
            {
                itemSummary.Append(" (");
                if(includeOverworldArea)
                {
                    itemSummary.Append(OverworldAreaNames[(int)locationInfo.overworldArea]);
                    if(includeRelativeLocation)
                    {
                        itemSummary.Append(", ");
                    }
                }
                if (includeRelativeLocation)
                {
                    AddRelativeLocationToSummary(ref itemSummary, locationInfo.locationCoordinates, locationInfo.isHidden);
                }
                itemSummary.Append(")");
            }
            if(locationInfo.item != CollectibleItem.Invalid)
            {
                itemSummary.Append(": ");
                itemSummary.AppendLine(CollectibleItemNames[(int)locationInfo.item]);
            } else
            {
                // This else branch only applies to Great Palace
                itemSummary.AppendLine();
            }
        }

        public string GetSpellSummary()
        {
            StringBuilder spellSummary = new StringBuilder(1024);

            spellSummary.AppendLine("Western Hyrule");
            for(int town = (int)Town.Rauru; town <= (int)Town.Mido; town++)
            {
                AddTownToSpellSummary(ref spellSummary, (Town)town);
            }

            spellSummary.AppendLine();
            spellSummary.AppendLine("Eastern Hyrule");
            for (int town = (int)Town.Nabooru; town <= (int)Town.OldKasuto; town++)
            {
                AddTownToSpellSummary(ref spellSummary, (Town)town);
            }

            return spellSummary.ToString();
        }

        private void AddTownToSpellSummary(ref StringBuilder spellSummary, Town town)
        {
            Spell spell = _z2rReader.GetSpellAssignedToTown(town);
            spellSummary.Append("  ");
            spellSummary.Append(TownNameStrings[(int)town]);
            spellSummary.Append(" (");
            AddRelativeLocationToSummary(ref spellSummary, _townLocationTable[(int)town].locationCoordinates,
                _townLocationTable[(int)town].isHidden);
            spellSummary.Append("): ");
            spellSummary.Append(SpellNames[(int)spell]);
            if(spell == Spell.Fire)
            {
                Spell combinedSpell = _z2rReader.GetSpellCombinedWithFire();
                if(combinedSpell != Spell.Invalid)
                {
                    spellSummary.Append(" (combined with ");
                    spellSummary.Append(SpellNames[(int)combinedSpell]);
                    spellSummary.Append(")");
                }
            }
            spellSummary.AppendLine();
        }

        public string GetSpellCostsSummary()
        {
            StringBuilder spellCostsSummary = new StringBuilder(1024);
            byte[][] spellCostsTable = _z2rReader.GetSpellCastingCosts();

            spellCostsSummary.AppendLine("        |   1|   2|   3|   4|   5|   6|   7|   8|");
            for(int spellPosition = 0; spellPosition < 8; spellPosition++)
            {
                spellCostsSummary.AppendLine("--------+----+----+----+----+----+----+----+----+");
                string spellName = SpellNames[(int)_z2rReader.GetSpellAssignedToTown((Town)spellPosition)];
                spellCostsSummary.Append(string.Format("{0,-8}", spellName));
                for (int magicLevel = 0; magicLevel < 8; magicLevel++)
                {
                    spellCostsSummary.Append(string.Format("|{0,4}", (spellCostsTable[spellPosition][magicLevel] >> 1)));
                }
                spellCostsSummary.AppendLine("|");
            }
            spellCostsSummary.AppendLine("--------+----+----+----+----+----+----+----+----+");

            return spellCostsSummary.ToString();
        }

        private void AddRelativeLocationToSummary(ref StringBuilder summaryText, Point xyCoords, bool isHidden)
        {
                string[] xLocationStrings = new string[]
                {
                        "left", "middle", "right"
                };
                string[] yLocationStrings = new string[]
                {
                        "upper", "middle", "lower"
                };

                int xStringIndex;
                if (xyCoords.X < 21)
                {
                    xStringIndex = 0;
                }
                else if (xyCoords.X < 43)
                {
                    xStringIndex = 1;
                }
                else
                {
                    xStringIndex = 2;
                }

                int yStringIndex;
                if (xyCoords.Y < 25)
                {
                    yStringIndex = 0;
                }
                else if (xyCoords.Y < 50)
                {
                    yStringIndex = 1;
                }
                else
                {
                    yStringIndex = 2;
                }

                summaryText.Append(yLocationStrings[yStringIndex]);
                if (xStringIndex != 1 || yStringIndex != 1)
                {
                    summaryText.Append(" ");
                    summaryText.Append(xLocationStrings[xStringIndex]);
                }
                if (isHidden)
                {
                    summaryText.Append(", hidden");
                }
        }

        public string GetStartingStatsSummary(bool showMaxHeartContainers, bool showSpellCombinedWithFire)
        {
            StringBuilder statsSummary = new StringBuilder(1024);

            StartingStats stats = _z2rReader.GetStaringStats();
            statsSummary.AppendLine("Starting Stats:");
            statsSummary.AppendLine(string.Format("  Lives: {0}", stats.numLives));
            statsSummary.AppendLine(string.Format("  Heart Containers: {0}", stats.numHeartContainers));
            if(showMaxHeartContainers)
            {
                int maxHeartContainers = stats.numHeartContainers + _z2rReader.GetNumberOfExtraHeartContainers();
                statsSummary.AppendLine(string.Format("  Max Heart Containers: {0}", maxHeartContainers));
            }
            statsSummary.AppendLine(string.Format("  Gems: {0}", stats.numGems));

            // Attack strengths based on output of the randomizer
            // 01 01 02 03 04 06 09 0C      Low
            // 02 03 04 06 09 0C 12 18      Normal
            // 02 04 05 08 0C 10 19 21      High
            // C0 C0 C0 C0 C0 C0 C0 C0      OHKO  (except for bubbles)

            // Attack Effectiveness doesn't seem like a worthwhile thing to show.
            //byte[] attackStrengths = _z2rReader.GetAttackStrengths();
            //statsSummary.Append("  Attack Effectiveness: ");
            //if (attackStrengths[4] >= 0xC0)
            //{
            //    statsSummary.AppendLine("OHKO");
            //}
            //else if (attackStrengths[4] < 0x09)
            //{
            //    statsSummary.AppendLine("Low");
            //}
            //else if (attackStrengths[4] > 0x09)
            //{
            //    statsSummary.AppendLine("High");
            //} else
            //{
            //    statsSummary.AppendLine("Normal");
            //}

            statsSummary.AppendLine();
            statsSummary.AppendLine("Starting Spells:");
            if(stats.startingSpells.Length > 0)
            {
                for(int i = 0; i < stats.startingSpells.Length; i++)
                {
                    statsSummary.Append(string.Format("  {0}", SpellNames[(int)stats.startingSpells[i]]));
                    if (showSpellCombinedWithFire && (stats.startingSpells[i] == Spell.Fire))
                    {
                        Spell combinedSpell = _z2rReader.GetSpellCombinedWithFire();
                        if (combinedSpell != Spell.Invalid)
                        {
                            statsSummary.Append(" (combined with ");
                            statsSummary.Append(SpellNames[(int)combinedSpell]);
                            statsSummary.AppendLine(")");
                        } else
                        {
                            statsSummary.AppendLine();
                        }
                    }
                    else
                    {
                        statsSummary.AppendLine();
                    }
                }
            } else
            {
                statsSummary.AppendLine("  None");
            }

            statsSummary.AppendLine();
            statsSummary.AppendLine("Starting Items:");
            if (stats.startingItems.Length > 0)
            {
                for (int i = 0; i < stats.startingItems.Length; i++)
                {
                    statsSummary.AppendLine(string.Format("  {0}", CollectibleItemNames[(int)stats.startingItems[i]]));
                }
            }
            else
            {
                statsSummary.AppendLine("  None");
            }

            statsSummary.AppendLine();
            statsSummary.AppendLine("Starting Techs:");
            if(stats.downthrustEnabled || stats.upthrustEnabled)
            {
                if (stats.downthrustEnabled)
                    statsSummary.AppendLine("  Downthrust");
                if (stats.upthrustEnabled)
                    statsSummary.AppendLine("  Upthrust");
            } else
            {
                statsSummary.Append("  None");
            }

            return statsSummary.ToString();
        }

        private void AddConnectionAndItemInfo(ref Bitmap mapToUpdate, OverworldArea overworldArea)
        {
            using (Graphics grD = Graphics.FromImage(mapToUpdate))
            {
                Point[] connectionLocations = _z2rReader.GetOverworldConnectionLocations(overworldArea);
                for (int connectionID = 0; connectionID < 63; connectionID++)
                {
                    ConnectionInfo connInfo;
                    switch(overworldArea)
                    {
                        case OverworldArea.WesternHyrule:
                            connInfo = WesternHyruleConnectionInfoTable[connectionID];
                            break;
                        case OverworldArea.DeathMountain:
                        case OverworldArea.MazeIsland:
                            // Originally it appears Nintendo intended to use the same map area for both
                            // Death Mountain and Maze Island.  The real Maze Island later got copied into
                            // ROM bank 2.  Both Death Mountain and Maze Island have the exact same map,
                            // with death mountain on the left and maze island on the right, both using
                            // all the same connection IDs.
                            // That's in the vanilla game at least.  The randomizer truly creates them
                            // separate, but it still uses the same connections IDs, meaning there is
                            // no connection ID overlap between Death Mountain and Maze Island.
                            connInfo = DeathMountainAndMazeIslandConnectionInfoTable[connectionID];
                            break;
                        case OverworldArea.EasternHyrule:
                            connInfo = EasternHyruleConnectionInfoTable[connectionID];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Bitmap image = Properties.Resources.Blank_Square;
                    Point xyLoc = connectionLocations[connectionID];
                    switch (connInfo.drawingInstructions)
                    {
                        case ConnectionInfoDrawingInstructions.IgnoreThisConnection:
                            // Do nothing
                            break;

                        case ConnectionInfoDrawingInstructions.LookupItem:
                            // Unused connection IDs will be placed off the map.  This is how we know to ignore them.
                            // This is pertinent for Death Mountain and Maze Island.
                            if ((xyLoc.X >= 0 && xyLoc.X < 64) && (xyLoc.Y >= 0 && xyLoc.Y < 75))
                            {
                                CollectibleItem item = _z2rReader.GetItemAtConnectionID(overworldArea, connectionID);
                                image = _z2rReader.GetCollectibleItemBitmap(item);
                                grD.DrawImage(image, xyLoc.X * 32, xyLoc.Y * 32, 32, 32);
                                if (connInfo.includeInItemSummary)
                                {
                                    _itemLocationTable[(int)overworldArea, connInfo.itemSummaryIndex] = new ItemLocationInfo
                                    {
                                        locationCoordinates = new Point(xyLoc.X, xyLoc.Y),
                                        locationName = connInfo.locationName,
                                        item = item
                                    };
                                }
                            }
                            break;

                        case ConnectionInfoDrawingInstructions.LookupTown:
                            Town thisTown = _z2rReader.GetTownAtConnectionID(overworldArea, connectionID);
                            Bitmap townName = TownNameBitmaps[(int)thisTown];

                            if(thisTown >= Town.Rauru && thisTown <= Town.OldKasuto)
                            {
                                _townLocationTable[(int)thisTown] = new ItemLocationInfo
                                {
                                    locationCoordinates = new Point(xyLoc.X, xyLoc.Y),
                                    isHidden = (thisTown == Town.NewKasuto) ? _z2rReader.CheckIsNewKasutoHidden() : false,
                                };
                            }

                            // Center town name under town
                            int bitmapX = (xyLoc.X * 32) + 16 - (townName.Width / 2);
                            int bitmapY = (xyLoc.Y + 1) * 32 + 4;
                            grD.DrawImage(townName, bitmapX, bitmapY, townName.Width, 24);
                            break;

                        case ConnectionInfoDrawingInstructions.LookupPalace:
                            // For Death Mountain connection info, the Randomizer will place valid palace connection
                            // data at the connection ID that is for the Maze Island palace, although it will be
                            // located off the screen.  We need to treat palace info in Death Mountain as invalid.
                            if (overworldArea != OverworldArea.DeathMountain)
                            {
                                int palaceNumber = _z2rReader.GetPalaceAtConnectionID(overworldArea, connectionID);
                                switch (palaceNumber)
                                {
                                    case 1:
                                        image = Properties.Resources.P1;
                                        break;
                                    case 2:
                                        image = Properties.Resources.P2;
                                        break;
                                    case 3:
                                        image = Properties.Resources.P3;
                                        break;
                                    case 4:
                                        image = Properties.Resources.P4;
                                        break;
                                    case 5:
                                        image = Properties.Resources.P5;
                                        break;
                                    case 6:
                                        image = Properties.Resources.P6;
                                        break;
                                    case 7:
                                        image = Properties.Resources.GP;
                                        break;
                                }
                                //xyLoc.Y++;
                                grD.DrawImage(image, xyLoc.X * 32, xyLoc.Y * 32, 32, 32);

                                string palaceName = (palaceNumber < 7) ? "Palace " + palaceNumber.ToString() : "Great Palace";
                                _itemLocationTable[5, palaceNumber - 1] = new ItemLocationInfo
                                {
                                    overworldArea = overworldArea,
                                    locationCoordinates = new Point(xyLoc.X, xyLoc.Y),
                                    locationName = palaceName,
                                    item = _z2rReader.GetItemInPalace(palaceNumber),
                                    isHidden = _z2rReader.CheckIsConnectionHidden(overworldArea, connectionID),
                                };
                            }
                            break;

                        case ConnectionInfoDrawingInstructions.TagWithSpecificImage:
                            image = connInfo.imageToUse;
                            grD.DrawImage(image, xyLoc.X * 32, xyLoc.Y * 32, 32, 32);
                            break;
                    }
                }
            }
        }

        private void PopulateNewKasutoItemLocationInfo()
        {
            foreach (NewKasutoItemArea itemArea in (NewKasutoItemArea[])Enum.GetValues(typeof(NewKasutoItemArea)))
            {
                _itemLocationTable[4, (int)itemArea] = new ItemLocationInfo
                {
                    locationName = NewKasutoItemAreaNames[(int)itemArea],
                    item = _z2rReader.GetNewKasutoItem(itemArea),
                };
            }
        }

        private Bitmap GetCroppedBitmap(Bitmap bitmapToCrop, OverworldArea overworldArea)
        {
            Size croppingSizeInTiles = OverworldAreaBitmapCroppingDimensions[(int)overworldArea];
            Rectangle croppingRectangle = new Rectangle(0, 0, croppingSizeInTiles.Width * 32, croppingSizeInTiles.Height * 32);

            Bitmap retVal = new Bitmap(croppingRectangle.Width, croppingRectangle.Height);
            using (Graphics grD = Graphics.FromImage(retVal))
            {
                grD.DrawImageUnscaledAndClipped(bitmapToCrop, croppingRectangle);
            }
            return retVal;
        }

        public string GetPalaceRoutingSummary(bool showDirections, bool showRequirements, bool showItemToBossRoutes)
        {
            StringBuilder routingSummary = new StringBuilder(1024);
            RoutingSolution[] routingSolutionSet;

            // Palace index  = 0 to 5
            // Palace number = 1 to 6
            for(int palaceIndex = 0; palaceIndex < 6; palaceIndex++)
            {
                //routingSummary.AppendLine(string.Format("Palace {0}:", palaceIndex + 1));
                ItemLocationInfo locationInfo;
                if ((locationInfo = _itemLocationTable[5, palaceIndex]) != null)
                {
                    AddItemToItemSummary(ref routingSummary, locationInfo, false, false, 0);
                }
                foreach (PalaceRouteType routeType in (PalaceRouteType[])Enum.GetValues(typeof(PalaceRouteType)))
                {
                    if(showItemToBossRoutes || routeType != PalaceRouteType.ItemToBoss)
                    {
                        routingSummary.AppendLine(string.Format("  {0}:", PalaceRouteTypeNames[(int)routeType]));
                        routingSolutionSet = _allPalaceRoutingSolutions[palaceIndex][(int)routeType];
                        AddSolutionSetToRoutingSummary(ref routingSummary, routingSolutionSet, showDirections, showRequirements);
                    }
                }
                routingSummary.AppendLine();
            }

            routingSummary.AppendLine("Great Palace:");
            routingSummary.AppendLine("  Entrance to Dark Link:");
            routingSolutionSet = _allPalaceRoutingSolutions[6][(int)PalaceRouteType.EntranceToBoss];
            AddSolutionSetToRoutingSummary(ref routingSummary, routingSolutionSet, showDirections, showRequirements);
            routingSummary.AppendLine();

            return routingSummary.ToString();
        }

        private void AddSolutionSetToRoutingSummary(ref StringBuilder routingSummary, RoutingSolution[] routingSolutionSet, bool showDirections, bool showRequirements)
        {
            if (routingSolutionSet.Length == 0)
            {
                routingSummary.AppendLine("    No routes found");
            }
            else if (routingSolutionSet.Length == 1)
            {
                routingSummary.Append("    ");
                if (showDirections)
                {
                    AddDirectionsToRoutingSummary(ref routingSummary, routingSolutionSet[0]);
                    routingSummary.Append("  ");
                }
                if (showRequirements)
                {
                    AddRequirementsToRoutingSummary(ref routingSummary, routingSolutionSet[0]);
                }
                routingSummary.AppendLine();
            }
            else
            {
                for (int solutionIndex = 0; solutionIndex < routingSolutionSet.Length; solutionIndex++)
                {
                    routingSummary.Append(string.Format("    Route {0}: ", solutionIndex + 1));
                    if (showDirections)
                    {
                        AddDirectionsToRoutingSummary(ref routingSummary, routingSolutionSet[solutionIndex]);
                        routingSummary.Append("  ");
                    }
                    if (showRequirements)
                    {
                        AddRequirementsToRoutingSummary(ref routingSummary, routingSolutionSet[solutionIndex]);
                    }
                    routingSummary.AppendLine();
                }
            }
        }

        private void AddDirectionsToRoutingSummary(ref StringBuilder routingSummary, RoutingSolution routingSolution)
        {
            Direction[] routingDirections = routingSolution.directions;
            for (int routeIndex = 0; routeIndex < routingDirections.Length; routeIndex++)
            {
                if(routeIndex < (routingDirections.Length - 1))
                {
                    routingSummary.Append(string.Format("{0}, ", DirectionNames[(int)routingDirections[routeIndex]]));
                } else
                {
                    routingSummary.Append(string.Format("{0}", DirectionNames[(int)routingDirections[routeIndex]]));
                }
            }
        }

        private void AddRequirementsToRoutingSummary(ref StringBuilder routingSummary, RoutingSolution routingSolution)
        {
            routingSummary.Append("(");

            if(routingSolution.numLockedDoors == 0 && routingSolution.requirementFlags == 0x00)
            {
                routingSummary.Append("No requirements");
            } else
            {
                byte requirementFlags = routingSolution.requirementFlags;
                if (routingSolution.numLockedDoors == 1)
                {
                    routingSummary.Append("1 locked door");
                    if (requirementFlags > 0)
                    {
                        routingSummary.Append(", ");
                    }
                } else if (routingSolution.numLockedDoors > 1)
                {
                    routingSummary.Append(string.Format("{0} locked doors", routingSolution.numLockedDoors));
                    if (requirementFlags > 0)
                    {
                        routingSummary.Append(", ");
                    }
                }

                for (int requirementNameIndex = 0; requirementFlags != 0; requirementNameIndex++)
                {
                    if ((requirementFlags & 0x01) > 0)
                    {
                        routingSummary.Append(RequirementNames[requirementNameIndex]);
                        requirementFlags >>= 1;
                        if (requirementFlags > 0)
                        {
                            routingSummary.Append(", ");
                        }
                    }
                    else
                    {
                        requirementFlags >>= 1;
                    }
                }
            }
            routingSummary.Append(")");
        }

        private void GeneratePalaceRoutingSolutionsTable()
        {
            _allPalaceRoutingSolutions = new RoutingSolution[7][][];
            for (int palaceIndex = 0; palaceIndex < 7; palaceIndex++)
            {
                _allPalaceRoutingSolutions[palaceIndex] = new RoutingSolution[3][];
                for (int routeSelection = 0; routeSelection < 3; routeSelection++)
                {
                    RoutingSolution[] solutionSet = _z2rReader.DoPalacePathFindOperation(palaceIndex + 1, 
                        (PalaceRouteType)routeSelection);
                    if((solutionSet != null) && ((PalaceRouteType)routeSelection == PalaceRouteType.EntranceToItem))
                    {
                        foreach(RoutingSolution solution in solutionSet)
                        {
                            // Pathfinder algorithm does not account for final locked door guarding item.
                            solution.numLockedDoors++;
                        }
                    }
                    _allPalaceRoutingSolutions[palaceIndex][routeSelection] = solutionSet;
                }
            }
        }
    }
}
