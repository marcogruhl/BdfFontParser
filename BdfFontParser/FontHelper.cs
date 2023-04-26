using System;

namespace BdfFontParser
{
    public static class FontHelper
    {
        public static bool[,] GetMapOfString(this BdfFont font, string text)
        {
            var xStart = 0;
            var height = font.BoundingBox.Y;
            var baseline = font.BoundingBox.Y + font.BoundingBox.OffsetY - 1;
            var width = font.GetWithOfString(text);
            
            var map = new bool[width, height];

            foreach (var character in text.ToCharArray())
            {
                var charArray = font[character];
                var y = 0;

                for (var r = charArray.Bitmap.Length - 1; r >= 0; r--)
                {
                    var b = charArray.Bitmap[r];

                    if(b == null)
                        continue;

                    var x = xStart;

                    var length = b.Length * 8;
                    var chars = new char[length];

                    var count = 0;

                    foreach (var byteArray in b)
                    {
                        foreach (var bi in Convert.ToString(byteArray, 2).PadLeft(8, '0').ToCharArray())
                        {
                            chars[count++] = bi;
                        }
                    }

                    for (int i = 0; i < charArray.DeviceWidth.X - charArray.BoundingBox.OffsetX - 1; i++)
                    {
                        var bin = chars[i];
                        var charX = x + charArray.BoundingBox.OffsetX;
                        var charY = y + baseline - charArray.BoundingBox.OffsetY;

                        if (charX > width || charY > height)
                            continue;
                        
                        if(bin == '1' && charX >= 0 && charY >= 0)
                        {
                            try
                            {
                                map[charX,charY] = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                        }

                        x++;
                    }

                    y--;
                }
                
                xStart += charArray.DeviceWidth.X;
            }

            return map;
        }

        public static int GetWithOfString(this BdfFont font, string text)
        {
            var width = 0;

            foreach (var c in text.ToCharArray())
            {
                var charArray = font[c];
                width += charArray.DeviceWidth.X + charArray.BoundingBox.OffsetX;
            }

            return width;
        }
    }
}
