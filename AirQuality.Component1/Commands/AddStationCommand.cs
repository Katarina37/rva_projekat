using AirQuality.Common.Models;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public class AddStationCommand : StationCommand
    {
        private readonly MonitoringStation _station;

        public AddStationCommand(MonitoringStation station, MonitoringStationManagementService receiver)
            : base(receiver)
        {
            _station = station;
        }

        public override void Execute()
        {
            Receiver.AddStation(_station);
        }

        public override void Undo()
        {
            Receiver.DeleteStation(_station);
        }
    }
}
