using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace IgniteApp.Converters
{
    public class RecipeUnlockConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                return false;

            int selectedIndex = (int)values[0];
            int currentIndex = (int)values[1];

            // 这里可以根据你的业务逻辑调整解锁条件
            // 例如: 只解锁当前选中的配方或特定索引的配方
            return selectedIndex == currentIndex;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}