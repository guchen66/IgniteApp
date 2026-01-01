using IgniteShared.Models;
using System.Collections.Generic;

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