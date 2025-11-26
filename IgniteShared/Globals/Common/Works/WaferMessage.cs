using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Globals.Common.Works
{
    public class WaferMessage
    {
        public string CellId { get; }
        public DateTime EnterTime { get; }

        public WaferMessage(string cellId)
        {
            CellId = cellId;
            EnterTime = DateTime.Now;
        }
    }
}