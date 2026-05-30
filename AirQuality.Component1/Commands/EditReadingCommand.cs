using AirQuality.Common.Models;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public class EditReadingCommand : ReadingCommand
    {
        private readonly AirQualityReading _reading;

        private readonly double _oldPM25, _oldNO2Level, _oldOzoneLevel;
        private readonly AirQualityState _oldState;

        private readonly double _newPM25, _newNO2Level, _newOzoneLevel;
        private readonly AirQualityState _newState;

        public EditReadingCommand(AirQualityReading reading,
            AirQualityReadingService receiver,
            double newPM25, double newNO2Level, double newOzoneLevel,
            AirQualityState newState)
            : base(receiver)
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

        public override void Execute()
        {
            Receiver.EditReading(_reading, _newPM25, _newNO2Level, _newOzoneLevel, _newState);
        }

        public override void Undo()
        {
            Receiver.EditReading(_reading, _oldPM25, _oldNO2Level, _oldOzoneLevel, _oldState);
        }
    }
}
