using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public abstract class ReadingCommand : IUndoableCommand
    {
        protected AirQualityReadingService Receiver;

        protected ReadingCommand(AirQualityReadingService receiver)
        {
            Receiver = receiver;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}
