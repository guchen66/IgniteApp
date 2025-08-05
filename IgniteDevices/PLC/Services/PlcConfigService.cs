using IgniteShared.Globals.System;
using IgniteShared.Models;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IgniteDevices.PLC.Services
{
    public class PlcConfigService : IPlcConfigService
    {
        private readonly FileSystemWatcher _fileWatcher;
        private Dictionary<string, PlcConfig> _configs = new Dictionary<string, PlcConfig>();

        public PlcConfigService()
        {
            LoadConfig();
            // 配置热重载监听
            //_fileWatcher = new FileSystemWatcher
            //{
            //    Path = Path.GetDirectoryName(_configPath),
            //    Filter = Path.GetFileName(_configPath),
            //    NotifyFilter = NotifyFilters.LastWrite
            //};
            //_fileWatcher.Changed += OnConfigChanged;
            //_fileWatcher.EnableRaisingEvents = true;
        }

        public void ReloadConfig() => LoadConfig();

        public PlcConfig GetConfig(string plcName = "DefaultPlc")
        {
            lock (_configs)
            {
                return _configs.TryGetValue(plcName, out var config)
                    ? config
                    : throw new KeyNotFoundException($"PLC配置 {plcName} 不存在");
            }
        }

        public IReadOnlyDictionary<string, PlcConfig> GetAllConfigs()
        {
            lock (_configs)
            {
                return new ReadOnlyDictionary<string, PlcConfig>(_configs);
            }
        }

        private void LoadConfig()
        {
            lock (_configs)
            {
                var path = DirectoryHelper.SelectDirectoryByName("appsetting.json");
                string json = File.ReadAllText(path);
                var configCollection = JsonConvert.DeserializeObject<PlcConfigCollection>(json);
                _configs = configCollection?.PlcConfigs ?? throw new InvalidDataException("配置格式错误");
            }
        }

        private void OnConfigChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(100); // 避免文件被占用
            LoadConfig();
        }

        public void Dispose() => _fileWatcher?.Dispose();
    }

    public class PlcConfigCollection
    {
        public Dictionary<string, PlcConfig> PlcConfigs { get; set; }
    }
}