﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace Shazzam.Shaders {
	
	public class GradientColorEffect : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GradientColorEffect), 0);
		public static readonly DependencyProperty IResolutionProperty = DependencyProperty.Register("IResolution", typeof(Point3D), typeof(GradientColorEffect), new UIPropertyMetadata(new Point3D(1D, 1D, 1D), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty ITimeProperty = DependencyProperty.Register("ITime", typeof(double), typeof(GradientColorEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty Color1Property = DependencyProperty.Register("Color1", typeof(Point3D), typeof(GradientColorEffect), new UIPropertyMetadata(new Point3D(0.957D, 0.804D, 0.623D), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty Color2Property = DependencyProperty.Register("Color2", typeof(Point3D), typeof(GradientColorEffect), new UIPropertyMetadata(new Point3D(0.192D, 0.384D, 0.933D), PixelShaderConstantCallback(3)));
		public static readonly DependencyProperty Color3Property = DependencyProperty.Register("Color3", typeof(Point3D), typeof(GradientColorEffect), new UIPropertyMetadata(new Point3D(0.91D, 0.51D, 0.8D), PixelShaderConstantCallback(4)));
		public static readonly DependencyProperty Color4Property = DependencyProperty.Register("Color4", typeof(Point3D), typeof(GradientColorEffect), new UIPropertyMetadata(new Point3D(0.35D, 0.71D, 0.953D), PixelShaderConstantCallback(5)));
		public static readonly DependencyProperty EnableLightWaveProperty = DependencyProperty.Register("EnableLightWave", typeof(double), typeof(GradientColorEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(6)));
		public static readonly DependencyProperty RandomValue1Property = DependencyProperty.Register("RandomValue1", typeof(double), typeof(GradientColorEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(7)));
		public static readonly DependencyProperty RandomValue2Property = DependencyProperty.Register("RandomValue2", typeof(double), typeof(GradientColorEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(8)));
		public static readonly DependencyProperty RandomValue3Property = DependencyProperty.Register("RandomValue3", typeof(double), typeof(GradientColorEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(9)));
		public GradientColorEffect() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/Shazzam.Shaders;component/GradientColorEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(IResolutionProperty);
			this.UpdateShaderValue(ITimeProperty);
			this.UpdateShaderValue(Color1Property);
			this.UpdateShaderValue(Color2Property);
			this.UpdateShaderValue(Color3Property);
			this.UpdateShaderValue(Color4Property);
			this.UpdateShaderValue(EnableLightWaveProperty);
			this.UpdateShaderValue(RandomValue1Property);
			this.UpdateShaderValue(RandomValue2Property);
			this.UpdateShaderValue(RandomValue3Property);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		public Point3D IResolution {
			get {
				return ((Point3D)(this.GetValue(IResolutionProperty)));
			}
			set {
				this.SetValue(IResolutionProperty, value);
			}
		}
		public double ITime {
			get {
				return ((double)(this.GetValue(ITimeProperty)));
			}
			set {
				this.SetValue(ITimeProperty, value);
			}
		}
		public Point3D Color1 {
			get {
				return ((Point3D)(this.GetValue(Color1Property)));
			}
			set {
				this.SetValue(Color1Property, value);
			}
		}
		public Point3D Color2 {
			get {
				return ((Point3D)(this.GetValue(Color2Property)));
			}
			set {
				this.SetValue(Color2Property, value);
			}
		}
		public Point3D Color3 {
			get {
				return ((Point3D)(this.GetValue(Color3Property)));
			}
			set {
				this.SetValue(Color3Property, value);
			}
		}
		public Point3D Color4 {
			get {
				return ((Point3D)(this.GetValue(Color4Property)));
			}
			set {
				this.SetValue(Color4Property, value);
			}
		}
		public double EnableLightWave {
			get {
				return ((double)(this.GetValue(EnableLightWaveProperty)));
			}
			set {
				this.SetValue(EnableLightWaveProperty, value);
			}
		}
		public double RandomValue1 {
			get {
				return ((double)(this.GetValue(RandomValue1Property)));
			}
			set {
				this.SetValue(RandomValue1Property, value);
			}
		}
		public double RandomValue2 {
			get {
				return ((double)(this.GetValue(RandomValue2Property)));
			}
			set {
				this.SetValue(RandomValue2Property, value);
			}
		}
		public double RandomValue3 {
			get {
				return ((double)(this.GetValue(RandomValue3Property)));
			}
			set {
				this.SetValue(RandomValue3Property, value);
			}
		}
	}
}
