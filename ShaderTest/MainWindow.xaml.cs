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
using Color = System.Windows.Media.Color;
using ColorThiefDotNet;
using PaletteMixr;

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
            Loaded += (s, a) =>
            {
                SetColor();
                //((Storyboard)LayoutRoot.Resources["TimeAnimation"]).Begin();
            };
        }
        public Task SetColor()
        {
            var thief = new ColorThief();
            using var image = new Bitmap(@"C:\Users\21945\Pictures\illust_112988473_20231103_170358.png");
            var sourceColor = thief.GetPalette(image, 5, 10, false);
            var result = sourceColor.Select(x=>x.Color).ToList();
            var brush = new LinearGradientBrush()
            {
                GradientStops = new()
                {
                    new()
                    {
                        Offset = 0,
                        Color=Color.FromArgb(result[2].A,result[2].R,result[2].G,result[2].B)
                    },
                    new()
                    {
                        Offset = 0.25,
                        Color=Color.FromArgb(result[0].A,result[0].R,result[0].G,result[0].B)

                    },
                    new()
                    {
                        Offset = 0.5,
                        Color=Color.FromArgb(result[1].A,result[1].R,result[1].G,result[1].B)
                    },
                    new()
                    {
                        Offset = 0.75,
                        Color=Color.FromArgb(result[3].A,result[3].R,result[3].G,result[3].B)
                    }

                }
            };
            Retangle1.Fill = brush;
            return Task.CompletedTask;
        }
    }
}