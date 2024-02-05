using ColorThiefDotNet;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Media3D;

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
                var colors = await CreateColorBrushAsync();
                ((IsolationBrushProvider)LayoutRoot.Resources["BrushProvider"]).Color1 = colors[0];
                ((IsolationBrushProvider)LayoutRoot.Resources["BrushProvider"]).Color2 = colors[1];
                ((IsolationBrushProvider)LayoutRoot.Resources["BrushProvider"]).Color3 = colors[2];
                ((IsolationBrushProvider)LayoutRoot.Resources["BrushProvider"]).Color4 = colors[3];
            };
        }

        public async Task<Point3D[]> CreateColorBrushAsync()
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
                    var mainColorPattern = thief.GetColor(scaledImage ?? originalImage, 10, false).IsDark;
                    var sourceColor = thief.GetPalette(scaledImage ?? originalImage, 8, 10, false)
                        .Where(t => t.IsDark == mainColorPattern)
                        .OrderBy(t => t.Population);
                    return sourceColor.Select(color => new Point3D((float)color.Color.R / 255, (float)color.Color.G / 255, (float)color.Color.B / 255)).ToArray();
                }
                finally
                {
                    originalImage?.Dispose();
                    scaledImage?.Dispose();
                }
            });
            return result;
        }
    }
}