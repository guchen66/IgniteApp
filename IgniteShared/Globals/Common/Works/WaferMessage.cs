using System;

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