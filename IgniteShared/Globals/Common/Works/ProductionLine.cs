using System.Threading.Channels;

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