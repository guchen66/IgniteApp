using AutoMapper;
using IgniteAdmin.Providers;
using IgniteApp.Interfaces;
using IgniteApp.Tests;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteDb.Repositorys;
using IgniteShared.Delegates;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoDtos.Options;
using IT.Tangdao.Framework.DaoEnums;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.IO;
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
            // 注册配置

            // 使用 ToFactory 方法
            //Bind<IMonitorService>().ToFactory(container =>
            //{
            //    return new XmlMonitorService(@"E:\IgniteDatas\", true);
            //}).InSingletonScope();

            //  Bind<IMonitorService>().To<XmlMonitorService>().InSingletonScope();
            // Bind<IMonitorService>().To<XmlFileMonitorService>().InSingletonScope();
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