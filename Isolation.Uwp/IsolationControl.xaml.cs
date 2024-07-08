using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Isolation.Uwp
{
    public sealed partial class IsolationControl : UserControl
    {
        private PixelShaderEffect effect;
        private CanvasRenderTarget target;
        private GaussianBlurEffect effectGaussianBlur;
        private CanvasGradientStop[] stops;
        private CanvasLinearGradientBrush brush;
        private float width;
        private float height;
        private float time = 0;
        
        public void SetColor(Vector3 color1, Vector3 color2, Vector3 color3, Vector3 color4)
        {
            effect.Properties["color1"] = color1;
            effect.Properties["color2"] = color2;
            effect.Properties["color3"] = color3;
            effect.Properties["color4"] = color4;
        }

        public IsolationControl()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        public void Init()
        {

            MainCanvas.CreateResources += async (s, e) =>
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Shaders/effect.bin"));
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                var bytes = buffer.ToArray();
                effect = new PixelShaderEffect(bytes);
                effect.Properties["Width"] = width;
                effect.Properties["Height"] = height;
            };
            MainCanvas.Update += (s, e) =>
            {
                if (effect is null)
                    return;
                time = Convert.ToSingle(e.Timing.TotalTime.TotalSeconds);
                effect.Properties["iTime"] = time;
            };
            MainCanvas.Draw += (s, e) =>
            {
                if (effect is null)
                    return;
                e.DrawingSession.DrawImage(effect);
            };
            MainCanvas.SizeChanged += (s, e) =>
            {
                if (effect is null) return;
                width = (float)MainCanvas.ActualWidth;
                height = (float)MainCanvas.ActualHeight;
                effect.Properties["Width"] = width;
                effect.Properties["Height"] = height;
            };

        }
    }
}
