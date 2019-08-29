using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2R_Mapper.Palace_Routing
{
    public class AnalyzerSettings
    {
        public PalaceRouteType routeType;
        public bool includeP1;
        public bool includeP2;
        public bool includeP3;
        public bool includeP4;
        public bool includeP5;
        public bool includeP6;
        public bool includeGP;
    }

    public enum ReportType
    {
        WhichWayToGo,
        CorrectPathEntering,
        CorrectPathExiting,
        RouteRequirements,
    }

    public enum RoomsToInclude
    {
        Decision,
        Passthrough,
        All,
    }

    class RoutingAnalytics
    {
        // Room descriptions must not contain commas, or it will
        // screw with CSV output format.
        public static string[,] RoomDescriptions = new string[3, 63]
            {{
                // Palace 1
                "Entrance",                         // 0
                "Passthru Down-Right",              // 1
                "Right-hand Dead End",              // 2
                "Left-hand Dead End",               // 3
                "Elevator Bottom",                  // 4
                "Passthru with Windows",            // 5
                "Elevator Top",                     // 6
                "Elevator Middle",                  // 7
                "Item Room",                        // 8
                "Crumble Bridge",                   // 9
                "Passthru Left-Up",                 // 10
                "Passthru Up-Right",                // 11
                "Passthru with Windows",            // 12
                "Boss Room",                        // 13
                // Palace 2
                "Entrance",                         // 14
                "Passthru Down-Right",              // 15
                "Passthru High Ceiling",            // 16
                "Bot Lava Room",                    // 17
                "Passthru with Windows",            // 18
                "Elevator Middle with Drippers",    // 19
                "Item Room",                        // 20
                "Block Room",                       // 21
                "Elevator Middle no Drippers",      // 22
                "Left-hand Dead End",               // 23
                "Elevator Middle no Drippers",      // 24
                "Passthru Up-Right with Moa",       // 25
                "Passthru (Glove Req'd)",           // 26
                "Elevator Top",                     // 27
                "Right-hand Dead End",              // 28
                "Passthru Up-Right",                // 29
                "Right-hand Dead End",              // 30
                "Passthru Up-Right",                // 31
                "Jump Req'd Going Left",            // 32
                "Passthru with Windows",            // 33
                "Boss Room",                        // 34
                // Palace 5
                "Entrance",                         // 35
                "Passthru Up-Right",                // 36
                "Eco-Key Room",                     // 37
                "Crumble Bridge",                   // 38
                "Passthru Left-Down",               // 39
                "Passthru Down-Right",              // 40
                "Boss Room",                        // 41
                "Passthru Down-Right",              // 42
                "Block Room",                       // 43
                "Elevator Bottom with Columns",     // 44
                "Right-hand Dead End",              // 45
                "Elevator Middle with Jar",         // 46
                "Elevator Top with Item",           // 47
                "Elevator Bottom 3 Statues",        // 48
                "Block Lava Room",                  // 49
                "Double Dead End (Swag Key)",       // 50
                "Passthru Left-Down",               // 51
                "Left-hand Dead End",               // 52
                "Passthru Left-Up",                 // 53
                "Elevator Middle no Jar",           // 54
                "Passthru High Ceiling",            // 55
                "Passthru False Wall",              // 56
                "Elevator Top no Item",             // 57
                "Passthru Left-Up",                 // 58
                "Left-hand Dead End",               // 59
                "Passthru Left-Up",                 // 60
                "Item Room",                        // 61
                "Passthru Left-Up",                 // 62
            },
            {
                // Palace 3
                "Entrance",                         // 0
                "Passthru Up-Right",                // 1
                "Passthru Block Floor",             // 2
                "Passthru Falling Blocks",          // 3
                "Passthru Block Floor",             // 4
                "Elevator Top with Blocks",         // 5
                "Right-hand Dead End",              // 6
                "Left-hand Dead End",               // 7
                "Elevator Top Normal",              // 8
                "Elevator Bottom",                  // 9
                "Bot Lava Room (Glove Req'd)",      // 10
                "Item Room",                        // 11
                "Passthru Up-Right",                // 12
                "Passthru (Glove Req'd)",           // 13
                "Boss Room",                        // 14
                // Palace 4
                "Entrance",                         // 15
                "Left-hand Dead End",               // 16
                "Elevator Top",                     // 17
                "Elevator Bottom (door on left)",   // 18
                "Passthru Left-Down",               // 19
                "Passthru Down-Right (crumble)",    // 20
                "Elevator Bottom (door on left)",   // 21
                "Dead End Lava Room",               // 22
                "Elevator Middle",                  // 23
                "Top of Pit",                       // 24
                "Dead End Lava Room",               // 25
                "Passthru Up-Right",                // 26
                "Passthru Left-Right",              // 27
                "Boss Room",                        // 28
                "Passthru Up-Down",                 // 29
                "Middle of Pit",                    // 30
                "Item Room",                        // 31
                "Left-hand Dead End",               // 32
                "Elevator Bottom (door on right)",  // 33
                "Bottom of Pit (crumble bridge)",   // 34
                "Dead End Lava Room",               // 35
                // Palace 6
                "Entrance",                         // 36
                "Left-hand Dead End",               // 37
                "Elevator Middle",                  // 38
                "Passthru Up-Right",                // 39
                "Endless Pit (4-way Room)",         // 40
                "Passthru with shooters",           // 41
                "Elevator Top with Blocks",         // 42
                "Canadian Hole",                    // 43
                "Item Room",                        // 44
                "Fallthru Passthru",                // 45
                "Passthrough Up-Right",             // 46
                "Pit Bottom Forced Left",           // 47
                "Endless Pit (right or down)",      // 48
                "Glove Lava Room",                  // 49
                "Passthru Left-Right",              // 50
                "Passthru (Fairy Req'd)",           // 51
                "Elevator Top (normal)",            // 52
                "Passthru Rebonack",                // 53
                "Passthru Left-Drop",               // 54
                "Fallthru Passthru",                // 55
                "Passthru Up-Down",                 // 56
                "Drop or Right (Fairy Req'd)",      // 57
                "Boss Room",                        // 58
                "Left-hand Dead End",               // 59
                "Elevator Bottom",                  // 60
                "Passthru Left-Right",              // 61
                "Pit Bottom Forced Left",           // 62
            },
            {
                // Great Palace
                "Entrance",                             // 0
                "Paperclip Room",                       // 1
                "Elevator Bottom (1 enemy on left)",    // 2
                "Lava Bridge Room",                     // 3
                "Right-hand Dead End",                  // 4
                "Paperclip Room",                       // 5
                "Elevator Bottom (no enemies)",         // 6
                "Lava Bridge Room",                     // 7
                "Passthru Left-Down (no ceiling)",      // 8
                "Waffle Room",                          // 9
                "Jackimus Room",                        // 10
                "Drop Room (enter from left)",          // 11
                "Waffle Room",                          // 12
                "Block Column Room",                    // 13
                "Paperclip Room",                       // 14
                "Fairy Req'd to go left (crumble)",     // 15
                "Passthru Up-Down (jar/enemy)",         // 16
                "Passthru Up-Down (jar/enemy)",         // 17
                "Passthru Up-Down (bubble)",            // 18
                "Passthru Up-Right (curtains)",         // 19
                "Passthru Left-Down (no ceiling)",      // 20
                "Paperclip Room",                       // 21
                "Elevator Bottom (enemy either side)",  // 22
                "Passthru Left-Down (no ceiling)",      // 23
                "Passthru Up-Down (fairy)",             // 24
                "Waffle Room",                          // 25
                "Passthru Left-Down (crumble)",         // 26
                "Waffle Room",                          // 27
                "Passthru Left-Down (crumble)",         // 28
                "Passthru Up-Right (curtains)",         // 29
                "Passthru Left-Down (crumble)",         // 30
                "Passthru Up-Right (curtains)",         // 31
                "Block Column Room",                    // 32
                "Passthru Up-Down (jar/enemy)",         // 33
                "Passthru Up-Down (jar/enemy)",         // 34
                "Waffle Room",                          // 35
                "Trailz Room",                          // 36
                "Lava Bridge Room",                     // 37
                "Lava Bridge Room",                     // 38
                "Passthru Left-Down (crumble)",         // 39
                "Passthru Up-Right (curtains)",         // 40
                "Drop from Left-Dead End from Right",   // 41
                "Fairy Room",                           // 42
                "Waffle Room",                          // 43
                "Passthru Left-Right (Fairy Req'd)",    // 44
                "L-7 Room",                             // 45
                "Elevator Bottom (2 enemies on left)",  // 46
                "Right-hand Dead End",                  // 47
                "Drop Into Forced Right (no jars)",     // 48
                "EON Drop",                             // 49
                "Lava Bridge Room",                     // 50
                "Right-hand Dead End",                  // 51
                "Drop Into Forced Right (2 jars)",      // 52
                "Thunderbird",                          // 53
                "Dark Link",                            // 54
                "Unused",
                "Unused",
                "Unused",
                "Unused",
                "Unused",
                "Unused",
                "Unused",
                "Unused",
            }
        };

        private readonly bool[,] IsDecisionRoom = new bool[3, 63]
            {  // Palaces 1, 2, and 5
               {false, false, false, false, true,  false, true,  true,      // Rooms 0 - 7
                false, false, false, false, false, false, false, false,     // Rooms 8 - 15
                false, false, false, true,  false, false, true,  false,     // Rooms 16 - 23
                true,  false, false, true,  false, false, false, false,     // Rooms 24 - 31
                false, false, false, false, false, false, false, false,     // Rooms 32 - 39
                false, false, false, false, true,  false, true,  true,      // Rooms 40 - 47
                true,  false, false, false, false, false, true,  false,     // Rooms 48 - 55
                false, true,  false, false, false, false, false },          // Rooms 56 - 62
               // Palaces 3, 4, and 6
               {false, false, false, false, false, true,  false, false,     // Rooms 0 - 7
                true,  true,  false, false, false, false, false, false,     // Rooms 8 - 15
                false, true,  true,  false, false, true,  false, true,      // Rooms 16 - 23
                true,  false, false, false, false, false, true,  false,     // Rooms 24 - 31
                false, true,  true,  false, false, false, true,  false,     // Rooms 32 - 39
                true,  false, true,  true,  false, false, false, false,     // Rooms 40 - 47
                true,  false, false, false, true,  false, false, false,     // Rooms 48 - 55
                false, true,  false, false, true,  false, false },          // Rooms 56 - 62
               // Great Palace
               {false, false, true,  false, false, false, true,  false,     // Rooms 0 - 7
                false, false, true,  false, false, false, false, false,     // Rooms 8 - 15
                false, false, false, false, false, false, true,  false,     // Rooms 16 - 23
                false, false, false, false, false, false, false, false,     // Rooms 24 - 31
                false, false, false, false, true,  false, false, false,     // Rooms 32 - 39
                false, false, true,  false, false, false, true,  false,     // Rooms 40 - 47
                false, true,  false, false, false, false, false, false,     // Rooms 48 - 55
                false, false, false, false, false, false, false }};         // Rooms 56 - 62

        private readonly bool[,] IsPassthroughRoom = new bool[3, 63]
            {  // Palaces 1, 2, and 5
               {true,  true,  false, false, false, true,  false, false,     // Rooms 0 - 7
                false, true,  true,  true,  true,  false, true,  true,      // Rooms 8 - 15
                true,  true,  true,  false, false, true,  false, false,     // Rooms 16 - 23
                false, true,  true,  false, false, true,  false, true,      // Rooms 24 - 31
                true,  true,  false, true,  true,  true,  true,  true,      // Rooms 32 - 39
                true,  false, true,  true,  false, false, false, false,     // Rooms 40 - 47
                false, true,  false, true,  false, true,  false, true,      // Rooms 48 - 55
                true,  false, true,  false, true,  false, true  },          // Rooms 56 - 62
               // Palaces 3, 4, and 6
               {true,  true,  true,  true,  true,  false, false, false,     // Rooms 0 - 7
                false, false, true,  false, true,  true,  false, true,      // Rooms 8 - 15
                false, false, false, true,  true,  false, false, false,     // Rooms 16 - 23
                false, false, true,  true,  false, true,  false, false,     // Rooms 24 - 31
                false, false, false, false, true,  false, false, true,      // Rooms 32 - 39
                false, true,  false, false, false, true,  true,  true,      // Rooms 40 - 47
                false, true,  true,  true,  false, true,  true,  true,      // Rooms 48 - 55
                true,  false, false, false, false, true,  true  },          // Rooms 56 - 62
               // Great Palace
               {true,  true,  false, true,  false, true,  false, true,      // Rooms 0 - 7
                true,  true,  false, true,  true,  true,  true,  true,      // Rooms 8 - 15
                true,  true,  true,  true,  true,  true,  false, true,      // Rooms 16 - 23
                true,  true,  true,  true,  true,  true,  true,  true,      // Rooms 24 - 31
                true,  true,  true,  true,  false, true,  true,  true,      // Rooms 32 - 39
                true,  true,  false, true,  true,  true,  false, false,     // Rooms 40 - 47
                true,  false, true,  false, true,  true,  false, false,     // Rooms 48 - 55
                false, false, false, false, false, false, false }};         // Rooms 56 - 62


        // Connection Map 0: Palaces 1, 2, and 5
        // Connection Map 1: Palaces 3, 4, and 6
        // Connection Map 2: Great Palace
        private readonly int[] PalaceNumberToConnectionMapIndex = new int[8] { -1, 0, 0, 1, 1, 0, 1, 2 };

        private class PalaceRoomDefinition
        {
            public int connectionMap;
            public int firstIndex;
            public int lastIndex;
        }

        private readonly PalaceRoomDefinition[] PalaceRoomDefinitions = new PalaceRoomDefinition[8]
        {
            new PalaceRoomDefinition { connectionMap = 0, firstIndex = 0,  lastIndex = 0 },
            new PalaceRoomDefinition { connectionMap = 0, firstIndex = 0,  lastIndex = 13 },    // Palace 1
            new PalaceRoomDefinition { connectionMap = 0, firstIndex = 14, lastIndex = 34 },    // Palace 2
            new PalaceRoomDefinition { connectionMap = 1, firstIndex = 0,  lastIndex = 14 },    // Palace 3
            new PalaceRoomDefinition { connectionMap = 1, firstIndex = 15, lastIndex = 35 },    // Palace 4
            new PalaceRoomDefinition { connectionMap = 0, firstIndex = 35, lastIndex = 62 },    // Palace 5
            new PalaceRoomDefinition { connectionMap = 1, firstIndex = 36, lastIndex = 62 },    // Palace 6
            new PalaceRoomDefinition { connectionMap = 2, firstIndex = 0,  lastIndex = 54 }     // Great Palace
        };

        // These are the columns to exclude from the final report output
        // since they don't make since to include.
        // 1st index: Previous room exit direction
        // 2nd index: Current room exit direction
        private readonly bool[,] outputColumnExclusionMap = new bool[5, 5]
            {{false, false, false, true,  false },
             {false, false, true,  false, true  },
             {false, true,  false, false, true  },
             {true,  false, false, false, false },
             {false, true,  true,  false, false }};


        private Z2R_Reader _z2rReader;

        private PalaceRouteType _routeType;
        private bool[] _palacesToInclude = new bool[8];

        // 1st index = connection map index
        // 2nd index = room index
        // 3rd index = previous room exit direction
        // 4th index = this room exit direction
        // Value = Number of routes that entered from previous direction and took this exit
        private int[,,,] _routingStatistics;
        // Indexed = palace number
        // Value = the total number of solutions counted for that palace across all ROM files analyzed.
        private int[] _totalSolutionCount;
        // Lock object to allow updating of these fields from multiple threads.
        private readonly object _statFieldsLock = new object();

        public RoutingAnalytics()
        {
            // Default settings
            _routeType = PalaceRouteType.EntranceToBoss;
            _palacesToInclude[0] = false;
            for(int palaceNumber = 1; palaceNumber <= 7; palaceNumber++)
            {
                _palacesToInclude[palaceNumber] = true;
            }
            ResetStatistics();
        }

        public void ChangeSettings(AnalyzerSettings settings)
        {
            _routeType = settings.routeType;
            _palacesToInclude[1] = settings.includeP1;
            _palacesToInclude[2] = settings.includeP2;
            _palacesToInclude[3] = settings.includeP3;
            _palacesToInclude[4] = settings.includeP4;
            _palacesToInclude[5] = settings.includeP5;
            _palacesToInclude[6] = settings.includeP6;
            _palacesToInclude[7] = settings.includeGP;
        }

        public void ResetStatistics()
        {
            // Elemenents of new arrays automatically default to 0, so there is
            // no need to explicitly zero them out here.
            lock(_statFieldsLock)
            {
                _routingStatistics = new int[3, 63, 5, 5];
                _totalSolutionCount = new int[8];
            }
        }

        public void AnalyzeNextROMFile(string fileName)
        {
            _z2rReader = new Z2R_Reader(fileName, false);
            for (int palaceNumber = 1; palaceNumber <= 7; palaceNumber++)
            {
                if (_palacesToInclude[palaceNumber])
                {
                    int connectionMapIndex = PalaceNumberToConnectionMapIndex[palaceNumber];

                    RoutingSolution[] solutionSet = _z2rReader.DoPalacePathFindOperation(palaceNumber, _routeType);

                    foreach (RoutingSolution solution in solutionSet)
                    {
                        for (int routeIndex = 0; routeIndex < solution.directions.Length; routeIndex++)
                        {
                            int roomIndex = solution.roomIndices[routeIndex];
                            Direction prevRoomExitDirection = (routeIndex == 0) ? Direction.Right : solution.directions[routeIndex - 1];
                            Direction thisRoomExitDirection = solution.directions[routeIndex];
                            lock(_statFieldsLock)
                            {
                                _routingStatistics[connectionMapIndex, roomIndex,
                                    (int)prevRoomExitDirection, (int)thisRoomExitDirection] += 1;
                            }
                        }
                        lock(_statFieldsLock)
                        {
                            _totalSolutionCount[palaceNumber] += 1;
                        }
                    }
                }
            }
        }

        public string GenerateReport(ReportType reportType, RoomsToInclude roomsToInclude)
        {
            StringBuilder csvOutput = new StringBuilder(4096);

            switch(reportType)
            {
                case ReportType.WhichWayToGo:
                    csvOutput.AppendLine(GenerateWhichWayReportHeader());
                    break;
                case ReportType.CorrectPathEntering:
                    csvOutput.AppendLine(GenerateCorrectPathEnteringReportHeader());
                    break;
                case ReportType.CorrectPathExiting:
                    csvOutput.AppendLine(GenerateCorrectPathExitingReportHeader());
                    break;
                case ReportType.RouteRequirements:
                    csvOutput.AppendLine(GenerateRequirementsReportHeader());
                    break;
            }

            for (int palaceNumber = 1; palaceNumber <= 7; palaceNumber++)
            {
                if (!_palacesToInclude[palaceNumber])
                    continue;

                if (reportType != ReportType.RouteRequirements)
                {
                    // Spacer line between each palace
                    csvOutput.AppendLine(GetPalaceName(palaceNumber));

                    int connectionMapIndex = PalaceRoomDefinitions[palaceNumber].connectionMap;
                    int firstRoomIndex = PalaceRoomDefinitions[palaceNumber].firstIndex;
                    int lastRoomIndex = PalaceRoomDefinitions[palaceNumber].lastIndex;

                    for (int roomIndex = firstRoomIndex; roomIndex <= lastRoomIndex; roomIndex++)
                    {
                        bool includeThisRoom = false;
                        switch (roomsToInclude)
                        {
                            case RoomsToInclude.Decision:
                                includeThisRoom = IsDecisionRoom[connectionMapIndex, roomIndex];
                                break;
                            case RoomsToInclude.Passthrough:
                                includeThisRoom = IsPassthroughRoom[connectionMapIndex, roomIndex];
                                break;
                            case RoomsToInclude.All:
                                includeThisRoom = true;
                                break;
                        }
                        if (includeThisRoom)
                        {
                            switch (reportType)
                            {
                                case ReportType.WhichWayToGo:
                                    csvOutput.AppendLine(GenerateLineForWhichWayReport(connectionMapIndex, roomIndex));
                                    break;
                                case ReportType.CorrectPathEntering:
                                    csvOutput.AppendLine(GenerateLineForCorrectPathEnteringReport(palaceNumber, connectionMapIndex, roomIndex));
                                    break;
                                case ReportType.CorrectPathExiting:
                                    csvOutput.AppendLine(GenerateLineForCorrectPathExitingReport(palaceNumber, connectionMapIndex, roomIndex));
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    csvOutput.AppendLine(GenerateLineForRequirementsReport());
                }
            }

            return csvOutput.ToString();
        }

        private string GenerateWhichWayReportHeader()
        {
            StringBuilder outputLine = new StringBuilder();
            outputLine.Append(",From the Right");
            for (int i = 0; i < 3; i++)
            {
                outputLine.Append(",");
            }
            outputLine.Append(",From Above");
            for (int i = 0; i < 2; i++)
            {
                outputLine.Append(",");
            }
            outputLine.Append(",From Below");
            for (int i = 0; i < 2; i++)
            {
                outputLine.Append(",");
            }
            outputLine.Append(",From the Left");
            for (int i = 0; i < 3; i++)
            {
                outputLine.Append(",");
            }
            outputLine.Append(",From Drop");
            for (int i = 0; i < 3; i++)
            {
                outputLine.Append(",");
            }
            outputLine.AppendLine();

            outputLine.Append("Room Index");
            outputLine.Append(",Go Left,Go Down,Go Up,Take Drop");   // From the Right
            outputLine.Append(",Go Left,Go Down,Go Right");          // From Above (elevator)
            outputLine.Append(",Go Left,Go Up,Go Right");            // From Below (elevator)
            outputLine.Append(",Go Down,Go Up,Go Right,Take Drop");  // From the Left
            outputLine.Append(",Go Left,Go Right,Take Drop");        // Drop from Above
            return outputLine.ToString();
        }

        private string GenerateCorrectPathEnteringReportHeader()
        {
            StringBuilder outputLine = new StringBuilder();
            outputLine.Append("Room Index");
            outputLine.Append(",From the Right");
            outputLine.Append(",From Above");
            outputLine.Append(",From Below");
            outputLine.Append(",From the Left");
            outputLine.Append(",Drop from Above");
            return outputLine.ToString();
        }

        private string GenerateCorrectPathExitingReportHeader()
        {
            StringBuilder outputLine = new StringBuilder();
            outputLine.Append("Room Index");
            outputLine.Append(",Exiting Left");
            outputLine.Append(",Exiting Down");
            outputLine.Append(",Exiting Up");
            outputLine.Append(",Exiting Right");
            outputLine.Append(",Taking Drop");
            return outputLine.ToString();
        }

        private string GenerateRequirementsReportHeader()
        {
            StringBuilder outputLine = new StringBuilder();
            outputLine.Append("Palace");
            outputLine.Append(",Avg Locked Doors");
            outputLine.Append(",Glove Required");
            outputLine.Append(",Fairy Required");
            outputLine.Append(",Thunderbird Required");
            return outputLine.ToString();
        }

        private string GenerateLineForWhichWayReport(int connectionMapIndex, int roomIndex)
        {
            StringBuilder outputLine = new StringBuilder();
            outputLine.Append(GetRoomDescription(connectionMapIndex, roomIndex));

            for (Direction prevExitDirection = Direction.Left; prevExitDirection < Direction.Invalid; prevExitDirection++)
            {
                int denominator = 0;
                for (Direction currExitDirection = Direction.Left; currExitDirection < Direction.Invalid; currExitDirection++)
                {
                    denominator += _routingStatistics[connectionMapIndex, roomIndex, (int)prevExitDirection, (int)currExitDirection];
                }
                for (Direction currExitDirection = Direction.Left; currExitDirection < Direction.Invalid; currExitDirection++)
                {
                    if (!outputColumnExclusionMap[(int)prevExitDirection, (int)currExitDirection])
                    {
                        if (denominator == 0)
                        {
                            outputLine.Append(",0.00");
                        }
                        else
                        {
                            outputLine.Append(",");
                            float percentOddsForThisPath = (float)_routingStatistics[connectionMapIndex,
                                roomIndex, (int)prevExitDirection, (int)currExitDirection];
                            percentOddsForThisPath /= (float)denominator;
                            outputLine.Append(percentOddsForThisPath.ToString());
                        }
                    }
                }
            }
            return outputLine.ToString();
        }

        private string GenerateLineForCorrectPathEnteringReport(int palaceNumber, int connectionMapIndex, int roomIndex)
        {
            StringBuilder outputLine = new StringBuilder();
            outputLine.Append(GetRoomDescription(connectionMapIndex, roomIndex));

            for (Direction prevExitDirection = Direction.Left; prevExitDirection < Direction.Invalid; prevExitDirection++)
            {
                int routesThatWentThisWay = 0;
                for (Direction currExitDirection = Direction.Left; currExitDirection < Direction.Invalid; currExitDirection++)
                {
                    routesThatWentThisWay += _routingStatistics[connectionMapIndex, roomIndex, 
                        (int)prevExitDirection, (int)currExitDirection];
                }
                if (routesThatWentThisWay == 0)
                {
                    outputLine.Append(",0.00");
                }
                else
                {
                    outputLine.Append(",");
                    float percentOddsForThisPath = (float)routesThatWentThisWay /
                        (float)_totalSolutionCount[palaceNumber];
                    outputLine.Append(percentOddsForThisPath.ToString());
                }
            }
            return outputLine.ToString();
        }

        private string GenerateLineForCorrectPathExitingReport(int palaceNumber, int connectionMapIndex, int roomIndex)
        {
            StringBuilder outputLine = new StringBuilder();
            outputLine.Append(GetRoomDescription(connectionMapIndex, roomIndex));

            for (Direction currExitDirection = Direction.Left; currExitDirection < Direction.Invalid; currExitDirection++)
            {
                int routesThatWentThisWay = 0;
                for (Direction prevExitDirection = Direction.Left; prevExitDirection < Direction.Invalid; prevExitDirection++)
                {
                    routesThatWentThisWay += _routingStatistics[connectionMapIndex, roomIndex, (int)prevExitDirection, (int)currExitDirection];
                }
                if (routesThatWentThisWay == 0)
                {
                    outputLine.Append(",0.00");
                }
                else
                {
                    outputLine.Append(",");
                    float percentOddsForThisPath = (float)routesThatWentThisWay /
                        (float)_totalSolutionCount[palaceNumber];
                    outputLine.Append(percentOddsForThisPath.ToString());
                }
            }
            return outputLine.ToString();
        }

        private string GenerateLineForRequirementsReport()
        {
            return "";
        }

        private string GetRoomDescription(int connectionMapIndex, int roomIndex)
        {
            StringBuilder outputStr = new StringBuilder();
            outputStr.Append(roomIndex.ToString());
            outputStr.Append(": ");
            outputStr.Append(RoomDescriptions[connectionMapIndex, roomIndex]);
            return outputStr.ToString();
        }

        private string GetPalaceName(int palaceNumber)
        {
            if (palaceNumber < 7)
            {
                return "Palace " + palaceNumber.ToString();
            }
            else
            {
                return "Great Palace";
            }
        }
    }
}
