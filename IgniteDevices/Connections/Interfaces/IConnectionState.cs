using IgniteShared.Enums;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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