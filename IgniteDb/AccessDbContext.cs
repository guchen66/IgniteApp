//using EntityFramework.DynamicFilters;
using EntityFramework.DynamicFilters;
using IgniteShared.Entitys;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
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
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<AccessDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
            //   modelBuilder.Entity<HeaderInfo>().ToTable("headerinfo");
            base.OnModelCreating(modelBuilder);

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

       
    }
}
