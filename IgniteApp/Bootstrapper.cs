using IgniteAdmin.Managers.Transmit;
using IgniteApp.Assets.Themes;
using IgniteApp.Common;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Modules;
using IgniteApp.Shell.Maintion.ViewModels;
using IgniteApp.ViewModels;
using IgniteApp.Views;
using IgniteShared.Globals.Local;
using IT.Tangdao.Bridge.Enums;
using IT.Tangdao.Bridge.Infrastructure;
using IT.Tangdao.Bridge.Sockets;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Messaging;
using IT.Tangdao.Framework.Configurations;
using IT.Tangdao.Framework.Events;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using StyletIoC;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Yitter.IdGenerator;

namespace IgniteApp
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(Bootstrapper));

        private TcpTangdaoSocket TCP;

        protected override void OnStart()
        {
            base.OnStart();
            RegisterExceptionEvents();            //注册全局异常捕获
            RegisterWCFEvent();                   //注册WCF事件
            RegisterAutoMapper();
            ComboboxOptions.SetTheme();
            // throw new NotImplementedException();
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // LogPathConfig.SetRoot($@"{IgniteInfoLocation.Logger}");

            LogEntry logEntry = new LogEntry()
            {
                SaveDir = IgniteInfoLocation.Logger
            };

            LogEnsureConfig.Load(logEntry);
            //  builder.Assemblies = builder.Assemblies.Distinct().ToList();
            builder.AddModule(new TangdaoModules());
            builder.AddModule(new HomeModules());
            builder.AddModule(new SqliteModules());
            builder.AddModule(new AutoModules());
            // 只有真正需要全局唯一的对象才注册为单例
            builder.Bind<AlarmPopupViewModel>().ToSelf().InSingletonScope();
            //  builder.Bind<LoginView>().ToSelf().InSingletonScope();
            //string connUri = "tcp://127.0.0.1:8970";
            //var uri = new TangdaoUri(connUri);
            // TCP = new TcpTangdaoSocket(NetMode.Client, uri);//我是客户端，我要连接的服务端是connUri

            //  builder.Bind<ITangdaoSocket>().ToInstance(TCP);

            Logger.Info($"注册成功");
        }

        protected override void Configure()
        {
            Logger.WriteLocal($"{Container.GetHashCode()}");
            ServiceLocator.Init(Container);
            // 配置雪花Id算法机器码
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions
            {
                WorkerId = 1,// 取值范围0~63,默认1
                // DataCenterId=1,//数据中心Id
            });
        }

        public override Window GetActiveWindow()
        {
            return Container.Get<MainView>();
        }

        protected override void DisplayRootView(object rootViewModel)
        {
            var loginViewModel = Container.Get<LoginViewModel>();
            var count = Application.Current.Windows.Count;
            var result = Container.Get<IWindowManager>().ShowDialog(loginViewModel);

            if (result == false)
            {
                System.Windows.Application.Current?.Shutdown();
            }
            else
            {
                var s1 = System.Windows.Application.Current.MainWindow;
                count = Application.Current.Windows.Count;
                base.DisplayRootView(rootViewModel);
                var s2 = System.Windows.Application.Current.MainWindow;
            }
        }

        protected override async void OnLaunch()
        {
            base.OnLaunch();

            Logger.WriteLocal($"{Container.GetHashCode()}");
            // 启动监控服务
            var monitorService = Container.Get<IFileMonitor>();
            monitorService.FileChanged += OnFileChanged;
            monitorService.StartMonitoring();
        }

        private void OnFileChanged(object sender, DaoFileChangedEventArgs e)
        {
            Logger.WriteLocal($"文件变化: {e.FilePath}, 变化类型: {e.ChangeType},{Environment.NewLine}变化详情：{e.ChangeDetails}，{Environment.NewLine}old:{e.OldContent},{Environment.NewLine}new:{e.NewContent}");
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

            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        }

        private static int count = 1;

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Console.WriteLine($"第{count}次异常信息名称：{e.Exception.GetType().Name}");
            Console.WriteLine($"第{count}次异常信息：{e.Exception.Message}");
            count++;
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

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
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