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

namespace IgniteApp.Modules
{
    public class HomeModules : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<IViewFactory>().ToAbstractFactory();
            Bind<INavigateRoute>().To<NavigateRoute>().InSingletonScope();
            Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            Bind<IAutoRun>().To<AutoRun>().InSingletonScope();
            Bind<LoginViewModel>().ToSelf().InSingletonScope();
            Bind<RegisterViewModel>().ToSelf().InSingletonScope();
            Bind<TempAndHumClient>().ToSelf().InSingletonScope();
            Bind<ITaskController>().To<TaskController>().InSingletonScope();
            Bind<ITaskService>().To<TaskService>().InSingletonScope();
            Bind<TTForgeViewModel>().ToSelf().InSingletonScope();
            // Bind<DefaultViewModel>().ToSelf().InSingletonScope();
        }
    }
}