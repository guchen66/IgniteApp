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
            RegisterEvents();            //注册全局异常捕获
        }

        private void RegisterEvents()
        {
            ExceptionHandler handler= new ExceptionHandler();
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += handler.TaskScheduler_UnobservedTaskException;
            //UI线程未捕获异常处理事件（UI主线程）
            Application.DispatcherUnhandledException += handler.App_DispatcherUnhandledException;
            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += handler.CurrentDomain_UnhandledException;
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
            Container= container;

            return Container;
        }
        public static T GetService<T>()
        {
           return Container.Get<T>();
        }
    }
}
