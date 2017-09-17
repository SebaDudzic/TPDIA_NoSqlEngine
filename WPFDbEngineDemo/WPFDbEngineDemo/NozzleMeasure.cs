using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace NoSqlEngineConsoleApp
{
    class NozzleMeasure
    {
        public DateTime date;
        public int locationID;
        public int nozzleID;
        public int tankID;
        public float literCounter;
        public float totalCounter;
        public int status;

        public NozzleMeasure()
        {

        }

        public NozzleMeasure(DateTime date,
                         int locationID,
                         int nozzleID,
                         int tankID,
                         float literCounter,
                         float totalCounter,
                         int status)
        {
            this.date = date;
            this.locationID = locationID;
            this.nozzleID = nozzleID;
            this.tankID = tankID;
            this.literCounter = literCounter;
            this.totalCounter = totalCounter;
            this.status = status;
        }

        public static NozzleMeasure Parse(string item)
        {
            var splited = item.Split(';');
            var result = new NozzleMeasure();
            result.date = DateTime.Parse(splited[0]);
            result.locationID = Utilities.ParseToInt(splited[1]);
            result.nozzleID = Utilities.ParseToInt(splited[2]);
            result.tankID = Utilities.ParseToInt(splited[3]);
            result.literCounter = Utilities.ParseToFloat(splited[4]);
            result.totalCounter = Utilities.ParseToFloat(splited[5]);
            result.status = Utilities.ParseToInt(splited[6]);
            return result;
        }

        public static BsonDocument Parse(NozzleMeasure data)
        {
            var result = new BsonDocument()
            {
                { "id", Guid.NewGuid().ToString("N") },
                { "date", data.date },
                { "locationID", data.locationID },
                { "nozzleID", data.nozzleID },
                { "tankID", data.tankID },
                { "literCounter", data.literCounter },
                { "totalCounter", data.totalCounter },
                { "status", data.status },
            };

            return result;
        }

        public static NozzleMeasure Parse(BsonDocument doc)
        {
            var result = new NozzleMeasure();
            result.date = doc["date"].ToUniversalTime();
            result.locationID = doc["locationID"].AsInt32;
            result.nozzleID = doc["nozzleID"].AsInt32;
            result.tankID = doc["tankID"].AsInt32;
            result.literCounter = (float)doc["literCounter"].AsDouble;
            result.totalCounter = (float)doc["totalCounter"].AsDouble;
            result.status = doc["status"].AsInt32;
            return result;
        }
    }
}
