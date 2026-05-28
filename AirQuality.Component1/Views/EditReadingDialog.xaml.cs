using AirQuality.Common.Models;
using System;
using System.Windows;

namespace AirQuality.Component1.Views
{
    public partial class EditReadingDialog : Window
    {
        public double PM25 { get; private set; }
        public double NO2Level { get; private set; }
        public double OzoneLevel { get; private set; }
        public AirQualityState State { get; private set; }

        public EditReadingDialog(AirQualityReading reading)
        {
            InitializeComponent();

            CmbState.ItemsSource = Enum.GetValues(typeof(AirQualityState));

            TxtPM25.Text = reading.PM25.ToString();
            TxtNO2.Text = reading.NO2Level.ToString();
            TxtOzone.Text = reading.OzoneLevel.ToString();
            CmbState.SelectedItem = reading.State;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            PM25 = double.Parse(TxtPM25.Text);
            NO2Level = double.Parse(TxtNO2.Text);
            OzoneLevel = double.Parse(TxtOzone.Text);
            State = (AirQualityState)CmbState.SelectedItem;

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool Validate()
        {
            TxtError.Text = string.Empty;

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
            if (CmbState.SelectedItem == null)
            {
                TxtError.Text = "Morate odabrati stanje.";
                return false;
            }

            return true;
        }
    }
}