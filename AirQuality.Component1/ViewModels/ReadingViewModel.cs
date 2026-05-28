using AirQuality.Common.Models;
using AirQuality.Component1.Commands;
using AirQuality.Component1.Helpers;
using AirQuality.Component1.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AirQuality.Component1.ViewModels
{
    public class ReadingViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly LogService _logService;
        private readonly UndoRedoManager _undoRedoManager;

        private ObservableCollection<AirQualityReading> _readings;
        private AirQualityReading _selectedReading;
        private string _searchText;
        public ICommand EditCommand { get; }

        public ObservableCollection<AirQualityReading> Readings
        {
            get => _readings;
            set { _readings = value; OnPropertyChanged(); }
        }

        public AirQualityReading SelectedReading
        {
            get => _selectedReading;
            set { _selectedReading = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); Search(); }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public ReadingViewModel()
        {
            _dataService = DataService.Instance;
            _logService = new LogService();
            _undoRedoManager = new UndoRedoManager();

            Readings = new ObservableCollection<AirQualityReading>(_dataService.Readings);

            AddCommand = new RelayCommand(AddReading);
            DeleteCommand = new RelayCommand(DeleteReading, _ => SelectedReading != null);
            UndoCommand = new RelayCommand(_ => Undo(), _ => _undoRedoManager.CanUndo);
            RedoCommand = new RelayCommand(_ => Redo(), _ => _undoRedoManager.CanRedo);
            EditCommand = new RelayCommand(EditReading, _ => SelectedReading != null);
        }

        private void AddReading(object parameter)
        {
            var dialog = new Views.AddReadingDialog();
            if (dialog.ShowDialog() == true)
            {
                var newReading = dialog.NewReading;
                var command = new AddReadingCommand(newReading, _dataService.Readings, Readings);
                _undoRedoManager.ExecuteCommand(command);
                _logService.Log($"Dodano mjerenje za stanicu: {newReading.StationId}, stanje: {newReading.State}");
            }
        }

        private void DeleteReading(object parameter)
        {
            if (SelectedReading == null) return;

            var command = new DeleteReadingCommand(SelectedReading, _dataService.Readings, Readings);
            _logService.Log($"Obrisano mjerenje: StationId={SelectedReading.StationId}, stanje={SelectedReading.State}");
            _undoRedoManager.ExecuteCommand(command);
            SelectedReading = null;
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
                Readings = new ObservableCollection<AirQualityReading>(_dataService.Readings);
                return;
            }

            var lower = SearchText.ToLower();
            var filtered = _dataService.Readings.FindAll(r =>
                r.State.ToString().ToLower().Contains(lower) ||
                r.StationId.ToString().ToLower().Contains(lower));

            Readings = new ObservableCollection<AirQualityReading>(filtered);
        }

        private void EditReading(object parameter)
        {
            if (SelectedReading == null) return;

            var dialog = new Views.EditReadingDialog(SelectedReading);
            if (dialog.ShowDialog() == true)
            {
                var command = new EditReadingCommand(
                    SelectedReading,
                    dialog.PM25,
                    dialog.NO2Level,
                    dialog.OzoneLevel,
                    dialog.State);

                _undoRedoManager.ExecuteCommand(command);
                _logService.Log($"Izmijenjeno mjerenje: StationId={SelectedReading.StationId}, novo stanje={SelectedReading.State}");
            }
        }
    }
}