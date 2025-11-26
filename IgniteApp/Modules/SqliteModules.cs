using IgniteApp.Interfaces;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteDb.Repositorys;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        private readonly IContentWriter _writeService;

        public Person(string name)
        {
        }
    }
}