using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public abstract class StationCommand : IUndoableCommand
    {
        protected MonitoringStationManagementService Receiver;

        protected StationCommand(MonitoringStationManagementService receiver)
        {
            Receiver = receiver;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}
