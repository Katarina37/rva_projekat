using AirQuality.Common.Models;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public class EditStationCommand : StationCommand
    {
        private readonly MonitoringStation _station;
        private readonly string _oldName, _oldCity, _oldDistrict;
        private readonly double _oldLatitude, _oldLongitude;
        private readonly string _newName, _newCity, _newDistrict;
        private readonly double _newLatitude, _newLongitude;

        public EditStationCommand(MonitoringStation station,
            MonitoringStationManagementService receiver,
            string newName, string newCity, string newDistrict,
            double newLatitude, double newLongitude)
            : base(receiver)
        {
            _station = station;

            _oldName = station.Name;
            _oldCity = station.City;
            _oldDistrict = station.District;
            _oldLatitude = station.Latitude;
            _oldLongitude = station.Longitude;

            _newName = newName;
            _newCity = newCity;
            _newDistrict = newDistrict;
            _newLatitude = newLatitude;
            _newLongitude = newLongitude;
        }

        public override void Execute()
        {
            Receiver.EditStation(_station, _newName, _newCity, _newDistrict, _newLatitude, _newLongitude);
        }

        public override void Undo()
        {
            Receiver.EditStation(_station, _oldName, _oldCity, _oldDistrict, _oldLatitude, _oldLongitude);
        }
    }
}
