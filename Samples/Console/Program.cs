using BdfFontParser;
using System.Text;

var font = new BdfFont("fonts/7x13B.bdf");

Console.Write("Text to parse: ");
var text = Console.ReadLine();

if (String.IsNullOrEmpty(text))
    text = "Hello World";

Console.WriteLine(BuildString(font, text));

string BuildString(BdfFont bdfFont, string s)
{
    var sb = new StringBuilder();

    // draw line by line
    for (int line = 2; line < bdfFont.BoundingBox.Y; line++)
    {
        // iterate through every character
        foreach (var c in s.ToCharArray())
        {
            // get the bitmap from the character
            var bitmap = bdfFont[c].Bitmap;

            // get the byte for the specific line
            var b = (byte)bitmap.GetValue(line);

            // iterate through every bit
            foreach (var bin in Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray())
            {
                sb.Append(bin == '1' ? '#' : ' ');
            }
        }

        sb.AppendLine();
    }

    return sb.ToString();
}