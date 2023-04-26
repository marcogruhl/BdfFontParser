using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BdfFontParser;

namespace Wpf;

public partial class MainWindow
{
    private static SolidColorBrush?[,] _matrixArray= new SolidColorBrush?[MatrixWidth, MatrixHeight];
    private const int MatrixWidth = 64;
    private const int MatrixHeight = 32;

    private static readonly Dictionary<string, BdfFont> Fonts= new()
    {
        {"7x13B", new BdfFont("fonts/7x13B.bdf")},
        {"helvR12", new BdfFont("fonts/helvR12.bdf")}
    };


    public MainWindow()
    {
        InitializeComponent();
            
        _matrixArray = new SolidColorBrush[MatrixWidth, MatrixHeight];

        ShowTime();
    }

    private async void ShowTime()
    {
        while (true)
        {
            _matrixArray = new SolidColorBrush[MatrixWidth, MatrixHeight];

            var font1 = "helvR12";
            var font2 = "7x13B";
            // var font2 = "helvR12";

            var baselineFont1 = Fonts[font1].BoundingBox.Y + Fonts[font1].BoundingBox.OffsetY;

            Clear();
            DrawText(Fonts[font1], 0, baselineFont1, Colors.Salmon, DateTime.Now.ToString("T"));
            DrawText(Fonts[font2], 0, MatrixHeight + Fonts[font2].BoundingBox.OffsetY, Colors.DeepSkyBlue, DateTime.Now.ToString("M", CultureInfo.CurrentCulture));

            await Refresh();
            await Task.Delay(1000/30);
        }
    }

    private void Clear()
    {
        _matrixArray = new SolidColorBrush[MatrixWidth, MatrixHeight];
    }

    private static void DrawText(BdfFont font, int xStart, int yStart, Color color, string text)
    {
        var map = font.GetMapOfString(text);
        var width = map.GetLength(0);
        var height = map.GetLength(1);

        // draw line by line
        for (int line = 0; line < height; line++)
        {
            // iterate through every bit
            for (int bit = 0; bit < width; bit++)
            {
                var charX = bit + xStart;
                var charY = line + (yStart - font.BoundingBox.Y - font.BoundingBox.OffsetY);

                if(map[bit,line] && charX >= 0 && charY >= 0 && charX <= MatrixWidth-1 && charY <= MatrixHeight-1)
                {
                    try
                    {
                        _matrixArray[charX,charY] = new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
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

    private async Task Refresh()
    {            
        MatrixCanvas.Children.Clear();

        for (int x = 0; x < MatrixWidth; x++)
        {
            for (int y = 0; y < MatrixHeight; y++)
            {
                var fill = _matrixArray[x, y];

                if(fill == null)
                    continue;

                var rectangle = new Ellipse();

                rectangle.SetValue(Canvas.LeftProperty, x * MatrixCanvas.ActualWidth / MatrixWidth);
                rectangle.SetValue(Canvas.TopProperty, y * MatrixCanvas.ActualHeight / MatrixHeight);

                rectangle.Width = MatrixCanvas.ActualWidth / MatrixWidth - MatrixCanvas.ActualWidth / MatrixWidth * 0.33;
                rectangle.Height = rectangle.Width; // lock height to width
                rectangle.Fill = fill;

                MatrixCanvas.Children.Add(rectangle);
            }
        }
    }
}