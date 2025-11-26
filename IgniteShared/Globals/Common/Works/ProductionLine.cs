using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IgniteShared.Globals.Common.Works
{
    public static class ProductionLine
    {
        // 创建工位间的通道
        public static readonly Channel<WaferMessage> LoadToPre = Channel.CreateUnbounded<WaferMessage>();

        public static readonly Channel<WaferMessage> PreToCut = Channel.CreateUnbounded<WaferMessage>();

        public static readonly Channel<WaferMessage> CutToUnload = Channel.CreateUnbounded<WaferMessage>();
    }
}