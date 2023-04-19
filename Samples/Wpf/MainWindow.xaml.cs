using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        {"7x13B", new BdfFont("fonts/7x13B.bdf")}
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

            Clear();
            DrawText(Fonts["7x13B"], 2, 11, Colors.Salmon, DateTime.Now.ToString("T"));
            DrawText(Fonts["7x13B"], 2, 26, Colors.DeepSkyBlue, DateTime.Now.ToString("M", CultureInfo.CurrentCulture));

            await Refresh();
            await Task.Delay(1000/30);
        }
    }

    private void Clear()
    {
        _matrixArray = new SolidColorBrush[MatrixWidth, MatrixHeight];
    }

    private static void DrawText(BdfFont font, int xStart, int yStart, Color color, string toString)
    {
        yStart -= 1;

        foreach (var c in toString.ToCharArray())
        {
            var charArray = font[c];

            var y = yStart;

            foreach (var b in charArray.Bitmap.Reverse())
            {
                var x = xStart;

                foreach (var bin in Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray())
                {
                    if(bin == '1' && x+charArray.BoundingBox.OffsetX >= 0 && y-charArray.BoundingBox.OffsetY >= 0 && x+charArray.BoundingBox.OffsetX <= MatrixWidth-1 && y-charArray.BoundingBox.OffsetY <= MatrixHeight-1)
                    {
                        try
                        {
                            _matrixArray[x+charArray.BoundingBox.OffsetX,y-charArray.BoundingBox.OffsetY] = new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
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