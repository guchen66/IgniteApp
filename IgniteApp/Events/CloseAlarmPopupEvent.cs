using IgniteDevices.PLC;
using IT.Tangdao.Framework.Events;
using System;

namespace IgniteApp.Events
{
    public class CloseAlarmPopupEvent : DaoEventBase
    {
        public DateTime CurrentTime { get; set; }
    }

    public class OpenAlarmPopupEvent
    {
        public AlarmMessage AlarmMessage { get; set; }
    }
}