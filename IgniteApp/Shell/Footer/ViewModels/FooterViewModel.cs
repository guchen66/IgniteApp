using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Set.Models;
using IgniteShared.Dtos;
using IgniteShared.Globals.Common;
using IgniteShared.Globals.Local;
using IT.Tangdao.Bridge.Sockets;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Messaging;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Events;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Infrastructure;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
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

        // private readonly IPlcProvider _plcProvider;
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(FooterViewModel));

        #endregion

        #region--ctor--
        private IWindowManager _windowManager;
        private readonly System.Timers.Timer _statusTimer;
        private IDialogService _dialogService;
        private IContentAccess _contentAccess;
        private readonly ITangdaoSocket _tangdaoSocket;
        private readonly IActionTable _actionTable;
        private readonly ITangdaoPublisher _tangdaoPublisher;

        public FooterViewModel(IWindowManager windowManager, IDialogService dialogService, Func<TTForgeViewModel> viewModelFactory, IContentAccess contentAccess, ITangdaoSocket tangdaoSocket, IActionTable actionTable, ITangdaoPublisher tangdaoPublisher)
        {
            // _plcProvider = plcProvider;
            _windowManager = windowManager;
            _dialogService = dialogService;
            _viewModelFactory = viewModelFactory;
            _contentAccess = contentAccess;
            _tangdaoSocket = tangdaoSocket;
            _actionTable = actionTable;
            _tangdaoPublisher = tangdaoPublisher;
            // TTForgeViewModel = tTForgeViewModel;
            // 初始化定时器（间隔1秒，自动重置）
            // _statusTimer = new System.Timers.Timer(1000) { AutoReset = true };
            // _statusTimer.Elapsed += async (s, e) => await CheckPlcStatusAsync();
            // _statusTimer.Start();

            // 立即执行PLC连接首次检查
            // QueryPlcStatus();

            //_tangdaoChannel.Messages.ObserveOnDispatcher()
            //    .Subscribe(msg => ReceivedText += msg + Environment.NewLine);

            //// 错误
            //_tangdaoChannel.Errors.ObserveOnDispatcher()
            //         .Subscribe(ex => MessageBox.Show(ex.Message));
            QueryServerStatus();

            TangdaoWeakEvent.Instance.OnHandlerTableReceived += Instance_OnHandlerTableReceived;
        }

        private void Instance_OnHandlerTableReceived(object sender, HandlerTableEventArgs e)
        {
            if (e.Key == "Open")                       // 只关心自己的 key
            {
                var handler = e.HandlerTable.GetResultHandler("Open");
                var result = new ActionResult { Result = ActionStatus.Success };
                handler?.Invoke(result);               // 执行父之前注册的逻辑
            }
        }

        private void ExecuteOpen()
        {
            MessageBox.Show("打开弹窗");
        }

        #endregion

        //private async Task CheckPlcStatusAsync()
        //{
        //    var isConnected = _plcProvider.ConnectionSiglePLC().IsSuccess;
        //    await Execute.OnUIThreadAsync(() => IsConn = isConnected);
        //}

        public void Dispose()
        {
            _statusTimer?.Stop();
            _statusTimer?.Dispose();
        }

        public async void QueryServerStatus()
        {
            await _tangdaoSocket.ConnectAsync();
            IsConnServer = _tangdaoSocket.IsConnected;
            if (IsConnServer)
            {
                //await _tangdaoSocket.SendAsync("Hello");
                //Logger.WriteLocal($"客户端发送Hello");
                var rely = await _tangdaoSocket.ReceiveAsync();
                Logger.WriteLocal($"打印：{rely}");
            }
            else
            {
                Logger.WriteLocal($"通信失败");
            }
        }

        private string _bgColor;

        public string BgColor
        {
            get => _bgColor ?? _colorBuffer.Next;
            set => SetAndNotify(ref _bgColor, value);
        }

        // 颜色池
        private readonly CircularBuffer<string> _colorBuffer = new CircularBuffer<string>(
            new[] { "Green", "Orange", "DodgerBlue" });

        public void TestColor()
        {
            BgColor = _colorBuffer.GetNext();
        }

        public void TestMessage()
        {
            //  MessageEventArgs messageEventArgs = new MessageEventArgs(BgColor);
            //  TangdaoWeakEvent.Instance.Publish(messageEventArgs);
            //  TangdaoWeakEvent.Instance.Publish("222", messageEventArgs);
        }

        public void TestObserver()
        {
            //  MessageEventArgs messageEventArgs = new MessageEventArgs(BgColor);
            //_tangdaoPublisher.Publish(messageEventArgs);
        }

        public void TestNoticeAll()
        {
            MessageContext Context = new MessageContext();
            Context.CurrentTime = DateTime.Now;

            TangdaoMessenger.Instance.NotifyObservers(Context);
        }

        public void TestNotice()
        {
            MessageContext Context = new MessageContext();
            Context.CurrentTime = DateTime.Now;
            Context.Message = "单独数据改变";
            TangdaoMessenger.Instance.NotifyObserverByKey("LightViewModel", Context);
        }

        public void TestWindow()
        {
            Logger.WriteLocal($"FooterViewModel测试：{_actionTable.GetActionInfo().Count}");
            _actionTable.Register("Open", x =>
            {
                if (x.Result == ActionStatus.Success)
                {
                    MessageBox.Show("成功返回");
                    Logger.WriteLocal($"FooterViewModel测试弹窗成功返回后：{_actionTable.GetActionInfo().Count}");
                }
            });
            _windowManager.ShowDialog(TestViewModel);
            Logger.WriteLocal($"FooterViewModel测试打开后：{_actionTable.GetActionInfo().Count}");
        }

        public void Test2()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "ProcessItem.xml");
            var xmlData = _contentAccess.Default.Read(foldPath).Content;

            if (xmlData == null)
            {
                return;
            }

            var readResult = _contentAccess.Default.Read(foldPath).AsXml().SelectNodes<ProcessItem>();
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

        #region --命令--

        // public ICommand OpenCommand { get; set; }
        #endregion
    }
}