﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static NozzleMeasure Parse(string item)
        {
            var splited = item.Split(';');
            var result = new NozzleMeasure();
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
    }
}