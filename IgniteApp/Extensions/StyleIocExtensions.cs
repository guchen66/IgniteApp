using IgniteDevices.Connections;
using IgniteDevices.PLC.Services;
using Stylet;
using StyletIoC;
using StyletIoC.Creation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Extensions
{
    public static class StyleIocExtensions
    {
        public static Func<int> RegisterEvent;

        public static IBindTo Register(this IBindTo Bind, Func<int> action)
        {
            action.Invoke();
            return Bind;
        }

        public static IInScopeOrWithKeyOrAsWeakBinding ToCustom<T>(this IToAnyService service) where T : IRegistration
        {
            return service.ToFactory(c => c.Get<IRegistration>().GetGenerator()(c));
        }

        //public static IInScopeOrWithKeyOrAsWeakBinding ToConnectedInstance<T>(this IToAnyService service) where T : class, IDisposable
        //{
        //    return (IInScopeOrWithKeyOrAsWeakBinding)service.ToFactory(container =>
        //    {
        //        // 获取配置服务
        //        var configService = container.Get<IPlcConfigService>();

        //        // 创建并连接实例
        //        if (typeof(T) == typeof(ConnectionContext))
        //        {
        //            var context = new ConnectionContext(configService.GetConfig());
        //            context.Connect();
        //            return context;
        //        }

        //        throw new InvalidOperationException($"不支持的类型: {typeof(T).Name}");
        //    }).InSingletonScope();
        //}
    }
}