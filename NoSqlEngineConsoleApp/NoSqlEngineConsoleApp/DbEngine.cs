using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NoSqlEngineConsoleApp
{
    class DbEngine
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        private const string DB_NAME = "db";

        public DbEngine()
        {
            _client = new MongoClient();
            _client.DropDatabase(DB_NAME);
            _database = _client.GetDatabase(DB_NAME);
        }
    }
}
