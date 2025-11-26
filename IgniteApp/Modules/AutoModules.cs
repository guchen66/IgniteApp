using AutoMapper;
using IgniteAdmin.Providers;
using IgniteAdmin.Workers;
using IT.Tangdao.Framework.Enums;
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
            Bind<IAutoMapperProvider>().To<AutoMapperProvider>().InSingletonScope();
            Bind<IMapper>().ToFactory(GetMapper);

            Bind<WorkstationManager>().ToSelf().InSingletonScope();
        }

        private IMapper GetMapper(IContainer container)
        {
            var provider = container.Get<IAutoMapperProvider>();
            return provider.GetMapper();
        }
    }
}