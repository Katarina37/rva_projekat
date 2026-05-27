using AirQuality.Common.Models;
using AirQuality.Component1.Commands;
using AirQuality.Component1.Helpers;
using AirQuality.Component1.Services;
using AirQuality.Component1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirQuality.Component1.ViewModels
{
    public class StationViewModel : BaseViewModel
    {
        private readonly DataService dataService;
        private readonly LogService logService;
        private readonly UndoRedoService undoRedoService;

        private ObservableCollection<MonitoringStation> stations;
        private MonitoringStation selectedStation;
        private string searchText;
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ObservableCollection<MonitoringStation> Stations
        {
            get => stations;
            set { stations = value; OnPropertyChanged(); }
        }

        public MonitoringStation SelectedStation
        {
            get => selectedStation;
            set {  selectedStation = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => searchText;
            set { searchText = value; OnPropertyChanged(); Search(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public StationViewModel()
        {
            dataService = DataService.Instance;
            logService = new LogService();

            Stations = new ObservableCollection<MonitoringStation>(dataService.Stations);

            AddCommand = new RelayCommand(AddStation);
            EditCommand = new RelayCommand(EditStation, _ => SelectedStation != null);
            DeleteCommand = new RelayCommand(DeleteStation, _ => SelectedStation != null);

            undoRedoService = new UndoRedoService();
            UndoCommand = new RelayCommand(_ => undoRedoService.Undo(), _ => undoRedoService.CanUndo);
            RedoCommand = new RelayCommand(_ => undoRedoService.Redo(), _ => undoRedoService.CanRedo);
        }

        private void AddStation(object parameter)
        {
            var dialog = new AddStationDialog();
            if (dialog.ShowDialog() == true)
            {
                var command = new AddStationCommand(dataService, Stations, dialog.Result);
                undoRedoService.ExecuteCommand(command);
                SelectedStation = dialog.Result;
                logService.Log($"Dodana stanica: {dialog.Result.Name}, Grad: {dialog.Result.City}");
            }
        }

        private void EditStation(object parameter)
        {
            if (SelectedStation == null) return;

            var dialog = new AddStationDialog(SelectedStation);
            if (dialog.ShowDialog() == true)
            {
                var command = new EditStationCommand(Stations, SelectedStation, dialog.Result);
                undoRedoService.ExecuteCommand(command);
                logService.Log($"Izmijenjena stanica: {SelectedStation.Name}");
            }
        }

        private void DeleteStation(object parameter)
        {
            if (SelectedStation == null) return;

            var command = new DeleteStationCommand(dataService, Stations, SelectedStation);
            undoRedoService.ExecuteCommand(command);
            logService.Log($"Obrisana stanica: {SelectedStation.Name}");
            SelectedStation = null;
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Stations = new ObservableCollection<MonitoringStation>(dataService.Stations);
                return;
            }

            var filtered = dataService.Stations.FindAll(s =>
                s.Name.ToLower().Contains(SearchText.ToLower()) ||
                s.City.ToLower().Contains(SearchText.ToLower()) ||
                s.District.ToLower().Contains(SearchText.ToLower()));

            Stations = new ObservableCollection<MonitoringStation>(filtered);
        }

    }
}
