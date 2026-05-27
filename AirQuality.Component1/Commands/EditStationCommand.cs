using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;
using System.Collections.ObjectModel;

namespace AirQuality.Component1.Commands
{
    public class EditStationCommand : IUndoableCommand
    {
        private readonly ObservableCollection<MonitoringStation> stations;
        private readonly MonitoringStation station;
        private readonly string oldName, oldCity, oldDistrict;
        private readonly double oldLatitude, oldLongitude;
        private readonly string newName, newCity, newDistrict;
        private readonly double newLatitude, newLongitude;

        public EditStationCommand(ObservableCollection<MonitoringStation> stations,
            MonitoringStation station, MonitoringStation newValues)
        {
            this.stations = stations;
            this.station = station;

            oldName = station.Name;
            oldCity = station.City;
            oldDistrict = station.District;
            oldLatitude = station.Latitude;
            oldLongitude = station.Longitude;

            newName = newValues.Name;
            newCity = newValues.City;
            newDistrict = newValues.District;
            newLatitude = newValues.Latitude;
            newLongitude = newValues.Longitude;
        }

        public void Execute() => ApplyValues(newName, newCity, newDistrict, newLatitude, newLongitude);

        public void Undo() => ApplyValues(oldName, oldCity, oldDistrict, oldLatitude, oldLongitude);

        private void ApplyValues(string name, string city, string district, double lat, double lon)
        {
            station.Name = name;
            station.City = city;
            station.District = district;
            station.Latitude = lat;
            station.Longitude = lon;

            var index = stations.IndexOf(station);
            stations.RemoveAt(index);
            stations.Insert(index, station);
        }
    }
}