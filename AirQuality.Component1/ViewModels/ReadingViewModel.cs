using AirQuality.Common.Models;
using AirQuality.Component1.Commands;
using AirQuality.Component1.Helpers;
using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace AirQuality.Component1.ViewModels
{
    public class ReadingViewModel : BaseViewModel
    {
        private readonly AirQualityReadingService _readingService;
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
            _readingService = AirQualityReadingService.Instance;
            _logService = new LogService();
            _undoRedoManager = new UndoRedoManager();

            Readings = new ObservableCollection<AirQualityReading>(_readingService.GetReadings());

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
                var command = new AddReadingCommand(newReading, _readingService);
                var loggedCommand = CreateLoggedCommand(
                    command,
                    $"Dodano mjerenje za stanicu: {newReading.StationId}, stanje: {newReading.State}",
                    $"Poništeno dodavanje mjerenja za stanicu: {newReading.StationId}");

                _undoRedoManager.ExecuteCommand(loggedCommand);
                RefreshReadings();
            }
        }

        private void DeleteReading(object parameter)
        {
            if (SelectedReading == null) return;

            var reading = SelectedReading;
            var command = new DeleteReadingCommand(reading, _readingService);
            var loggedCommand = CreateLoggedCommand(
                command,
                $"Obrisano mjerenje: StationId={reading.StationId}, stanje={reading.State}",
                $"Poništeno brisanje mjerenja: StationId={reading.StationId}");

            _undoRedoManager.ExecuteCommand(loggedCommand);
            SelectedReading = null;
            RefreshReadings();
        }

        private void Undo()
        {
            _undoRedoManager.Undo();
            RefreshReadings();
        }

        private void Redo()
        {
            _undoRedoManager.Redo();
            RefreshReadings();
        }

        private void Search()
        {
            RefreshReadings();
        }

        private void RefreshReadings()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Readings = new ObservableCollection<AirQualityReading>(_readingService.GetReadings());
                return;
            }

            var lower = SearchText.ToLower();
            var filtered = _readingService.GetReadings().FindAll(r => MatchesReading(r, lower));
            Readings = new ObservableCollection<AirQualityReading>(filtered);
        }

        private bool MatchesReading(AirQualityReading reading, string lower)
        {
            return Contains(reading.StationId.ToString(), lower) ||
                   Contains(reading.ReadingTime.ToString(CultureInfo.CurrentCulture), lower) ||
                   Contains(reading.ReadingTime.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture), lower) ||
                   Contains(reading.PM25.ToString(CultureInfo.InvariantCulture), lower) ||
                   Contains(reading.NO2Level.ToString(CultureInfo.InvariantCulture), lower) ||
                   Contains(reading.OzoneLevel.ToString(CultureInfo.InvariantCulture), lower) ||
                   Contains(reading.State.ToString(), lower);
        }

        private bool Contains(string value, string lower)
        {
            return value != null && value.ToLower().Contains(lower);
        }

        private void EditReading(object parameter)
        {
            if (SelectedReading == null) return;

            var reading = SelectedReading;
            var oldState = reading.State;
            var dialog = new Views.EditReadingDialog(reading);
            if (dialog.ShowDialog() == true)
            {
                var command = new EditReadingCommand(
                    reading,
                    _readingService,
                    dialog.PM25,
                    dialog.NO2Level,
                    dialog.OzoneLevel,
                    dialog.State);

                var loggedCommand = CreateLoggedCommand(
                    command,
                    $"Izmijenjeno mjerenje: StationId={reading.StationId}, novo stanje={dialog.State}",
                    $"Poništena izmjena mjerenja: StationId={reading.StationId}, vraćeno stanje={oldState}");

                _undoRedoManager.ExecuteCommand(loggedCommand);
                RefreshReadings();
            }
        }

        private IUndoableCommand CreateLoggedCommand(IUndoableCommand command, string executeMessage, string undoMessage)
        {
            return new LoggingCommandDecorator(command, _logService, executeMessage, undoMessage);
        }
    }
}
