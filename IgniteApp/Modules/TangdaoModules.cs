using AutoMapper;
using IgniteAdmin.Providers;
using IgniteApp.Interfaces;
using IgniteShared.Models;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Extensions;
using IgniteApp.Extensions;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.DaoDevices;
using System.IO.Ports;
using IgniteApp.ViewModels;
using IgniteApp.Shell.Maintion.ViewModels;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Events;
using StyletIoC.Creation;
using IgniteShared.Dtos;
using IgniteShared.Globals.Local;
using IgniteApp.Tests;
using IT.Tangdao.Framework.Ioc;
using IgniteAdmin.Interfaces;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Shell.ProcessParame.ViewModels;
using IT.Tangdao.Framework.Configurations;
using IT.Tangdao.Framework.Paths;

namespace IgniteApp.Modules
{
    public class TangdaoModules : StyletIoCModule
    {
        protected override void Load()
        {
            //简单的翻页导航，只用于翻页，没有提供拦截器
            Bind<ISingleNavigateView>().ToAllImplementations();
            //简单导航的路由功能
            Bind<ISingleRouter>().To<SingleRouter>();
            Bind<ISingleRouterDemo>().To<SingleRouterDemo>();
            //GlobalPhotoViewModel
            //注册导航，有拦截器功能
            Bind<ITangdaoRouter>().To<TangdaoRouter>().InSingletonScope();

            //注册转换器
            Bind<ITypeConvertService>().To<TypeConvertService>().InSingletonScope();
            Bind<FileMonitorConfig>().ToFactory(container =>
            {
                return new FileMonitorConfig
                {
                    MonitorRootPath = IgniteInfoLocation.AppData,
                    IncludeSubdirectories = true,
                    MonitorFileTypes = new List<DaoFileType>
                    {
                        DaoFileType.Xml,
                    },
                    DebounceMilliseconds = 800,
                    FileReadRetryCount = 3
                };
            }).InSingletonScope();
            // 2. 注册服务注册器本身
            //  Bind<ITangdaoServiceRegistrar>().To<StyletServiceRegistrar>().InSingletonScope();
            // 注册Tangdao监控服务
            Bind<IMonitorService>().To<FileMonitorService>().InSingletonScope();
            //注册Tangdao读写服务
            Bind<IWriteService>().To<WriteService>();
            Bind<IReadService>().To<ReadService>();
            // 正确的方式：为每个实现单独注册并指定Key
            Bind<ITangdaoPage>().To<DigitalSmartGaugeViewModel>().WithKey("DigitalSmartGauge");
            Bind<ITangdaoPage>().To<DifferentialGaugeViewModel>().WithKey("DifferentialGauge");
            Bind<ITangdaoPage>().To<VacuumGaugeViewModel>().WithKey("VacuumGauge");
            Bind<ITangdaoPage>().To<CO2TeachViewModel>().WithKey("CO2Teach");
            Bind<ITangdaoPage>().To<UVTeachViewModel>().WithKey("UVTeach");
            Bind<ITangdaoPage>().To<IRTeachViewModel>().WithKey("IRTeach");

            Bind<IPlcProvider>().To<PlcProvider>();
            Bind<IDeviceProvider>().To<DeviceProvider>().InSingletonScope();
            Bind<IDialogService>().To<DialogService>().InSingletonScope();
            //注册Tangdao事件聚合器
            Bind<IDaoEventAggregator>().To<DaoEventAggregator>().InSingletonScope();
            Bind<ITangdaoProvider>().To<TangdaoProvider>().InSingletonScope();
            // Bind<INavigateService>().To<NavigateService>().InSingletonScope();
            //   Bind<IServiceProvider>().To<ServiceProvider>().InSingletonScope();
            Bind<ITangdaoParameter>().To<TangdaoParameter>().WithKey("parameter").InSingletonScope();
            // Bind<IEventTransmit>().To<EventTransmit>().InSingletonScope();
            //  Bind<ITangdaoHandler>().To<TangdaoHandler>().InSingletonScope();
            //  Bind<ITangdaoMessage>().ToFactory(Builder);   //只执行一次
            Bind<TangdaoEventDispatcher>().ToSelf().InSingletonScope();//注册事件分发器是单例
                                                                       // 获取事件分发器和所有事件处理器
                                                                       // var dispatcher = ServerLocator.Current.Resolve<TangdaoEventDispatcher>();
                                                                       // var handlers = ServerLocator.Current.Resolve<ITangdaoHandler>();

            Bind(typeof(IReadProvider<>)).To(typeof(XmlReadProvider<>));

            //Fluent
        }

        private ITangdaoMessage Builder(IRegistrationContext context)
        {
            var s1 = context.GetAll<ITangdaoMessage>();
            return default;
        }
    }

    public class Test
    {
        public static MultiKeyDictionary<string> keyValuePairs = new MultiKeyDictionary<string>();

        public static MultiKeyDictionary<string> Print()
        {
            //keyValuePairs["Sample"] = "Test";
            keyValuePairs["SampleView"] = "Test";
            // keyValuePairs["SampleViewModel"] = "Test";

            return keyValuePairs;
        }
    }
}