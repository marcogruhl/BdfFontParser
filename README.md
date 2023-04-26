# BdfFontParser
Parses BDF Fonts (Glyph Bitmap Distribution Format) to Byte Array for using as Pixel Art in consoles or LED Matrixes

Tested with fonts from [rpi-rgb-led-matrix](https://github.com/hzeller/rpi-rgb-led-matrix/tree/master/fonts)

I needed this for my Matrix Simulator:

![Matrix Simulator](/MatrixSimulator.png "Matrix Simulator")

## Usage

```C#
using BdfFontParser;
using System.Text;

var font = new BdfFont("fonts/7x13B.bdf");

var map = bdfFont.GetMapOfString("Hello World");
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

Console.WriteLine(sb.ToString());
```
This code is shown in the Sample/Console app:

![Console Sample](/SampleConsole.png "Console Sample")