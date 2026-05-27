using AirQuality.Common.Models;
using System.Windows;

namespace AirQuality.Component1.Views
{
    public partial class AddStationDialog : Window
    {
        public MonitoringStation Result { get; private set; }

        public AddStationDialog()
        {
            InitializeComponent();
        }

        public AddStationDialog(MonitoringStation existing) : this()
        {
            TxtName.Text = existing.Name;
            TxtCity.Text = existing.City;
            TxtDistrict.Text = existing.District;
            TxtLatitude.Text = existing.Latitude.ToString();
            TxtLongitude.Text = existing.Longitude.ToString();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            Result = new MonitoringStation
            {
                Name = TxtName.Text.Trim(),
                City = TxtCity.Text.Trim(),
                District = TxtDistrict.Text.Trim(),
                Latitude = double.Parse(TxtLatitude.Text.Trim()),
                Longitude = double.Parse(TxtLongitude.Text.Trim())
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

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                TxtError.Text = "Naziv je obavezan.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtCity.Text))
            {
                TxtError.Text = "Grad je obavezan.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtDistrict.Text))
            {
                TxtError.Text = "Četvrt je obavezna.";
                return false;
            }
            if (!double.TryParse(TxtLatitude.Text, out double lat) || lat < -90 || lat > 90)
            {
                TxtError.Text = "Geografska širina mora biti broj između -90 i 90.";
                return false;
            }
            if (!double.TryParse(TxtLongitude.Text, out double lon) || lon < -180 || lon > 180)
            {
                TxtError.Text = "Geografska dužina mora biti broj između -180 i 180.";
                return false;
            }

            return true;
        }
    }
}