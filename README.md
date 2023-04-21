# BdfFontParser
Parses BDF Fonts (Glyph Bitmap Distribution Format) to Byte Array for using as Pixel Art in consoles or LED Matrixes

Tested with fonts from [rpi-rgb-led-matrix](https://github.com/hzeller/rpi-rgb-led-matrix/tree/master/fonts)

I needed this for my Matrix Simulator:

![Matrix Simulator](https://github.com/marcogruhl/BdfFontParser/raw/main/MatrixSimulator.png "Matrix Simulator")

## Usage

```C#
var font = new BdfFont("fonts/7x13B.bdf");

foreach (var c in "Hello World".ToCharArray())
{
    var charArray = font[c];

    var y = yStart;

    foreach (var b in charArray.Bitmap.Reverse())
    {
        var x = xStart;

        foreach (var bin in Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray())
        {
```