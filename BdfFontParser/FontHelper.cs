using System;
using System.Linq;

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

            foreach (var c in text.ToCharArray())
            {
                var charArray = font[c];
                
                var y = 0;

                foreach (var b in charArray.Bitmap.Reverse())
                {
                    var x = xStart;

                    var array = Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray();

                    for (int bi = 0; bi < charArray.DeviceWidth.X - charArray.BoundingBox.OffsetX; bi++)
                    {
                        var bin = array[bi];
                        var charX = x + charArray.BoundingBox.OffsetX;
                        var charY = y + baseline + font.BoundingBox.Y - charArray.BoundingBox.Y - charArray.BoundingBox.OffsetY;

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
