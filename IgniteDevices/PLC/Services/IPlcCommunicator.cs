using IgniteDevices.Core.Models;
using IgniteDevices.Core.Models.Results;
using IT.Tangdao.Framework.Abstractions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.PLC.Services
{
    /// <summary>
    /// 用于读写数据
    /// </summary>
    public interface IPlcCommunicator
    {
        // 精准读取单个寄存器
        ResponseResult<ushort> ReadSingleRegister(ushort address);

        // 范围读取（起始地址 + 寄存器数量）
        ResponseResult<PlcData> ReadRegisterRange(ushort startAddress, ushort numberOfPoints);

        // 批量写入（优化后）
        void WriteRegisters(ushort registerAddress, ushort value);
    }
}