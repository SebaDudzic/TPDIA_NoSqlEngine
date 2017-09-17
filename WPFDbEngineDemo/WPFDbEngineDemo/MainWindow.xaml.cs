using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NoSqlEngineConsoleApp;

namespace WPFDbEngineDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbEngine dbEngine;
        DataSender dataSender;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            dbEngine = new DbEngine();
            dataSender = new DataSender(dbEngine, GetSenderTimeScale);
            RefreshUI();
        }

        private async Task RefreshUI()
        {
            while (true)
            {
                await Task.Delay(100);
                timeScaleLabel.Content = string.Format("{0:0.00}", GetSenderTimeScale());
                currentSimulationTimeLabel.Content = dataSender.GetCurrentDataTime().ToString();
                tankMeasuresCount.Content = dbEngine.GetTankMeasuresCount();
                nozzleMeasuresCount.Content = dbEngine.GetNozzleMeasureCount();

                TankMeasure latestTankMeasure = dbEngine.GetLatestTankMeasure();

                if (latestTankMeasure != null)
                {
                    tank0_LastRefresh.Content = latestTankMeasure.date;
                    tank0_FuelAmount.Content = latestTankMeasure.fuelHeight;
                    tank0_FuelTemperature.Content = latestTankMeasure.fuelTemperature;
                }

                List<int> uniqueTankIDs = dbEngine.GetTankMeasuresUniqueIDs();
                tankIDsLabel.Content = uniqueTankIDs.ListToString();
            }
        }

        private double GetSenderTimeScale()
        {
            double value = senderTimeScale.Value;

            if (value <= 1)
            {
                return value;
            }
            else
            {
                return Math.Pow(10, value - 1);
            }
        }

        private void TestReadLastTankMeasuresAmount(object sender, RoutedEventArgs e)
        {
            List<TankMeasure> tankMeasures = dbEngine.GetLatestTankMeasures(Int32.Parse(TestReadLastTankMeasuresAmountTextBox.Text));
            MessageBox.Show(string.Format("Success! Readed measures count: {0}", tankMeasures.Count));
        }
    }
}
