using IgniteApp.Bases;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Notices;
using IT.Tangdao.Framework.DaoTasks;
using IT.Tangdao.Framework.Extensions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class ResistiveViewModel : BaseDeviceViewModel
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(ResistiveViewModel));

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
                Data = daoTask.Duration();
            });

            TangdaoTaskScheduler.ExecuteAsyncTask(daoTask =>
            {
            });

            TangdaoTaskScheduler.ExecuteBackgroundThenUI(daoAsync => { }, dao => { });
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
}