using System;
using System.Collections.Generic;
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
}
