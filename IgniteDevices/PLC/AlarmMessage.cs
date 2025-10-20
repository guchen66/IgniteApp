using IgniteShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteDevices.PLC
{
    public class AlarmMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Solution { get; set; }
        public DateTime TriggerTime { get; } = DateTime.Now;
        public AlarmLevel AlarmLevel { get; set; }
    }
}