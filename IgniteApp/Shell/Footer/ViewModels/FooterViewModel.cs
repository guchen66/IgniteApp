using HandyControl.Controls;
using IgniteAdmin.Providers;
using IgniteApp.Bases;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Extensions;
using IgniteApp.Modules;
using IgniteApp.Shell.Home.Models;
using IgniteApp.ViewModels;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoCommands;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly IPlcProvider _plcProvider;

        #endregion

        #region--ctor--
        private IWindowManager _windowManager;
        private readonly System.Timers.Timer _statusTimer;

        public FooterViewModel(IPlcProvider plcProvider, IWindowManager windowManager)
        {
            _plcProvider = plcProvider;
            _windowManager = windowManager;
            // 初始化定时器（间隔1秒，自动重置）
            _statusTimer = new System.Timers.Timer(1000) { AutoReset = true };
            _statusTimer.Elapsed += async (s, e) => await CheckPlcStatusAsync();
            // _statusTimer.Start();

            // 立即执行首次检查
            // Task.Run(CheckPlcStatusAsync);
            QueryPlcStatus();
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
            _windowManager.ShowDialogEx(TestViewModel);
            // _windowManager.ShowWindow(ImageInfoCardViewModel);
        }

        public void OpenEQP()
        {
        }

        [Inject]
        public TestViewModel TestViewModel { get; set; }

        [Inject]
        public ImageInfoCardViewModel ImageInfoCardViewModel { get; set; }

        [Inject]
        public TTForgeViewModel tTForgeViewModel { get; set; }

        public void OpenTTView()
        {
            _windowManager.ShowWindow(tTForgeViewModel);
        }
    }
}