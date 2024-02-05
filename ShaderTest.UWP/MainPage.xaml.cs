using ColorThiefDotNet;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ShaderTest.UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public PixelShaderEffect effect;
        public CanvasRenderTarget target;
        public GaussianBlurEffect effectGaussianBlur;
        public CanvasGradientStop[] stops;
        public CanvasLinearGradientBrush brush;
        public float width;
        public float height;
        public float time = 0;
        public MainPage()
        {
            this.InitializeComponent();
            Init();
            canvas.SizeChanged += Canvas_SizeChanged;
            width = (float)canvas.ActualWidth;
            height = (float)canvas.ActualHeight;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            width = (float)canvas.ActualWidth;
            height = (float)canvas.ActualHeight;
            if (effect != null)
            {
                effect.Properties["Width"] = width;
                effect.Properties["Height"] = height;
            }
        }

        public void Init()
        {
            canvas.CreateResources += async (s, e) =>
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Shaders/effect.bin"));
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                var bytes = buffer.ToArray();
                effect = new PixelShaderEffect(bytes);
                effect.Properties["Width"] = width;
                effect.Properties["Height"] = height;
            };
            selectPicture.Click += async (s, e) =>
            {
                var filePicker = new FileOpenPicker();
                filePicker.FileTypeFilter.Add(".png");
                filePicker.FileTypeFilter.Add(".jpg");
                var file = await filePicker.PickSingleFileAsync();
                if (file == null) return;

                using (var stream = await file.OpenReadAsync())
                {
                    var thief = new ColorThief();
                    var decoder = await BitmapDecoder.CreateAsync(stream);
                    var mainColorPattern = (await thief.GetColor(decoder, 10, false)).IsDark;
                    var colors = (await thief.GetPalette(decoder, 8, 10, false))
                        .OrderBy(t => t.Population)
                        .Where(t => t.IsDark == mainColorPattern)
                        .Select(t => new Vector3((float)t.Color.R / 0xff, (float)t.Color.G / 0xff, (float)t.Color.B / 0xff)).ToList();
                    effect.Properties["color1"] = colors[0];
                    effect.Properties["color2"] = colors[1];
                    effect.Properties["color3"] = colors[2];
                    effect.Properties["color4"] = colors[3];
                }
            };
            canvas.Update += (s, e) =>
            {
                if (effect == null)
                    return;
                time = Convert.ToSingle(e.Timing.TotalTime.TotalSeconds);
                effect.Properties["iTime"] = time;
            };
            canvas.Draw += (s, e) =>
            {
                if (effect == null)
                    return;
                e.DrawingSession.DrawImage(effect);
            };
        }
    }
}
