using IgniteAdmin.Providers;
using IgniteAdmin.Workers;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Aside.ViewModels;
using IgniteApp.Shell.Footer.ViewModels;
using IgniteApp.Shell.Home.ViewModels;
using IgniteApp.Shell.Maintion.Models;
using IgniteApp.Shell.Maintion.ViewModels;
using IgniteApp.Shell.ProcessParame.Services;
using IgniteApp.Shell.Set.ViewModels;
using IgniteApp.ViewModels;
using IgniteDevices.Connections;
using IgniteDevices.Connections.Interfaces;
using IgniteDevices.PLC.Services;
using IgniteDevices.TempAndHum;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using StyletIoC;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using IContainer = StyletIoC.IContainer;

namespace IgniteApp.Modules
{
    public class HomeModules : StyletIoCModule
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(HomeModules));

        protected override void Load()
        {
            #region--硬件界面ViewModel--
            Bind<ElectViewModel>().ToSelf().InSingletonScope();
            Bind<LightViewModel>().ToSelf().InSingletonScope();
            Bind<ResistiveViewModel>().ToSelf().InSingletonScope();
            Bind<PressureViewModel>().ToSelf().InSingletonScope();
            Bind<HardawreSetViewModel>().ToSelf().InSingletonScope();
            Bind<DefaultViewModel>().ToSelf().InSingletonScope();
            Bind<TempAndHumViewModel>().ToSelf().InSingletonScope();
            //HardawreSetViewModel
            #endregion

            #region--服务接口--

            #endregion

            Bind<IViewFactory>().ToAbstractFactory();

            Bind<INavigateRoute>().To<NavigateRoute>().InSingletonScope();
            Bind<INavigationService>().To<NavigationService<IContainer>>().InSingletonScope();
            Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            Bind<IAutoRun>().To<AutoRun>().InSingletonScope();
            Bind<IElectService>().To<ElectService>().InSingletonScope();
            Bind<LoginViewModel>().ToSelf().InSingletonScope();
            Bind<AsideViewModel>().ToSelf().InSingletonScope();
            Bind<UserInfoViewModel>().ToSelf().InSingletonScope();
            Bind<RegisterViewModel>().ToSelf().InSingletonScope();
            Bind<IPlcConfigService>().To<PlcConfigService>().InSingletonScope();
            Bind<TempAndHumClient>().ToSelf().InSingletonScope();
            Bind<ITaskController>().To<TaskController>().InSingletonScope();
            Bind<ITaskService>().To<TaskService>().InSingletonScope();
            Bind<TTForgeViewModel>().ToSelf().InSingletonScope();

            //WorkstationBase
            // ① 碰类型

            var loaded = AppDomain.CurrentDomain.GetAssemblies()
                      .Select(a => a.GetName().Name)
                      .ToList();
            Logger.WriteLocal($"已加载程序集：{string.Join(", ", loaded)}");
            //  Assembly assembly= Assembly.GetExecutingAssembly();
            // Assembly.Load("IgniteAdmin");
            Bind<WorkstationBase>().ToAllImplementations(true, Assembly.Load("IgniteAdmin"));
            Bind<WorkstationManager>().ToSelf().InSingletonScope();
        }

        private IConnectionState GetConnectionObject(IContainer container)
        {
            var context = container.Get<ConnectionContext>();
            if (context.ConnectionResult.Message.Contains("串口"))
            {
                return context.ConnectionStates.OfType<SerialState>().First();
            }
            else
            {
                return context.ConnectionStates.OfType<TcpState>().First();
            }
        }
    }

    //public class ConnectionContextRegistration : IRegistration
    //{
    //    private readonly Func<IRegistrationContext, object> _generator;

    //    public RuntimeTypeHandle TypeHandle => typeof(ConnectionContext).TypeHandle;

    //    public ConnectionContextRegistration(IPlcConfigService configService)
    //    {
    //        var config = configService.GetConfig();
    //        var context = new ConnectionContext(config);
    //        context.Connect(); // 立即连接

    //        // 固定返回已连接的实例
    //        _generator = _ => context;
    //    }

    //    public Func<IRegistrationContext, object> GetGenerator() => _generator;

    //    public Expression GetInstanceExpression(ParameterExpression registrationContext)
    //    {
    //        return Expression.Constant(_generator(null));
    //    }
    //}
}