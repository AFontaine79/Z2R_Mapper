using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2R_Mapper.ROM_Utils;

namespace Z2R_Mapper.Palace_Routing
{
    public enum Direction
    {
        Left = 0,
        Down,
        Up,
        Right,
        DropDownPit,
        Invalid,
    }

    public enum PassthroughFlag
    {
        LockedDoor = 0x01,
        GloveRequired = 0x02,
        JumpOrFairyRequired = 0x04,
        FairyRequired = 0x08,
        DownstabRequired = 0x10,
        UpstabRequired = 0x20,
        FightRebonack = 0x40,
        FightThunderbird = 0x80,
    }

    public struct RoomConnectionInfo
    {
        public RoomExit[] roomExits;
        public bool pitInsteadOfElevator;
    }

    // The impassible flags are only populated by a few rooms.
    // They are used to indicate when it is not possible to reach
    // one room exit from another (e.g. swag key room in P5).
    public struct RoomExit
    {
        public bool isValid;
        public int indexOfNextRoom;
        public bool[] impassibilityFlags;
        public Byte[] passThroughFlags;
    }

    public class RoutingSolution
    {
        public int numLockedDoors;
        public byte requirementFlags;
        public Direction[] directions;

        public RoutingSolution(int numConnections)
        {
            numLockedDoors = 0;
            requirementFlags = 0;
            directions = new Direction[numConnections];
        }
    }

    public class RoomConnectionMap
    {
        public RoomConnectionInfo[] _roomConnections;
        private List<RoutingSolution> _routingSolutionSet;

        public void LoadConnectionTableFromROM(ROM_Info romInfo, int romBankNumber, int romBankOffset)
        {
            byte[] roomConnectionTable = romInfo.ReadBytesFromROMBank(romBankNumber, romBankOffset, 252);

            int roomConnectionTableIndex = 0;
            for(int roomIndex = 0; roomIndex < 63; roomIndex++)
            {
                for(int exitIndex = 0; exitIndex < 4; exitIndex++)
                {
                    // Note that this is reversed from how the bitfields are handled for room
                    // connections when connecting from the overworld.  
                    // See Z2R_Reader::GetPalaceAtConnectionID() for overworld connection info bitfield format.
                    // +---+---+---+---+---+---+---+---+
                    // |       Room Number     |Scrn # |
                    // +---+---+---+---+---+---+---+---+
                    this._roomConnections[roomIndex].roomExits[exitIndex].indexOfNextRoom =
                        (roomConnectionTable[roomConnectionTableIndex] >> 2);
                    roomConnectionTableIndex++;
                }
            }
        }

        public RoutingSolution[] FindRoutes(int startingRoomIndex, int endingRoomIndex)
        {
            _routingSolutionSet = new List<RoutingSolution>();

            int[] roomIndices = new int[64];
            Direction[] directions = new Direction[64];
            PathFind(directions, roomIndices, 0, startingRoomIndex, endingRoomIndex);

            return _routingSolutionSet.ToArray();
        }

        private void PathFind(Direction[] directions, int[] roomIndices, int numConnections, int currentRoomIndex, int endingRoomIndex)
        {
            roomIndices[numConnections] = currentRoomIndex;
            if (currentRoomIndex == endingRoomIndex)
            {
                AddSolution(directions, roomIndices, numConnections);
            } else
            {
                RoomConnectionInfo connections = _roomConnections[currentRoomIndex];
                for (int direction = 0; direction < 4; direction++)
                {
                    bool isImpassible = false;
                    if (numConnections > 0)
                    {
                        RoomExit exitInfo = connections.roomExits[direction];
                        bool[] impassibilityFlags = exitInfo.impassibilityFlags ?? new bool[] { false, false, false, false };
                        isImpassible = impassibilityFlags[(int)directions[numConnections - 1]];
                    }

                    if (connections.roomExits[direction].isValid && !isImpassible)
                    {
                        directions[numConnections] = (Direction)direction;
                        if(!RoomExitAlreadyUsed(directions, roomIndices, numConnections + 1, connections.pitInsteadOfElevator))
                        {
                            PathFind(directions, roomIndices, numConnections + 1, connections.roomExits[direction].indexOfNextRoom, endingRoomIndex);
                        }
                    }
                }
            }
        }

