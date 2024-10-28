using IgniteShared.Entitys;
using IgniteShared.Globals.System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IgniteApp.Converters
{
    public class RoleTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RoleType)
            {
                // 将枚举转换为更友好的字符串形式
                return value.ToString(); // 或者使用资源文件来本地化这些字符串
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
