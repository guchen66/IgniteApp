using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using IT.Tangdao.Xaml.Enums;
using System.Windows.Controls;

namespace IgniteApp.Behaviors
{
    public static class ImageAttach
    {
        // 定义附加属性
        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.RegisterAttached(
                "Shape",
                typeof(ImageShape),
                typeof(ImageAttach),
                new PropertyMetadata(ImageShape.Rectangle, OnShapeChanged));

        // Get 和 Set 方法
        public static ImageShape GetShape(DependencyObject obj)
        {
            return (ImageShape)obj.GetValue(ShapeProperty);
        }

        public static void SetShape(DependencyObject obj, ImageShape value)
        {
            obj.SetValue(ShapeProperty, value);
        }

        // 属性变化时的处理
        private static void OnShapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Image image)
            {
                UpdateImageClip(image);
            }
        }

        // 更新图片裁剪
        private static void UpdateImageClip(Image image)
        {
            var shape = GetShape(image);

            switch (shape)
            {
                case ImageShape.Circle:
                    // 延迟到图片加载完成后设置裁剪，确保能获取到实际尺寸
                    if (image.IsLoaded)
                    {
                        ApplyCircleClip(image);
                    }
                    else
                    {
                        image.Loaded += OnImageLoaded;
                    }
                    break;

                case ImageShape.Rectangle:
                    image.Clip = null;
                    image.Loaded -= OnImageLoaded;
                    break;
            }
        }

        private static void OnImageLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image image)
            {
                image.Loaded -= OnImageLoaded;
                ApplyCircleClip(image);
            }
        }

        private static void ApplyCircleClip(Image image)
        {
            var center = new Point(image.ActualWidth / 2, image.ActualHeight / 2);
            var radius = Math.Min(image.ActualWidth, image.ActualHeight) / 2;

            image.Clip = new EllipseGeometry(center, radius, radius);
        }
    }
}