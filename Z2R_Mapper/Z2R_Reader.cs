using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2R_Mapper.ROM_Utils;
using Z2R_Mapper.Palace_Routing;

namespace Z2R_Mapper
{
    // Zelda II has 16 terrain types that define overworld appearance.
    // These values match those used within the ROM.
    public enum TerrainType
    {
        Town = 0,
        Cave,
        Palace,
        Bridge,
        Desert,
        Grass,
        Forest,
        Swamp,
        Grave,
        Road,
        Lava,
        Mountain,
        Water,
        WalkableWater,
        Boulder,
        RiverDevil
    }

    // Zelda II has four different overworld areas
    public enum OverworldArea
    {
        WesternHyrule,
        DeathMountain,
        EasternHyrule,
        MazeIsland,
    }

    public enum Town
    {
        Rauru = 0,
        Ruto,
        Saria,
        Mido,
        Nabooru,
        Darunia,
        NewKasuto,
        OldKasuto,
        Saria_L,
        NorthPalace,
        Invalid,
    };

    public enum NewKasutoItemArea
    {
        MagicContainerBasement,
        MagicKeyBasement,
    }

    // These values match the collectible item codes used in the ROM.
    public enum CollectibleItem
    {
        Candle = 0,     // Palace items
        Glove,
        Raft,
        Boots,
        Flute,
        Cross,
        Hammer,         // Other items
        MagicKey,
        Key,
        Unused1,        // ...value 9 is not used...
        PBag50,
        PBag100,
        PBag200,
        PBag500,
        MagicContainer,
        HeartContainer,
        BlueJar,
        RedJar,
        ExtraLife,
        Child,          // Fetch quest items for spells
        Trophy,
        Medicine,

        // Values starting here are not official values from the game.
        // They are added to this enum for the purpose of our lookup tables.
        Fairy,

        Invalid = 0xFF
    }

    public enum Spell
    {
        Shield = 0,
        Jump,
        Life,
        Fairy,
        Fire,
        Reflect,
        Spell,
        Thunder,
        Invalid,
    }

    public enum PalaceRouteType
    {
        EntranceToItem,
        ItemToBoss,
        EntranceToBoss,
    }

    public struct StartingStats
    {
        public int numLives;
        public int numHeartContainers;
        public int maxHeartContainers;
        public int numGems;
        public bool downthrustEnabled;
        public bool upthrustEnabled;
        public CollectibleItem[] startingItems;
        public Spell[] startingSpells;
    };

    // For convenience, this is ROM bank and CPU address as they appear when
    // looking at disassembled code.  The ROM bank load address (not provided
    // by this structure) needs to be subtracted from the CPU address to
    // get the offset within the ROM bank.
    public class AbsoluteROMAddress
    {
        public int RomBankNumber;
        public int CPUAddress;

        public AbsoluteROMAddress() { }
        public AbsoluteROMAddress(AbsoluteROMAddress copyFrom)
        {
            this.RomBankNumber = copyFrom.RomBankNumber;
            this.CPUAddress = copyFrom.CPUAddress;
        }
        public AbsoluteROMAddress(Z2R_Reader.ItemLookupHelper itemLookup)
        {
            this.RomBankNumber = itemLookup.RomBankNumber;
            this.CPUAddress = itemLookup.RomBankAddress;
        }
    }

    public class Z2R_Reader
    {
        private readonly ROM_Info _romInfo;
        private readonly PPU_Bitmap_Generator _ppuBitmapGenerator;

        private bool _newKasutoIsHidden;
        private bool _threeEyeRockPalaceIsHidden;

        // Number of heart containers available as collectible items.
        // This is counted as Z2R_Reader scans through all the item locations.
        private int _extraHeartContainers;

        // These constants are for uncompressing overworld terrain data.
        // Not sure if using them increases or decreases readability.
        private static readonly Byte CompressedOverworldDataTerrainTypeMask = 0x0F;
        private static readonly Byte CompressedOverworldDataRepeatCountMask = 0xF0;
        private static readonly int CompressedOverworldDataRepeatCountShiftCount = 4;

        private static readonly int[] ROMBankLoadAddresses =
        {
            0x8000,     // ROM banks 0 through 6 swapped out at 0x8000
            0x8000,
            0x8000,
            0x8000,
            0x8000,
            0x8000,
            0x8000,
            0xC000,     // ROM bank 7 permanently available at 0xC000
        };

        private const byte downthrustEnabledFlag = 0x10;
        private const byte upthrustEnabledFlag = 0x04;
        private enum StartingStatsIndex
        {
            NumLives,
            StartingSpellsBaseAddress,
            NumHeartContainers,
            StartingItemsBaseAddress,
            NumGems,
            StartingSwordTechniques,
        }
        private static readonly AbsoluteROMAddress[] StartingStatsROMPointers =
        {
            new AbsoluteROMAddress { RomBankNumber = 7, CPUAddress = 0xC359 },
            new AbsoluteROMAddress { RomBankNumber = 5, CPUAddress = 0xBAE7 },
            new AbsoluteROMAddress { RomBankNumber = 5, CPUAddress = 0xBAF0 },
            new AbsoluteROMAddress { RomBankNumber = 5, CPUAddress = 0xBAF1 },
            new AbsoluteROMAddress { RomBankNumber = 5, CPUAddress = 0xBB00 },
            new AbsoluteROMAddress { RomBankNumber = 5, CPUAddress = 0xBB02 },
        };

