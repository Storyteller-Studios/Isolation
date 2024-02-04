using ColorThiefDotNet;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        public float time = 0;
        public MainPage()
        {
            this.InitializeComponent();
            Init();
        }
        public void Init()
        {
            canvas.CreateResources += async (s, e) =>
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Shaders/effect.bin"));
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                var bytes = buffer.ToArray();
                effect = new PixelShaderEffect(bytes);
                effectGaussianBlur = new GaussianBlurEffect();
                effectGaussianBlur.BlurAmount = 50;
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
                    var mainColorPattern = (await thief.GetColor(decoder,10,false)).IsDark;
                    var colors = (await thief.GetPalette(decoder, 8, 10, false))
                        .OrderBy(t=>t.Population)
                        .Where(t=>t.IsDark == mainColorPattern)
                        .Select(t=> Windows.UI.Color.FromArgb(t.Color.A,t.Color.R,t.Color.G,t.Color.B)).ToList();
                    if (colors.Count < 3)
                    {
                        stops = new CanvasGradientStop[colors.Count];
                        for(int i = 0; i<colors.Count; i++)
                        {
                            stops[i] = new CanvasGradientStop()
                            {
                                Position = i / colors.Count,
                                Color = colors[i]
                            };
                        }
                    }
                    else
                    {
                        stops = new CanvasGradientStop[3];
                        stops[0] = new CanvasGradientStop()
                        {
                            Position = 0,
                            Color = colors[1]
                        };
                        stops[1] = new CanvasGradientStop()
                        {
                            Position = 0.25f,
                            Color = colors[0]
                        };
                        stops[2] = new CanvasGradientStop()
                        {
                            Position = 0.75f,
                            Color = colors[2]
                        };
                        target = new CanvasRenderTarget(canvas, 1000, 1000);
                    }
                    
                }
            };
            canvas.Update += (s, e) =>
            {
                if (effect == null || stops == null)
                    return;
                time = Convert.ToSingle(e.Timing.TotalTime.TotalSeconds);
                s.Invalidate();
            };
            canvas.Draw += (s, e) =>
            {
                if (effect == null || target == null)
                    return;
                using (var session = target.CreateDrawingSession())
                {
                    using (var brush = new CanvasLinearGradientBrush(session, stops))
                    {
                        brush.StartPoint = new Vector2(0, 0);
                        brush.EndPoint = new Vector2(1000, 1000);
                        session.FillRectangle(new Rect(new Point(0, 0), new Point(1000, 1000)), brush);
                    }
                }
                effect.Source1 = target;
                effectGaussianBlur.Source = effect;
                effect.Properties["_iTime"] = time;
                e.DrawingSession.DrawImage(effectGaussianBlur);
                e.DrawingSession.Dispose();
            };
        }
    }
}
