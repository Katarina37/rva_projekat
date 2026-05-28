using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;

namespace AirQuality.Component1.Commands
{
    public class EditStationCommand : IUndoableCommand
    {
        private readonly MonitoringStation _station;
        private readonly string _oldName, _oldCity, _oldDistrict;
        private readonly double _oldLatitude, _oldLongitude;
        private readonly string _newName, _newCity, _newDistrict;
        private readonly double _newLatitude, _newLongitude;

        public EditStationCommand(MonitoringStation station,
            string newName, string newCity, string newDistrict,
            double newLatitude, double newLongitude)
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

        public void Execute()
        {
            _station.Name = _newName;
            _station.City = _newCity;
            _station.District = _newDistrict;
            _station.Latitude = _newLatitude;
            _station.Longitude = _newLongitude;
        }

        public void Undo()
        {
            _station.Name = _oldName;
            _station.City = _oldCity;
            _station.District = _oldDistrict;
            _station.Latitude = _oldLatitude;
            _station.Longitude = _oldLongitude;
        }
    }
}