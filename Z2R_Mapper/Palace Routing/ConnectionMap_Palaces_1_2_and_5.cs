using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2R_Mapper.Palace_Routing
{
    public partial class PalaceRouting
    {
        // Room connection set belonging to palaces 1, 2, and 5.
        // Preloaded with mappings for vanilla game.
        // indexOfNextRoom fields will be overwritten when Z2R ROM is read in.
        public static RoomConnectionMap ConnectionMapForPalaces_1_2_and_5 = new RoomConnectionMap
        {
            _roomConnections = new RoomConnectionInfo[]
            {
                // Rooms connections for palace 1
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 0 - Entrance
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 4 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 1
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 7 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 2 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 2
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 1 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 3
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 4 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 4
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 3 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 0 },
                    new RoomExit { isValid = true, indexOfNextRoom = 5 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 5
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 4, passThroughFlags = new byte[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 6, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 6
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 5 },
                    new RoomExit { isValid = true, indexOfNextRoom = 10 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 7 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 7
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 6 },
                    new RoomExit { isValid = true, indexOfNextRoom = 11 },
                    new RoomExit { isValid = true, indexOfNextRoom = 1 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 8 - Item Room
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 9 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 9
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 8 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 10 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 10
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 9 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 6 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 11
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 7 },
                    new RoomExit { isValid = true, indexOfNextRoom = 12 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 12
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 11, passThroughFlags = new byte[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 13, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 13 - Boss
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 12 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },

                // Room connections for palace 2
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 14 - Entrance
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 19 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 15
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 22 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 16 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 16
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 15 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 17 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 17
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 16 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 18 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 18
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 17, passThroughFlags = new byte[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 19, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 19
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 18 },
                    new RoomExit { isValid = true, indexOfNextRoom = 24 },
                    new RoomExit { isValid = true, indexOfNextRoom = 14 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 20 - Item Room
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 21 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 21
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 20 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 22 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 22
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 21, passThroughFlags = new byte[] { 0x00, 0x01, 0x01, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 25, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 15, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 23
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 24 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 24
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 23 },
                    new RoomExit { isValid = true, indexOfNextRoom = 29 },
                    new RoomExit { isValid = true, indexOfNextRoom = 19 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 25
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 22 },
                    new RoomExit { isValid = true, indexOfNextRoom = 26 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 26
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 25, passThroughFlags = new byte[] { 0x02, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 27, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x02 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 27
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 26 },
                    new RoomExit { isValid = true, indexOfNextRoom = 31 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 28 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 28
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
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 24 },
                    new RoomExit { isValid = true, indexOfNextRoom = 30 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 30
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 29 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 31
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 27 },
                    new RoomExit { isValid = true, indexOfNextRoom = 32 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 32
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 31, passThroughFlags = new byte[] { 0x04, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 33 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 33
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 32, passThroughFlags = new byte[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 34, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 34 - Boss
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 33 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },

                // Room connections for palace 5
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 35 - Entrance
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 36 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 36
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 35 },
                    new RoomExit { isValid = true, indexOfNextRoom = 37 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 37
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 36 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 38, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x08 } },
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
                    new RoomExit { isValid = true, indexOfNextRoom = 38, passThroughFlags = new byte[] { 0x00, 0x00, 0x01, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 44, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 40
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 46, passThroughFlags = new byte[] { 0x01, 0x00, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 41, passThroughFlags = new byte[] { 0x00, 0x00, 0x01, 0x00 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 41 - Boss
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 40 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 42
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 48 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 43 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 43
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 42 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 44 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 44
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 43 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 39 },
                    new RoomExit { isValid = true, indexOfNextRoom = 45 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 45
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 44 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 46
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 53 },
                    new RoomExit { isValid = true, indexOfNextRoom = 40 },
                    new RoomExit { isValid = true, indexOfNextRoom = 47 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 47
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 46, passThroughFlags = new byte[] { 0x01, 0x00, 0x01, 0x00 } },
                    new RoomExit { isValid = true, indexOfNextRoom = 54, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 48, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 48
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 47 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 42 },
                    new RoomExit { isValid = true, indexOfNextRoom = 49 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 49
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 48 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 50 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 50
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 49, impassibilityFlags = new bool[] { true, false, false, false } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 51, impassibilityFlags = new bool[] { false, false, false, true } },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 51
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 50 },
                    new RoomExit { isValid = true, indexOfNextRoom = 58 },
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
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 53
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 52 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 46 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 54
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 60 },
                    new RoomExit { isValid = true, indexOfNextRoom = 47 },
                    new RoomExit { isValid = true, indexOfNextRoom = 55 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 55
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 54 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 56 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 56 - False wall
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 55 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 57 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 57
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 56 },
                    new RoomExit { isValid = true, indexOfNextRoom = 62 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 58 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 58
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 57 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 51 },
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
                    new RoomExit { isValid = true, indexOfNextRoom = 59 },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 54 },
                    new RoomExit { isValid = false },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 61 - Item room
                {
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 62 },
                }
                },
                new RoomConnectionInfo { pitInsteadOfElevator = false, roomExits = new RoomExit[]   // 62
                {
                    new RoomExit { isValid = true, indexOfNextRoom = 61, passThroughFlags = new byte[] { 0x00, 0x01, 0x00, 0x00 } },
                    new RoomExit { isValid = false },
                    new RoomExit { isValid = true, indexOfNextRoom = 57, passThroughFlags = new byte[] { 0x00, 0x00, 0x00, 0x01 } },
                    new RoomExit { isValid = false },
                }
                },
            }
        };
    }
}
