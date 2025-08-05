using AutoMapper;
using IgniteAdmin.Providers;
using IgniteApp.Interfaces;
using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
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
using IT.Tangdao.Framework.DaoEnums;
using IT.Tangdao.Framework.DaoDtos.Options;
using IT.Tangdao.Framework.DaoDevices;
using System.IO.Ports;
using IgniteApp.ViewModels;
using IgniteApp.Shell.Maintion.ViewModels;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoEvents;
using Microsoft.Extensions.DependencyInjection;
using StyletIoC.Creation;
using IgniteShared.Dtos;

namespace IgniteApp.Modules
{
    public class TangdaoModules : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<IWriteService>().To<WriteService>();
            Bind<IReadService>().To<ReadService>();//.WithKey("111");
            Bind<IPlcProvider>().To<PlcProvider>();
            Bind<IDeviceProvider>().To<DeviceProvider>().InSingletonScope();
            //ITangdaoProvider
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

            // 订阅所有事件处理器
            /*  foreach (var handler in handlers)
              {
                  dispatcher.Subscribe(handler);
              }*/
        }

        private ITangdaoMessage Builder(IRegistrationContext context)
        {
            var s1 = context.GetAll<ITangdaoMessage>();
            return default;
        }

        /*  private object Builder(reg container)
          {
              //var message = container.Get<ITangdaoMessage>();
              //TangdaoEventDispatcher dispatcher = container.Get<TangdaoEventDispatcher>();
              var handlers = container.Get<ITangdaoMessage>();
              //dispatcher.Subscribe(handlers);
              // ITangdaoParameter parameter = new TangdaoParameter();
              // parameter.Add("userName", "Hello");
              //  message.Response(parameter);

              return default;
          }
  */
    }
}