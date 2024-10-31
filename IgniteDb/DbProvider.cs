using IT.Tangdao.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteDb
{
    public class DbProvider
    {
        public static void InitConnection()
        {
           
            var dbPath = DirectoryHelper.SelectDirectoryByName("demo.db");
            // 创建连接字符串
            string connectionString = $"Data Source={dbPath};";
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connStr);
                //按照路径创建数据库文件
                conn.Open();
                var command = conn.CreateCommand();
                //创建user_info表
                command.CommandText = $"CREATE table IF NOT EXISTS 'user_info' ('id'  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 'name' TEXT, 'address' TEXT);";
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
        }
        private static string connStr = ConfigurationManager.ConnectionStrings["Sqlite"].ConnectionString;
       
    }
}
