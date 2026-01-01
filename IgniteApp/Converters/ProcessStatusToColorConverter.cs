using IgniteShared.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace IgniteApp.Converters
{
    public class ProcessStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProcessStatus current && parameter is ProcessStatus target && target == current)
                return new SolidColorBrush(Colors.Green);   // 当前选中
            return new SolidColorBrush(Colors.Red);         // 其余红色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}