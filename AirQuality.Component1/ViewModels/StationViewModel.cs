using AirQuality.Common.Models;
using AirQuality.Component1.Commands;
using AirQuality.Component1.Helpers;
using AirQuality.Component1.Services;
using AirQuality.Component1.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AirQuality.Component1.ViewModels
{
    public class StationViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly LogService _logService;
        private readonly UndoRedoManager _undoRedoManager;

        private ObservableCollection<MonitoringStation> _stations;
        private MonitoringStation _selectedStation;
        private string _searchText;

        public ObservableCollection<MonitoringStation> Stations
        {
            get => _stations;
            set { _stations = value; OnPropertyChanged(); }
        }

        public MonitoringStation SelectedStation
        {
            get => _selectedStation;
            set { _selectedStation = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); Search(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public StationViewModel()
        {
            _dataService = DataService.Instance;
            _logService = new LogService();
            _undoRedoManager = new UndoRedoManager();

            Stations = new ObservableCollection<MonitoringStation>(_dataService.Stations);

            AddCommand = new RelayCommand(AddStation);
            EditCommand = new RelayCommand(EditStation, _ => SelectedStation != null);
            DeleteCommand = new RelayCommand(DeleteStation, _ => SelectedStation != null);
            UndoCommand = new RelayCommand(_ => Undo(), _ => _undoRedoManager.CanUndo);
            RedoCommand = new RelayCommand(_ => Redo(), _ => _undoRedoManager.CanRedo);
        }

        private void AddStation(object parameter)
        {
            var dialog = new AddStationDialog();
            if (dialog.ShowDialog() == true)
            {
                var newStation = dialog.Result;
                var command = new AddStationCommand(newStation, _dataService.Stations, Stations);
                _undoRedoManager.ExecuteCommand(command);
                SelectedStation = newStation;
                _logService.Log($"Dodana stanica: {newStation.Name}");
            }
        }

        private void EditStation(object parameter)
        {
            if (SelectedStation == null) return;

            var dialog = new EditStationDialog(SelectedStation);
            if (dialog.ShowDialog() == true)
            {
                var command = new EditStationCommand(
                    SelectedStation,
                    dialog.StationName,
                    dialog.City,
                    dialog.District,
                    dialog.Latitude,
                    dialog.Longitude);

                _undoRedoManager.ExecuteCommand(command);
                _logService.Log($"Izmijenjena stanica: {SelectedStation.Name}");
                OnPropertyChanged(nameof(SelectedStation));
            }
        }

        private void DeleteStation(object parameter)
        {
            if (SelectedStation == null) return;

            var command = new DeleteStationCommand(SelectedStation, _dataService.Stations, Stations);
            _logService.Log($"Obrisana stanica: {SelectedStation.Name}");
            _undoRedoManager.ExecuteCommand(command);
            SelectedStation = null;
        }

        private void Undo()
        {
            _undoRedoManager.Undo();
            _logService.Log("Undo akcija izvršena.");
        }

        private void Redo()
        {
            _undoRedoManager.Redo();
            _logService.Log("Redo akcija izvršena.");
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Stations = new ObservableCollection<MonitoringStation>(_dataService.Stations);
                return;
            }

            var filtered = _dataService.Stations.FindAll(s =>
                s.Name.ToLower().Contains(SearchText.ToLower()) ||
                s.City.ToLower().Contains(SearchText.ToLower()) ||
                s.District.ToLower().Contains(SearchText.ToLower()));

            Stations = new ObservableCollection<MonitoringStation>(filtered);
        }
    }
}