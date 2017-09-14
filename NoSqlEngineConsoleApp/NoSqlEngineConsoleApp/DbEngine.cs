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
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private IMongoCollection<BsonDocument> tankMeasuresCollection;

        private const string DB_NAME = "db";
        private const string TANK_MEASURES_NAME = "TankMeasures";

        //debug
        private System.Random random;

        public DbEngine()
        {
            _client = new MongoClient();
            //_client.DropDatabase(DB_NAME);
            _database = _client.GetDatabase(DB_NAME);
            tankMeasuresCollection = _database.GetCollection<BsonDocument>(TANK_MEASURES_NAME);

            //debug
            random = new System.Random();
        }

        public async Task RunAllTests()
        {
            await Task.WhenAll(RunTestAdd(), RunTestRead());
        }

        private async Task RunTestAdd()
        {
            while(true)
            {
                await Task.Delay(1000);
                await AddTankMeasureAsync();           
            }
        }

        private async Task RunTestRead()
        {
            while (true)
            {
                await Task.Delay(300);
                await ReadCollectionCount(tankMeasuresCollection);
            }
        }

        private async Task AddTankMeasureAsync()
        {
            Console.WriteLine("AddTankMeasureAsync");

            var document = new BsonDocument
            {
                { "id", Guid.NewGuid().ToString("N") },
                { "fuel", random.Next(0,1000) },
            };

            Console.WriteLine(document);

            await tankMeasuresCollection.InsertOneAsync(document);
        }

        private async Task ReadCollectionCount(IMongoCollection<BsonDocument> collection)
        {
            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        // process document
                        count++;
                    }
                }
            }

            Console.WriteLine("Count: " + count);
        }
    }
}
