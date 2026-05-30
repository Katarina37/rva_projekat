using AirQuality.Component1.Services;
using AirQuality.Component1.Views;
using System.Windows;

namespace AirQuality.Component1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new StationView();
        }

        private void BtnStations_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new StationView();
        }

        private void BtnReadings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReadingView();
        }

        private void BtnCharts_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ChartView();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DataService.Instance.SaveToJson();
            MessageBox.Show("Podaci su uspešno sačuvani.", "Čuvanje", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            DataService.Instance.LoadFromJson();
            MainContent.Content = new StationView();
            MessageBox.Show("Podaci su uspešno učitani.", "Učitavanje", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
