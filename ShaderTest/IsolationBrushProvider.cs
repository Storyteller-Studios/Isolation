using ShaderTest.Effects;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace ShaderTest
{
    public sealed class IsolationBrushProvider : DependencyObject
    {
        static IsolationBrushProvider()
        {
            StretchProperty = TileBrush.StretchProperty.AddOwner(typeof(IsolationBrushProvider), new FrameworkPropertyMetadata(Stretch.UniformToFill));
            IResolutionProperty = DependencyProperty.Register("EffectIResolution", typeof(Point3D), typeof(IsolationBrushProvider), new PropertyMetadata(new Point3D(1D, 1D, 1D)));
            Color1Property = DependencyProperty.Register("EffectColor1", typeof(Point3D), typeof(IsolationBrushProvider), new PropertyMetadata(new Point3D(0.957D, 0.804D, 0.623D)));
            Color2Property = DependencyProperty.Register("EffectColor2", typeof(Point3D), typeof(IsolationBrushProvider), new PropertyMetadata(new Point3D(0.192D, 0.384D, 0.933D)));
            Color3Property = DependencyProperty.Register("EffectColor3", typeof(Point3D), typeof(IsolationBrushProvider), new PropertyMetadata(new Point3D(0.91D, 0.51D, 0.8D)));
            Color4Property = DependencyProperty.Register("EffectColor4", typeof(Point3D), typeof(IsolationBrushProvider), new PropertyMetadata(new Point3D(0.35D, 0.71D, 0.953D)));
        }

        private Brush? isolationBrush;
        private Rectangle? brushHost;

        public Brush IsolationBrush => EnsureBrush();

        public Point3D IResolution
        {
            get { return (Point3D)GetValue(IResolutionProperty); }
            set { SetValue(IResolutionProperty, value); }
        }
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        public Point3D Color1
        {
            get { return (Point3D)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }
        public Point3D Color2
        {
            get { return (Point3D)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }
        public Point3D Color3
        {
            get { return (Point3D)GetValue(Color3Property); }
            set { SetValue(Color3Property, value); }
        }
        public Point3D Color4
        {
            get { return (Point3D)GetValue(Color4Property); }
            set { SetValue(Color4Property, value); }
        }

        public static readonly DependencyProperty IResolutionProperty;

        public static readonly DependencyProperty Color1Property;

        public static readonly DependencyProperty Color2Property;

        public static readonly DependencyProperty Color3Property;

        public static readonly DependencyProperty Color4Property;

        public static readonly DependencyProperty StretchProperty;


        private Brush EnsureBrush()
        {
            if (isolationBrush == null)
            {
                brushHost = new Rectangle()
                {
                    Width = 1,
                    Height = 1
                };

                var effect = new GradientColorEffect();
                brushHost.Fill = new SolidColorBrush(Colors.White);
                brushHost.Effect = effect;

                isolationBrush = new VisualBrush(brushHost);

                var animation = new DoubleAnimation(0, 60, TimeSpan.FromMinutes(1))
                {
                    RepeatBehavior = RepeatBehavior.Forever,
                };

                // 锁定到30帧以降低资源消耗
                Timeline.SetDesiredFrameRate(animation, 30);

                effect.BeginAnimation(GradientColorEffect.ITimeProperty, animation);

                SetBinding("EffectIResolution", effect, GradientColorEffect.IResolutionProperty);
                SetBinding("EffectColor1", effect, GradientColorEffect.Color1Property);
                SetBinding("EffectColor2", effect, GradientColorEffect.Color2Property);
                SetBinding("EffectColor3", effect, GradientColorEffect.Color3Property);
                SetBinding("EffectColor4", effect, GradientColorEffect.Color4Property);
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
