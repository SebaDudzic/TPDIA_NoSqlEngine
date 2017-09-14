using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlEngineConsoleApp
{
    class DataSender
    {
        private const string NOZZLE_MEASURES_FILE_PATH = "nozzleMeasures.data";
        private const string TANK_MEASURES_FILE_PATH = "tankMeasures.data";
        private const string REFUELS_FILE_PATH = "tankMeasures.data";

        private const int preCachedData = 100;

        private List<NozzleMeasure> nozzleMeasures = new List<NozzleMeasure>();
        private List<TankMeasure> tankMeasures = new List<TankMeasure>();
        private List<Refuel> refuels = new List<Refuel>();
        private DbEngine dbEngine;

        public DataSender(DbEngine dbEngine)
        {
            this.dbEngine = dbEngine;
            ReadFile();
        }

        private void ReadFile()
        {
            //var path = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).Parent.FullName;
            //var nozzleMeasures = File.ReadLines(Path.Combine(path, NOZZLE_MEASURES_FILE_PATH)).ToArray();
            //for (int i = 0; i < 100; i++)
            //{
            //    this.nozzleMeasures.Add(NozzleMeasure.Parse(nozzleMeasures[i]));
            //}

            //var tankMeasures = File.ReadAllLines(Path.Combine(path, TANK_MEASURES_FILE_PATH)).ToArray();
            //for (int i = 0; i < 100; i++)
            //{
            //    this.tankMeasures.Add(TankMeasure.Parse(tankMeasures[i]));
            //}

            //var refuels = File.ReadAllLines(Path.Combine(path, REFUELS_FILE_PATH)).ToArray();
            //for (int i = 0; i < refuels.Count() - 1; i++)
            //{
            //    this.refuels.Add(Refuel.Parse(refuels[i]));
            //}

        }


    }
}
