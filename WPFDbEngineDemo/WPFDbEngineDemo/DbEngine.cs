﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace NoSqlEngineConsoleApp
{
    class DbEngine
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private IMongoCollection<BsonDocument> tankMeasuresCollection;
        private IMongoCollection<BsonDocument> nozzleMeasuresCollection;
        private IMongoCollection<BsonDocument> refuelsCollection;
        private Func<DateTime> getTime;

        private const string DB_NAME = "db";
        private const string TANK_MEASURES_NAME = "TankMeasures";
        private const string NOZZLE_MEASURES_NAME = "NozzleMeasures";
        private const string REFUELS_NAME = "Refuels";

        public DbEngine()
        {
            _client = new MongoClient();
            _client.DropDatabase(DB_NAME);
            _database = _client.GetDatabase(DB_NAME);
            tankMeasuresCollection = _database.GetCollection<BsonDocument>(TANK_MEASURES_NAME);
            nozzleMeasuresCollection = _database.GetCollection<BsonDocument>(NOZZLE_MEASURES_NAME);
            refuelsCollection = _database.GetCollection<BsonDocument>(REFUELS_NAME);
        }

        public void SetTimeGetter(Func<DateTime> getTime)
        {
            this.getTime = getTime;
        }

        //--Public_Interface_Save------------------------------------//

        public async void AddTankMeasure(TankMeasure data)
        {
            await tankMeasuresCollection.InsertOneAsync(TankMeasure.Parse(data));
        }

        public async void AddNozzleMeasure(NozzleMeasure data)
        {
            await nozzleMeasuresCollection.InsertOneAsync(NozzleMeasure.Parse(data));
        }

        public async void AddRefuel(Refuel data)
        {
            await refuelsCollection.InsertOneAsync(Refuel.Parse(data));
        }

        //--Public_Interface_Read------------------------------------//

        public int GetTankMeasuresCount()
        {
            try
            {
                Task<int> task = Task<int>.Factory.StartNew(() => ReadCollectionCount(tankMeasuresCollection).Result);
                return task.Result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int GetNozzleMeasuresCount()
        {
            try
            {
                Task<int> task = Task<int>.Factory.StartNew(() => ReadCollectionCount(nozzleMeasuresCollection).Result);
                return task.Result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int GetRefuelsCount()
        {
            try
            {
                Task<int> task = Task<int>.Factory.StartNew(() => ReadCollectionCount(refuelsCollection).Result);
                return task.Result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public TankMeasure GetLatestTankMeasure(int tankID)
        {
            try
            {
                BsonDocument filter = new BsonDocument()
                {
                    { "tankID", tankID }
                };
                Task<BsonDocument> result = tankMeasuresCollection.Find(filter).SortByDescending((a) => a["date"]).FirstAsync();
                return TankMeasure.Parse(result.Result);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public NozzleMeasure GetLatestNozzleMeasure(int nozzleID)
        {
            try
            {
                BsonDocument filter = new BsonDocument()
                {
                    { "nozzleID", nozzleID }
                };
                Task<BsonDocument> result = nozzleMeasuresCollection.Find(filter).SortByDescending((a) => a["date"]).FirstAsync();
                return NozzleMeasure.Parse(result.Result);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public List<int> GetTankMeasuresUniqueIDs()
        //{
        //    try
        //    {
        //        return tankMeasuresCollection.Distinct<int>("tankID", new BsonDocument()).ToList().OrderBy(x => x).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        return new List<int>();
        //    }
        //}

        public List<TankMeasure> GetLatestTankMeasures(int amount)
        {
            try
            {
                return tankMeasuresCollection.Aggregate().SortByDescending((a) => a["date"]).Limit(amount)
                    .ToList().Select(x => TankMeasure.Parse(x)).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<NozzleMeasure> GetLatestNozzleMeasures(int amount)
        {
            try
            {
                return nozzleMeasuresCollection.Aggregate().SortByDescending((a) => a["date"]).Limit(amount)
                    .ToList().Select(x => NozzleMeasure.Parse(x)).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Refuel> GetLatestRefuels(int amount)
        {
            try
            {
                return refuelsCollection.Aggregate().SortByDescending((a) => a["date"]).Limit(amount)
                    .ToList().Select(x => Refuel.Parse(x)).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<TankMeasure> GetLatestTankMeasuresByTime(int hours)
        {
            try
            { 
                return tankMeasuresCollection.Aggregate().SortByDescending((a) => a["date"]).ToList()
                .FindAll(x=>(getTime() - DateTime.Parse(x["date"].AsString)).TotalHours <= hours )
                    .ToList().Select(x => TankMeasure.Parse(x)).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<NozzleMeasure> GetLatestNozzleMeasuresByTime(int hours)
        {
            try
            {
                return nozzleMeasuresCollection.Aggregate().SortByDescending((a) => a["date"]).ToList()
                .FindAll(x => (getTime() - DateTime.Parse(x["date"].AsString)).TotalHours <= hours)
                    .ToList().Select(x => NozzleMeasure.Parse(x)).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Refuel> GetLatestRefuelsByTime(int hours)
        {
            try
            {
                return refuelsCollection.Aggregate().SortByDescending((a) => a["date"]).ToList()
                .FindAll(x => (getTime() - DateTime.Parse(x["date"].AsString)).TotalHours <= hours)
                    .ToList().Select(x => Refuel.Parse(x)).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
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
    }
}