        // The routing algorithm needs to determine if the connection between two rooms has already been
        // used.  It's not enough to see whether we're entering a room we've already been in.  Consider
        // GP's L7 room.  It could wrap to itself and effectively behave as a "2 screen" passthrough from
        // right to left or top to bottom.  At the same time we need the algorithm to terminate on paths
        // that are effectively dead ends because they wrap on themselves.
        // What we need to do is check the exit we're about to take.  If we've taken this exit before OR
        // entered from this direction before, then this connection has already been encountered.
        private bool RoomExitAlreadyUsed(Direction[] exitDirections, int[] roomIndices, int length, bool pitInsteadOfElevator)
        {
            if(length < 2)
            {
                // 2 rooms would be 1 connection, which means no possibility for repeat yet.
                return false;
            }

            // The direction being attempted will be the last one added to the route.
            int currentRoomIndex = roomIndices[length - 1];
            Direction currentExitDirection = exitDirections[length - 1];

            for(int routeIndex = 0; routeIndex < length; routeIndex++)
            {
                // We've encountered the same room somewhere in the route...
                if(roomIndices[routeIndex] == currentRoomIndex)
                {
                    // First, check whether we've already entered from this direction.
                    if((routeIndex > 1) && (InvertDirection(exitDirections[routeIndex - 1]) == currentExitDirection))
                    {
                        if(pitInsteadOfElevator && (currentExitDirection == Direction.Down || currentExitDirection == Direction.Up))
                        {
                            // Vanilla game never misaligns pits between up exit (screen 3) and down exit (screen 2), but
                            // the randomizer will do this.  This has to be treated as a special case here or we'll return
                            // a false positive.
                            return false;
                        } else
                        {
                            // This room has an elevator.  This is valid flag to say this exit goes back the way we came.
                            return true;
                        }
                    }

                    // Second, check whether we've already exited through this direction
                    if((routeIndex < (length - 1)) && (exitDirections[routeIndex] == currentExitDirection))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        private Direction InvertDirection(Direction directionToInvert)
        {
            switch(directionToInvert)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                default:
                    return directionToInvert;
            }
        }

        private void AddSolution(Direction[] directions, int[] roomIndices, int numConnections)
        {
            RoutingSolution thisSolution = new RoutingSolution(numConnections);
            Direction previousDirection = Direction.Invalid;
            Direction currentDirection;

            // Now that we have the list of room exit directions and room indices,
            // we rewalk the list and build a proper RoutingSolution object.
            for(int i = 0; i < numConnections; i++)
            {
                currentDirection = directions[i];
                RoomConnectionInfo connectionInfo = _roomConnections[roomIndices[i]];
                RoomExit exitInfo = connectionInfo.roomExits[(int)currentDirection];

                if(connectionInfo.pitInsteadOfElevator && (currentDirection == Direction.Down || currentDirection == Direction.Up))
                {
                    thisSolution.directions[i] = Direction.DropDownPit;
                } else
                {
                    thisSolution.directions[i] = currentDirection;
                }

                // If passing through room in this direction passes through a locked door,
                // then we increment the locked door count.
                byte passthroughFlags;
                if(previousDirection == Direction.Invalid)
                {
                    passthroughFlags = 0;
                } else
                {
                    byte[] passThroughFlagsList = exitInfo.passThroughFlags ?? new byte[] { 0, 0, 0, 0 };
                    passthroughFlags = passThroughFlagsList[(int)previousDirection];
                }

                if ((passthroughFlags & (byte)PassthroughFlag.LockedDoor) != 0)
                {
                    thisSolution.numLockedDoors++;
                }

                // Now we zero out the locked door flag, and OR in the set of requirements
                // from this room to the total requirements set.
                byte lockedDoorFlag = (byte)PassthroughFlag.LockedDoor;
                passthroughFlags &= (byte)(~lockedDoorFlag);
                thisSolution.requirementFlags |= passthroughFlags;

                previousDirection = currentDirection;
            }

            // Now that we have created the routing solution, there is one more thing to do.
            // If the Fairy Required flag is set, this nulls the Jump or Fairy Required flag.
            if ((thisSolution.requirementFlags & (byte)PassthroughFlag.FairyRequired) != 0)
            {
                byte jumpOrFairyFlag = (byte)PassthroughFlag.JumpOrFairyRequired;
                thisSolution.requirementFlags &= (byte)(~jumpOrFairyFlag);
            }

            _routingSolutionSet.Add(thisSolution);
        }
    }
}
