using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlEngineConsoleApp
{
    class DataSender
    {
        private const int PRECACHED_TANK_MESUARES = 20;
        private const int PRECACHED_NOZZLE_MESUARES = 2000;
        private const int PRECACHED_REFUEL = 20;
        private const int REFRESH_TIME = 1000;
        private const string NOZZLE_MEASURES_FILE_PATH = "nozzleMeasures.log";
        private const string TANK_MEASURES_FILE_PATH = "tankMeasures.log";
        private const string REFUELS_FILE_PATH = "refuel.log";

        private StreamReader tankMeasuresFile;
        private List<TankMeasure> waitingTankMeasures = new List<TankMeasure>();

        private StreamReader nozzleMeasuresFile;
        private List<NozzleMeasure> waitingNozzleMeasures = new List<NozzleMeasure>();

        private StreamReader refuelFile;
        private List<Refuel> waitingRefuel = new List<Refuel>();

        private System.DateTime startDataTime;
        private TimeSpan simulationTime;
        private DbEngine dbEngine;

        //SenderConfigurationFromUI
        private Func<double> getSenderTimeScale;

        public DataSender(DbEngine dbEngine, Func<double> getSenderTimeScale)
        {
            this.dbEngine = dbEngine;
            this.getSenderTimeScale = getSenderTimeScale;
            simulationTime = new TimeSpan();
            OpenFilesStreams();
            RunAllSenders();
        }

        private void RunAllSenders()
        {
            SimulateTimer();
            RunTankMeasureSender();
            RunNozzleMeasureSender();
            RunRefuelSender();
        }

        private async Task SimulateTimer()
        {
            while (true)
            {
                await Task.Delay(REFRESH_TIME);
                simulationTime += new TimeSpan(0, 0, 0, 0, (int)(REFRESH_TIME * getSenderTimeScale()));
            }
        }

        private async Task RunTankMeasureSender()
        {
            while (true)
            {
                await Task.Delay(REFRESH_TIME);

                while (waitingTankMeasures.Count > 0 && IsTimeToSend(waitingTankMeasures.First().date))
                {
                    dbEngine.AddTankMeasure(waitingTankMeasures.First());
                    waitingTankMeasures.RemoveAt(0);
                    if (waitingTankMeasures.Count == 0)
                    {
                        ReadDataTankMeasures(1);
                    }
                }
            }
        }

        private async Task RunNozzleMeasureSender()
        {
            while (true)
            {
                await Task.Delay(REFRESH_TIME);

                while (waitingNozzleMeasures.Count > 0 && IsTimeToSend(waitingNozzleMeasures.First().date))
                {
                    dbEngine.AddNozzleMeasure(waitingNozzleMeasures.First());
                    waitingNozzleMeasures.RemoveAt(0);
                    if (waitingNozzleMeasures.Count == 0)
                    {
                        ReadDataNozzleMeasures(1);
                    }
                }
                Debug.WriteLine(waitingNozzleMeasures.Count);
            }
        }

        private async Task RunRefuelSender()
        {
            while (true)
            {
                await Task.Delay(REFRESH_TIME);

                while (waitingRefuel.Count > 0 && IsTimeToSend(waitingRefuel.First().date))
                {
                    dbEngine.AddRefuel(waitingRefuel.First());
                    waitingRefuel.RemoveAt(0);
                    if (waitingRefuel.Count == 0)
                    {
                        ReadDataRefuel(1);
                    }
                }
            }
        }

        public DateTime GetCurrentDataTime()
        {
            return startDataTime + simulationTime;
        }

        private void OpenFilesStreams()
        {
            var currentDirPath = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).Parent.FullName;

            tankMeasuresFile = new System.IO.StreamReader(Path.Combine(currentDirPath, TANK_MEASURES_FILE_PATH));
            ReadDataTankMeasures(PRECACHED_TANK_MESUARES);
            nozzleMeasuresFile = new System.IO.StreamReader(Path.Combine(currentDirPath, NOZZLE_MEASURES_FILE_PATH));
            ReadDataNozzleMeasures(PRECACHED_NOZZLE_MESUARES);
            refuelFile = new System.IO.StreamReader(Path.Combine(currentDirPath, REFUELS_FILE_PATH));
            ReadDataRefuel(PRECACHED_REFUEL);
            startDataTime = new DateTime(2014, 01, 01, 0, 0, 0);
        }

        private void ReadDataTankMeasures(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (!tankMeasuresFile.EndOfStream)
                {
                    waitingTankMeasures.Add(TankMeasure.Parse(tankMeasuresFile.ReadLine()));
                }
                else
                {
                    return;
                }
            }
        }

        private void ReadDataNozzleMeasures(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (!nozzleMeasuresFile.EndOfStream)
                {
                    waitingNozzleMeasures.Add(NozzleMeasure.Parse(nozzleMeasuresFile.ReadLine()));
                }
                else
                {
                    return;
                }
            }
        }

        private void ReadDataRefuel(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (!refuelFile.EndOfStream)
                {
                    waitingRefuel.Add(Refuel.Parse(refuelFile.ReadLine()));
                }
                else
                {
                    return;
                }

            }
        }

        private bool IsTimeToSend(DateTime dataTime)
        {
            TimeSpan dataTimeSpan = dataTime - startDataTime;

            return simulationTime >= dataTimeSpan;
        }
    }
}