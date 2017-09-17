using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace NoSqlEngineConsoleApp
{
    class Refuel
    {
        public DateTime date;
        public int tankID;
        public float fuelCapacity;
        public float tankSpeed;

        public Refuel()
        {

        }

        public Refuel(DateTime date,
                         int tankID,
                         float fuelCapacity,
                         float tankSpeed
                        )
        {
            this.date = date;
            this.tankID = tankID;
            this.fuelCapacity = fuelCapacity;
            this.tankSpeed = tankSpeed;
        }

        public static Refuel Parse(string item)
        {
            var splited = item.Split(';');
            var result = new Refuel();
            result.date = DateTime.Parse(splited[0]);
            result.tankID = Utilities.ParseToInt(splited[1]);
            result.fuelCapacity = Utilities.ParseToFloat(splited[2]);
            result.tankSpeed = Utilities.ParseToFloat(splited[3]);
            return result;
        }

        public static BsonDocument Parse(Refuel data)
        {
            var result = new BsonDocument()
            {
                { "id", Guid.NewGuid().ToString("N") },
                { "date", data.date },
                { "tankID", data.tankID },
                { "fuelCapacity", data.fuelCapacity },
                { "tankSpeed", data.tankSpeed },
            };

            return result;
        }

        public static Refuel Parse(BsonDocument doc)
        {
            var result = new Refuel();
            result.date = doc["date"].ToUniversalTime();
            result.tankID = doc["tankID"].AsInt32;
            result.fuelCapacity = (float)doc["fuelCapacity"].AsDouble;
            result.tankSpeed = (float)doc["tankSpeed"].AsDouble;
            return result;
        }
    }
}
