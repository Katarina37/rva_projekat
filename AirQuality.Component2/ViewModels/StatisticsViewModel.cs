using AirQuality.Common.Interfaces;
using AirQuality.Common.Models;
using AirQuality.Component2.Adapters;
using AirQuality.Component2.Helpers;
using AirQuality.Component2.Models;
using AirQuality.Component2.Services;
using AirQuality.Component2.Strategies;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace AirQuality.Component2.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly IReadingDataAdapter _adapter;
        private readonly WcfClientService _wcfClientService;
        private readonly CsvExportService _csvExportService;
        private readonly StatisticsProcessor _statisticsProcessor;

        private MonitoringStation _selectedStation;
        private string _selectedMonth;
        private int _selectedYear;
        private IStatisticsStrategy _selectedStrategy;
        private Dictionary<string, List<AirQualityReading>> _adaptedReadings;
        private StatisticsResult _result;

        public ObservableCollection<MonitoringStation> Stations { get; set; }
        public ObservableCollection<string> Months { get; set; }
        public ObservableCollection<IStatisticsStrategy> Strategies { get; set; }
        public ObservableCollection<string> DisplayReadings { get; set; }

        public MonitoringStation SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                OnPropertyChanged();
            }
        }

        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();
            }
        }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }

        public IStatisticsStrategy SelectedStrategy
        {
            get { return _selectedStrategy; }
            set
            {
                _selectedStrategy = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, List<AirQualityReading>> AdaptedReadings
        {
            get { return _adaptedReadings; }
            set
            {
                _adaptedReadings = value;
                OnPropertyChanged();
            }
        }

        public StatisticsResult Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDataCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand ExportCsvCommand { get; set; }

        public StatisticsViewModel()
        {
            _wcfClientService = new WcfClientService();
            _adapter = new WcfReadingDataAdapter(_wcfClientService);
            _csvExportService = new CsvExportService();
            _statisticsProcessor = new StatisticsProcessor();

            Stations = new ObservableCollection<MonitoringStation>();
            Months = new ObservableCollection<string>();
            Strategies = new ObservableCollection<IStatisticsStrategy>();
            DisplayReadings = new ObservableCollection<string>();
            AdaptedReadings = new Dictionary<string, List<AirQualityReading>>();

            LoadDataCommand = new RelayCommand(_ => LoadData());
            CalculateCommand = new RelayCommand(_ => Calculate());
            ExportCsvCommand = new RelayCommand(_ => ExportCsv());

            FillMonths();
            FillStrategies();

            SelectedMonth = DateTime.Now.Month.ToString("00", CultureInfo.InvariantCulture);
            SelectedYear = DateTime.Now.Year;
            SelectedStrategy = Strategies.FirstOrDefault();

            LoadStations();
        }

        private void FillMonths()
        {
            for (int month = 1; month <= 12; month++)
            {
                Months.Add(month.ToString("00", CultureInfo.InvariantCulture));
            }
        }

        private void FillStrategies()
        {
            Strategies.Add(new AveragePm25Strategy());
            Strategies.Add(new MaxNo2Strategy());
            Strategies.Add(new HazardousCountStrategy());
        }

        private void LoadStations()
        {
            IMonitoringStationService client = null;

            try
            {
                client = _wcfClientService.CreateClient();
                List<MonitoringStation> stations = client.GetAllStations();

                Stations.Clear();
                foreach (MonitoringStation station in stations)
                {
                    Stations.Add(station);
                }

                SelectedStation = Stations.FirstOrDefault();
                CloseClient(client);
            }
            catch
            {
                AbortClient(client);
                AddFallbackStation();
                MessageBox.Show("WCF servis nije dostupan. Prikazana je probna stanica.",
                    "Komponenta 2", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadData()
        {
            if (SelectedStation == null || string.IsNullOrWhiteSpace(SelectedMonth) || SelectedYear <= 0)
            {
                MessageBox.Show("Izaberite stanicu, mesec i godinu.",
                    "Komponenta 2", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int month = int.Parse(SelectedMonth, CultureInfo.InvariantCulture);

            try
            {
                AdaptedReadings = _adapter.GetReadings(SelectedStation.Id, month, SelectedYear);
            }
            catch
            {
                AdaptedReadings = CreateFallbackReadings(SelectedStation.Id, month, SelectedYear);
                MessageBox.Show("WCF servis nije dostupan. Prikazana su probna merenja.",
                    "Komponenta 2", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Result = null;
            FillDisplayReadings();
        }

        private void Calculate()
        {
            if (AdaptedReadings == null || AdaptedReadings.Count == 0)
            {
                MessageBox.Show("Prvo preuzmite podatke.",
                    "Komponenta 2", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedStrategy == null)
            {
                MessageBox.Show("Izaberite statističku metodu.",
                    "Komponenta 2", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _statisticsProcessor.SetStrategy(SelectedStrategy);
            Result = _statisticsProcessor.Process(AdaptedReadings);
        }

        private void ExportCsv()
        {
            if (Result == null)
            {
                MessageBox.Show("Prvo izračunajte statistiku.",
                    "Komponenta 2", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "CSV fajl (*.csv)|*.csv",
                FileName = "statistika.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                _csvExportService.Export(dialog.FileName, Result);
                MessageBox.Show("CSV fajl je sačuvan.",
                    "Komponenta 2", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FillDisplayReadings()
        {
            DisplayReadings.Clear();

            if (AdaptedReadings == null || AdaptedReadings.All(item => item.Value == null || item.Value.Count == 0))
            {
                DisplayReadings.Add("Nema merenja za izabranu stanicu i mesec.");
                return;
            }

            foreach (KeyValuePair<string, List<AirQualityReading>> item in AdaptedReadings)
            {
                DisplayReadings.Add(FormatDisplayLine(item.Key, item.Value));
            }
        }

        private string FormatDisplayLine(string key, List<AirQualityReading> readings)
        {
            string stationId = key.Length >= 36 ? key.Substring(0, 36) : SelectedStation.Id.ToString();
            string monthYear = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", SelectedMonth, SelectedYear);
            string readingText = string.Join(", ", readings.Select(FormatReading));

            return string.Format(CultureInfo.InvariantCulture,
                "({0}, {1}) -> {2}", stationId, monthYear, readingText);
        }

        private string FormatReading(AirQualityReading reading)
        {
            return string.Format(CultureInfo.InvariantCulture,
                "[PM2.5: {0:0.##}, NO2: {1:0.##}, stanje: {2}]",
                reading.PM25,
                reading.NO2Level,
                AirQualityStateDisplay.ToSerbianText(reading.State));
        }

        private void AddFallbackStation()
        {
            Stations.Clear();
            Stations.Add(new MonitoringStation
            {
                Id = Guid.NewGuid(),
                Name = "Probna stanica",
                City = "Sarajevo",
                District = "Centar",
                Latitude = 43.8563,
                Longitude = 18.4131
            });
            SelectedStation = Stations.FirstOrDefault();
        }

        private Dictionary<string, List<AirQualityReading>> CreateFallbackReadings(Guid stationId, int month, int year)
        {
            string key = string.Format(CultureInfo.InvariantCulture, "{0}-{1:00}-{2}", stationId, month, year);

            var readings = new List<AirQualityReading>
            {
                new AirQualityReading
                {
                    StationId = stationId,
                    ReadingTime = new DateTime(year, month, 1),
                    PM25 = 18.3,
                    NO2Level = 42.1,
                    OzoneLevel = 30,
                    State = AirQualityState.Good
                },
                new AirQualityReading
                {
                    StationId = stationId,
                    ReadingTime = new DateTime(year, month, 2),
                    PM25 = 55.7,
                    NO2Level = 88.4,
                    OzoneLevel = 60,
                    State = AirQualityState.Unhealthy
                },
                new AirQualityReading
                {
                    StationId = stationId,
                    ReadingTime = new DateTime(year, month, 3),
                    PM25 = 90,
                    NO2Level = 130,
                    OzoneLevel = 95,
                    State = AirQualityState.Hazardous
                }
            };

            return new Dictionary<string, List<AirQualityReading>>
            {
                { key, readings }
            };
        }

        private void CloseClient(IMonitoringStationService client)
        {
            var channel = client as IClientChannel;
            if (channel != null)
            {
                channel.Close();
            }
        }

        private void AbortClient(IMonitoringStationService client)
        {
            var channel = client as IClientChannel;
            if (channel != null)
            {
                channel.Abort();
            }
        }
    }
}
