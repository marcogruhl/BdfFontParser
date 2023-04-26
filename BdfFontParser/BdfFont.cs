using System;
using System.Collections.Generic;
using System.IO;
using BdfFontParser.Models;

namespace BdfFontParser
{
    public class BdfFont
    {
        private readonly Dictionary<char, CharData> _charMap = new Dictionary<char, CharData>();

        public BoundingBox BoundingBox { get; set; }

        public BdfFont(string path)
        {
            BuildMap(path);
        }

        public CharData this[char c] => _charMap[c];

        /// <summary>
        /// https://en.wikipedia.org/wiki/Glyph_Bitmap_Distribution_Format
        /// Sample used: https://en.wikipedia.org/wiki/GNU_Unifont #Hexdraw representation of the example
        /// </summary>
        /// <param name="path"></param>
        private void BuildMap(string path)
        {
            using (new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IEnumerable<string> lines = File.ReadLines(path);

                string name = default;
                char character = default;
                Width sWidth = default;
                Width dWidth = default;
                BoundingBox boundingBox = default;
                bool error = false;

                var bitmapMode = false;
                var byteLineIndex = 0;

                try
                {
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("FONTBOUNDINGBOX "))
                        {
                            var lineChar = line.Split(' ');
                            BoundingBox = new BoundingBox()
                            {
                                X = Convert.ToInt16(lineChar[1]),
                                Y = Convert.ToInt16(lineChar[2]),
                                OffsetX = Convert.ToInt16(lineChar[3]),
                                OffsetY = Convert.ToInt16(lineChar[4])
                            };
                        }
                        else if (line.StartsWith("STARTCHAR "))
                        {
                            name = line.Substring(10, line.Length - 10);
                            error = false;
                        }
                        else if (line.StartsWith("ENCODING "))
                        {
                            var lineChar = line.Substring(9, line.Length - 9);

                            if(UInt32.TryParse(lineChar, out var charInt))
                                character = Convert.ToChar(charInt);
                            else
                                error = true;
                        }
                        else if (line.StartsWith("SWIDTH "))
                        {
                            var lineChar = line.Split(' ');
                            sWidth = new Width()
                            {
                                X = Convert.ToInt16(lineChar[1]),
                                Y = Convert.ToInt16(lineChar[2])
                            };

                        }
                        else if (line.StartsWith("DWIDTH "))
                        {
                            var lineChar = line.Split(' ');
                            dWidth = new Width()
                            {
                                X = Convert.ToInt16(lineChar[1]),
                                Y = Convert.ToInt16(lineChar[2])
                            };

                        }
                        else if (line.StartsWith("BBX "))
                        {
                            var lineChar = line.Split(' ');
                            boundingBox = new BoundingBox()
                            {
                                X = Convert.ToInt16(lineChar[1]),
                                Y = Convert.ToInt16(lineChar[2]),
                                OffsetX = Convert.ToInt16(lineChar[3]),
                                OffsetY = Convert.ToInt16(lineChar[4])
                            };
                        }
                        else if (line.StartsWith("BITMAP"))
                        {
                            bitmapMode = true;

                            if(error)
                                continue;

                            _charMap[character] = new CharData()
                            {
                                Character = character,
                                Bitmap = new byte[BoundingBox.Y][],
                                Name = name,
                                ScalableWidth = sWidth,
                                DeviceWidth = dWidth,
                                BoundingBox = boundingBox
                            };
                        }
                        else if (line.StartsWith("ENDCHAR"))
                        {
                            bitmapMode = false;
                            byteLineIndex = 0;
                        }
                        else if (bitmapMode)
                        {
                            if(error)
                                continue;

                            _charMap[character].Bitmap[byteLineIndex] = new byte[line.Length / 2];

                            for (var i = 0; i < line.Length; i += 2)
                            {
                                _charMap[character].Bitmap[byteLineIndex][i / 2] = Convert.ToByte(line.Substring(i, 2), 16);
                            }
                            
                            byteLineIndex++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}