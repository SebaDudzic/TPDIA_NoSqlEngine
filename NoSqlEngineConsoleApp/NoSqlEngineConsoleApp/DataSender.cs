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
        private const int dataCount = 100;

        private List<NozzleMeasure> nozzleMeasures = new List<NozzleMeasure>();
        private List<TankMeasure> tankMeasures = new List<TankMeasure>();
        private DbEngine dbEngine;

        public DataSender()
        {
            ReadFile();
        }

        public DataSender(DbEngine dbEngine)
        {
            this.dbEngine = dbEngine;
        }

        private void ReadFile()
        {
            var path = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).Parent.FullName;
            var nozzleMeasures = File.ReadLines(Path.Combine(path, NOZZLE_MEASURES_FILE_PATH)).ToArray();
            for (int i = 0; i < 100; i++)
            {
                this.nozzleMeasures.Add(NozzleMeasure.Parse(nozzleMeasures[i]));
            }

            var tankMeasures = File.ReadAllLines(Path.Combine(path, TANK_MEASURES_FILE_PATH)).ToArray();
            for (int i = 0; i < 100; i++)
            {
                this.tankMeasures.Add(TankMeasure.Parse(tankMeasures[i]));
            }
        }


    }
}
