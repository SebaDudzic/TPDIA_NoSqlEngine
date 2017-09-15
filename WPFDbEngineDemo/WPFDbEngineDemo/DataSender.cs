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

        //SenderConfigurationFromUI
        private Func<double> getSenderTimeScale;

        //debug
        private System.Random random;

        public DataSender(DbEngine dbEngine, Func<double> getSenderTimeScale)
        {
            this.dbEngine = dbEngine;
            this.getSenderTimeScale = getSenderTimeScale;
            random = new System.Random();
            ReadFile();
            RunAllSenders();
        }

        private void RunAllSenders()
        {
            RunTankMeasureSender();
            RunTestAddNozzleMeasure();
        }

        private async Task RunTankMeasureSender()
        {
            while (true)
            {
                double defaultCountPerSecond = 1;

                if (getSenderTimeScale() != 0)
                {
                    await Task.Delay((int)((1000 / defaultCountPerSecond) / getSenderTimeScale()));

                    dbEngine.AddTankMeasure(new TankMeasure(
                        System.DateTime.Now,
                        random.Next(0, 5),
                        random.Next(0, 5),
                        random.Next(0, 5),
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble()));
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }

        private async Task RunTestAddNozzleMeasure()
        {
            while (true)
            {
                double defaultCountPerSecond = 2;

                if (getSenderTimeScale() != 0)
                {
                    await Task.Delay((int)((1000 / defaultCountPerSecond) / getSenderTimeScale()));

                    dbEngine.AddNozzleMeasure(new NozzleMeasure(System.DateTime.Now,
                        random.Next(0, 5),
                        random.Next(0, 5),
                        random.Next(0, 5),
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        random.Next(0, 5)));
                }
                else
                {
                    await Task.Delay(100);
                }
            }
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
