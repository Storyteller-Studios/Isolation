using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace ShaderTest
{
    public sealed class IsolationBrushProvider : DependencyObject
    {
        static IsolationBrushProvider()
        {
            StretchProperty = TileBrush.StretchProperty.AddOwner(typeof(IsolationBrushProvider), new FrameworkPropertyMetadata(Stretch.UniformToFill));
            SpeedRatioProperty = Timeline.SpeedRatioProperty.AddOwner(typeof(IsolationBrushProvider));

            BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(IsolationBrushProvider), new PropertyMetadata(null));
            EffectScaleProperty = DependencyProperty.Register("EffectScale", typeof(Point), typeof(IsolationBrushProvider), new PropertyMetadata(new Point(0.5d, 0.5d)));
            EffectPowerProperty = DependencyProperty.Register("EffectPower", typeof(double), typeof(IsolationBrushProvider), new PropertyMetadata(3d));
        }

        private Brush? isolationBrush;
        private Rectangle? brushHost;

        public Brush IsolationBrush => EnsureBrush();

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        public double SpeedRatio
        {
            get { return (double)GetValue(SpeedRatioProperty); }
            set { SetValue(SpeedRatioProperty, value); }
        }
        public Point EffectScale
        {
            get { return (Point)GetValue(EffectScaleProperty); }
            set { SetValue(EffectScaleProperty, value); }
        }
        public double EffectPower
        {
            get { return (double)GetValue(EffectPowerProperty); }
            set { SetValue(EffectPowerProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty;

        public static readonly DependencyProperty StretchProperty;

        public static readonly DependencyProperty SpeedRatioProperty;

        public static readonly DependencyProperty EffectScaleProperty;

        public static readonly DependencyProperty EffectPowerProperty;

        private Brush EnsureBrush()
        {
            if (isolationBrush == null)
            {
                brushHost = new Rectangle()
                {
                    Width = 1,
                    Height = 1
                };

                var effect = new MyShaderEffect();
                brushHost.Effect = effect;

                isolationBrush = new VisualBrush(brushHost);

                var animation = new DoubleAnimation(0, 62.836, TimeSpan.FromMinutes(1))
                {
                    RepeatBehavior = RepeatBehavior.Forever,
                };

                // 锁定到30帧以降低资源消耗
                Timeline.SetDesiredFrameRate(animation, 30);

                effect.BeginAnimation(MyShaderEffect.TimeProperty, animation);

                SetBinding("EffectScale", effect, MyShaderEffect.ScaleProperty);
                SetBinding("EffectPower", effect, MyShaderEffect.PowerProperty);
                SetBinding("SpeedRatio", animation, Timeline.SpeedRatioProperty);
                SetBinding("Background", brushHost, Shape.FillProperty);
                SetBinding("Stretch", isolationBrush, TileBrush.StretchProperty);
            }

            return isolationBrush;
        }

        private void SetBinding(string path, DependencyObject target, DependencyProperty dp)
        {
            BindingOperations.SetBinding(target, dp, new Binding(path) { Source = this });
        }
    }
}
