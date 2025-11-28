using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace IgniteApp.Behaviors
{
    public class NumboxInputBehavior
    {
        public static DependencyProperty OnLostFocusProperty = DependencyProperty.RegisterAttached("OnLostFocus", typeof(ICommand), typeof(NumboxInputBehavior), new UIPropertyMetadata(OnLostFocus));

        public static void SetOnLostFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(NumboxInputBehavior.OnLostFocusProperty, value);
        }

        private static void OnLostFocus(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as TextBox;

            if (element == null)
                return;

            if (e.NewValue != null && e.OldValue == null)
            {
                element.LostFocus += OnPreviewLostFocus;
            }
            else if (e.NewValue == null && e.OldValue != null)
            {
                element.LostFocus -= OnPreviewLostFocus;
            }
        }

        private static void OnPreviewLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox element = (TextBox)sender;

            ICommand command = (ICommand)element.GetValue(NumboxInputBehavior.OnLostFocusProperty);

            if (command != null)
            {
                command.Execute(element);
            }
        }

        public static DependencyProperty OnValueChangedProperty = DependencyProperty.RegisterAttached("OnValueChanged", typeof(ICommand), typeof(NumboxInputBehavior), new UIPropertyMetadata(ValueChanged));

        public static void SetOnValueChanged(DependencyObject target, ICommand value)
        {
            target.SetValue(NumboxInputBehavior.OnValueChangedProperty, value);
        }

        private static void ValueChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as TextBox;

            if (element == null)
                return;
            if (e.NewValue != null && e.OldValue == null)
            {
                element.TextChanged += OnValueChanged;
            }
            else if (e.NewValue == null && e.OldValue != null)
            {
                element.TextChanged -= OnValueChanged;
            }
        }

        private static void OnValueChanged(object sender, TextChangedEventArgs e)
        {
            TextBox element = (TextBox)sender;

            ICommand command = (ICommand)element.GetValue(NumboxInputBehavior.OnValueChangedProperty);

            if (command != null)
            {
                command.Execute(new ValueObjContext() { Target = element, });
            }
        }

        private static void OnValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            TextBox element = (TextBox)sender;

            ICommand command = (ICommand)element.GetValue(NumboxInputBehavior.OnValueChangedProperty);

            if (command != null)
            {
                command.Execute(new ValueObjContext() { Target = element, Args = e });
            }
        }
    }

    public class ValueObjContext
    {
        public object Target { get; set; }

        public ValueChangedEventArgs<double> Args { get; set; }
    }

    public class ValueChangedEventArgs<T> : RoutedEventArgs
    {
        public T OldValue { get; set; }

        public T NewValue { get; set; }

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public ValueChangedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        public ValueChangedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }
    }
}