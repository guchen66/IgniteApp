using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Yitter.IdGenerator;

namespace IgniteApp.Converters
{
    public class IdConverter : IValueConverter
    {
        private static long _id = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LoginDto info)
            {
                _id = SysLoginInfo .Id= YitIdHelper.NextId();
                return _id;
            }
            return _id;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
