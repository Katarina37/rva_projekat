using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace AirQuality.Component1.Commands
{
    public class EditReadingCommand : IUndoableCommand
    {
        private readonly ObservableCollection<AirQualityReading> readings;
        private readonly AirQualityReading reading;
        private readonly Guid oldStationId;
        private readonly double oldPM25, oldNO2, oldOzone;
        private readonly AirQualityState oldState;
        private readonly Guid newStationId;
        private readonly double newPM25, newNO2, newOzone;
        private readonly AirQualityState newState;

        public EditReadingCommand(ObservableCollection<AirQualityReading> readings,
            AirQualityReading reading, AirQualityReading newValues)
        {
            this.readings = readings;
            this.reading = reading;

            oldStationId = reading.StationId;
            oldPM25 = reading.PM25;
            oldNO2 = reading.NO2Level;
            oldOzone = reading.OzoneLevel;
            oldState = reading.State;

            newStationId = newValues.StationId;
            newPM25 = newValues.PM25;
            newNO2 = newValues.NO2Level;
            newOzone = newValues.OzoneLevel;
            newState = newValues.State;
        }

        public void Execute() => ApplyValues(newStationId, newPM25, newNO2, newOzone, newState);

        public void Undo() => ApplyValues(oldStationId, oldPM25, oldNO2, oldOzone, oldState);

        private void ApplyValues(Guid stationId, double pm25, double no2, double ozone, AirQualityState state)
        {
            reading.StationId = stationId;
            reading.PM25 = pm25;
            reading.NO2Level = no2;
            reading.OzoneLevel = ozone;
            reading.State = state;

            var index = readings.IndexOf(reading);
            readings.RemoveAt(index);
            readings.Insert(index, reading);
        }
    }
}