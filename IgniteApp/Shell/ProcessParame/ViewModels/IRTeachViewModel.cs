using HandyControl.Controls;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class IRTeachViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "";
        private string _responseData;

        public string ResponseData
        {
            get => _responseData;
            set => SetAndNotify(ref _responseData, value);
        }

        public IRTeachViewModel()
        {
            ExecuteBinCommand = MinidaoCommand.Create(ExecuteBin);
            OpenBinCommand = MinidaoCommand.Create(OpenBin);
        }

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
            ResponseData = parameter.Get<string>("Name");
        }

        private const string BinPath = "hello.bin";

        public void ExecuteBin()
        {
            string origin = "Hello,World";
            var path = Path.Combine(IgniteInfoLocation.Profiles, BinPath);

            path.UseBinaryWriteString(origin);

            MessageBox.Show("快照已生成：" + Path.GetFullPath(IgniteInfoLocation.Profiles));
        }

        public void OpenBin()
        {
            var path = Path.Combine(IgniteInfoLocation.Profiles, BinPath);
            // 链式调用：读取内容
            ResponseData = path.UseBinaryReadString();

            if (ResponseData == null)
            {
                MessageBox.Show("先点第一个按钮！");
                return;
            }

            MessageBox.Show($"读回内容：{ResponseData}");
        }

        #region--命令--
        public ICommand ExecuteBinCommand { get; set; }
        public ICommand OpenBinCommand { get; set; }
        public ICommand MenuCommand { get; set; }
        #endregion
    }
}