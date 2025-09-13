using IgniteAdmin.Interfaces;
using IgniteApp.Bases;
using IgniteShared.Extensions;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.DaoTasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class ResistiveViewModel : BaseDeviceViewModel
    {
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(ResistiveViewModel));

        public ResistiveViewModel() : base("Resistance")
        {
            // 初始化代码
        }

        private TextBox _content;

        public TextBox Content
        {
            get => _content;
            set => SetAndNotify(ref _content, value);
        }

        private string _data;

        public string Data
        {
            get => _data;
            set => SetAndNotify(ref _data, value);
        }

        public void BreakUI()
        {
            // 完整的性能测试
            TangdaoTaskScheduler.Execute(dao: daoTask =>
            {
                Content = new TextBox();
                Content.Text = "qqq";

                Thread.Sleep(1000);
                Data = daoTask.Duration;
            });

            TangdaoTaskScheduler.Execute(daoAsync: daoTask =>
            {
            });

            TangdaoTaskScheduler.Execute(daoAsync => { }, dao => { });
        }

        public void GetData()
        {
            Task.Run(() =>
            {
                string cache = "Hello";
                Data = cache;
            });
        }
    }

    public class PathConverter
    {
        public static string GetAbsolutePath(string fromPath, string toPath)
        {
            return Path.Combine(fromPath, toPath);
        }

        public static string GetRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException(nameof(fromPath));
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException(nameof(toPath));

            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                // 不是同一种路径，无法转换成相对路径。
                return toPath;
            }

            if (fromUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase)
                && !fromPath.EndsWith("/", StringComparison.OrdinalIgnoreCase)
                && !fromPath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                // 如果是文件系统，则视来源路径为文件夹。
                fromUri = new Uri(fromPath + Path.DirectorySeparatorChar);
            }

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}