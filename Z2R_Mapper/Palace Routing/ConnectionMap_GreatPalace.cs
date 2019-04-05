using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2R_Mapper.Palace_Routing
{
    public partial class PalaceRouting
    {
        // Room connection set belonging to Great Palace.
        // Preloaded with mappings for vanilla game.
        // indexOfNextRoom fields will be overwritten when Z2R ROM is read in.
        public static RoomConnectionMap ConnectionMapForGreatPalace = new RoomConnectionMap
        {
            _roomConnections = new RoomConnectionInfo[]
            {
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 0 - Entrance
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 2 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 1
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 6 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 2 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 2
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 1 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 0 },
                    new RoomExit { isValid = true, indexOfNextRoom = 3 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 3
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 2 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 4 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 4
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 3 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 5
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 9 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 6 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 6
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 5 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 1 },
                    new RoomExit { isValid = true, indexOfNextRoom = 7 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 7
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 6 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 8 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 8
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 7 },
                    new RoomExit { isValid = true, indexOfNextRoom = 12 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 9
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 5, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 10, passThroughFlags = new UInt16[] { 0x00, 0x02, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]    // 10
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 9, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 15, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 11, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x02, 0x02 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]    // 11
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 10 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 36, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x02 } },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 12
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 8, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 13, passThroughFlags = new UInt16[] { 0x00, 0x02, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 13
                {
                    // Coming up elevator and going left, glove and upstab are required.  Technically jump/fairy is not
                    // required, although it's much easier to get through with one of those spells.  I am choosing not
                    // to set the "Jump or Fairy required" flag when coming up elevator and going left.
                    new RoomExit { isValid = true, indexOfNextRoom = 12, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x22, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 16, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x12 }  },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 14
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 17 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 15 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 15
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 14, passThroughFlags = new UInt16[] { 0x00, 0x08, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 10 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 16
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 18 },
                    new RoomExit { isValid = true, indexOfNextRoom = 13 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 17
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 19 },
                    new RoomExit { isValid = true, indexOfNextRoom = 14 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 18
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 22 },
                    new RoomExit { isValid = true, indexOfNextRoom = 16 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 19
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 17 },
                    new RoomExit { isValid = true, indexOfNextRoom = 20 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 20
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 19 },
                    new RoomExit { isValid = true, indexOfNextRoom = 24 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 21
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 25 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 22 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 22
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 21 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 18 },
                    new RoomExit { isValid = true, indexOfNextRoom = 23 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 23
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 22 },
                    new RoomExit { isValid = true, indexOfNextRoom = 27 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 24
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 35 },
                    new RoomExit { isValid = true, indexOfNextRoom = 20 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 25
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 21, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 26, passThroughFlags = new UInt16[] { 0x00, 0x02, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 26
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 25 },
                    new RoomExit { isValid = true, indexOfNextRoom = 29 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 27
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 23, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 28, passThroughFlags = new UInt16[] { 0x00, 0x02, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 28
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 27 },
                    new RoomExit { isValid = true, indexOfNextRoom = 31 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 29
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 26 },
                    new RoomExit { isValid = true, indexOfNextRoom = 30 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 30
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 29 },
                    new RoomExit { isValid = true, indexOfNextRoom = 33 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 31
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 28 },
                    new RoomExit { isValid = true, indexOfNextRoom = 32 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 32
                {
                    // See comment for room 13.
                    new RoomExit { isValid = true, indexOfNextRoom = 31, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x22, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 34, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x12 }  },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 33
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 40 },
                    new RoomExit { isValid = true, indexOfNextRoom = 30 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 34
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 42 },
                    new RoomExit { isValid = true, indexOfNextRoom = 32 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 35
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 24, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 36, passThroughFlags = new UInt16[] { 0x00, 0x02, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 36
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 35 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 37 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 37
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 36 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 38 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 38
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 37 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 39 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 39
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 38 },
                    new RoomExit { isValid = true, indexOfNextRoom = 43 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 40
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 33 },
                    new RoomExit { isValid = true, indexOfNextRoom = 41 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 41 - Extra Life Room
                {
                    // Upstab not required with glove only and no fairy spell.
                    new RoomExit { isValid = true, indexOfNextRoom = 40, impassibilityFlags = new bool[] { true, false, false, false } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 45, impassibilityFlags = new bool[] { true, false, false, false },
                        passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x80 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 42, impassibilityFlags = new bool[] { false, false, false, true } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 42 - Fairy Room
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 41 },
                    new RoomExit { isValid = true, indexOfNextRoom = 46 },
                    new RoomExit { isValid = true, indexOfNextRoom = 34 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 43
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 39, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 44, passThroughFlags = new UInt16[] { 0x00, 0x02, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 44
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 43, passThroughFlags = new UInt16[] { 0x08, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 45, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x08 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 45 - L7 Room
                {
                    // Technically, if playing with minor glitches allowed, then there may or may not be
                    // any impassibility restrictions.  We ignore that and just treat it in the vanilla manner:
                    // Drop in from top can exit left only.  Enter from right can drop through bottom only.
                    new RoomExit { isValid = true, indexOfNextRoom = 44, impassibilityFlags = new bool[] { true, false, false, false } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 48, impassibilityFlags = new bool[] { false, true, true, true },
                        passThroughFlags = new UInt16[] { 0x12, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 46, impassibilityFlags = new bool[] { false, true, true, true } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 46
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 45 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 42 },
                    new RoomExit { isValid = true, indexOfNextRoom = 47 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 47
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 46 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 48
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 49 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 49 - EON Drop
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 48 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 52 },
                    new RoomExit { isValid = true, indexOfNextRoom = 50 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 50
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 49 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 51 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 51
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 50 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 52
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 53 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 53 - Thunderbird
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 52, passThroughFlags = new UInt16[] { 0x100, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 54, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x100 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 54 - Dark Link
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 53 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },

                // Everything after this point is garbage.
                // It is here to prevent an ArrayOutOfBounds exception when reading in
                // room connection info from the ROM, which is just a generic read of 63
                // rooms worth of data.
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 55
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 56
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 57
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 58
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 59
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 60
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 61
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 62
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
            }
        };
    }
}
