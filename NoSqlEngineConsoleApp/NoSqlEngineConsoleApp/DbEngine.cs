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
        private IMongoCollection<BsonDocument> nozzleMeasuresCollection;
        private IMongoCollection<BsonDocument> refuelsCollection;

        private const string DB_NAME = "db";
        private const string TANK_MEASURES_NAME = "TankMeasures";
        private const string NOZZLE_MEASURES_NAME = "NozzleMeasures";
        private const string REFUELS_NAME = "Refuels";

        //debug
        private System.Random random;

        public DbEngine()
        {
            _client = new MongoClient();
            _client.DropDatabase(DB_NAME);
            _database = _client.GetDatabase(DB_NAME);
            tankMeasuresCollection = _database.GetCollection<BsonDocument>(TANK_MEASURES_NAME);
            nozzleMeasuresCollection = _database.GetCollection<BsonDocument>(NOZZLE_MEASURES_NAME);
            refuelsCollection = _database.GetCollection<BsonDocument>(REFUELS_NAME);
            //debug
            random = new System.Random();
        }

        //--Public_Interface_Save------------------------------------//

        public async void AddTankMeasure(TankMeasure data)
        {
            Console.WriteLine("AddTankMeasure");

            var document = new BsonDocument
            {
                { "id", Guid.NewGuid().ToString("N") },
                { "date", data.date },
                { "locationID", data.locationID },
                { "meterID", data.meterID },
                { "tankID", data.tankID },
                { "fuelHeight", data.fuelHeight },
                { "fuelCapacity", data.fuelCapacity },
                { "fuelTemperature", data.fuelTemperature },
                { "waterHeight", data.waterHeight },
                { "waterCapacity", data.waterCapacity },
            };

            //Console.WriteLine(document);
            await tankMeasuresCollection.InsertOneAsync(document);
        }

        public async void AddNozzleMeasure(NozzleMeasure data)
        {
            Console.WriteLine("AddNozzleMeasure");

            var document = new BsonDocument
            {
                { "id", Guid.NewGuid().ToString("N") },
                { "date", data.date },
                { "locationID", data.locationID },
                { "fuelGun", data.fuelGun },
                { "tankID", data.tankID },
                { "literCounter", data.literCounter },
                { "totalCounter", data.totalCounter },
                { "status", data.status },
            };

            //Console.WriteLine(document);
            await nozzleMeasuresCollection.InsertOneAsync(document);
        }

        //private async void AddNozzleMeasure(NozzleMeasure data)
        //{
        //    Console.WriteLine("AddNozzleMeasure");

        //    var document = new BsonDocument
        //    {
        //        { "id", Guid.NewGuid().ToString("N") },
        //        { "date", data.date },
        //        { "locationID", data.locationID },
        //        { "fuelGun", data.fuelGun },
        //        { "tankID", data.tankID },
        //        { "literCounter", data.literCounter },
        //        { "totalCounter", data.totalCounter },
        //        { "status", data.status },
        //    };

        //    Console.WriteLine(document);

        //    await tankMeasuresCollection.InsertOneAsync(document);
        //}

        //--Public_Interface_Read------------------------------------//

        public int GetTankMeasuresCount()
        {
            Task<int> task = Task<int>.Factory.StartNew(() => ReadCollectionCount(tankMeasuresCollection).Result);
            return task.Result;
        }

        //--Private--------------------------------------------------//

        private async Task<int> ReadCollectionCount(IMongoCollection<BsonDocument> collection)
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
            return count;
        }

        //--DEBUG----------------//

        public async Task RunAllTests()
        {
            await Task.WhenAll(RunTestAddTankMeasure(), RunTestAddNozzleMeasure(), RunTestReadTankMeasuesCount());
        }

        private async Task RunTestAddTankMeasure()
        {
            while (true)
            {
                await Task.Delay(1000);
                AddTankMeasure(new TankMeasure(System.DateTime.Now,
                    random.Next(0, 5),
                    random.Next(0, 5),
                    random.Next(0, 5),
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble()));
            }
        }

        private async Task RunTestAddNozzleMeasure()
        {
            while (true)
            {
                await Task.Delay(750);
                AddNozzleMeasure(new NozzleMeasure(System.DateTime.Now,
                    random.Next(0, 5),
                    random.Next(0, 5),
                    random.Next(0, 5),
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    random.Next(0, 5)));
            }
        }

        private async Task RunTestReadTankMeasuesCount()
        {
            while (true)
            {
                await Task.Delay(300);
                Console.WriteLine("TankMeasuesCount: " + GetTankMeasuresCount());
            }
        }

        //--DEBUG_END----------------//
    }
}
