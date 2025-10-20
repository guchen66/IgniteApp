using HandyControl.Controls;
using IgniteApp.Shell.ProcessParame.Models;
using IgniteApp.Shell.ProcessParame.Services;
using IgniteApp.ViewModels;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Abstractions.Sockets;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class CO2TeachViewModel : Screen, ITangdaoPage
    {
        private string _noticeValue;

        public string NoticeValue
        {
            get => _noticeValue;
            set => SetAndNotify(ref _noticeValue, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetAndNotify(ref _isEnabled, value);
        }

        private string _text;

        public string Text
        {
            get => _text;
            set => SetAndNotify(ref _text, value);
        }

        private BindableCollection<TeachItem> _teachList;

        public BindableCollection<TeachItem> TeachList
        {
            get => _teachList;
            set => SetAndNotify(ref _teachList, value);
        }

        private bool _isStatusActive;
        private readonly ITangdaoChannel _channel;
        private readonly ITangdaoRequest _request;

        [DefaultValue(true)]
        public bool IsStatusActive
        {
            get => _isStatusActive;
            set => SetAndNotify(ref _isStatusActive, value);
        }

        public string PageTitle => "";
        private readonly IReadService _readService;

        public CO2TeachViewModel(IReadService readService, ITangdaoChannel channel, ITangdaoRequest request)
        {
            _readService = readService;
            _channel = channel;
            SetCommand = MinidaoCommand.Create(ExecuteSet);
            UnlockCommand = MinidaoCommand.Create<string>(ExecuteUnlock);
            StartCommand = MinidaoCommand.CreateFromTask(ExecuteStart);
            _request = request;

            TeachList = new BindableCollection<TeachItem>()
            {
                new TeachItem(1,2,3,4),
                new TeachItem(11,22,33,44),
                new TeachItem(5,6,7,8),
                new TeachItem(55,66,77,88),
            };
        }

        private string _loadText;

        public string LoadText
        {
            get => _loadText;
            set => SetAndNotify(ref _loadText, value);
        }

        private int _loadNumber;

        public int LoadNumber
        {
            get => _loadNumber;
            set => SetAndNotify(ref _loadNumber, value);
        }

        private async Task ExecuteStart()
        {
            var buffer = new CircularBuffer<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            const int JOB = 5;                 // 5 个任务
            var cd = new CountdownEvent(JOB);

            LoadText = "加载中...";
            for (int i = 0; i < JOB; i++)
            {
                int idx = i;                   // 捕获局部变量

                _ = Task.Run(() =>
                 {
                     // 模拟耗时 200~1200 ms
                     Thread.Sleep(RandomCompat.Shared.Next(200, 1200));
                     LoadNumber = buffer.Next;
                     // 每完成一个就“打卡”
                     cd.Signal();
                 });
            }

            // 等待所有任务打卡完毕，但**不阻塞主线程**
            await cd.WaitAsync();

            // 回到 UI 线程更新
            LoadText = $"全部加载完成！({JOB} 个)";
        }

        private void ExecuteUnlock(string name)
        {
            if (Text == "123")
            {
                IsEnabled = true;
            }
            else
            {
                MessageBox.Error($"密码:{Text}错误");
            }
        }

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedFrom()
        {
            //MessageBox.Ask("准备从CO2离开");
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
            if (_channel == null) return;
            if (_channel.IsConnected)
            {
                _isStatusActive = true;
            }
        }

        public void ExecuteSet()
        {
            IsStatusActive = !IsStatusActive;
        }

        protected override void OnClose()
        {
            base.OnClose();
            IsEnabled = false;
            //Text = null;
        }

        public ICommand UnlockCommand { get; set; }
        public ICommand SetCommand { get; set; }
        public ICommand StartCommand { get; set; }
    }
}