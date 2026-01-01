using System;

namespace IgniteDevices.PLC
{
    public class OmronManager
    {
        public static Action<string> AlarmChenged { get; set; }

        public static Action<AlarmMessage> AlarmErrorChenged { get; set; }
    }
}