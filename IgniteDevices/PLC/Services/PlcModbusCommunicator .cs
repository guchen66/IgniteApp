using IgniteDevices.Connections;
using IgniteDevices.Connections.Interfaces;
using IgniteDevices.Core.Models;
using IgniteDevices.Core.Models.Results;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Results;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.PLC.Services
{
    public class PlcModbusCommunicator : IPlcCommunicator
    {
        private readonly IConnectionState _connectionStrategy;

        private IModbusMaster _master;
        private readonly ITangdaoLogger _logger = TangdaoLogger.Get(typeof(PlcModbusCommunicator));

        public PlcModbusCommunicator(ConnectionContext context)
        {
            _master = context.Master;
        }

        /// <summary>
        /// 默认从 从站1开始读取
        /// </summary>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public DeviceResult<PlcData> ReadRegisterRange(ushort startAddress, ushort length)
        {
            try
            {
                var rawData = _master.ReadHoldingRegisters(1, startAddress, length);
                var data = PlcData.ConvertToPlcData(rawData, startAddress, DataType.UInt16);
                return DeviceResult<PlcData>.Success(data: data, "1");
            }
            catch (Exception ex)
            {
                return DeviceResult<PlcData>.Failure($"地址[{startAddress}-{startAddress + length}]读取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 注意Modbus是从0开始的
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public DeviceResult<ushort> ReadSingleRegister(ushort address)
        {
            try
            {
                var value = _master.ReadHoldingRegisters(1, address, 1)[0];
                return DeviceResult<ushort>.Success(value, "1");
            }
            catch (Exception ex)
            {
                return DeviceResult<ushort>.Failure($"地址{address}读取失败: {ex.Message}");
            }
        }

        public void WriteRegisters(ushort registerAddress, ushort value)
        {
            if (_master == null) throw new InvalidOperationException("未建立连接");
            _master.WriteSingleRegister(1, registerAddress, value);
        }

        public void Dispose()
        {
            _master?.Dispose();
        }
    }
}