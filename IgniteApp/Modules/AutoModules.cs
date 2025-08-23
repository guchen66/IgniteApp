using AutoMapper;
using IgniteAdmin.Providers;
using IgniteApp.Interfaces;
using IgniteApp.Tests;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteDb.Repositorys;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Modules
{
    public class AutoModules : StyletIoCModule
    {
        protected override void Load()
        {
            //Bind<IMonitorService>().To<MonitorService>().InSingletonScope();
            Bind<IMonitorService>().To<XmlFileMonitorService>().InSingletonScope();
            // 如果需要，可以注册其他服务
            // builder.Bind<IFileWatcherService>().To<FileWatcherService>().InSingletonScope();
            Bind<IAutoMapperProvider>().To<AutoMapperProvider>().InSingletonScope();
            Bind<IMapper>().ToFactory(GetMapper);
        }

        private IMapper GetMapper(IContainer container)
        {
            var provider = container.Get<IAutoMapperProvider>();
            return provider.GetMapper();
        }
    }
}