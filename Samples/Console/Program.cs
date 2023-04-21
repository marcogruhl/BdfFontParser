using BdfFontParser;
using System.Text;

var font = new BdfFont("fonts/7x13B.bdf");

// draw line by line
for (int line = 2; line < font.BoundingBox.Y; line++)
{
    var sb = new StringBuilder();

    // iterate through every character
    foreach (var c in "Hello World".ToCharArray())
    {
        // get the bitmap from the character
        var bitmap = font[c].Bitmap;

        // get the byte for the specific line
        var b = (byte)bitmap.GetValue(line);

        // iterate through every bit
        foreach (var bin in Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray())
        {
            sb.Append(bin == '1' ? '#' : ' ');
        }
    }

    Console.WriteLine(sb.ToString());
}