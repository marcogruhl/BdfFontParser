using BdfFontParser;
using System.Text;

var font = new BdfFont("fonts/7x13B.bdf");

// draw line by line
for (int line = 2; line < font.BoundingBox.Y; line++)
{
    var sb = new StringBuilder();

    foreach (var c in "Hello World".ToCharArray())
    {
        var charArray = font[c];
        var x = 0;
        
        // foreach (var b in charArray.Bitmap)
        // {

        byte b = (byte)charArray.Bitmap.GetValue(line);

            foreach (var bin in Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray())
            {
                // if (x != line)
                //     continue;

                sb.Append(bin == '1' ? '#' : ' ');
            }
                    x++;

            // }
        // }

    } 

    Console.WriteLine(sb.ToString());

}