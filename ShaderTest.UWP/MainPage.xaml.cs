using Microsoft.Graphics.Canvas;
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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ShaderTest.UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public PixelShaderEffect effect;
        public CanvasBitmap bitmap;
        public float time = 0;
        public MainPage()
        {
            this.InitializeComponent();
            Init();
        }
        public void Init()
        {
            Canvas.CreateResources += async (s, e) =>
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Shaders/effect.bin"));
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                var bytes = buffer.ToArray();
                effect = new PixelShaderEffect(bytes);
            };
            selectPicture.Click += async (s, e) =>
            {
                var filePicker = new FileOpenPicker();
                filePicker.FileTypeFilter.Add(".png");
                var file = await filePicker.PickSingleFileAsync();
                bitmap = await CanvasBitmap.LoadAsync(Canvas.Device, await file.OpenAsync(FileAccessMode.Read));
                effect.Source1 = bitmap;
            };
            Canvas.Update += (s, e) =>
            {
                if (effect == null || bitmap == null)
                    return;
                time = Convert.ToSingle(e.Timing.TotalTime.TotalSeconds);
                s.Invalidate();
            };
            Canvas.Draw += (s, e) =>
            {
                if (effect == null || bitmap == null)
                    return;
                effect.Properties["_iTime"] = time;
                e.DrawingSession.DrawImage(effect);
                e.DrawingSession.Dispose();
            };
        }
    }
}
