using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace NoSqlEngineConsoleApp
{
    class TankMeasure
    {
        public DateTime date;
        public int locationID;
        public int meterID;
        public int tankID;
        public float fuelHeight;
        public float fuelCapacity;
        public float fuelTemperature;
        public float waterHeight;
        public float waterCapacity;

        public TankMeasure()
        {

        }

        public TankMeasure(DateTime date,
                         int locationID,
                         int meterID,
                         int tankID,
                         float fuelHeight,
                         float fuelCapacity,
                         float fuelTemperature,
                         float waterHeight,
                         float waterCapacity)
        {
            this.date = date;
            this.locationID = locationID;
            this.meterID = meterID;
            this.tankID = tankID;
            this.fuelHeight = fuelHeight;
            this.fuelCapacity = fuelCapacity;
            this.fuelTemperature = fuelTemperature;
            this.waterHeight = waterHeight;
            this.waterCapacity = waterCapacity;
        }

        public static TankMeasure Parse(string item)
        {
            var splited = item.Split(';');
            var result = new TankMeasure();
            result.date = DateTime.Parse(splited[0]);
            result.locationID = Utilities.ParseToInt(splited[1]);
            result.meterID = Utilities.ParseToInt(splited[2]);
            result.tankID = Utilities.ParseToInt(splited[3]);
            result.fuelHeight = Utilities.ParseToInt(splited[4]);
            result.fuelCapacity = Utilities.ParseToFloat(splited[5]);
            result.fuelTemperature = Utilities.ParseToFloat(splited[6]);
            result.waterHeight = Utilities.ParseToFloat(splited[7]);
            result.waterCapacity = Utilities.ParseToFloat(splited[8]);
            return result;
        }


        public static BsonDocument Parse(TankMeasure data)
        {
            var result = new BsonDocument()
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

            return result;
        }

        public static TankMeasure Parse(BsonDocument doc)
        {
            var result = new TankMeasure();
            result.date = doc["date"].ToUniversalTime();
            result.locationID = doc["locationID"].AsInt32;
            result.meterID = doc["meterID"].AsInt32;
            result.tankID = doc["tankID"].AsInt32;
            result.fuelHeight = (float)doc["fuelHeight"].AsDouble;
            result.fuelCapacity = (float)doc["fuelCapacity"].AsDouble;
            result.fuelTemperature = (float)doc["fuelTemperature"].AsDouble;
            result.waterHeight = (float)doc["waterHeight"].AsDouble;
            result.waterCapacity = (float)doc["waterCapacity"].AsDouble;
            return result;
        }
    }
}
