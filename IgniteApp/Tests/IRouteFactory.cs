using IgniteApp.Shell.Maintion.ViewModels;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.Navigates;
using IT.Tangdao.Framework.DaoAttributes;
using IT.Tangdao.Framework.DaoIoc;
using StyletIoC;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IgniteApp.Tests
{
    /// <summary>
    /// 将刀道框架服务注册到Stylet容器的注册器。
    /// </summary>
    public class StyletServiceRegistrar : ITangdaoServiceRegistrar
    {
        private readonly IStyletIoCBuilder _builder;

        public StyletServiceRegistrar(IStyletIoCBuilder builder)
        {
            _builder = builder;
        }

        public void RegisterFrameworkServices()
        {
            // 在这里集中注册您框架的所有服务
            // 这些原本可能是在您自研容器中注册的服务

            _builder.Bind<ITangdaoPage>().To<DigitalSmartGaugeViewModel>().WithKey("DigitalSmartGauge");
            _builder.Bind<ITangdaoPage>().To<DifferentialGaugeViewModel>().WithKey("DifferentialGauge");
            _builder.Bind<ITangdaoPage>().To<VacuumGaugeViewModel>().WithKey("VacuumGauge");
            _builder.Bind<ITangdaoRouter>().To<TangdaoRouter>().InSingletonScope(); // 注册路由器
            // 注册其他所有框架核心服务...
            // _builder.Bind<IMyService>().To<MyServiceImpl>();
            // _builder.Bind<IOtherService>().To<OtherService>();
        }
    }

    /// <summary>
    /// 刀道服务注册器。
    /// 负责将框架内部的服务注册到主IOC容器中。
    /// </summary>
    public interface ITangdaoServiceRegistrar
    {
        /// <summary>
        /// 向主容器注册所有框架需要的服务。
        /// </summary>
        void RegisterFrameworkServices();
    }

    public class RouteConfig
    {
    }

    public interface IRouteFactory
    {
        IRoute CreateRouteFromDefault();

        string RouteName { get; }
    }

    [Export(typeof(IRouteFactory))]
    public class RouteFactory : IRouteFactory
    {
        public string RouteName
        {
            get
            {
                return "";//RouteViewModel.CreateName();
            }
        }

        public IRoute CreateRouteFromDefault()
        {
            return new Route();
        }
    }

    public interface IRoute
    {
    }

    public class Route : IRouteEntry, IEnumerable<RouteBindable>, IEnumerable, IRoute
    {
        //public Route(RouteConfig routeConfig)
        //{
        //}
        private Lazy<List<RouteBindable>> _routeBindables = new Lazy<List<RouteBindable>>();

        public IEnumerator<RouteBindable> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public interface IRouteEntry : IEnumerable<RouteBindable>, IEnumerable, IRoute
    {
    }

    public class RouteBindable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class RouteViewModel
    {
        // 1) 延迟缓存：每个 T 只算一次名称
        private static readonly ConcurrentDictionary<Type, string> _cache = new ConcurrentDictionary<Type, string>();

        // 2) 默认规则：去掉 "ViewModel" 后缀，再 Pascal→Kebab
        private static readonly Func<Type, string> _defaultRule = type =>
            Regex.Replace(type.Name.Replace("ViewModel", ""), "([a-z])([A-Z])", "$1-$2")
                 .ToLowerInvariant();

        // 3) 可注入的命名策略（框架扩展点）
        private static Func<Type, string> _namingStrategy = _defaultRule;

        /// <summary>
        /// 动态计算名称；首次调用后缓存，后续 O(1)。
        /// </summary>
        public static string CreateName<T>() => _cache.GetOrAdd(typeof(T), _namingStrategy);

        /// <summary>
        /// 允许外部替换命名策略（例如本地化、重命名表）。
        /// </summary>
        public static void UseNamingStrategy(Func<Type, string> strategy) => _namingStrategy = strategy;
    }

    [Scanning()]
    public class Test1PageViewModel
    {
    }

    [Scanning()]
    public class Test2PageViewModel
    {
    }
}