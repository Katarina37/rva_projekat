using AirQuality.Common.Models;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public class DeleteStationCommand : StationCommand
    {
        private readonly MonitoringStation _station;

        public DeleteStationCommand(MonitoringStation station, MonitoringStationManagementService receiver)
            : base(receiver)
        {
            _station = station;
        }

        public override void Execute()
        {
            Receiver.DeleteStation(_station);
        }

        public override void Undo()
        {
            Receiver.AddStation(_station);
        }
    }
}
