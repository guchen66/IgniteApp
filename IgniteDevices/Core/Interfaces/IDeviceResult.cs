using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.Core.Interfaces
{
    /// <summary>
    /// 基础结果接口（所有设备通用）
    /// </summary>
    public interface IDeviceResult
    {
        bool IsSuccess { get; }
        string Message { get; }
        DateTime Timestamp { get; }
    }
}