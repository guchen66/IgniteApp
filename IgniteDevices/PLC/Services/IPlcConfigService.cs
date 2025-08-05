using IgniteShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.PLC.Services
{
    /// <summary>
    /// PLC文件配置
    /// </summary>
    public interface IPlcConfigService
    {
        PlcConfig GetConfig(string plcName = "DefaultPlc");

        IReadOnlyDictionary<string, PlcConfig> GetAllConfigs();

        void ReloadConfig();
    }
}