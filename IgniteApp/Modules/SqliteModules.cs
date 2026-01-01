using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteDb.Repositorys;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using StyletIoC;

namespace IgniteApp.Modules
{
    public class SqliteModules : StyletIoCModule
    {
        protected override void Load()
        {
            // Bind<AccessDbContext>().ToFactory(container => new AccessDbContext());

            Bind<AccessDbContext>().ToSelf().InSingletonScope();
            Bind<IMaterialRepository>().To<MaterialRepository>().InSingletonScope();
            Bind<IRecipeRepository>().To<RecipeRepository>().InSingletonScope();
            Bind<IProductRepository>().To<ProductRepository>().InSingletonScope();
        }
    }

    public interface IPerson
    {
    }

    public class Person : IPerson
    {
        public Person(string name)
        {
        }
    }
}