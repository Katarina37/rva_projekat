using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;

namespace AirQuality.Component1.Commands
{
    public class EditReadingCommand : IUndoableCommand
    {
        private readonly AirQualityReading _reading;

        private readonly double _oldPM25, _oldNO2Level, _oldOzoneLevel;
        private readonly AirQualityState _oldState;

        private readonly double _newPM25, _newNO2Level, _newOzoneLevel;
        private readonly AirQualityState _newState;

        public EditReadingCommand(AirQualityReading reading,
            double newPM25, double newNO2Level, double newOzoneLevel,
            AirQualityState newState)
        {
            _reading = reading;

            _oldPM25 = reading.PM25;
            _oldNO2Level = reading.NO2Level;
            _oldOzoneLevel = reading.OzoneLevel;
            _oldState = reading.State;

            _newPM25 = newPM25;
            _newNO2Level = newNO2Level;
            _newOzoneLevel = newOzoneLevel;
            _newState = newState;
        }

        public void Execute()
        {
            _reading.PM25 = _newPM25;
            _reading.NO2Level = _newNO2Level;
            _reading.OzoneLevel = _newOzoneLevel;
            _reading.State = _newState;
        }

        public void Undo()
        {
            _reading.PM25 = _oldPM25;
            _reading.NO2Level = _oldNO2Level;
            _reading.OzoneLevel = _oldOzoneLevel;
            _reading.State = _oldState;
        }
    }
}