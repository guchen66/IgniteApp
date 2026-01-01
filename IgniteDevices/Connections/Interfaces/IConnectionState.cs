using IgniteShared.Enums;
using Modbus.Device;

namespace IgniteDevices.Connections.Interfaces
{
    // 状态接口
    public interface IConnectionState
    {
        ConnectionResult Connect();

        bool IsConnected { get; }
        int Timeout { get; }
        ConnectionType Type { get; }

        IModbusMaster CreateModbusMaster();
    }
}