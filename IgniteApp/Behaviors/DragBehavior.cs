using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace IgniteApp.Behaviors
{
    public class DragBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(AssociatedObject);
            if (window != null)
            {
                window.DragMove();
            }
        }
    }

    public static class WindowHelper
    {
        public static readonly DependencyProperty DragMoveProperty =
            DependencyProperty.RegisterAttached("DragMove", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false, OnDragMoveChanged));

        public static bool GetDragMove(DependencyObject obj)
        {
            return (bool)obj.GetValue(DragMoveProperty);
        }

        public static void SetDragMove(DependencyObject obj, bool value)
        {
            obj.SetValue(DragMoveProperty, value);
        }

        private static void OnDragMoveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element && (bool)e.NewValue)
            {
                element.MouseDown += Element_MouseDown;
            }
        }

        private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var window = ((FrameworkElement)sender).TemplatedParent as Window;
                if (window != null)
                {
                    window.DragMove();
                }
            }
        }
    }
}
