using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorThiefDotNet;
using Color = System.Windows.Media.Color;
using Brush = System.Windows.Media.Brush;

namespace ShaderTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, a) =>
            {
                var colorBrush = await CreateColorBrushAsync();
                ((IsolationBrushProvider)LayoutRoot.Resources["BrushProvider"]).Background = colorBrush;
            };
        }

        public async Task<Brush> CreateColorBrushAsync()
        {
            const int MaxSize = 100;

            var result = await Task.Run(() =>
            {
                var thief = new ColorThief();

                Bitmap? originalImage = null;
                Bitmap? scaledImage = null;

                try
                {
                    originalImage = new Bitmap(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.jpg"));

                    if (originalImage.Width > MaxSize || originalImage.Height > MaxSize)
                    {
                        var scale = Math.Max(originalImage.Width, originalImage.Height) * 1.0 / MaxSize;
                        
                        if (scale > 1)
                        {
                            scaledImage = new Bitmap(originalImage, (int)(originalImage.Width / scale), (int)(originalImage.Height / scale));
                        }
                    }

                    var sourceColor = thief.GetPalette(scaledImage ?? originalImage, 4, 10, false);
                    return sourceColor.Select(x => x.Color).ToArray();
                }
                finally
                {
                    originalImage?.Dispose();
                    scaledImage?.Dispose();
                }


            });

            var brush = new LinearGradientBrush();
            if(result.Length < 3)
            {
                GradientStopCollection collection = new();
                for(int i = 1;  i<result.Length; i++ )
                {
                    collection.Add(new() { Color = MapColor(result[i]), Offset = i/result.Length });
                }
                brush.GradientStops = collection;
            }
            else
            {
                brush.GradientStops = new()
                {
                    new()
                    {
                        Offset = 0,
                        Color = MapColor(result[2])
                    },
                    new()
                    {
                        Offset = 0.25,
                        Color = MapColor(result[0])
                    },
                    new()
                    {
                        Offset = 0.75,
                        Color = MapColor(result[1])
                    }
                };
            }
            return brush;

            static Color MapColor(ColorThiefDotNet.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}