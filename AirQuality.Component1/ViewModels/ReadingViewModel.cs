using AirQuality.Common.Models;
using AirQuality.Component1.Helpers;
using AirQuality.Component1.Services;
using AirQuality.Component1.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AirQuality.Component1.ViewModels
{
    public class ReadingViewModel : BaseViewModel
    {
        private readonly DataService dataService;
        private readonly LogService logService;

        private ObservableCollection<AirQualityReading> readings;
        private AirQualityReading selectedReading;
        private string searchText;

        public ObservableCollection<AirQualityReading> Readings
        {
            get => readings;
            set { readings = value; OnPropertyChanged(); }
        }

        public AirQualityReading SelectedReading
        {
            get => selectedReading;
            set { selectedReading = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => searchText;
            set { searchText = value; OnPropertyChanged(); Search(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ReadingViewModel()
        {
            dataService = DataService.Instance;
            logService = new LogService();

            Readings = new ObservableCollection<AirQualityReading>(dataService.Readings);

            AddCommand = new RelayCommand(AddReading);
            EditCommand = new RelayCommand(EditReading, _ => SelectedReading != null);
            DeleteCommand = new RelayCommand(DeleteReading, _ => SelectedReading != null);
        }

        private void AddReading(object parameter)
        {
            var dialog = new AddReadingDialog(dataService.Stations);
            if (dialog.ShowDialog() == true)
            {
                dataService.Readings.Add(dialog.Result);
                Readings.Add(dialog.Result);
                SelectedReading = dialog.Result;
                logService.Log($"Dodano mjerenje za stanicu: {dialog.Result.StationId}, PM2.5: {dialog.Result.PM25}, Stanje: {dialog.Result.State}");
            }
        }

        private void EditReading(object parameter)
        {
            if (SelectedReading == null) return;

            var dialog = new AddReadingDialog(dataService.Stations, SelectedReading);
            if (dialog.ShowDialog() == true)
            {
                SelectedReading.StationId = dialog.Result.StationId;
                SelectedReading.PM25 = dialog.Result.PM25;
                SelectedReading.NO2Level = dialog.Result.NO2Level;
                SelectedReading.OzoneLevel = dialog.Result.OzoneLevel;
                SelectedReading.State = dialog.Result.State;

                logService.Log($"Izmijenjeno mjerenje za stanicu: {SelectedReading.StationId}, Stanje: {SelectedReading.State}");

                var index = Readings.IndexOf(SelectedReading);
                Readings.RemoveAt(index);
                Readings.Insert(index, SelectedReading);
            }
        }

        private void DeleteReading(object parameter)
        {
            if (SelectedReading == null) return;
            logService.Log($"Obrisano mjerenje za stanicu: {SelectedReading.StationId}");
            dataService.Readings.Remove(SelectedReading);
            Readings.Remove(SelectedReading);
            SelectedReading = null;
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Readings = new ObservableCollection<AirQualityReading>(dataService.Readings);
                return;
            }

            var lower = SearchText.ToLower();
            var filtered = dataService.Readings.Where(r =>
                r.StationId.ToString().ToLower().Contains(lower) ||
                r.State.ToString().ToLower().Contains(lower) ||
                r.PM25.ToString().Contains(lower) ||
                r.NO2Level.ToString().Contains(lower));

            Readings = new ObservableCollection<AirQualityReading>(filtered);
        }
    }
}