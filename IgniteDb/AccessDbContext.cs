//using EntityFramework.DynamicFilters;
using EntityFramework.DynamicFilters;
using IgniteShared.Entitys;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json.Linq;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb
{
    public class AccessDbContext:DbContext
    {
        public DbSet<ProductInfo> Products { get; set; }
        public DbSet<MaterialInfo> MaterialInfos { get; set; }
        public DbSet<RecipeInfo> RecipeInfos { get; set; }
        public DbSet<StaticticInfo> StaticticInfos { get; set; }
        public AccessDbContext() : base("Sqlite")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Filter("IsDeleted", d=>d.IsDeleted,false);
          //  modelBuilder.Filter("IsDeleted", (IDeletionWare d) => d.IsDeleted, false);      //使用全局过滤器进行软删除
            modelBuilder.Entity<ProductInfo>().ToTable("ProductInfo");
            modelBuilder.Entity<MaterialInfo>().ToTable("MaterialInfo");
            modelBuilder.Entity<RecipeInfo>().ToTable("RecipeInfo");
            IsInitTable(out bool isShould);
            if (isShould)
            {
                //TODO改变实体类后，可以创建新的数据库，注意，这种方式会删除表内的所有数据
                var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<AccessDbContext>(modelBuilder);
                Database.SetInitializer(sqliteConnectionInitializer);
            }
            else
            {
                var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<AccessDbContext>(modelBuilder);
                Database.SetInitializer(sqliteConnectionInitializer);
            }

        }

        public override int SaveChanges()
        {
           // ChangeTracker.Entries<IDeletionWare>().ToList().ForEach(entry => SetFilterDelete(entry));
            return base.SaveChanges();
        }

        private void SetFilterDelete(DbEntityEntry<IDeletionWare> entry)
        {
            switch (entry.State)
            {
                //防止模型字段被污染
                case EntityState.Added:
                    entry.Property(model => model.IsDeleted).CurrentValue = false;
                    entry.Property(model => model.DeleteTime).CurrentValue = null;
                    break;
                case EntityState.Modified:
                    entry.Property(model => model.IsDeleted).CurrentValue = false;
                    entry.Property(model => model.DeleteTime).CurrentValue = null;
                    break;
                case EntityState.Deleted:

                    //阻止默认删除操作
                    entry.State = EntityState.Unchanged;
                    entry.Property(model => model.IsDeleted).CurrentValue = false;
                    entry.Property(model => model.DeleteTime).CurrentValue = null;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 是否初始化数据表
        /// </summary>
        /// <returns></returns>
        private bool IsInitTable(out bool isShould)
        {
            var path = DirectoryHelper.SelectDirectoryByName("appsetting.json");
            string jsonContent = File.ReadAllText(path);
            isShould = JObject.Parse(jsonContent)["Sqlite"]["InitTable"].Value<bool>();//.ToString();
            return isShould;

        }
    }
}
