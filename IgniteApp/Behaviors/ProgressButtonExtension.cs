using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteApp.Behaviors
{
    public class ProgressButtonExtension
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(
                "Value",
                typeof(double),
                typeof(ProgressButtonExtension),
                new PropertyMetadata(0.0, OnValueChanged));

        public static double GetValue(DependencyObject obj)
        {
            return (double)obj.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject obj, double value)
        {
            obj.SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressButton button)
        {
                // 确保Content总是显示为格式化的进度百分比
                button.Content = $"{e.NewValue:0}%";
            }
        }
    }
}
