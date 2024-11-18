using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Globals.System
{
    public class SysTempAndHum
    {
        public static double Temp {  get; set; }
        public static double Hum { get; set;}
        public static bool IsConnTemp {  get; set; }
        public static bool IsConnHum { get;  set; }
    }
}
