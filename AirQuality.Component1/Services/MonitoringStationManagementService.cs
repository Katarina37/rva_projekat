using AirQuality.Common.Models;
using System.Collections.Generic;

namespace AirQuality.Component1.Services
{
    public class MonitoringStationManagementService
    {
        private static MonitoringStationManagementService _instance;
        public static MonitoringStationManagementService Instance => _instance ?? (_instance = new MonitoringStationManagementService());

        private readonly DataService _dataService;

        private MonitoringStationManagementService()
        {
            _dataService = DataService.Instance;
        }

        public void AddStation(MonitoringStation station)
        {
            if (!_dataService.Stations.Contains(station))
            {
                _dataService.Stations.Add(station);
            }
        }

        public void DeleteStation(MonitoringStation station)
        {
            _dataService.Stations.Remove(station);
        }

        public void EditStation(MonitoringStation station, string name, string city, string district, double latitude, double longitude)
        {
            station.Name = name;
            station.City = city;
            station.District = district;
            station.Latitude = latitude;
            station.Longitude = longitude;
        }

        public List<MonitoringStation> GetStations()
        {
            return _dataService.Stations;
        }
    }
}
