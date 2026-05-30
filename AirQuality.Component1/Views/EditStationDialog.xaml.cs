using AirQuality.Common.Models;
using System.Globalization;
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
            TxtLatitude.Text = station.Latitude.ToString(CultureInfo.InvariantCulture);
            TxtLongitude.Text = station.Longitude.ToString(CultureInfo.InvariantCulture);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            StationName = TxtName.Text.Trim();
            City = TxtCity.Text.Trim();
            District = TxtDistrict.Text.Trim();
            Latitude = double.Parse(TxtLatitude.Text, CultureInfo.InvariantCulture);
            Longitude = double.Parse(TxtLongitude.Text, CultureInfo.InvariantCulture);

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
            if (string.IsNullOrWhiteSpace(TxtDistrict.Text))
            {
                TxtError.Text = "Četvrt ne smije biti prazna.";
                return false;
            }
            if (!double.TryParse(TxtLatitude.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) || lat < -90 || lat > 90)
            {
                TxtError.Text = "Geografska širina mora biti broj između -90 i 90.";
                return false;
            }
            if (!double.TryParse(TxtLongitude.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon) || lon < -180 || lon > 180)
            {
                TxtError.Text = "Geografska dužina mora biti broj između -180 i 180.";
                return false;
            }

            return true;
        }
    }
}
