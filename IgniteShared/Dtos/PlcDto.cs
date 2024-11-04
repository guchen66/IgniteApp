using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Dtos
{
    public class PlcDto
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string PlcType { get; set; }

        public string PlcName { get; set; }
    }
}
