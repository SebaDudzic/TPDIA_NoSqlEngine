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
using System.Diagnostics;

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
            dbEngine.SetTimeGetter(dataSender.GetCurrentDataTime);
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
                nozzleMeasuresCount.Content = dbEngine.GetNozzleMeasuresCount();
                refuelsCount.Content = dbEngine.GetRefuelsCount();

                SetTankUI(1, tank1_LastRefreshLabel, tank1_FuelAmountLabel, tank1_FuelTemperatureLabel);
                SetTankUI(2, tank2_LastRefreshLabel, tank2_FuelAmountLabel, tank2_FuelTemperatureLabel);
                SetTankUI(3, tank3_LastRefreshLabel, tank3_FuelAmountLabel, tank3_FuelTemperatureLabel);
                SetTankUI(4, tank4_LastRefreshLabel, tank4_FuelAmountLabel, tank4_FuelTemperatureLabel);

                SetNozzleUI(13, nozzle13_LastRefreshLabel, nozzle13_tankIDLabel, nozzle13_LiterCounterLabel, 
                    nozzle13_TotalLiterCounterLabel, nozzle13_StatusLabel, nozzle13bg);
                SetNozzleUI(14, nozzle14_LastRefreshLabel, nozzle14_tankIDLabel, nozzle14_LiterCounterLabel,
                    nozzle14_TotalLiterCounterLabel, nozzle14_StatusLabel, nozzle14bg);
                SetNozzleUI(15, nozzle15_LastRefreshLabel, nozzle15_tankIDLabel, nozzle15_LiterCounterLabel,
                    nozzle15_TotalLiterCounterLabel, nozzle15_StatusLabel, nozzle15bg);
                SetNozzleUI(16, nozzle16_LastRefreshLabel, nozzle16_tankIDLabel, nozzle16_LiterCounterLabel,
                    nozzle16_TotalLiterCounterLabel, nozzle16_StatusLabel, nozzle16bg);
                SetNozzleUI(17, nozzle17_LastRefreshLabel, nozzle17_tankIDLabel, nozzle17_LiterCounterLabel,
                    nozzle17_TotalLiterCounterLabel, nozzle17_StatusLabel, nozzle17bg);
                SetNozzleUI(18, nozzle18_LastRefreshLabel, nozzle18_tankIDLabel, nozzle18_LiterCounterLabel,
                    nozzle18_TotalLiterCounterLabel, nozzle18_StatusLabel, nozzle18bg);
                SetNozzleUI(19, nozzle19_LastRefreshLabel, nozzle19_tankIDLabel, nozzle19_LiterCounterLabel,
                    nozzle19_TotalLiterCounterLabel, nozzle19_StatusLabel, nozzle19bg);
                SetNozzleUI(20, nozzle20_LastRefreshLabel, nozzle20_tankIDLabel, nozzle20_LiterCounterLabel,
                    nozzle20_TotalLiterCounterLabel, nozzle20_StatusLabel, nozzle20bg);
                SetNozzleUI(21, nozzle21_LastRefreshLabel, nozzle21_tankIDLabel, nozzle21_LiterCounterLabel,
                    nozzle21_TotalLiterCounterLabel, nozzle21_StatusLabel, nozzle21bg);
                SetNozzleUI(22, nozzle22_LastRefreshLabel, nozzle22_tankIDLabel, nozzle22_LiterCounterLabel,
                    nozzle22_TotalLiterCounterLabel, nozzle22_StatusLabel, nozzle22bg);
                SetNozzleUI(23, nozzle23_LastRefreshLabel, nozzle23_tankIDLabel, nozzle23_LiterCounterLabel,
                    nozzle23_TotalLiterCounterLabel, nozzle23_StatusLabel, nozzle23bg);
                SetNozzleUI(24, nozzle24_LastRefreshLabel, nozzle24_tankIDLabel, nozzle24_LiterCounterLabel,
                    nozzle24_TotalLiterCounterLabel, nozzle24_StatusLabel, nozzle24bg);
            }
        }
     
        private void SetTankUI(int tankID, Label lastRefreshLabel, Label fuelAmountLabel, Label fuelTemparatureLabel)
        {
            TankMeasure latestTankMeasure = dbEngine.GetLatestTankMeasure(tankID);

            if (latestTankMeasure != null)
            {
                lastRefreshLabel.Content = latestTankMeasure.date;
                fuelAmountLabel.Content = string.Format("{0:0.00}", latestTankMeasure.fuelCapacity);
                fuelTemparatureLabel.Content = latestTankMeasure.fuelTemperature;
            }
        }

        private void SetNozzleUI(int nozzleID, Label lastRefreshLabel, Label tankIDLabel, Label literCounterLabel,
            Label totalLiterCounterLabel, Label statusLabel, Canvas background)
        {
            NozzleMeasure latestNozzleMeasure = dbEngine.GetLatestNozzleMeasure(nozzleID);

            if (latestNozzleMeasure != null)
            {
                lastRefreshLabel.Content = latestNozzleMeasure.date;
                tankIDLabel.Content = latestNozzleMeasure.tankID;
                literCounterLabel.Content = string.Format("{0:0.00}",latestNozzleMeasure.literCounter);
                totalLiterCounterLabel.Content = string.Format("{0:0.00}", latestNozzleMeasure.totalCounter);
                statusLabel.Content = latestNozzleMeasure.status == 1 ? "idle" : "using";
                background.Background = latestNozzleMeasure.status == 1 ? 
                    new SolidColorBrush(Color.FromRgb(100,170,110)) : new SolidColorBrush(Color.FromRgb(230, 150, 70));
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

        private void TestReadLastNozzleMeasuresAmount(object sender, RoutedEventArgs e)
        {
            List<NozzleMeasure> nozzleMeasures = dbEngine.GetLatestNozzleMeasures(Int32.Parse(TestReadLastNozzleMeasuresAmountTextBox.Text));
            MessageBox.Show(string.Format("Success! Readed measures count: {0}", nozzleMeasures.Count));
        }

        private void TestReadLastRefuelsAmount(object sender, RoutedEventArgs e)
        {
            List<Refuel> refuelsCount = dbEngine.GetLatestRefuels(Int32.Parse(TestReadLastRefuelsAmountTextBox.Text));
            MessageBox.Show(string.Format("Success! Readed refuels count: {0}", refuelsCount.Count));
        }

        private void TestReadLastTankMeasuresAmountByTime(object sender, RoutedEventArgs e)
        {
            List<TankMeasure> tankMeasures = dbEngine.GetLatestTankMeasuresByTime(Int32.Parse(TestReadLastTankMeasuresAmountTextBoxByTime.Text));
            if (tankMeasures != null)
            {
                MessageBox.Show(string.Format("Success! Readed measures count: {0}", tankMeasures.Count));
            }
        }

        private void TestReadLastNozzleMeasuresAmountByTime(object sender, RoutedEventArgs e)
        {
            List<NozzleMeasure> nozzleMeasures = dbEngine.GetLatestNozzleMeasuresByTime(Int32.Parse(TestReadLastNozzleMeasuresAmountTextBoxByTime.Text));
            if (nozzleMeasures != null)
            {
                MessageBox.Show(string.Format("Success! Readed measures count: {0}", nozzleMeasures.Count));
            }
        }

        private void TestReadLastRefuelsAmountByTime(object sender, RoutedEventArgs e)
        {
            List<Refuel> refuels = dbEngine.GetLatestRefuelsByTime(Int32.Parse(TestReadLastRefuelsAmountTextBoxByTime.Text));
            if (refuels != null)
            {
                MessageBox.Show(string.Format("Success! Readed refuels count: {0}", refuels.Count));
            }
        }
    }
}
