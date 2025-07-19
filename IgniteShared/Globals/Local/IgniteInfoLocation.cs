using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Globals.Local
{
    /// <summary>
    /// 保存的信息地址
    /// </summary>
    public class IgniteInfoLocation
    {
        /// <summary>
        /// 用户登录信息地址
        /// </summary>
        public const string UserInfoPath = "E://IgniteDatas//userInfo.xml";

        /// <summary>
        /// 流程保存地址
        /// </summary>
        public const string ProcessInfoPath = @"E://IgniteDatas//processInfo.xml";

        /// <summary>
        /// 轴参数保存地址
        /// </summary>
        public const string AxisInfoPath = "E://IgniteDatas//axisInfo.xml";

        public const string PlcInfoPath = "E://IgniteDatas//plcInfo.xml";

        public const string LoggerPath = "E://Loggers//IgniteDatas//";

        public const string ColorPath = "E://IgniteDatas//colorInfo.xml";

        /// <summary>
        ///注意操作SaveFileDialog的时候使用反斜杠，否则报错System.ArgumentException:“值不在预期的范围内
        /// </summary>
        public const string CameraPhotoPath = "E:\\IgniteDatas\\Images";
    }
}