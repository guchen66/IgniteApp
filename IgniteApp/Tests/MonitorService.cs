using IgniteShared.Globals.Local;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace IgniteApp.Tests
{
    public class MonitorService : IMonitorService, IDisposable
    {
        private readonly ConcurrentDictionary<string, (PhysicalFileProvider FileProvider, IDisposable Registration)> _monitors;

        private bool _isDisposed;

        public event EventHandler<XmlFileChangedEventArgs> XmlFileChanged;

        public MonitorService()
        {
            _monitors = new ConcurrentDictionary<string, (PhysicalFileProvider, IDisposable)>();
        }

        public void StartMonitoring()
        {
            // 获取所有需要监控的目录
            var directories = new[]
            {
            IgniteInfoLocation.User,
            IgniteInfoLocation.Profiles,
            IgniteInfoLocation.Images,
            IgniteInfoLocation.Framework,
            IgniteInfoLocation.Recipe,
            IgniteInfoLocation.Common,
            IgniteInfoLocation.Cache,
            IgniteInfoLocation.Database,
            IgniteInfoLocation.Logger
        };

            foreach (var directory in directories)
            {
                if (Directory.Exists(directory))
                {
                    StartMonitoringDirectory(directory);
                }
            }
        }

        private void StartMonitoringDirectory(string directoryPath)
        {
            if (_monitors.ContainsKey(directoryPath)) return;

            try
            {
                var fileProvider = new PhysicalFileProvider(directoryPath);

                // 创建 XML 文件过滤器
                var changeToken = fileProvider.Watch("*.xml");

                var registration = ChangeToken.OnChange(
                    () => fileProvider.Watch("*.xml"),
                    () => OnDirectoryChanged(directoryPath)
                );

                _monitors[directoryPath] = (fileProvider, registration);
                Console.WriteLine($"开始监控目录: {directoryPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"监控目录 {directoryPath} 失败: {ex.Message}");
            }
        }

        private void OnDirectoryChanged(string directoryPath)
        {
            // 延迟处理，避免文件正在被写入
            Thread.Sleep(100);

            try
            {
                // 获取目录中所有 XML 文件
                var xmlFiles = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);

                foreach (var filePath in xmlFiles)
                {
                    // 这里可以添加更详细的变化检测逻辑
                    OnXmlFileChanged(filePath, WatcherChangeTypes.Changed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理目录变化时出错 {directoryPath}: {ex.Message}");
            }
        }

        protected virtual void OnXmlFileChanged(string filePath, WatcherChangeTypes changeType, string oldPath = null)
        {
            XmlFileChanged?.Invoke(this, new XmlFileChangedEventArgs(filePath, changeType, oldPath));
        }

        public void StopMonitoring()
        {
            foreach (var monitor in _monitors.Values)
            {
                monitor.Registration?.Dispose();
                monitor.FileProvider?.Dispose();
            }
            _monitors.Clear();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            StopMonitoring();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        ~MonitorService()
        {
            Dispose();
        }
    }
}