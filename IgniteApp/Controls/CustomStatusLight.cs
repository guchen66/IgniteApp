using System.Windows;
using System.Windows.Controls;

namespace IgniteApp.Controls
{
    public class CustomStatusLight : Control
    {
        #region 依赖属性

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(
                "IsActive",
                typeof(bool),
                typeof(CustomStatusLight),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        #endregion 依赖属性

        static CustomStatusLight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CustomStatusLight),
                new FrameworkPropertyMetadata(typeof(CustomStatusLight)));
        }
    }
}