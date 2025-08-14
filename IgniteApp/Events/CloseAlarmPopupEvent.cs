using IgniteDevices.PLC;
using IT.Tangdao.Framework.DaoEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Events
{
    public class CloseAlarmPopupEvent : DaoEventBase
    {
        public DateTime CurrentTime { get; set; }
    }

    public class OpenAlarmPopupEvent
    {
    }
}