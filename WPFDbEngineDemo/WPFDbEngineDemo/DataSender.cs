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
        private const string NOZZLE_MEASURES_FILE_PATH = "nozzleMeasures.log";
        private const string TANK_MEASURES_FILE_PATH = "tankMeasures.log";
        private const string REFUELS_FILE_PATH = "tankMeasures.log";

        private StreamReader tankMeasuresFile;
        private List<TankMeasure> waitingTankMeasures = new List<TankMeasure>();

        private System.DateTime simulationTimeStart;
        private System.DateTime dataTimeStart;

        //private List<NozzleMeasure> nozzleMeasures = new List<NozzleMeasure>();
        //private List<TankMeasure> tankMeasures = new List<TankMeasure>();
        //private List<Refuel> refuels = new List<Refuel>();
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
            simulationTimeStart = System.DateTime.Now;
            OpenFilesStreams();
            RunAllSenders();
        }

        private void RunAllSenders()
        {
            RunTankMeasureSender();
            //RunNozzleMeasureSender();
        }

        private async Task RunTankMeasureSender()
        {
            while (true)
            {
                //double defaultCountPerSecond = 1;

                await Task.Delay(10);
                
                while(waitingTankMeasures.Count > 0 && IsTimeToSend(waitingTankMeasures.First().date))
                {
                    dbEngine.AddTankMeasure(waitingTankMeasures.First());
                    waitingTankMeasures.RemoveAt(0);
                }
            }
        }

        //private async Task RunNozzleMeasureSender()
        //{
        //    while (true)
        //    {
        //        double defaultCountPerSecond = 2;

        //        if (getSenderTimeScale() != 0)
        //        {
        //            await Task.Delay((int)((1000 / defaultCountPerSecond) / getSenderTimeScale()));

        //            dbEngine.AddNozzleMeasure(new NozzleMeasure(System.DateTime.Now,
        //                random.Next(0, 5),
        //                random.Next(0, 5),
        //                random.Next(0, 5),
        //                (float)random.NextDouble(),
        //                (float)random.NextDouble(),
        //                random.Next(0, 5)));
        //        }
        //        else
        //        {
        //            await Task.Delay(100);
        //        }
        //    }
        //}

        private void OpenFilesStreams()
        {
            var currentDirPath = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).Parent.FullName;

            tankMeasuresFile = new System.IO.StreamReader(Path.Combine(currentDirPath, TANK_MEASURES_FILE_PATH));
            ReadDataTankMeasures(20);
            dataTimeStart = waitingTankMeasures[0].date;
        }

        private void ReadDataTankMeasures(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                waitingTankMeasures.Add(TankMeasure.Parse(tankMeasuresFile.ReadLine()));
            }
        }

        private bool IsTimeToSend(DateTime dataTime)
        {
            TimeSpan simulationTimeSpan = DateTime.Now - simulationTimeStart;
            TimeSpan dataTimeSpan = dataTime - dataTimeStart;

            return simulationTimeSpan >= dataTimeSpan;
        }
    }
}