using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteShared.Extensions
{
    public static class LoggerExtensions
    {
        public static void WriteLocal1(this ITangdaoLogger daoLogger, string message, string category = null)
        {
            try
            {
                // 确定日志目录路径
                var logDirectory = string.IsNullOrEmpty(category)
                    ? IgniteInfoLocation.Logger
                    : Path.Combine(IgniteInfoLocation.Logger, category);

                // 确保目录存在
                Directory.CreateDirectory(logDirectory);

                // 格式化日志消息
                var timestamp = DateTime.Now;
                var formattedMessage = $"{message}    {timestamp:F}{Environment.NewLine}";

                // 构建日志文件路径
                var logFileName = $"{timestamp:yyyyMMdd}.log";
                var logFilePath = Path.Combine(logDirectory, logFileName);

                // 使用更高效的文件追加方法
                File.AppendAllText(logFilePath, formattedMessage);
            }
            catch (Exception ex)
            {
                // 在实际应用中，可以考虑将错误记录到其他位置或通知管理员
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        // 可以移除这个重载方法，因为上面的方法已经处理了category为null的情况
        public static void WriteLocal2(this ITangdaoLogger logger, string category, string message, Exception e = null)
        {
            try
            {
                // 确保目录存在
                LogDirectoryHelper.EnsureDirectoryExists();

                // 构建日志内容
                message += Environment.NewLine;
                if (e != null)
                {
                    message = message + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine;
                }
                var s = Environment.CurrentManagedThreadId;
                message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} " +
                         $"[{Environment.CurrentManagedThreadId}] " +
                         category.ToUpper() + " " +
                         logger.GetType().FullName +
                         Environment.NewLine + message + Environment.NewLine;

                // 写入文件
                File.AppendAllText(LogDirectoryHelper.GetLogFilePath(), message);

                // 保留原有调试输出
                System.Diagnostics.Debug.Write(message);
            }
            catch (Exception ex)
            {
                // 如果结构化日志失败，回退到原始桌面日志
                System.Diagnostics.Debug.WriteLine($"Failed to write structured log: {ex}");
                logger.Info(message, e);
            }
        }
    }

    public static class LogDirectoryHelper
    {
        public static string GetSolutionName()
        {
            // 通过反射获取程序集名称作为解决方案名称
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            return assembly.GetName().Name;
        }

        public static string GetLogDirectoryPath()
        {
            // E:\Logger\{解决方案名称}\{当前日期}
            string solutionName = GetSolutionName();
            string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");

            return Path.Combine(
                "E:",
                "Logger",
                solutionName,
                dateFolder);
        }

        public static string GetLogFilePath()
        {
            string directory = GetLogDirectoryPath();
            string fileName = $"{DateTime.Now:yyyy-MM-dd}.log";
            var s1 = Path.Combine(directory, fileName);
            return Path.Combine(directory, fileName);
        }

        public static void EnsureDirectoryExists()
        {
            string dir = GetLogDirectoryPath();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}