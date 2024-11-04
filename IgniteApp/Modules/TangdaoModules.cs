using AutoMapper;
using IgniteAdmin.Providers;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Modules
{
    public class TangdaoModules : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<IWriteService>().To<WriteService>();
            Bind<IReadService>().To<ReadService>();
            Bind<IPlcProvider>().To<PlcProvider>().InSingletonScope();
            Bind<IPlcBuilder>().ToFactory(Builder);
        }

        private IPlcBuilder Builder(IContainer container)
        {
            var provider = container.Get<IPlcProvider>();
            return provider.Builder();
        }
    }
}
