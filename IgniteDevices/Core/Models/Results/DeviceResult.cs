using IgniteDevices.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.Core.Models.Results
{
    /// <summary>
    /// 泛型扩展结果（支持设备特定数据）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeviceResult<T> : IDeviceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public T Data { get; set; }

        // 快速创建成功/失败结果
        public static DeviceResult<T> Success(T data) => new DeviceResult<T> { IsSuccess = true, Data = data };

        public static DeviceResult<T> Fail(string error) => new DeviceResult<T>() { Message = error };
    }
}