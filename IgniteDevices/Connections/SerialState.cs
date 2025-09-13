using IgniteDevices.Connections.Interfaces;
using IgniteShared.Enums;
using IgniteShared.Extensions;
using IT.Tangdao.Framework.Abstractions;
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
    public class SerialState : IConnectionState
    {
        private readonly string _portName;
        private readonly int _baudRate;
        private SerialPort _serialPort;
        public bool IsConnected => _serialPort?.IsOpen ?? false;
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(SerialState));

        public int Timeout { get; } = 500; // 串口超时更短
        public ConnectionType Type => ConnectionType.SerialPort;

        public SerialState(string portName, int baudRate)
        {
            _portName = portName;
            _baudRate = baudRate;
        }

        public ConnectionResult Connect()
        {
            try
            {
                Logger.WriteLocal($"尝试串口连接 {_portName}@{_baudRate}");
                _serialPort = new SerialPort(_portName, _baudRate)
                {
                    ReadTimeout = Timeout,
                    WriteTimeout = Timeout
                };
                _serialPort.Open();

                return _serialPort.IsOpen
                    ? new ConnectionResult(true, "串口连接成功")
                    : new ConnectionResult(false, "串口连接失败");
            }
            catch (Exception ex)
            {
                Logger.WriteLocal($"串口连接异常: {ex.Message}");
                return new ConnectionResult(false, $"串口错误: {ex.Message}");
            }
        }

        public IModbusMaster CreateModbusMaster()
        {
            return _serialPort != null && _serialPort.IsOpen
                ? ModbusSerialMaster.CreateRtu(_serialPort)
                : throw new InvalidOperationException("串口连接未建立");
        }

        public void Dispose()
        {
            _serialPort?.Dispose();
        }
    }
}