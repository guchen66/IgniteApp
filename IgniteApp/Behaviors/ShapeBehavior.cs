using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IgniteApp.Behaviors
{
    public class ShapeBehavior : Behavior<Ellipse>
    {
        public static readonly DependencyProperty IsConnectedProperty =
        DependencyProperty.Register(
            "IsConnected",
            typeof(bool),
            typeof(ShapeBehavior),
            new PropertyMetadata(false, OnIsConnectedChanged));

        public bool IsConnected
        {
            get => (bool)GetValue(IsConnectedProperty);
            set => SetValue(IsConnectedProperty, value);
        }

        // 新增核心逻辑：处理控件附加
        protected override void OnAttached()
        {
            base.OnAttached();
            // UpdateColor(); // 确保控件附加后立即生效
        }

        // 重构颜色更新方法
        private void UpdateColor()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Fill = IsConnected ? Brushes.Green : Brushes.Red;
            }
        }

        // 优化属性变更处理
        private static void OnIsConnectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(d as ShapeBehavior)?.UpdateColor();

            //如果直接绑定会导致触发机制提前，此时AssociatedObject是null
            //如果在ViewModel解析的时候，已经确定了数据，此时重新触发AssociatedObject才有值
            if (d is ShapeBehavior behavior && behavior.AssociatedObject != null)
            {
                behavior.AssociatedObject.Fill = (bool)e.NewValue ?
                    Brushes.Green :
                    Brushes.Red;
            }
        }
    }
}