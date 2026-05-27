using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;
using System.Collections.ObjectModel;

namespace AirQuality.Component1.Commands
{
    public class DeleteReadingCommand : IUndoableCommand
    {
        private readonly DataService dataService;
        private readonly ObservableCollection<AirQualityReading> readings;
        private readonly AirQualityReading reading;

        public DeleteReadingCommand(DataService dataService,
            ObservableCollection<AirQualityReading> readings,
            AirQualityReading reading)
        {
            this.dataService = dataService;
            this.readings = readings;
            this.reading = reading;
        }

        public void Execute()
        {
            dataService.Readings.Remove(reading);
            readings.Remove(reading);
        }

        public void Undo()
        {
            dataService.Readings.Add(reading);
            readings.Add(reading);
        }
    }
}