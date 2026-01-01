using IgniteShared.Enums;
using System;

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