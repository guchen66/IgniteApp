using System;
using Stylet;
using StyletIoC;
using IgniteApp.ViewModels;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Threading;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IgniteApp.Modules;
using Yitter.IdGenerator;
using IgniteAdmin.Managers.Transmit;
using System.Threading;
using IgniteShared.Models;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using IgniteDevices.TempAndHum;
using IgniteApp.Extensions;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Common;
using IgniteDevices.PLC;
using IgniteApp.Shell.Footer.ViewModels;
using IgniteApp.Tests;
using IT.Tangdao.Framework.Events;
using IT.Tangdao.Framework;
using System.Windows.Documents;
using System.Collections.Generic;
using HandyControl.Data.Enum;
using IT.Tangdao.Framework.Enums;
using System.Security.Policy;
using System.Runtime.Remoting.Contexts;
using IT.Tangdao.Framework.EventArg;
using IT.Tangdao.Framework.Infrastructure;
using IgniteApp.Assets.Themes;
using IT.Tangdao.Framework.Extensions;
using IgniteShared.Globals.Local;
using System.Windows.Media.Media3D;
using System.IO;
using System.Configuration;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Maintion.Services;
using IT.Tangdao.Framework.Abstractions.Notices;
using System.Linq;
using IT.Tangdao.Bridge.Infrastructure;
using IT.Tangdao.Bridge.Sockets;
using IT.Tangdao.Bridge.Enums;
using IT.Tangdao.Framework.Configurations;

namespace IgniteApp
{
    public class Bootstrapper : Bootstrapper<LoginViewModel>
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(Bootstrapper));

        private TcpTangdaoSocket TCP;
        private static IDeviceRegistry _deviceRegistry;

        /// <summary>
        /// 使用自定义设备提供者配置
        /// </summary>
        public static void Configure(IDeviceProvider deviceProvider)
        {
            _deviceRegistry = new DeviceRegistry(deviceProvider);
            _deviceRegistry.RegisterAll();
        }

        public static IDeviceRegistry DeviceRegistry => _deviceRegistry;

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            LogPathConfig.SetRoot($@"{IgniteInfoLocation.Logger}");
            //  builder.Assemblies = builder.Assemblies.Distinct().ToList();
            builder.AddModule(new TangdaoModules());
            builder.AddModule(new HomeModules());
            builder.AddModule(new SqliteModules());
            builder.AddModule(new AutoModules());
            // 只有真正需要全局唯一的对象才注册为单例
            builder.Bind<AlarmPublisher>().ToSelf().InSingletonScope();
            builder.Bind<AlarmPopupViewModel>().ToSelf().InSingletonScope();
            builder.Bind<AlarmPopupNotifier>().ToSelf();

            string connUri = "tcp://127.0.0.1:8970";
            var uri = new TangdaoUri(connUri);
            TCP = new TcpTangdaoSocket(NetMode.Client, uri);//我是客户端，我要连接的服务端是connUri

            builder.Bind<ITangdaoSocket>().ToInstance(TCP);

            Logger.WriteLocal($"注册成功");
        }

        protected override void Configure()
        {
            ServiceLocator.Init(Container);
            // 配置雪花Id算法机器码
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions
            {
                WorkerId = 1,// 取值范围0~63,默认1
                // DataCenterId=1,//数据中心Id
            });
        }

        protected override async void OnLaunch()
        {
            base.OnLaunch();

            // 创建设备工厂
            IDeviceFactory deviceFactory = new DefaultDeviceFactory(Container);

            // 创建设备提供者
            IDeviceProvider deviceProvider = new DefaultDeviceProvider(deviceFactory);

            // 创建设备注册表
            _deviceRegistry = new DeviceRegistry(deviceProvider);

            // 注册所有设备
            _deviceRegistry.RegisterAll();

            //NoticeMediator.Instance.UseNoticeKit().Add("Badge").Add("Alert");

            // 启动监控服务
            var monitorService = Container.Get<IFileMonitor>();
            monitorService.FileChanged += OnFileChanged;
            monitorService.StartMonitoring();
        }

        private void OnFileChanged(object sender, DaoFileChangedEventArgs e)
        {
            Logger.WriteLocal($"文件变化: {e.FilePath}, 变化类型: {e.ChangeType},{Environment.NewLine}变化详情：{e.ChangeDetails}，{Environment.NewLine}old:{e.OldContent},{Environment.NewLine}new:{e.NewContent}");
        }

        protected override void OnStart()
        {
            base.OnStart();
            RegisterExceptionEvents();            //注册全局异常捕获
            RegisterWCFEvent();                   //注册WCF事件
            RegisterAutoMapper();
            ComboboxOptions.SetTheme();
            // throw new NotImplementedException();
        }

        private void RegisterExceptionEvents()
        {
            TangdaoExceptionHandler handler = new TangdaoExceptionHandler();
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += handler.TaskScheduler_UnobservedTaskException;
            //UI线程未捕获异常处理事件（UI主线程）
            Application.DispatcherUnhandledException += handler.App_DispatcherUnhandledException;
            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += handler.CurrentDomain_UnhandledException;
        }

        private void RegisterWCFEvent()
        {
            var isStart = WcfTransmitManager.StartWcf();
            Thread thread = new Thread(() =>
            {
            });
            if (isStart)
            {
                thread.Start();
            }
        }

        private async void RegisterAutoMapper()
        {
            await Task.Delay(1000).ConfigureAwait(true);
        }
    }

    /// <summary>
    /// 自定义服务定位器
    /// </summary>
    public static class ServiceLocator
    {
        public static IContainer Container { get; set; }

        public static IContainer Init(IContainer container)
        {
            Container = container;

            return Container;
        }

        public static T GetService<T>()
        {
            return Container.Get<T>();
        }
    }
}