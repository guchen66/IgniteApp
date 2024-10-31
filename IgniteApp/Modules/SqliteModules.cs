using IgniteApp.Interfaces;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteDb.Repositorys;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
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
            Bind<AccessDbContext>().ToFactory(container => new AccessDbContext());
            Bind<IMaterialRepository>().To<MaterialRepository>().InSingletonScope();
           
        }
    }

    public interface IPerson
    {
       
    }
    public class Person : IPerson
    {
        private readonly IWriteService _writeService;
        public Person(string name)
        {
            
        }
    }
}