        private static readonly AbsoluteROMAddress[] OverworldTerrainDataPointerAddressesTable =
        {
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0x8508 },
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0x850A },
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0x8508 },
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0x850A },
        };

        // 600 bytes are copied from $861F or $A0FC in ROM to $6A00 in cartridge RAM when loading new side-scrolling area.
        //   E.g. entering a palace, entering a town, entering a new overworld area, etc.
        // ROM addresses of $861F and $A0FC are from hard-coded constants in ROM bank 7.
        // Which address and which ROM bank depend on the region being entered.

        // ROM location of table of overworld x,y locations for connections indexed by connection id.
        // Format is 63 bytes of y locations followed by 63 bytes of x locations.  (X is same as tile column and Y is same as tile row).
        // Table addresses in RAM are $6A00 for y locations and $6A3F for x locations.
        private static readonly AbsoluteROMAddress[] ConnectionXLocationsTableROMPointers =
        {
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0x865E },      // Western Hyrule
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0xA13B },      // Death Mountain
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0x865E },      // Eastern Hyrule
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0xA13B },      // Maze Island
        };
        private static readonly AbsoluteROMAddress[] ConnectionYLocationsTableROMPointers =
        {
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0x861F },      // Western Hyrule
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0xA0FC },      // Death Mountain
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0x861F },      // Eastern Hyrule
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0xA0FC },      // Maze Island
        };

        // Part of the region info is which side-scroll area we are entering.
        // "Side-scroll area" is what I call a set of side-scrolling "rooms".  Each region has up to 63 "rooms".
        // These are the values used by the game itself.
        // This is what CF207 calls the "Normalized World Number": https://github.com/cfrantz/z2doc/wiki/world_numbers
        //   See column "Normalized World" in the second table.
        private enum SideScrollArea
        {
            Overworld = 0,      // For this case, the game also analyzes the overworld area selection to figure out which rooms to load.
            WesternHyruleTowns,
            EasternHyruleTowns,
            Palaces_1_2_and_5,  // Only 63 rooms across all 3 of these palaces.
            Palaces_3_4_and_6,  // Only 63 rooms across all 3 of these palaces.
            Great_Palace,
        }

        // Table addresses in RAM are $6A7E for the room being entered (0 through 62) and 
        // $6ABD for the side-scroll-area/region.
        private static readonly AbsoluteROMAddress[] ConnectionRegionDestinationsTableROMPointers =
        {
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0x86DC },      // Western Hyrule
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0xA1B9 },      // Death Mountain
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0x86DC },      // Eastern Hyrule
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0xA1B9 },      // Maze Island
        };
        private static readonly AbsoluteROMAddress[] ConnectionRoomDestinationsTableROMPointers =
        {
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0x869D },      // Western Hyrule
            new AbsoluteROMAddress { RomBankNumber = 1, CPUAddress = 0xA17A },      // Death Mountain
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0x869D },      // Eastern Hyrule
            new AbsoluteROMAddress { RomBankNumber = 2, CPUAddress = 0xA17A },      // Maze Island
        };

        // Background palette used by each terrain type.
        // A game can set up to 4 background palettes in the PPU, each having 4 colors.
        // A tile on the display can only use one of these palettes at a time.
        // These are in order from Town to River Devil.
        private static readonly int[] OverworldTerrainTypePaletteIndices =
        {
            2, 1, 2, 1, 3, 0, 0, 0, 1, 1, 1, 1, 3, 3, 1, 1
        };

        // Patterns used for overworld tiles are from the right pattern table in CHR bank 8.
        // Each tile is made up of four 8x8 patterns.
        // Order of patterns in this array is top left, top right, bottom left, bottom right.
        private static readonly int[][] OverworldTerrainTypePatternTableIndices =
        {
            new int[4] { 0x5C, 0x5E, 0x5D, 0x5F },     // Town
            new int[4] { 0xF4, 0xF4, 0xF4, 0xF4 },     // Cave
            new int[4] { 0x60, 0x62, 0x61, 0x63 },     // Palace
            new int[4] { 0x5A, 0x5B, 0x5A, 0x5B },     // Bridge
            new int[4] { 0x6C, 0x6C, 0x6C, 0x6C },     // Desert
            new int[4] { 0x6D, 0x6D, 0x6D, 0x6D },     // Grass
            new int[4] { 0x68, 0x6A, 0x69, 0x6B },     // Forest
            new int[4] { 0x6F, 0x6F, 0x6F, 0x6F },     // Swamp
            new int[4] { 0x70, 0xFE, 0x71, 0xFE },     // Grave
            new int[4] { 0xFE, 0xFE, 0xFE, 0xFE },     // Road
            new int[4] { 0x6E, 0x6E, 0x6E, 0x6E },     // Lava
            new int[4] { 0x64, 0x66, 0x65, 0x67 },     // Mountain
            new int[4] { 0x6E, 0x6E, 0x6E, 0x6E },     // Water
            new int[4] { 0x6E, 0x6E, 0x6E, 0x6E },     // Walkable Water
            new int[4] { 0x56, 0x58, 0x57, 0x59 },     // Boulder
            new int[4] { 0x40, 0x42, 0x41, 0x43 },     // River Devil
        };

        private readonly int[][] OverworldBackgroundPalettes = {
            new int[4] { 0x0F, 0x30, 0x29, 0x19 },
            new int[4] { 0x0F, 0x30, 0x28, 0x17 },
            new int[4] { 0x0F, 0x30, 0x28, 0x00 },
            new int[4] { 0x0F, 0x30, 0x36, 0x21 }
        };

        private readonly Bitmap[] OverworldBackgroundTileBitmaps;

        public class ItemLookupHelper
        {
            public bool LookupRequired;
            public int RomBankNumber;
            public int RomBankAddress;
            public CollectibleItem ItemTypeIfNoLookup = CollectibleItem.Invalid;
        }

        // Indexed by connection IDs 0 through 63
        // Some locations have their item defined as part of the enemy/object array corresponding to that "room"
        // rather than having a CollectibleItem contained in the room definition.  These are mainly just fairies
        // and red jars.  So, if a 1-screen forced encounter has a PBag, that PBag is defined as part of the
        // room definition, but if it has a red jar, then that jar is not part of the room definition.
        private readonly ItemLookupHelper[] ItemLookupTableForWesternHyrule =
        {
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 00 North Palace
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8DDA },     // 01 Trophy Cave
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8E0B },     // 02 Forest 50 PBag
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x901A },     // 03 Magic Cave
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8E2A },     // 04 Forest 100 PBag
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8DC7 },     // 05 Heart Container Grass Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 06 Lost Woods Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 07 Bubble Encounter
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8E47 },     // 08 Swamp 1Up
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.RedJar },    // 09 Graveyard Red Jar
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 10 Parapa Cave Left
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 11 Parapa Cave Right
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 12 Jump Cave Left
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 13 Jump Cave Right   
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8FD2 },     // 14 PBag Cave
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x9059 },     // 15 Medicine Cave
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8FE5 },     // 16 Heart Container Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 17 Fairy Cave Left
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 18 Fairy Cave Right
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 19 Bago Bago Bridge
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 20 Bubble Lowder Bridge
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 21 Long Bridge Left
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 22 Long Bridge Right
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.Fairy },     // 23 Fairy Next to Jump Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.RedJar },    // 24 Swamp Red Jar
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.Fairy },     // 25 Fairy East of Saria Town
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 26 Lost Woods Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 27 Lost Woods Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 28 Lost Woods Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 29 Lost Woods Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.RedJar },    // 30 Road Tile Red Jar
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 30 Unused
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0x8FC3 },     // 31 Desert 1Up West of Graveyard
        };
        private readonly ItemLookupHelper[] ItemLookupTableForDeathMountainAndMazeIsland =
        {
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 00 Death Mountain Caves
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 05
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 10
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 15
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 20
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 25
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0xA502 },     // 28 Hammer Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 29 Death Mountain 4-way Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 33 Death Mountain 4-way Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 37 Maze Island Forced Encounter
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 38 Maze Island Forced Encounter
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0xA598 }, // 39 Maze Island Magic Container Pit
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 40 Long Bridge
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 41 Raft
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 42 Death Mountain Exit 1
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 43 Death Mountain Exit 2
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 44 Unused
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 50 Unused
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 52 Maze Island Palace
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },     // 53 Unused
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0xA57B }, // 55 Maze Island Child Pit
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 1, RomBankAddress = 0xA5B3 }, // 56 Spectacle Rock
        };
        private readonly ItemLookupHelper[] ItemLookupTableForEasternHyrule =
        {
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8DAE },     // 00 Forest PBag Mountain
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8E1D },     // 01 Forest PBag Stonehenge
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 02 Fence Encounters
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 06 Bago Bago Bridges
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 07 Darunia Path
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8F9A },     // 10 Heart Container Water Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 11 Nabooru Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8FA3 },     // 13 PBag Cave 1
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8EBC },     // 14 PBag Cave 2
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 15 Tektite Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 17 2nd VoD Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 19 1st VoD Cave
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8DF2 },     // 21 Swamp 1Up Mountain
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0 },         // 22 Unused
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8DCD },     // 23 Desert PBag Mountain
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.RedJar },
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x8F83 },     // 25 Desert Dazzle 1Up
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x9001 },     // 26 Heart Container Desert Tile
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.Fairy },
            new ItemLookupHelper { LookupRequired = true, RomBankNumber = 2, RomBankAddress = 0x9029 },     // 28 VoD PBag
            new ItemLookupHelper { LookupRequired = false, RomBankNumber = 0, RomBankAddress = 0, ItemTypeIfNoLookup = CollectibleItem.RedJar },
        };

        private readonly AbsoluteROMAddress[] ItemLookupTableForNewKasuto =
        {
            new AbsoluteROMAddress { RomBankNumber = 3, CPUAddress = 0x9B7C },
            new AbsoluteROMAddress { RomBankNumber = 3, CPUAddress = 0x9B85 },
        };

        private readonly AbsoluteROMAddress[] ItemLookupTableForPalaces =
        {
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0x8E81 },
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0x8E8A },
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0xA51D },
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0xA528 },
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0x8E93 },
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0xA764 },
        };

        private readonly AbsoluteROMAddress SpellCastingCostsTableBase = new AbsoluteROMAddress
        {
            RomBankNumber = 0,
            CPUAddress = 0x8D7B,
        };

        private readonly AbsoluteROMAddress SpellMasksTableBase = new AbsoluteROMAddress
        {
            RomBankNumber = 0,
            CPUAddress = 0x8DBB,
        };

        private readonly AbsoluteROMAddress AttackStrengthTableBase = new AbsoluteROMAddress
        {
            RomBankNumber = 7,
            CPUAddress = 0xE66D,
        };

        private readonly AbsoluteROMAddress DamageTableBase = new AbsoluteROMAddress
        {
            RomBankNumber = 7,
            CPUAddress = 0xE2AF
        };

        private class PalaceRoutingInfo
        {
            public int connectionMapToUse;
            public int startingRoomIndex;
            public int endingRoomIndex;
        }

        // First index is palace number - 1.  0 = Palace 1, 6 = Great Palace
        // Second index is PalaceRoute enum (e.g. EntraceToItem, ItemToBoss, or EntranceToBoss).
        private readonly PalaceRoutingInfo[][] PalaceRoutingInfoTable = new PalaceRoutingInfo[7][]
        {
            new PalaceRoutingInfo[3]
            {
                // Palace 1
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 0, endingRoomIndex = 8 },    // Entrance to Item
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 8, endingRoomIndex = 13 },   // Item to Boss
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 0, endingRoomIndex = 13 },   // Entrance to Boss
            },
            new PalaceRoutingInfo[3]
            {
                // Palace 2
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 14, endingRoomIndex = 20 },  // Entrance to Item
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 20, endingRoomIndex = 34 },  // Item to Boss
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 14, endingRoomIndex = 34 },  // Entrance to Boss
            },
            new PalaceRoutingInfo[3]
            {
                // Palace 3
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 0, endingRoomIndex = 11 },   // Entrance to Item
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 11, endingRoomIndex = 14 },  // Item to Boss
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 0, endingRoomIndex = 14 },   // Entrance to Boss
            },
            new PalaceRoutingInfo[3]
            {
                // Palace 4
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 15, endingRoomIndex = 31 },  // Entrance to Item
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 31, endingRoomIndex = 28 },  // Item to Boss
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 15, endingRoomIndex = 28 },  // Entrance to Boss
            },
            new PalaceRoutingInfo[3]
            {
                // Palace 5
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 35, endingRoomIndex = 61 },  // Entrance to Item
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 61, endingRoomIndex = 41 },  // Item to Boss
                new PalaceRoutingInfo{ connectionMapToUse = 0, startingRoomIndex = 35, endingRoomIndex = 41 },  // Entrance to Boss
            },
            new PalaceRoutingInfo[3]
            {
                // Palace 6
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 36, endingRoomIndex = 44 },  // Entrance to Item
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 44, endingRoomIndex = 58 },  // Item to Boss
                new PalaceRoutingInfo{ connectionMapToUse = 1, startingRoomIndex = 36, endingRoomIndex = 58 },  // Entrance to Boss
            },
            new PalaceRoutingInfo[3]
            {
                // Great Palace
                null,                                                                                           // Entrance to Item
                null,                                                                                           // Item to Boss
                new PalaceRoutingInfo{ connectionMapToUse = 2, startingRoomIndex = 0, endingRoomIndex = 54 },   // Entrance to Dark Link
            },
        };

        private readonly RoomConnectionMap[] PalaceRoomConnectionMaps = new RoomConnectionMap[]
        {
            PalaceRouting.ConnectionMapForPalaces_1_2_and_5,
            PalaceRouting.ConnectionMapForPalaces_3_4_and_6,
            PalaceRouting.ConnectionMapForGreatPalace,
        };

        private readonly AbsoluteROMAddress[] PalaceRoomConnectionTableROMPointers = new AbsoluteROMAddress[]
        {
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0x871B },      // Palaces 1, 2, and 5
            new AbsoluteROMAddress { RomBankNumber = 4, CPUAddress = 0xA1F8 },      // Palaces 3, 4, and 6
            new AbsoluteROMAddress { RomBankNumber = 5, CPUAddress = 0x871B },      // Great Palace
        };

        // Each collectible item sprite uses 2 patterns.
        // Patterns may come from either left hand or right hand pattern table.
        // All sprites are 8x16, meaning two consecutive patterns will be read.
        // Extend and flip
        //   False = The object sprite is only 8 pixels wide
        //   True = The object sprite is 16 pixels wide
        //      The right hand side of the object is created by horizontally flipping the left hand side.
        private struct CollectibleItemDrawingInfo
        {
            public int ChrBankNum;
            public int PaletteIndex;
            public bool IsRightPatternTable;
            public int PatternTableIndex;
            public bool ExtendAndFlip;
        }

        private readonly CollectibleItemDrawingInfo[] CollectibleItemDrawingInfoTable =
        {
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x8C, ExtendAndFlip = false },  // Candle
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x8E, ExtendAndFlip = false },  // Glove
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x90, ExtendAndFlip = false },  // Raft
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x92, ExtendAndFlip = false },  // Boots
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x94, ExtendAndFlip = false },  // Flute
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x96, ExtendAndFlip = false },  // Cross
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x98, ExtendAndFlip = false },  // Hammer
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x9A, ExtendAndFlip = false },  // Magic Key
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x66, ExtendAndFlip = false },  // Normal Key
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x66, ExtendAndFlip = false },  // Unused
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x72, ExtendAndFlip = false },  // PBag 50
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x72, ExtendAndFlip = false },  // PBag 100
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x72, ExtendAndFlip = false },  // PBag 200
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x72, ExtendAndFlip = false },  // PBag 500
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = true,  PatternTableIndex = 0x82, ExtendAndFlip = true },   // Magic Container
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = true,  PatternTableIndex = 0x80, ExtendAndFlip = true },   // Heart Container
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 3, IsRightPatternTable = false, PatternTableIndex = 0x8A, ExtendAndFlip = false },  // Blue Jar
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 2, IsRightPatternTable = false, PatternTableIndex = 0x8A, ExtendAndFlip = false },  // Red Jar
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 0, IsRightPatternTable = false, PatternTableIndex = 0xA8, ExtendAndFlip = false },  // 1Up Doll
            new CollectibleItemDrawingInfo { ChrBankNum = 2, PaletteIndex = 1, IsRightPatternTable = true,  PatternTableIndex = 0x30, ExtendAndFlip = true },   // Child
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = true,  PatternTableIndex = 0x2E, ExtendAndFlip = true },   // Trophy
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = true,  PatternTableIndex = 0x30, ExtendAndFlip = true },   // Medicine
            new CollectibleItemDrawingInfo { ChrBankNum = 1, PaletteIndex = 1, IsRightPatternTable = false, PatternTableIndex = 0x6A, ExtendAndFlip = false },  // Fairy
        };

        // These are the sprite/object palette colors used when in a cave.  Dunno if they are different elsewhere.
        // For sprites, 0x00 means transparent, not black.
        private readonly int[][] SpritePalettes = {
            new int[4] { 0x00, 0x18, 0x36, 0x2A },  // Link and extra life doll
            new int[4] { 0x00, 0x16, 0x27, 0x30 },  // Most items
            new int[4] { 0x00, 0x07, 0x16, 0x30 },  // Red jar and other red things
            new int[4] { 0x00, 0x0C, 0x2C, 0x30 }   // Blue jar and other blue things
        };

        private readonly Bitmap[] CollectibleItemBitmaps;

        public Z2R_Reader(String romFileName)
        {
            // Set up helper classes
            _romInfo = new ROM_Info(romFileName);
            _ppuBitmapGenerator = new PPU_Bitmap_Generator(_romInfo);
            _newKasutoIsHidden = false;
            _threeEyeRockPalaceIsHidden = false;
            _extraHeartContainers = 0;

            // Pre-load the room connection tables before Z2R_Mapper asks for palace routing solutions
            LoadPalaceRoomConnectionTables();

            // Pre-generate images for overworld terrain tiles and collectible item sprites
            OverworldBackgroundTileBitmaps = new Bitmap[Enum.GetValues(typeof(TerrainType)).Length];  // Better way to do this?
            foreach (TerrainType terrainType in (TerrainType[])Enum.GetValues(typeof(TerrainType)))
            {
                OverworldBackgroundTileBitmaps[(int)terrainType] = GenerateOverworldTerrainTileBitmap(terrainType);
            }
            CollectibleItemBitmaps = new Bitmap[(int)CollectibleItem.Fairy + 1];
            foreach (CollectibleItem item in (CollectibleItem[])Enum.GetValues(typeof(CollectibleItem)))
            {
                if((int)item <= (int)CollectibleItem.Fairy)
                {
                    CollectibleItemBitmaps[(int)item] = GenerateCollectibleItemBitmap(item);
                }
            }
        }

        public TerrainType[,] GetOverworldMap(OverworldArea overworldArea)
        {
            // Each overworld area is 75 rows and 64 columns, even if not all of it is used.
            Byte[] terrainData = new Byte[75 * 64];

            AbsoluteROMAddress overworldTerrainDataPointerAddress = OverworldTerrainDataPointerAddressesTable[(int)overworldArea];
            AbsoluteROMAddress overworldTerrainDataPointer = new AbsoluteROMAddress
            {
                RomBankNumber = overworldTerrainDataPointerAddress.RomBankNumber,
                CPUAddress = GetUInt16AtAbsoluteROMAddress(overworldTerrainDataPointerAddress)
            };

            try
            {
                int terrainDataIndex = 0;
                while (terrainDataIndex < (75 * 64))
                {
                    Byte compressedTerrainData = GetByteAtAbsoluteROMAddress(overworldTerrainDataPointer);
                    Byte terrainType = (Byte)(compressedTerrainData & CompressedOverworldDataTerrainTypeMask);
                    Byte repeatCount = (Byte)(compressedTerrainData & CompressedOverworldDataRepeatCountMask);
                    repeatCount >>= CompressedOverworldDataRepeatCountShiftCount;

                    // A single nibble is used to represent repeat count.
                    // This represents repetition of the tile 1 to 16 times, not 0 to 15.
                    repeatCount++;

                    while ((repeatCount > 0) && (terrainDataIndex < (75 * 64)))
                    {
                        terrainData[terrainDataIndex] = terrainType;
                        terrainDataIndex++;
                        repeatCount--;
                    }

                    overworldTerrainDataPointer.CPUAddress++;
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                throw new InvalidDataException("Error parsing overworld data");
            }

            // Now copy into return array
            TerrainType[,] retVal = new TerrainType[75, 64];
            for (int row = 0; row < 75; row++)
            {
                for(int col = 0; col < 64; col++)
                {
                    retVal[row, col] = (TerrainType)terrainData[(row * 64) + col];
                }
            }

            return retVal;
        }

        public Point[] GetOverworldConnectionLocations(OverworldArea overworldArea)
        {
            Point[] retVal = new Point[63];

            AbsoluteROMAddress XLocationsTableAddress = ConnectionXLocationsTableROMPointers[(int)overworldArea];
            AbsoluteROMAddress YLocationsTableAddress = ConnectionYLocationsTableROMPointers[(int)overworldArea];

            Byte[] xLocationsTable = GetBytesAtAbsoluteROMAddress(XLocationsTableAddress, 63);
            Byte[] yLocationsTable = GetBytesAtAbsoluteROMAddress(YLocationsTableAddress, 63);

            // 6 bits are used for connection IDs and one value is always a reserved value,
            // so Nintendo literally sized these arrays as 63 bytes each instead of 64.
            for (int connectionID = 0; connectionID < 63; connectionID++)
            {
                retVal[connectionID].X = (xLocationsTable[connectionID] & 0x3F);
                retVal[connectionID].Y = (yLocationsTable[connectionID] & 0x7F);
                retVal[connectionID].Y -= 30;       // Dunno why the Y-values in the ROM are offset by 30.
            }

            // Randomizer may choose to hide or not hide New Kasuto and/or whatever palace is assigned at 3-eye rock.
            // The way this works is that the conncection location's Y value is placed off the map and the terrain drawing
            // info has a normal looking tile.
            // There's some hard-coded compare values in ROM bank 2 and ROM bank 7 (they call between each other).  If you
            // happen to be chopping the hidden town forest tile or blowing flute at center of 3-eye rock, then the code
            // gets through these comparisons and reassigns the connection's Y value based on more values hard-coded in
            // ROM bank 7.  The randomizer will adjust all of these values properly, but we only care about two things:
            //   * Is the Y value 0?  If so, then this a hidden town/palace.
            //   * What is the appropriate Y value once unhidden?
            if(overworldArea == OverworldArea.EasternHyrule)
            {
                // Compare to -30 because the value in the ROM is 0 for hidden connections and we've already subtracted 30.
                if(retVal[49].Y == -30)
                {
                    _newKasutoIsHidden = true;
                    Byte newYValue = _romInfo.ReadByteFromROMBank(7, 0xDF69 - ROMBankLoadAddresses[7]);
                    retVal[49].Y = (newYValue & 0x7F) - 30;
                }
                if (retVal[53].Y == -30)
                {
                    _threeEyeRockPalaceIsHidden = true;
                    Byte newYValue = _romInfo.ReadByteFromROMBank(7, 0xDF68 - ROMBankLoadAddresses[7]);
                    retVal[53].Y = (newYValue & 0x7F) - 30;
                }
            }
            return retVal;
        }

        public CollectibleItem GetItemAtConnectionID(OverworldArea overworldArea, int connectionID)
        {
            ItemLookupHelper itemLookupInfo;
            switch(overworldArea)
            {
                case OverworldArea.WesternHyrule:
                    if(connectionID < ItemLookupTableForWesternHyrule.Length)
                    {
                        itemLookupInfo = ItemLookupTableForWesternHyrule[connectionID];
                    } else
                    {
                        return CollectibleItem.Invalid;
                    }
                    break;

                case OverworldArea.DeathMountain:
                case OverworldArea.MazeIsland:
                    if (connectionID < ItemLookupTableForDeathMountainAndMazeIsland.Length)
                    {
                        itemLookupInfo = ItemLookupTableForDeathMountainAndMazeIsland[connectionID];
                    }
                    else
                    {
                        return CollectibleItem.Invalid;
                    }
                    break;

                case OverworldArea.EasternHyrule:
                    if (connectionID < ItemLookupTableForEasternHyrule.Length)
                    {
                        itemLookupInfo = ItemLookupTableForEasternHyrule[connectionID];
                    }
                    else
                    {
                        return CollectibleItem.Invalid;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if(itemLookupInfo.LookupRequired)
            {
                return LookupItemAndCountHeartContainers(new AbsoluteROMAddress(itemLookupInfo));
            } else
            {
                return itemLookupInfo.ItemTypeIfNoLookup;
            }
        }

        public Town GetTownAtConnectionID(OverworldArea overworldArea, int connectionID)
        {
            switch(overworldArea)
            {
                case OverworldArea.WesternHyrule:
                    switch(connectionID)
                    {
                        case 0:
                            return Town.NorthPalace;
                        case 45:
                            return Town.Rauru;
                        case 47:
                            return Town.Ruto;
                        case 48:
                            return Town.Saria_L;
                        case 49:
                            return Town.Saria;
                        case 51:
                            return Town.Mido;
                    }
                    break;

                case OverworldArea.EasternHyrule:
                    switch(connectionID)
                    {
                        case 45:
                            return Town.Nabooru;
                        case 47:
                            return Town.Darunia;
                        case 49:
                            return Town.NewKasuto;
                        case 51:
                            return Town.OldKasuto;
                    }
                    break;
            }
            return Town.Invalid;
        }

        public int GetPalaceAtConnectionID(OverworldArea overworldArea, int connectionID)
        {
            AbsoluteROMAddress destinationRegionTableAddress = ConnectionRegionDestinationsTableROMPointers[(int)overworldArea];
            AbsoluteROMAddress destinationRoomTableAddress = ConnectionRoomDestinationsTableROMPointers[(int)overworldArea];

            Byte[] destinationRegionTable = GetBytesAtAbsoluteROMAddress(destinationRegionTableAddress, 63);
            Byte[] destinationRoomTable = GetBytesAtAbsoluteROMAddress(destinationRoomTableAddress, 63);

            // Format stored in ROM table:
            // +---+---+---+---+---+---+---+---+
            // | ? Flags ? |  SS Area  | OverW |
            // +---+---+---+---+---+---+---+---+
            int destinationRegion = (destinationRegionTable[connectionID] >> 2) & 0x07;
            // +---+---+---+---+---+---+---+---+
            // |Scrn # |       Room Number     |
            // +---+---+---+---+---+---+---+---+
            int destinationRoom = destinationRoomTable[connectionID] & 0x3F;

            switch ((SideScrollArea)destinationRegion)
            {
                case SideScrollArea.Palaces_1_2_and_5:
                    if (destinationRoom <= 13)
                        return 1;       // Should only be entering room 0
                    else if (destinationRoom <= 34)
                        return 2;       // Should only be entering room 14
                    else
                        return 5;       // Should only be entering room 35

                case SideScrollArea.Palaces_3_4_and_6:
                    if (destinationRoom <= 14)
                        return 3;       // Should only be entering room 0
                    else if (destinationRoom <= 35)
                        return 4;       // Should only be entering room 15
                    else
                        return 6;       // Should only be entering room 36

                case SideScrollArea.Great_Palace:
                    return 7;           // Should only be entering room 0
            }

            // This connection does not enter a palace
            return -1;
        }

        public CollectibleItem GetNewKasutoItem(NewKasutoItemArea itemArea)
        {
            AbsoluteROMAddress romAddress = ItemLookupTableForNewKasuto[(int)itemArea];
            return LookupItemAndCountHeartContainers(romAddress);
        }

        public CollectibleItem GetItemInPalace(int palaceNumber)
        {
            if(palaceNumber >= 1 && palaceNumber <= 6)
            {
                AbsoluteROMAddress romAddress = ItemLookupTableForPalaces[palaceNumber - 1];
                return LookupItemAndCountHeartContainers(romAddress);
            }
            else
            {
                return CollectibleItem.Invalid;
            }
        }

        public Spell GetSpellAssignedToTown(Town town)
        {
            AbsoluteROMAddress romAddress = new AbsoluteROMAddress(SpellMasksTableBase);
            romAddress.CPUAddress += (int)town;
            byte spellMask = GetByteAtAbsoluteROMAddress(romAddress);

            // Check Fire flag first, in case Fire is combined with another spell
            if(0 != (spellMask & 0x10))
            {
                return Spell.Fire;
            }

            int spellEnumIndex = PositionOfSetBit(spellMask);
            if (spellEnumIndex < 0)
            {
                return Spell.Invalid;   // Shouldn't get here
            }
            else
            {
                return (Spell)spellEnumIndex;
            }
        }

        public Spell GetSpellCombinedWithFire()
        {
            AbsoluteROMAddress romAddress = new AbsoluteROMAddress(SpellMasksTableBase);

            for(int spellLookupIndex = 0; spellLookupIndex < 8; spellLookupIndex++)
            {
                byte spellMask = GetByteAtAbsoluteROMAddress(romAddress);
                if (0 != (spellMask & 0x10))
                {
                    spellMask &= 0xEF;      // Clear the "cast fire" flag
                    int spellEnumIndex = PositionOfSetBit(spellMask);
                    if (spellEnumIndex >= 0)
                    {
                        return (Spell)spellEnumIndex;
                    }
                }
                romAddress.CPUAddress++;
            }
            return Spell.Invalid;
        }

        public bool CheckIsConnectionHidden(OverworldArea overworldArea, int connectionID)
        {
            if(overworldArea == OverworldArea.EasternHyrule)
            {
                if(connectionID == 49)
                {
                    return _newKasutoIsHidden;
                } else if(connectionID == 53)
                {
                    return _threeEyeRockPalaceIsHidden;
                }
            }
            return false;
        }

        public bool CheckIsNewKasutoHidden()
        {
            return _newKasutoIsHidden;
        }

        public StartingStats GetStaringStats()
        {
            StartingStats startingStats = new StartingStats();

            startingStats.numLives = GetByteAtAbsoluteROMAddress(StartingStatsROMPointers[(int)StartingStatsIndex.NumLives]);
            startingStats.numHeartContainers =
                GetByteAtAbsoluteROMAddress(StartingStatsROMPointers[(int)StartingStatsIndex.NumHeartContainers]);
            startingStats.numGems = GetByteAtAbsoluteROMAddress(StartingStatsROMPointers[(int)StartingStatsIndex.NumGems]);

            Byte[] startingSpells = GetBytesAtAbsoluteROMAddress(
                StartingStatsROMPointers[(int)StartingStatsIndex.StartingSpellsBaseAddress], 8);
            startingStats.startingSpells = new Spell[NumNonZeroBytes(startingSpells)];
            int startingSpellsIndex = 0;
            for(int i = 0; i < 8; i++)
            {
                if(startingSpells[i] != 0)
                {
                    startingStats.startingSpells[startingSpellsIndex] = GetSpellAssignedToTown((Town)i);
                    startingSpellsIndex++;
                }
            }

            Byte[] startingItems = GetBytesAtAbsoluteROMAddress(
                StartingStatsROMPointers[(int)StartingStatsIndex.StartingItemsBaseAddress], 8);
            startingStats.startingItems = new CollectibleItem[NumNonZeroBytes(startingItems)];
            int startingItemsIndex = 0;
            for (int i = 0; i < 8; i++)
            {
                if (startingItems[i] != 0)
                {
                    startingStats.startingItems[startingItemsIndex] = (CollectibleItem)i;
                    startingItemsIndex++;
                }
            }

            byte startingTechs = GetByteAtAbsoluteROMAddress(
                StartingStatsROMPointers[(int)StartingStatsIndex.StartingSwordTechniques]);
            if ((startingTechs & downthrustEnabledFlag) == 0)
                startingStats.downthrustEnabled = false;
            else
                startingStats.downthrustEnabled = true;
            if ((startingTechs & upthrustEnabledFlag) == 0)
                startingStats.upthrustEnabled = false;
            else
                startingStats.upthrustEnabled = true;

            return startingStats;
        }

        public int GetNumberOfExtraHeartContainers()
        {
            return _extraHeartContainers;
        }

        public byte[][] GetSpellCastingCosts()
        {
            AbsoluteROMAddress spellCostsROMPointer = new AbsoluteROMAddress(SpellCastingCostsTableBase);

            byte[][] retVal = new byte[8][];

            for (int i = 0; i < 8; i++)
            {
                retVal[i] = GetBytesAtAbsoluteROMAddress(spellCostsROMPointer, 8);
                spellCostsROMPointer.CPUAddress += 8;
            }

            return retVal;
        }

        public byte[] GetAttackStrengths()
        {
            return GetBytesAtAbsoluteROMAddress(AttackStrengthTableBase, 8);
        }

        // Amount of damage Link takes when hit
        // Indexed by [EnemiesOffensiveStrength][LinksLifeLevel]
        public byte[][] GetDamageTable()
        {
            AbsoluteROMAddress damageTableROMPointer = new AbsoluteROMAddress(DamageTableBase);
            byte[][] retVal = new byte[7][];

            for(int offensiveStrength = 0; offensiveStrength < 7; offensiveStrength++)
            {
                retVal[offensiveStrength] = GetBytesAtAbsoluteROMAddress(damageTableROMPointer, 8);
                damageTableROMPointer.CPUAddress += 8;
            }

            return retVal;
        }

        public RoutingSolution[] DoPalacePathFindOperation(int palaceNumber, PalaceRouteType routeType)
        {
            PalaceRoutingInfo routingInfo = PalaceRoutingInfoTable[palaceNumber - 1][(int)routeType];
            RoutingSolution[] retVal = null;

            if(routingInfo != null)
            {
                retVal = PalaceRoomConnectionMaps[routingInfo.connectionMapToUse].FindRoutes(
                    routingInfo.startingRoomIndex, routingInfo.endingRoomIndex);
            }
            return retVal;
        }

        public Bitmap GetTerrainTypeBitmap(TerrainType terrainType)
        {
            return OverworldBackgroundTileBitmaps[(int)terrainType];
        }

        public Bitmap GetCollectibleItemBitmap(CollectibleItem item)
        {
            if((int)item < CollectibleItemBitmaps.Length)
            {
                return CollectibleItemBitmaps[(int)item];
            } else
            {
                return Properties.Resources.Blank_Square;
            }
        }

        private void LoadPalaceRoomConnectionTables()
        {
            for(int i = 0; i < PalaceRoomConnectionMaps.Length; i++)
            {
                AbsoluteROMAddress connectionTableAddress = PalaceRoomConnectionTableROMPointers[i];
                int romBankNumber = connectionTableAddress.RomBankNumber;
                int romBankOffset = connectionTableAddress.CPUAddress - ROMBankLoadAddresses[romBankNumber];
                PalaceRoomConnectionMaps[i].LoadConnectionTableFromROM(_romInfo, romBankNumber, romBankOffset);
            }
        }

        private Bitmap GenerateOverworldTerrainTileBitmap(TerrainType terrainType)
        {
            Bitmap retVal = _ppuBitmapGenerator.GetBackgroundTile(
                8,          // CHR bank 8
                true,       // true for right hand pattern table
                OverworldTerrainTypePatternTableIndices[(int)terrainType],
                OverworldBackgroundPalettes[OverworldTerrainTypePaletteIndices[(int)terrainType]]
            );

            return retVal;
        }

        private Bitmap GenerateCollectibleItemBitmap(CollectibleItem item)
        {
            if ((int)item >= CollectibleItemDrawingInfoTable.Length)
                throw new ArgumentOutOfRangeException("No such image");

            CollectibleItemDrawingInfo itemDrawingInfo = CollectibleItemDrawingInfoTable[(int)item];
            Bitmap spriteHalf = _ppuBitmapGenerator.Get8x16Sprite(
                itemDrawingInfo.ChrBankNum,
                itemDrawingInfo.IsRightPatternTable,
                itemDrawingInfo.PatternTableIndex,
                SpritePalettes[itemDrawingInfo.PaletteIndex]
            );

            // Sprites don't have black in their color palettes.  They have transparent instead.
            // This is a problem for the child sprite, which has black pixels in the face.
            // The child sprite "cheats" and uses color index 0 as both black and transparent
            // counting on the child being placed against a black background.
            // This is a problem if we end up overlaying the child sprite on an overworld bitmap.
            // We "correct" the child sprite by converting a few pixels in his face from
            // transparent to black.
            if (item == CollectibleItem.Child)
            {
                Color transparentColor = Color.FromArgb(0, 0, 0, 0);
                for (int x = 4; x <= 7; x++)
                {
                    for (int y = 3; y <= 7; y++)
                    {
                        // Since the image is already scaled, we have to account for that here.
                        int xPixelOffset = x * 2;
                        int yPixelOffset = y * 2;
                        Color thisPixelColor = spriteHalf.GetPixel(xPixelOffset, yPixelOffset);
                        if (transparentColor.Equals(thisPixelColor))
                        {
                            spriteHalf.SetPixel(xPixelOffset, yPixelOffset, Color.Black);
                            spriteHalf.SetPixel(xPixelOffset + 1, yPixelOffset, Color.Black);
                            spriteHalf.SetPixel(xPixelOffset, yPixelOffset + 1, Color.Black);
                            spriteHalf.SetPixel(xPixelOffset + 1, yPixelOffset + 1, Color.Black);
                        }
                    }
                }
            }

            Bitmap retVal = new Bitmap(32, 32);
            using (Graphics retValGfx = Graphics.FromImage(retVal))
            {
                retValGfx.Clear(Color.FromArgb(0, 0, 0, 0));    // Make transparent to start
                if(itemDrawingInfo.ExtendAndFlip)
                {
                    retValGfx.DrawImage(spriteHalf, 0, 0);
                    spriteHalf.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    retValGfx.DrawImage(spriteHalf, 16, 0);
                } else
                {
                    retValGfx.DrawImage(spriteHalf, 8, 0);      // Place in middle of tile
                }
            }

            return retVal;
        }

        private CollectibleItem LookupItemAndCountHeartContainers(AbsoluteROMAddress romAddress)
        {
            CollectibleItem item = (CollectibleItem)GetByteAtAbsoluteROMAddress(romAddress);
            if (item == CollectibleItem.HeartContainer)
                _extraHeartContainers++;
            return item;
        }

        private byte GetByteAtAbsoluteROMAddress(AbsoluteROMAddress romAddress)
        {
            return _romInfo.ReadByteFromROMBank(romAddress.RomBankNumber, 
                romAddress.CPUAddress - ROMBankLoadAddresses[romAddress.RomBankNumber]);
        }

        private UInt16 GetUInt16AtAbsoluteROMAddress(AbsoluteROMAddress romAddress)
        {
            return _romInfo.ReadUInt16FromROMBank(romAddress.RomBankNumber,
                romAddress.CPUAddress - ROMBankLoadAddresses[romAddress.RomBankNumber]);
        }

        private byte[] GetBytesAtAbsoluteROMAddress(AbsoluteROMAddress romAddress, int count)
        {
            return _romInfo.ReadBytesFromROMBank(romAddress.RomBankNumber,
                romAddress.CPUAddress - ROMBankLoadAddresses[romAddress.RomBankNumber], count);
        }

        private static int PositionOfSetBit(byte value)
        {
            int nonZeroCount = 0;
            while (value != 0)
            {
                value >>= 1;
                nonZeroCount++;
            }

            return (nonZeroCount - 1);
        }

        private static int NumNonZeroBytes(byte[] array)
        {
            int count = 0;
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
                    count++;
            }
            return count;
        }
    }
}
