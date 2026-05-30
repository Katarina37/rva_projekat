using AirQuality.Common.Models;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public class ChangeReadingStateCommand : ReadingCommand
    {
        private readonly AirQualityReading _reading;
        private readonly AirQualityState _oldState;

        public ChangeReadingStateCommand(AirQualityReading reading, AirQualityReadingService receiver)
            : base(receiver)
        {
            _reading = reading;
            _oldState = reading.State;
        }

        public override void Execute()
        {
            Receiver.ChangeReadingState(_reading);
        }

        public override void Undo()
        {
            Receiver.SetReadingState(_reading, _oldState);
        }
    }
}
