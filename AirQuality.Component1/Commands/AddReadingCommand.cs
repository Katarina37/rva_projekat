using AirQuality.Common.Models;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public class AddReadingCommand : ReadingCommand
    {
        private readonly AirQualityReading _reading;

        public AddReadingCommand(AirQualityReading reading, AirQualityReadingService receiver)
            : base(receiver)
        {
            _reading = reading;
        }

        public override void Execute()
        {
            Receiver.AddReading(_reading);
        }

        public override void Undo()
        {
            Receiver.DeleteReading(_reading);
        }
    }
}
