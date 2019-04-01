using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2R_Mapper.ROM_Utils
{
    class PPU_Bitmap_Generator
    {
        private ROM_Info _romInfo;

        // See different palettes at http://emulation.gametechwiki.com/index.php/Famicom_Color_Palette
        private readonly Color[] FceuxPaletteColors = {
            Color.FromArgb(0x74, 0x74, 0x74),   // 00
            Color.FromArgb(0x24, 0x18, 0x8C),   // 01
            Color.FromArgb(0x00, 0x00, 0xA8),   // 02
            Color.FromArgb(0x44, 0x00, 0x9C),   // 03
            Color.FromArgb(0x8C, 0x00, 0x74),   // 04
            Color.FromArgb(0xA8, 0x00, 0x10),   // 05
            Color.FromArgb(0xA4, 0x00, 0x00),   // 06
            Color.FromArgb(0x7C, 0x08, 0x00),   // 07
            Color.FromArgb(0x40, 0x2C, 0x00),   // 08
            Color.FromArgb(0x00, 0x44, 0x00),   // 09
            Color.FromArgb(0x00, 0x50, 0x00),   // 0A
            Color.FromArgb(0x00, 0x3C, 0x14),   // 0B
            Color.FromArgb(0x18, 0x3C, 0x5C),   // 0C
            Color.FromArgb(0x00, 0x00, 0x00),   // 0D
            Color.FromArgb(0x00, 0x00, 0x00),   // 0E
            Color.FromArgb(0x00, 0x00, 0x00),   // 0F
            Color.FromArgb(0xBC, 0xBC, 0xBC),   // 10
            Color.FromArgb(0x00, 0x70, 0xEC),   // 11
            Color.FromArgb(0x20, 0x38, 0xEC),   // 12
            Color.FromArgb(0x80, 0x00, 0xF0),   // 13
            Color.FromArgb(0xBC, 0x00, 0xBC),   // 14
            Color.FromArgb(0xE4, 0x00, 0x58),   // 15
            Color.FromArgb(0xD8, 0x28, 0x00),   // 16
            Color.FromArgb(0xC8, 0x4C, 0x0C),   // 17
            Color.FromArgb(0x88, 0x70, 0x00),   // 18
            Color.FromArgb(0x00, 0x94, 0x00),   // 19
            Color.FromArgb(0x00, 0xA8, 0x00),   // 1A
            Color.FromArgb(0x00, 0x90, 0x38),   // 1B
            Color.FromArgb(0x00, 0x80, 0x88),   // 1C
            Color.FromArgb(0x00, 0x00, 0x00),   // 1D
            Color.FromArgb(0x00, 0x00, 0x00),   // 1E
            Color.FromArgb(0x00, 0x00, 0x00),   // 1F
            Color.FromArgb(0xFC, 0xFC, 0xFC),   // 20
            Color.FromArgb(0x3C, 0xBC, 0xFC),   // 21
            Color.FromArgb(0x5C, 0x94, 0xFC),   // 22
            Color.FromArgb(0xCC, 0x88, 0xFC),   // 23
            Color.FromArgb(0xF4, 0x78, 0xFC),   // 24
            Color.FromArgb(0xFC, 0x74, 0xB4),   // 25
            Color.FromArgb(0xFC, 0x74, 0x60),   // 26
            Color.FromArgb(0xFC, 0x98, 0x38),   // 27
            Color.FromArgb(0xF0, 0xBC, 0x3C),   // 28
            Color.FromArgb(0x80, 0xD0, 0x10),   // 29
            Color.FromArgb(0x4C, 0xDC, 0x48),   // 2A
            Color.FromArgb(0x58, 0xF8, 0x98),   // 2B
            Color.FromArgb(0x00, 0xE8, 0xD8),   // 2C
            Color.FromArgb(0x78, 0x78, 0x78),   // 2D
            Color.FromArgb(0x00, 0x00, 0x00),   // 2E
            Color.FromArgb(0x00, 0x00, 0x00),   // 2F
            Color.FromArgb(0xFC, 0xFC, 0xFC),   // 30
            Color.FromArgb(0xA8, 0xE4, 0xFC),   // 31
            Color.FromArgb(0xC4, 0xD4, 0xFC),   // 32
            Color.FromArgb(0xD4, 0xC8, 0xFC),   // 33
            Color.FromArgb(0xFC, 0xC4, 0xFC),   // 34
            Color.FromArgb(0xFC, 0xC4, 0xD8),   // 35
            Color.FromArgb(0xFC, 0xBC, 0xB0),   // 36
            Color.FromArgb(0xFC, 0xD8, 0xA8),   // 37
            Color.FromArgb(0xFC, 0xE4, 0xA0),   // 38
            Color.FromArgb(0xE0, 0xFC, 0xA0),   // 39
            Color.FromArgb(0xA8, 0xF0, 0xBC),   // 3A
            Color.FromArgb(0xB0, 0xFC, 0xCC),   // 3B
            Color.FromArgb(0x9C, 0xFC, 0xF0),   // 3C
            Color.FromArgb(0xC4, 0xC4, 0xC4),   // 3D
            Color.FromArgb(0x00, 0x00, 0x00),   // 3E
            Color.FromArgb(0x00, 0x00, 0x00)    // 3F
        };

        public PPU_Bitmap_Generator(ROM_Info romInfo)
        {
            _romInfo = romInfo;
        }

        public Bitmap GetBackgroundTile(int chrBank, bool isRightPatternTable, int[] patternTableIndices, int[] paletteColorsForThisTile)
        {
            // We create the bitmap scaled by 2, so 32x32 instead of 16x16
            Bitmap retVal = new Bitmap(32, 32);

            int xBase = 0, yBase = 0;
            for (int tilePatternNumber = 0; tilePatternNumber < 4; tilePatternNumber++)
            {
                Byte[] patternData = _romInfo.ReadPatternDataFromCHRBank(chrBank, isRightPatternTable, patternTableIndices[tilePatternNumber]);
                for (int x = 7; x >= 0; x--)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int colorIndex = (patternData[y] & 0x01) | ((patternData[y + 8] & 0x01) << 1);
                        int paletteIndex = paletteColorsForThisTile[colorIndex];

                        // For sprites, a palette color of 0 represents a transparent pixel.
                        retVal.SetPixel((xBase + x) * 2, (yBase + y) * 2, FceuxPaletteColors[paletteIndex]);
                        retVal.SetPixel((xBase + x) * 2 + 1, (yBase + y) * 2, FceuxPaletteColors[paletteIndex]);
                        retVal.SetPixel((xBase + x) * 2, (yBase + y) * 2 + 1, FceuxPaletteColors[paletteIndex]);
                        retVal.SetPixel((xBase + x) * 2 + 1, (yBase + y) * 2 + 1, FceuxPaletteColors[paletteIndex]);
                        patternData[y] >>= 1;
                        patternData[y + 8] >>= 1;
                    }
                }
                xBase += 8;
                if (xBase >= 16)
                {
                    xBase = 0;
                    yBase += 8;
                }
            }

            return retVal;
        }

        public Bitmap Get8x16Sprite(int chrBank, bool isRightPatternTable, int indexOfFirstPattern, int[] paletteColorsForThisTile)
        {
            // We create the bitmap scaled by 2, so 16x32 instead of 8x16
            Bitmap retVal = new Bitmap(16, 32);

            for (int patternOffset = 0; patternOffset < 2; patternOffset++)
            {
                Byte[] patternData = _romInfo.ReadPatternDataFromCHRBank(chrBank, isRightPatternTable, indexOfFirstPattern + patternOffset);
                for (int x = 7; x >= 0; x--)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int colorIndex = (patternData[y] & 0x01) | ((patternData[y + 8] & 0x01) << 1);
                        int paletteIndex = paletteColorsForThisTile[colorIndex];

                        // Palette index of 0 should be treated as transparent when drawing sprites
                        Color color = (paletteIndex == 0) ? Color.FromArgb(0, 0, 0, 0) : FceuxPaletteColors[paletteIndex];

                        retVal.SetPixel(x * 2, ((patternOffset * 8) + y) * 2, color);
                        retVal.SetPixel(x * 2 + 1, ((patternOffset * 8) + y) * 2, color);
                        retVal.SetPixel(x * 2, ((patternOffset * 8) + y) * 2 + 1, color);
                        retVal.SetPixel(x * 2 + 1, ((patternOffset * 8) + y) * 2 + 1, color);
                        patternData[y] >>= 1;
                        patternData[y + 8] >>= 1;
                    }
                }
            }

            return retVal;
        }

    }
}
