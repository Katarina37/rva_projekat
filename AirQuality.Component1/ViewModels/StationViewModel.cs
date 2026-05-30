using AirQuality.Common.Models;
using AirQuality.Component1.Commands;
using AirQuality.Component1.Helpers;
using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;
using AirQuality.Component1.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace AirQuality.Component1.ViewModels
{
    public class StationViewModel : BaseViewModel
    {
        private readonly MonitoringStationManagementService _stationService;
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
            _stationService = MonitoringStationManagementService.Instance;
            _logService = new LogService();
            _undoRedoManager = new UndoRedoManager();

            Stations = new ObservableCollection<MonitoringStation>(_stationService.GetStations());

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
                var command = new AddStationCommand(newStation, _stationService);
                var loggedCommand = CreateLoggedCommand(
                    command,
                    $"Dodana stanica: {newStation.Name}",
                    $"Poništeno dodavanje stanice: {newStation.Name}");

                _undoRedoManager.ExecuteCommand(loggedCommand);
                SelectedStation = newStation;
                RefreshStations();
            }
        }

        private void EditStation(object parameter)
        {
            if (SelectedStation == null) return;

            var station = SelectedStation;
            var oldName = station.Name;
            var dialog = new EditStationDialog(station);
            if (dialog.ShowDialog() == true)
            {
                var command = new EditStationCommand(
                    station,
                    _stationService,
                    dialog.StationName,
                    dialog.City,
                    dialog.District,
                    dialog.Latitude,
                    dialog.Longitude);

                var loggedCommand = CreateLoggedCommand(
                    command,
                    $"Izmijenjena stanica: {dialog.StationName}",
                    $"Poništena izmjena stanice: {oldName}");

                _undoRedoManager.ExecuteCommand(loggedCommand);
                RefreshStations();
                OnPropertyChanged(nameof(SelectedStation));
            }
        }

        private void DeleteStation(object parameter)
        {
            if (SelectedStation == null) return;

            var station = SelectedStation;
            var command = new DeleteStationCommand(station, _stationService);
            var loggedCommand = CreateLoggedCommand(
                command,
                $"Obrisana stanica: {station.Name}",
                $"Poništeno brisanje stanice: {station.Name}");

            _undoRedoManager.ExecuteCommand(loggedCommand);
            SelectedStation = null;
            RefreshStations();
        }

        private void Undo()
        {
            _undoRedoManager.Undo();
            RefreshStations();
        }

        private void Redo()
        {
            _undoRedoManager.Redo();
            RefreshStations();
        }

        private void Search()
        {
            RefreshStations();
        }

        private void RefreshStations()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Stations = new ObservableCollection<MonitoringStation>(_stationService.GetStations());
                return;
            }

            var lower = SearchText.ToLower();
            var filtered = _stationService.GetStations().FindAll(s => MatchesStation(s, lower));
            Stations = new ObservableCollection<MonitoringStation>(filtered);
        }

        private bool MatchesStation(MonitoringStation station, string lower)
        {
            return Contains(station.Id.ToString(), lower) ||
                   Contains(station.Name, lower) ||
                   Contains(station.City, lower) ||
                   Contains(station.District, lower) ||
                   Contains(station.Latitude.ToString(CultureInfo.InvariantCulture), lower) ||
                   Contains(station.Longitude.ToString(CultureInfo.InvariantCulture), lower);
        }

        private bool Contains(string value, string lower)
        {
            return value != null && value.ToLower().Contains(lower);
        }

        private IUndoableCommand CreateLoggedCommand(IUndoableCommand command, string executeMessage, string undoMessage)
        {
            return new LoggingCommandDecorator(command, _logService, executeMessage, undoMessage);
        }
    }
}
