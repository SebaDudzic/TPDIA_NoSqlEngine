using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlEngineConsoleApp
{
    class NozzleMeasure
    {
        public DateTime date;
        public int locationID;
        public int fuelGun;
        public int tankID;
        public float literCounter;
        public float totalCounter;
        public int status;

        public NozzleMeasure()
        {

        }

        public NozzleMeasure(DateTime date,
                         int locationID,
                         int fuelGun,
                         int tankID,
                         float literCounter,
                         float totalCounter,
                         int status)
        {
            this.date = date;
            this.locationID = locationID;
            this.fuelGun = fuelGun;
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
            result.fuelGun = Utilities.ParseToInt(splited[2]);
            result.tankID = Utilities.ParseToInt(splited[3]);
            result.literCounter = Utilities.ParseToInt(splited[4]);
            result.totalCounter = Utilities.ParseToFloat(splited[5]);
            result.status = Utilities.ParseToInt(splited[6]);
            return result;
        }


    }
}
