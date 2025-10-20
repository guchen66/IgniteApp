using IgniteAdmin.Providers;
using IgniteApp.Interfaces;
using IgniteApp.Shell.ProcessParame.Models;
using IgniteApp.Shell.Footer.ViewModels;
using IgniteApp.Shell.Home.ViewModels;
using IgniteApp.ViewModels;
using IgniteDb.IRepositorys;
using IgniteDb.Repositorys;
using IgniteDevices.TempAndHum;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Shell.ProcessParame.Services;
using IgniteApp.Shell.Maintion.Models;
using IgniteDevices.PLC.Services;
using IgniteDevices.Connections.Interfaces;
using IgniteDevices.Connections;
using IgniteShared.Enums;
using IT.Tangdao.Framework.Extensions;
using AutoMapper;
using System.Runtime.Remoting.Contexts;
using System.Data;
using StyletIoC.Creation;
using System.Linq.Expressions;
using IgniteApp.Extensions;
using Unity.Injection;
using System.ComponentModel;
using IContainer = StyletIoC.IContainer;
using IgniteApp.Dialogs.Manage;
using IgniteApp.Tests;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IgniteApp.Bases;
using IgniteApp.Shell.Maintion.ViewModels;
using IgniteApp.Shell.Set.ViewModels;
using IgniteApp.Shell.Maintion.Services;

namespace IgniteApp.Modules
{
    public class HomeModules : StyletIoCModule
    {
        protected override void Load()
        {
            #region--View-ViewModel--
            Bind<ElectViewModel>().ToSelf().InSingletonScope();
            Bind<LightViewModel>().ToSelf().InSingletonScope();
            Bind<ResistiveViewModel>().ToSelf().InSingletonScope();
            Bind<HardawreSetViewModel>().ToSelf().InSingletonScope();
            Bind<DefaultViewModel>().ToSelf().InSingletonScope();
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
            Bind<RegisterViewModel>().ToSelf().InSingletonScope();
            Bind<IPlcConfigService>().To<PlcConfigService>().InSingletonScope();
            Bind<TempAndHumClient>().ToSelf().InSingletonScope();
            Bind<ITaskController>().To<TaskController>().InSingletonScope();
            Bind<ITaskService>().To<TaskService>().InSingletonScope();
            Bind<TTForgeViewModel>().ToSelf().InSingletonScope();

            Bind<ConnectionContext>().ToSelf().InSingletonScope();
            Bind<IConnectionState>().ToFactory(GetConnectionObject).InSingletonScope();
            // 注册通信器
            Bind<IPlcCommunicator>().To<PlcModbusCommunicator>().InSingletonScope();
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