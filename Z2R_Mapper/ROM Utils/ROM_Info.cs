using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2R_Mapper.ROM_Utils
{
    public class ROM_Info
    {
        private readonly int _numROMBanks;
        private readonly int _numCHRBanks;

        private readonly Byte[][] _romBanks;
        private readonly Byte[][] _chrBanks;

        // "NES" + MS-DOS end-of-file character
        private readonly Byte[] _inesIdentifier = new byte[4] { 0x4E, 0x45, 0x53, 0x1A };

        private const int ROMBankSize = 16384;
        private const int CHRBankSize = 8192;

        public ROM_Info(String inesFilename)
        {
            if (File.Exists(inesFilename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(inesFilename, FileMode.Open)))
                {
                    Byte[] inesHeader = reader.ReadBytes(16);
                    if(!inesHeader.Take(4).SequenceEqual(_inesIdentifier))
                    {
                        throw new InvalidDataException("Not an iNES ROM file");
                    }

                    // Assuming MMC1.  No special logic for other mappers here.
                    _numROMBanks = inesHeader[4];
                    _numCHRBanks = inesHeader[5];

                    // Index first by bank number, then by offset within bank
                    _romBanks = new byte[_numROMBanks][];
                    _chrBanks = new byte[_numCHRBanks][];

                    try
                    {
                        for (int romBankIndex = 0; romBankIndex < _numROMBanks; romBankIndex++)
                        {
                            _romBanks[romBankIndex] = new byte[ROMBankSize];
                            Array.Copy(reader.ReadBytes(ROMBankSize), _romBanks[romBankIndex], ROMBankSize);
                        }
                    }
                    catch (ArgumentException)
                    {
                        throw new InvalidDataException("Invalid iNES file. Reached end-of-file before finished reading ROM banks.");
                    }

                    try
                    {
                        for (int chrBankIndex = 0; chrBankIndex < _numCHRBanks; chrBankIndex++)
                        {
                            _chrBanks[chrBankIndex] = new byte[CHRBankSize];
                            Array.Copy(reader.ReadBytes(CHRBankSize), _chrBanks[chrBankIndex], CHRBankSize);
                        }
                    }
                    catch (ArgumentException)
                    {
                        throw new InvalidDataException("Invalid iNES file. Reached end-of-file before finished reading CHR banks.");
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("File not found", inesFilename);
            }
        }

        public Byte ReadByteFromROMBank(int bankNum, int offsetWithinBank)
        {
            return _romBanks[bankNum][offsetWithinBank];
        }

        public Byte[] ReadBytesFromROMBank(int bankNum, int offsetWithinBank, int count)
        {
            Byte[] retVal = new byte[count];

            Array.Copy(_romBanks[bankNum], offsetWithinBank, retVal, 0, count);
            return retVal;
        }

        public UInt16 ReadUInt16FromROMBank(int bankNum, int offsetWithinBank)
        {
            UInt16 retVal;

            // Pointers and values are stored little-endian.
            Byte lsb = ReadByteFromROMBank(bankNum, offsetWithinBank++);
            Byte msb = ReadByteFromROMBank(bankNum, offsetWithinBank);

            retVal = (UInt16)(((UInt16)msb << 8) + lsb);
            return retVal;
        }

        public Byte[] ReadPatternDataFromCHRBank(int bankNum, bool isRightPatternTable, int patternIndex)
        {
            Byte[] retVal = new byte[16];

            // Each CHR bank holds 2 pattern tables, 4k per pattern table.
            // Each pattern table is 256 patterns, with 16 bytes of data per pattern.
            int offsetWithinCHRBank = isRightPatternTable ? 0x1000 : 0x0;
            offsetWithinCHRBank += (16 * patternIndex);
            Array.Copy(_chrBanks[bankNum], offsetWithinCHRBank, retVal, 0, 16);
            return retVal;
        }
    }
}
