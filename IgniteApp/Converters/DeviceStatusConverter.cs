using IgniteApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IgniteApp.Converters
{
    public class DeviceStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is RunStatus status) || !(parameter is string buttonName))
                return false;

            // 初始化按钮
            if (buttonName == "Init")
                return status == RunStatus.Stop;

            // 开始按钮
            if (buttonName == "Start")
                return status == RunStatus.Init || status == RunStatus.Pause;

            // 暂停按钮
            if (buttonName == "Pause")
                return status == RunStatus.Running;

            // 停止按钮
            if (buttonName == "Stop")
                return status == RunStatus.Running || status == RunStatus.Pause;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}