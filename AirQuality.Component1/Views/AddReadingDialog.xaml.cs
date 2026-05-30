using AirQuality.Common.Models;
using AirQuality.Component1.Services;
using System;
using System.Linq;
using System.Windows;

namespace AirQuality.Component1.Views
{
    public partial class AddReadingDialog : Window
    {
        public AirQualityReading NewReading { get; private set; }

        public AddReadingDialog()
        {
            InitializeComponent();
            LoadStations();
            LoadStates();
        }

        private void LoadStations()
        {
            CmbStation.ItemsSource = DataService.Instance.Stations;
            CmbStation.DisplayMemberPath = "Name";
            CmbStation.SelectedValuePath = "Id";
        }

        private void LoadStates()
        {
            CmbState.ItemsSource = Enum.GetValues(typeof(AirQualityState))
                .Cast<AirQualityState>()
                .Select(state => new StateOption(state))
                .ToList();
            CmbState.DisplayMemberPath = "Name";
            CmbState.SelectedValuePath = "Value";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            NewReading = new AirQualityReading
            {
                StationId = (Guid)CmbStation.SelectedValue,
                PM25 = double.Parse(TxtPM25.Text),
                NO2Level = double.Parse(TxtNO2.Text),
                OzoneLevel = double.Parse(TxtOzone.Text),
                State = (AirQualityState)CmbState.SelectedValue,
                ReadingTime = DateTime.Now
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
                TxtError.Text = "Morate odabrati stanicu.";
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

            if (CmbState.SelectedItem == null)
            {
                TxtError.Text = "Morate odabrati stanje.";
                return false;
            }

            return true;
        }
        private sealed class StateOption
        {
            public StateOption(AirQualityState value)
            {
                Value = value;
                Name = AirQualityStateDisplay.ToSerbianText(value);
            }

            public AirQualityState Value { get; }

            public string Name { get; }
        }
    }
}
