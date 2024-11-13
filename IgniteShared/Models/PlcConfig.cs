using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Models
{
    public class PlcConfig
    {
        public string IP { get; set; }
        public string Port { get; set; }
    }

    public class PlcBackResult
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }

    }
}
