using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.DaoAdmin;
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
        public static void WriteLocal(this IDaoLogger daoLogger, string message)
        {
            var currentTime = DateTime.Now.ToString("yyyyMMdd"); // 使用yyyyMMdd格式以避免文件名中的斜杠
            // 添加换行符
            message = $"{message}{DateTime.Now.ToString("F")}{Environment.NewLine}";

            // 获取日志文件的完整路径
            var logDirectory = IgniteInfoLocation.LoggerPath;

            // 检查日志目录是否存在，如果不存在则创建
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // 获取当前日期作为日志文件名的一部分

            // 日志文件的完整路径
            var logFilePath = Path.Combine(logDirectory, $"{currentTime}.log");

            // 将消息追加到日志文件
            File.AppendAllText(logFilePath, message);
        }

        public static void WriteLocal(this IDaoLogger logger, string category, string message, Exception e = null)
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

                message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} " +
                         $"[{Thread.CurrentThread.ManagedThreadId}] " +
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