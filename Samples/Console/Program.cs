using BdfFontParser;
using System.Text;

// var font = new BdfFont("fonts/helvR12.bdf");
var font = new BdfFont("fonts/7x13B.bdf");

while(true)
{
    Console.Write("Text to parse: ");
    var text = Console.ReadLine();

    if (String.IsNullOrEmpty(text))
        text = "Hello World";

    Console.WriteLine(BuildString(font, text));
}

string BuildString(BdfFont bdfFont, string s)
{
    var map = bdfFont.GetMapOfString(s);
    var width = map.GetLength(0);
    var height = map.GetLength(1);

    var sb = new StringBuilder();

    // draw line by line
    for (int line = 0; line < height; line++)
    {
        // iterate through every bit
        for (int bit = 0; bit < width; bit++)
        {
            sb.Append(map[bit,line] ? '#' : ' ');
        }    
        
        sb.AppendLine();
    }

    return sb.ToString();
}