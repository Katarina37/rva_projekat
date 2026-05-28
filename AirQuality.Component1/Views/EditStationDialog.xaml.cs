using AirQuality.Common.Models;
using System;
using System.Windows;

namespace AirQuality.Component1.Views
{
    public partial class EditStationDialog : Window
    {
        public string StationName { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public EditStationDialog(MonitoringStation station)
        {
            InitializeComponent();
            TxtName.Text = station.Name;
            TxtCity.Text = station.City;
            TxtDistrict.Text = station.District;
            TxtLatitude.Text = station.Latitude.ToString();
            TxtLongitude.Text = station.Longitude.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            StationName = TxtName.Text.Trim();
            City = TxtCity.Text.Trim();
            District = TxtDistrict.Text.Trim();
            Latitude = double.Parse(TxtLatitude.Text);
            Longitude = double.Parse(TxtLongitude.Text);

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool Validate()
        {
            TxtError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                TxtError.Text = "Naziv ne smije biti prazan.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtCity.Text))
            {
                TxtError.Text = "Grad ne smije biti prazan.";
                return false;
            }

            if (!double.TryParse(TxtLatitude.Text, out _))
            {
                TxtError.Text = "Geografska širina mora biti broj.";
                return false;
            }

            if (!double.TryParse(TxtLongitude.Text, out _))
            {
                TxtError.Text = "Geografska dužina mora biti broj.";
                return false;
            }

            return true;
        }
    }
}