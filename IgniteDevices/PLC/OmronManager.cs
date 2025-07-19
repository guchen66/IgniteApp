using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.PLC
{
    public class OmronManager
    {
        public static Action<string> AlarmChenged { get; set; }
    }
}