using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2R_Mapper.Palace_Routing
{
    public partial class PalaceRouting
    {
        // Room connection set belonging to palaces 3, 4, and 6.
        // Preloaded with mappings for vanilla game.
        // indexOfNextRoom fields will be overwritten when Z2R ROM is read in.
        public static RoomConnectionMap ConnectionMapForPalaces_3_4_and_6 = new RoomConnectionMap
        {
            _roomConnections = new RoomConnectionInfo[]
            {
                // Room connections for palace 3
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 0 - Entrance
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 1 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 1
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 0 },
                    new RoomExit { isValid = true, indexOfNextRoom = 2 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 2
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 1 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
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
                    new RoomExit { isValid = true, indexOfNextRoom = 5 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 5
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 4 },
                    new RoomExit { isValid = true, indexOfNextRoom = 9 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 6 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 6
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 5 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 7
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 8 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 8
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 7, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 12, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 9, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x01, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 9
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 8, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 5, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 10, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 10
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 9, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 11, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x02 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 11 - Item room
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 10 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 12
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 8, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 13, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 13
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 12, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 14, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x02 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 14 - Boss
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 13 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },

                // Room connections for palace 4
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 15 - Entrance
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 18 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 16
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 17 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 17
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 16, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 21, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 18, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x01, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 18
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 17, passThroughFlags = new UInt16[] { 0x01, 0x01, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 15, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x01 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 19, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 19
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 18 },
                    new RoomExit { isValid = true, indexOfNextRoom = 23 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 20
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 26 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 21 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 21
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 20, passThroughFlags = new UInt16[] { 0x01, 0x01, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 17, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x01 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 22, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 22
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 21 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 23
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 29 },
                    new RoomExit { isValid = true, indexOfNextRoom = 19 },
                    new RoomExit { isValid = true, indexOfNextRoom = 24 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]    // 24
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 23, passThroughFlags = new UInt16[] { 0x08, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 30 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 25 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 25
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 24 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 26
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 20, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 27, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 27
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 26 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 28 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 28 - Boss
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 27 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 29
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 33 },
                    new RoomExit { isValid = true, indexOfNextRoom = 23 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 30
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 34 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 31 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 31 - Item room
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 30 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 32
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 33 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 33
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 32, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 29, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 34, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 34
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 33 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 35 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 35
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 34 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },

                // Room connections for palace 6
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 36 - Entrance
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 38 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 37
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 38 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 38
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 37 },
                    new RoomExit { isValid = true, indexOfNextRoom = 39 },
                    new RoomExit { isValid = true, indexOfNextRoom = 36 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 39
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 38, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 40, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]    // 40
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 39, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 45, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 41, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 41
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 40 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 42 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 42
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 41 },
                    new RoomExit { isValid = true, indexOfNextRoom = 46 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 43 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]    // 43
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 42, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 47, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 44, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x02 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 44 - Item room
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 43 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 45
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 48 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 46
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 42, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 47, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 47
                {
                    // So, something to know about the randomizer... it doesn't make sure to line up pits so they exit and
                    // enter on the same screen.  Meaning, you can drop down a pit on screen 2 (using the "down exit") and
                    // end in a room that's supposed to be dropped into on screen 3 (such as this one).  What that means is
                    // that you will land on the ceiling if the previous room dropped from screen 2.
                    // So, I'm counting glove required for this room only if dropped into from screen 3 (i.e. the "up exit")
                    // of the previous screen.
                    new RoomExit { isValid = true, indexOfNextRoom = 46, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x02, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 48
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 55 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 49, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 49
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 48, passThroughFlags = new UInt16[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 50, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x02 } },
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
                    new RoomExit { isValid = true, indexOfNextRoom = 50, passThroughFlags = new UInt16[] { 0x08, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 52, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x08 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 52
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 51, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 56, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 53, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x01, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 53
                {
                    // Technically, since this is a mini-boss room, it cannot be entered from the right.
                    // So, we should only have to set the FightRebonack flag for directions of "right, right" and not "left, left".
                    // I set it for both anyway.
                    new RoomExit { isValid = true, indexOfNextRoom = 52, passThroughFlags = new UInt16[] { 0x40, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 54, passThroughFlags = new UInt16[] { 0x00, 0x00, 0x00, 0x40 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 54
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 53 },
                    new RoomExit { isValid = true, indexOfNextRoom = 57 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 55
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 40 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 56
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 60 },
                    new RoomExit { isValid = true, indexOfNextRoom = 52 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = true, roomExits = new RoomExit[]   // 57
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 62 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 58, passThroughFlags = new UInt16[] { 0x00, 0x08, 0x00, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 58 - Boss
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 57 },
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
                    new RoomExit { isValid = true, indexOfNextRoom = 60 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 60
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 59, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 56, passThroughFlags = new UInt16[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 61, passThroughFlags = new UInt16[] { 0x00, 0x01, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 61
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 60 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 62 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 62
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 61 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
            }
        };
    }
}
