using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace IgniteApp.Converters
{
    public class ForegroundBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#969696");

            if (value == null)
                return dColor;

            bool b = (bool)value;

            return b ? new SolidColorBrush(Colors.Red) : dColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}