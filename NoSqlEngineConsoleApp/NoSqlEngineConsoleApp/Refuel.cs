using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                         float fuelTemperature
                        )
        {
            this.date = date;
            this.tankID = tankID;
            this.fuelCapacity = fuelCapacity;
            this.tankSpeed = fuelTemperature;
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
    }
}
