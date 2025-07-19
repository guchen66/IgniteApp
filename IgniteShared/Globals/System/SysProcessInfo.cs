using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Globals.System
{
    public class SysProcessInfo
    {
        public static bool IsInitFinish {  get; set; }
        public static bool IsAuto {  get; set; }
        public static bool IsRunning { get; set; }
        public static bool IsCannel {  get; set; }
    }
}
