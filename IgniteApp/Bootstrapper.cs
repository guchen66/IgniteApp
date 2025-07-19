using System;
using Stylet;
using StyletIoC;
using IgniteApp.ViewModels;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Threading;
using IT.Tangdao.Framework.DaoAdmin;
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
using IgniteShared.Extensions;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Common;
using IgniteDevices.PLC;

namespace IgniteApp
{
    public class Bootstrapper : Bootstrapper<LoginViewModel>
    {
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(Bootstrapper));

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.AddModule(new TangdaoModules());
            builder.AddModule(new HomeModules());
            builder.AddModule(new SqliteModules());
            builder.AddModule(new AutoModules());
            // 只有真正需要全局唯一的对象才注册为单例
            builder.Bind<AlarmPublisher>().ToSelf().InSingletonScope();
            builder.Bind<AlarmPopupViewModel>().ToSelf().InSingletonScope(); // 弹窗VM单例
                                                                             // builder.Bind<IObserver<AlarmMessage>>().To<AlarmPopupNotifier>();
                                                                             // 其他对象保持瞬态（Transient）或根据需要注册
            builder.Bind<AlarmPopupNotifier>().ToSelf(); // 非单例
            Logger.WriteLocal("注册成功");
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

        protected override void OnStart()
        {
            base.OnStart();
            RegisterExceptionEvents();            //注册全局异常捕获
            RegisterWCFEvent();                   //注册WCF事件
            RegisterAutoMapper();
        }

        private void RegisterExceptionEvents()
        {
            ExceptionHandler handler = new ExceptionHandler();
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

        private void RegisterAutoMapper()
        {
        }
    }

    /// <summary>
    /// 自定义服务定位器
    /// </summary>
    public class ServiceLocator
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