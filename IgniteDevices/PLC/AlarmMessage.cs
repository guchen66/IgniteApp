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
        public string Name { get; set; }      // 报警名称（对应PLC的name参数）
        public DateTime TriggerTime { get; } = DateTime.Now;
        public bool IsCritical { get; set; } // 可扩展其他属性
    }
}