using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBTest.Database
{
    internal class DatabaseManager
    {
        private LiteDatabase _database;

        public DatabaseManager()
        {
            _database = new LiteDatabase(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"MyData.db");
        }
        
        public LiteDatabase GetDatabase()
        {
            return _database;
        }

    }
}
