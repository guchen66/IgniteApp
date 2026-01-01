using AutoMapper;
using IgniteAdmin.Providers;
using IgniteAdmin.Workers;
using StyletIoC;

namespace IgniteApp.Modules
{
    public class AutoModules : StyletIoCModule
    {
        protected override void Load()
        {
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