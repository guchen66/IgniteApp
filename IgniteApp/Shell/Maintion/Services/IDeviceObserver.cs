using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Services
{
    /// <summary>
    /// 一键通知所有设备连接
    /// </summary>
    public interface IDeviceObserver
    {
        void UpdateState(DeviceState state);

        string DeviceType { get; }
    }

    public class DeviceState
    {
        public bool IsActive { get; set; }
        public string DeviceType { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}