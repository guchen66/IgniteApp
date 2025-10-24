using IgniteAdmin.Providers;
using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Modules;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Set.Models;
using IgniteApp.ViewModels;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Extensions;
using IgniteShared.Globals.Local;
using IgniteShared.Models;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Sockets;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Unity;

namespace IgniteApp.Shell.Footer.ViewModels
{
    public class FooterViewModel : ViewModelBase
    {
        #region--属性--

        private PlcDto _plcDto;

        public PlcDto PlcDto
        {
            get => _plcDto;
            set => SetAndNotify(ref _plcDto, value);
        }

        private bool _isConn;

        public bool IsConn
        {
            get => _isConn;
            set => SetAndNotify(ref _isConn, value);
        }

        private bool _isConnServer;

        public bool IsConnServer
        {
            get => _isConnServer;
            set => SetAndNotify(ref _isConnServer, value);
        }

        private readonly IPlcProvider _plcProvider;
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(FooterViewModel));
        #endregion

        #region--ctor--
        private IWindowManager _windowManager;
        private readonly System.Timers.Timer _statusTimer;
        private IDialogService _dialogService;
        private IReadService _readService;
        private readonly ITangdaoChannel _channel;

        public FooterViewModel(IPlcProvider plcProvider, IWindowManager windowManager, IDialogService dialogService, Func<TTForgeViewModel> viewModelFactory, IReadService readService, ITangdaoChannel channel)
        {
            _plcProvider = plcProvider;
            _windowManager = windowManager;
            _dialogService = dialogService;
            _viewModelFactory = viewModelFactory;
            _readService = readService;
            _channel = channel;
            // TTForgeViewModel = tTForgeViewModel;
            // 初始化定时器（间隔1秒，自动重置）
            _statusTimer = new System.Timers.Timer(1000) { AutoReset = true };
            _statusTimer.Elapsed += async (s, e) => await CheckPlcStatusAsync();
            // _statusTimer.Start();

            // 立即执行PLC连接首次检查
            QueryPlcStatus();

            //_tangdaoChannel.Messages.ObserveOnDispatcher()
            //    .Subscribe(msg => ReceivedText += msg + Environment.NewLine);

            //// 错误
            //_tangdaoChannel.Errors.ObserveOnDispatcher()
            //         .Subscribe(ex => MessageBox.Show(ex.Message));
            QueryServerStatus();
        }

        #endregion

        private async Task CheckPlcStatusAsync()
        {
            var isConnected = _plcProvider.ConnectionSiglePLC().IsSuccess;
            await Execute.OnUIThreadAsync(() => IsConn = isConnected);
        }

        public void Dispose()
        {
            _statusTimer?.Stop();
            _statusTimer?.Dispose();
        }

        public async void QueryServerStatus()
        {
            //防止界面构建完成，异步连接未建立，所以应该等待异步连接成功，界面bool值在变化
            var result = await _channel.WaitConnectedAsync();
            if (result)
            {
                IsConnServer = true;
            }
        }

        public void QueryPlcStatus()
        {
            // 先订阅事件，再执行连接检查
            _plcProvider.Context.ConnectionStateChanged += Context_ConnectionStateChanged;

            Task.Run(() =>
            {
                IsConn = _plcProvider.ConnectionSiglePLC().IsSuccess;
            });
        }

        private void Context_ConnectionStateChanged(object sender, IgniteDevices.Connections.ConnectionStateEventArgs e)
        {
            IsConn = _plcProvider.ConnectionSiglePLC().IsSuccess;
            // Execute.OnUIThreadAsync(() => _plcProvider.Context.IsConnected = e.IsConnected);
        }

        public void Test()
        {
            // var wins = System.Windows.Application.Current.Windows;
            // var view = GetWindowFromViewModel(TestViewModel);
            SplitScreenManager.OpenOnSecondaryScreen(_windowManager, TestViewModel);
            var s = TestViewModel.View;
            // _windowManager.ShowDialog(TestViewModel);
            //_dialogService.Show(TestViewModel, result: (result) =>
            //{
            //    var str = result.ResultValue;
            //});
        }

        public void Test2()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "ProcessItem.xml");
            var xmlData = _readService.Default.Read(foldPath).Content;

            if (xmlData == null)
            {
                return;
            }
            //_readService.Current.Load(xmlData);
            var readResult = _readService.Default.AsXml().SelectNodes<ProcessItem>();
            if (readResult.IsSuccess)
            {
                var ProcessItems = new ObservableCollection<ProcessItem>(readResult.Data);
                if (ProcessItems.FirstOrDefault().IsFeeding)
                {
                }
            }
            var s = TestViewModel.View;
        }

        private Window GetWindowFromViewModel(object viewModel)
        {
            // 通过Application.Current.Windows查找对应的窗口
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == viewModel)
                {
                    return window;
                }
            }

            return null;
        }

        public void OpenEQP()
        {
        }

        [Inject]
        public TestViewModel TestViewModel { get; set; }

        [Inject]
        public ImageInfoCardViewModel ImageInfoCardViewModel { get; set; }

        [Inject]
        public TTForgeViewModel TTForgeViewModel;

        private readonly Func<TTForgeViewModel> _viewModelFactory;

        public void OpenTTView()
        {
            //TTForgeViewModel = ServiceLocator.GetService<TTForgeViewModel>();
            // TTForgeViewModel = _viewModelFactory.Invoke();
            // TTForgeViewModel = new TTForgeViewModel();
            Logger.WriteLocal(TTForgeViewModel.DisplayName);
            Logger.WriteLocal(TTForgeViewModel.IsActive.ToString());
            Logger.WriteLocal(TTForgeViewModel.ScreenState.ToString());
            Logger.WriteLocal(TTForgeViewModel.GetHashCode().ToString());
            _windowManager.ShowWindow(TTForgeViewModel);
        }

        [Inject]
        public GlobalPhotoViewModel GlobalPhotoViewModel { get; set; }

        public void OpenPhotoView()
        {
            // 获取窗口引用
            var win = GlobalPhotoViewModel.View as Window;

            // 情况1：窗口尚未创建或显示
            if (win == null || win.Visibility != Visibility.Visible)
            {
                _windowManager.ShowWindow(GlobalPhotoViewModel);
                return;
            }

            // 情况2：窗口已存在，需要激活并带到前台
            // 无论最小化还是被遮挡，都统一处理

            // 1. 确保可见（防止被隐藏）
            win.Visibility = Visibility.Visible;

            // 2. 如果是最小化，先还原到正常状态
            if (win.WindowState == WindowState.Minimized)
            {
                win.WindowState = WindowState.Normal;
            }

            // 3. 激活窗口并带到前台
            win.Activate();

            // 4. 可选：强制置顶一下（解决某些系统的激活限制）
            var originalTopmost = win.Topmost;
            win.Topmost = true;
            win.Topmost = originalTopmost;

            // 5. 尝试获取焦点（双重保险）
            win.Focus();
        }
    }
}