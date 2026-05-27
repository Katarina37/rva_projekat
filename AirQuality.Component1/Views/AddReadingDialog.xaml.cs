using AirQuality.Common.Models;
using System.Collections.Generic;
using System.Windows;

namespace AirQuality.Component1.Views
{
    public partial class AddReadingDialog : Window
    {
        public AirQualityReading Result { get; private set; }

        public AddReadingDialog(List<MonitoringStation> stations)
        {
            InitializeComponent();

            CmbStation.ItemsSource = stations;
            CmbStation.SelectedIndex = 0;

            CmbState.ItemsSource = System.Enum.GetValues(typeof(AirQualityState));
            CmbState.SelectedIndex = 0;
        }

        public AddReadingDialog(List<MonitoringStation> stations, AirQualityReading existing)
            : this(stations)
        {
            CmbStation.SelectedItem = stations.Find(s => s.Id == existing.StationId);
            TxtPM25.Text = existing.PM25.ToString();
            TxtNO2.Text = existing.NO2Level.ToString();
            TxtOzone.Text = existing.OzoneLevel.ToString();
            CmbState.SelectedItem = existing.State;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            var station = (MonitoringStation)CmbStation.SelectedItem;

            Result = new AirQualityReading
            {
                StationId = station.Id,
                PM25 = double.Parse(TxtPM25.Text.Trim()),
                NO2Level = double.Parse(TxtNO2.Text.Trim()),
                OzoneLevel = double.Parse(TxtOzone.Text.Trim()),
                State = (AirQualityState)CmbState.SelectedItem
            };

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool Validate()
        {
            TxtError.Text = string.Empty;

            if (CmbStation.SelectedItem == null)
            {
                TxtError.Text = "Odaberite stanicu.";
                return false;
            }
            if (!double.TryParse(TxtPM25.Text, out double pm25) || pm25 < 0)
            {
                TxtError.Text = "PM2.5 mora biti pozitivan broj.";
                return false;
            }
            if (!double.TryParse(TxtNO2.Text, out double no2) || no2 < 0)
            {
                TxtError.Text = "NO2 mora biti pozitivan broj.";
                return false;
            }
            if (!double.TryParse(TxtOzone.Text, out double ozone) || ozone < 0)
            {
                TxtError.Text = "Ozon mora biti pozitivan broj.";
                return false;
            }

            return true;
        }
    }
}