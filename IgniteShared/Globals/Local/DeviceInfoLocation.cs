using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Globals.Local
{
    public class DeviceInfoLocation
    {
        public const string AxisInfoPath = "E://IgniteDatas//axisInfo.xml";

        public const string PlcInfoPath = "E://IgniteDatas//plcInfo.xml";

        /// <summary>
        ///注意操作SaveFileDialog的时候使用反斜杠，否则报错System.ArgumentException:“值不在预期的范围内
        /// </summary>
        public const string CameraPhotoPath = "E:\\IgniteDatas\\Images";
    }
}
