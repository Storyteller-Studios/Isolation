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
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            MainCanvas.CreateResources -= MainCanvas_CreateResources;
            MainCanvas.Update -= MainCanvas_Update;
            MainCanvas.Draw -= MainCanvas_Draw;
            MainCanvas.SizeChanged -= MainCanvas_SizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MainCanvas.CreateResources += MainCanvas_CreateResources;
            MainCanvas.Update += MainCanvas_Update;
            MainCanvas.Draw += MainCanvas_Draw;
            MainCanvas.SizeChanged += MainCanvas_SizeChanged;
        }


        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (effect is null) return;
            effect.Properties["Width"] = (float)MainCanvas.ActualWidth;
            effect.Properties["Height"] = (float)MainCanvas.ActualHeight;
        }

        private void MainCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            if (effect is null)
                return;
            args.DrawingSession.DrawImage(effect);
        }

        private void MainCanvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            if (effect is null)
                return;
            effect.Properties["iTime"] = Convert.ToSingle(args.Timing.TotalTime.TotalSeconds);
        }

        private async void MainCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Shaders/effect.bin"));
            var buffer = await FileIO.ReadBufferAsync(file);
            var bytes = buffer.ToArray();
            effect = new PixelShaderEffect(bytes);
            effect.Properties["Width"] = (float)MainCanvas.ActualWidth;
            effect.Properties["Height"] = (float)MainCanvas.ActualHeight;
        }
    }
}
