using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.DaoAdmin;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Tests
{
    public class XmlFileMonitorService : IMonitorService, IDisposable
    {
        private readonly ConcurrentDictionary<string, (PhysicalFileProvider FileProvider, IDisposable Registration)> _monitors;
        private readonly ConcurrentDictionary<string, FileState> _fileStates;
        private bool _isDisposed;

        public event EventHandler<XmlFileChangedEventArgs> XmlFileChanged;

        public XmlFileMonitorService()
        {
            _monitors = new ConcurrentDictionary<string, (PhysicalFileProvider, IDisposable)>();
            _fileStates = new ConcurrentDictionary<string, FileState>();
        }

        public void StartMonitoring()
        {
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
                    // 初始化文件状态缓存
                    InitializeFileStates(directory);
                    StartMonitoringDirectory(directory);
                }
            }
        }

        private void InitializeFileStates(string directoryPath)
        {
            try
            {
                var xmlFiles = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);
                foreach (var filePath in xmlFiles)
                {
                    var content = SafeReadFileContent(filePath);
                    _fileStates[filePath] = new FileState
                    {
                        LastContent = content,
                        LastHash = ComputeContentHash(content),
                        LastWriteTime = File.GetLastWriteTime(filePath)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化文件状态失败: {ex.Message}");
            }
        }

        private void StartMonitoringDirectory(string directoryPath)
        {
            if (_monitors.ContainsKey(directoryPath)) return;

            try
            {
                var fileProvider = new PhysicalFileProvider(directoryPath);

                // 为每个XML文件单独创建监控
                var xmlFiles = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);
                foreach (var filePath in xmlFiles)
                {
                    var fileName = Path.GetFileName(filePath);
                    var registration = ChangeToken.OnChange(
                        () => fileProvider.Watch(fileName), // 监控特定文件
                        () => OnFileChanged(filePath)       // 文件变化回调
                    );
                }

                // 同时监控目录级别的变化（新文件创建等）
                var directoryRegistration = ChangeToken.OnChange(
                    () => fileProvider.Watch("*.xml"),
                    () => OnDirectoryChanged(directoryPath)
                );

                _monitors[directoryPath] = (fileProvider, directoryRegistration);
                Console.WriteLine($"开始监控目录: {directoryPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"监控目录 {directoryPath} 失败: {ex.Message}");
            }
        }

        private void OnFileChanged(string filePath)
        {
            Thread.Sleep(200); // 确保文件写入完成

            try
            {
                if (!File.Exists(filePath))
                {
                    // 文件被删除
                    _fileStates.TryRemove(filePath, out _);
                    OnXmlFileChanged(filePath, WatcherChangeTypes.Deleted,
                                   _fileStates.TryGetValue(filePath, out var state) ? state.LastContent : null,
                                   null, "文件被删除");
                    return;
                }

                var newContent = SafeReadFileContent(filePath);
                var newHash = ComputeContentHash(newContent);
                var newWriteTime = File.GetLastWriteTime(filePath);

                // 检查是否真的发生了变化
                if (_fileStates.TryGetValue(filePath, out var oldState))
                {
                    if (oldState.LastHash == newHash &&
                        oldState.LastWriteTime == newWriteTime)
                    {
                        return; // 没有实际变化
                    }
                }

                // 获取旧内容
                string oldContent = oldState?.LastContent;

                // 比较具体的变化
                string changeDetails = CompareXmlChanges(oldContent, newContent);

                // 触发事件
                OnXmlFileChanged(filePath, WatcherChangeTypes.Changed, oldContent, newContent, changeDetails);

                // 更新状态
                _fileStates[filePath] = new FileState
                {
                    LastContent = newContent,
                    LastHash = newHash,
                    LastWriteTime = newWriteTime
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理文件变化时出错 {filePath}: {ex.Message}");
            }
        }

        private void OnDirectoryChanged(string directoryPath)
        {
            // 处理新文件创建等情况
            Thread.Sleep(100);

            try
            {
                var currentFiles = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);

                // 检查新文件
                foreach (var filePath in currentFiles)
                {
                    if (!_fileStates.ContainsKey(filePath))
                    {
                        // 新文件
                        var content = SafeReadFileContent(filePath);
                        _fileStates[filePath] = new FileState
                        {
                            LastContent = content,
                            LastHash = ComputeContentHash(content),
                            LastWriteTime = File.GetLastWriteTime(filePath)
                        };

                        OnXmlFileChanged(filePath, WatcherChangeTypes.Created, null, content, "新文件创建");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理目录变化时出错 {directoryPath}: {ex.Message}");
            }
        }

        private string CompareXmlChanges(string oldXml, string newXml)
        {
            if (string.IsNullOrEmpty(oldXml) || string.IsNullOrEmpty(newXml))
                return "无法比较，内容为空";

            try
            {
                // 简单的文本比较，您可以替换为更复杂的XML结构比较
                if (oldXml == newXml)
                    return "内容相同（时间戳变化）";

                // 这里可以添加更详细的XML差异分析
                var oldLines = oldXml.Split('\n');
                var newLines = newXml.Split('\n');

                var changes = new List<string>();
                for (int i = 0; i < Math.Min(oldLines.Length, newLines.Length); i++)
                {
                    if (oldLines[i] != newLines[i])
                    {
                        changes.Add($"第{i + 1}行: {oldLines[i].Trim()} → {newLines[i].Trim()}");
                    }
                }

                return changes.Any() ? string.Join("; ", changes.Take(3)) + (changes.Count > 3 ? "..." : "") : "格式变化";
            }
            catch
            {
                return "内容变化（解析失败）";
            }
        }

        private string SafeReadFileContent(string filePath)
        {
            try
            {
                // 尝试多次读取
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        return File.ReadAllText(filePath);
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(50);
                    }
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string ComputeContentHash(string content)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
                return Convert.ToBase64String(hash);
            }
        }

        protected virtual void OnXmlFileChanged(string filePath, WatcherChangeTypes changeType,
                                              string oldContent, string newContent, string changeDetails)
        {
            XmlFileChanged?.Invoke(this, new XmlFileChangedEventArgs(filePath, changeType, oldContent, newContent, changeDetails));
        }

        public void StopMonitoring()
        {
            foreach (var monitor in _monitors.Values)
            {
                monitor.Registration?.Dispose();
                monitor.FileProvider?.Dispose();
            }
            _monitors.Clear();
            _fileStates.Clear();
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            StopMonitoring();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        ~XmlFileMonitorService()
        {
            Dispose();
        }

        private class FileState
        {
            public string LastContent { get; set; }
            public string LastHash { get; set; }
            public DateTime LastWriteTime { get; set; }
        }
    }
}