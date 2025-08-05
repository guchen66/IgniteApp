using IgniteDevices.Connections.Interfaces;
using IgniteShared.Enums;
using IgniteShared.Extensions;
using IT.Tangdao.Framework.DaoAdmin;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.Connections
{
    // TCP状态实现
    public class TcpState : IConnectionState
    {
        private readonly string _ip;
        private readonly int _port;
        private TcpClient _tcpClient;
        public bool IsConnected => _tcpClient?.Connected ?? false;
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(TcpState));

        public int Timeout { get; } = 1000; // 默认1秒超时
        public ConnectionType Type => ConnectionType.TCP;

        public TcpState(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public ConnectionResult Connect()
        {
            try
            {
                Logger.WriteLocal($"尝试TCP连接 {_ip}:{_port}");

                _tcpClient = new TcpClientWithTimeout(_ip, _port, Timeout).Connect();

                return _tcpClient.Connected
                    ? new ConnectionResult(true, "TCP连接成功")
                    : new ConnectionResult(false, "TCP连接失败");
            }
            catch (Exception ex)
            {
                Logger.WriteLocal($"TCP连接异常: {ex.Message}");
                return new ConnectionResult(false, $"TCP错误: {ex.Message}");
            }
        }

        public IModbusMaster CreateModbusMaster()
        {
            return _tcpClient != null && _tcpClient.Connected
                ? ModbusIpMaster.CreateIp(_tcpClient)
                : throw new InvalidOperationException("TCP连接未建立");
        }

        public void Dispose()
        {
            _tcpClient?.Dispose();
        }
    }
}