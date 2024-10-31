using IgniteShared.Entitys;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb
{
    public class CodeFirst
    {
        public static void CreateTable()
        {
            AccessDbContext _dbContext = new AccessDbContext();
            for (var i = 0; i < 10; i++)
            {

                _dbContext.MaterialInfos.Add(new MaterialInfo
                {
                    Remark = "小李",
                    Id = i,
                    Station = "男"
                });
            }

            _dbContext.SaveChanges();//保存缓存操作

            var ListRes = _dbContext.MaterialInfos.Where(t => t.Remark == "小李").ToList();
            DbProvider.InitConnection();
            using (SQLiteCommand command = new SQLiteCommand())
            {
                command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name NVARCHAR(100) NOT NULL,
                            Email NVARCHAR(100) UNIQUE NOT NULL
                        );";
                command.ExecuteNonQuery();
            }
        }

    }

    public class MyDbContextInitializer : SqliteDropCreateDatabaseAlways<AccessDbContext>
    {
        public MyDbContextInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder) { }

        protected override void Seed(AccessDbContext context)
        {
            // 在这里填充核心数据或测试数据
            context.Set<MaterialInfo>().Add(new MaterialInfo {  Id = 30, Station="第一站",Status="已处理" });
            context.SaveChanges();
        }
    }
}
