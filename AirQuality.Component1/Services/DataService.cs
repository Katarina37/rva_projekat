using AirQuality.Common.Models;
using System;
using System.Collections.Generic;

namespace AirQuality.Component1.Services
{
    public class DataService
    {
        private static DataService _instance;
        public static DataService Instance => _instance ?? (_instance = new DataService());

        private readonly PersistenceService _persistenceService;
        private const string DataFilePath = "data.json";

        public List<MonitoringStation> Stations { get; private set; }
        public List<AirQualityReading> Readings { get; private set; }

        private DataService()
        {
            _persistenceService = new PersistenceService();
            Stations = new List<MonitoringStation>();
            Readings = new List<AirQualityReading>();
            LoadFromJson();
        }

        public void SaveToJson()
        {
            _persistenceService.SaveToJson(DataFilePath, Stations, Readings);
        }

        public void LoadFromJson()
        {
            var (stations, readings) = _persistenceService.LoadFromJson(DataFilePath);

            if (stations != null && stations.Count > 0)
            {
                Stations = stations;
                Readings = readings ?? new List<AirQualityReading>();
                RestoreReadingStateObjects();
            }
            else
            {
                SeedData();
            }
        }

        private void SeedData()
        {
            var station1 = new MonitoringStation
            {
                Name = "Centar",
                City = "Sarajevo",
                District = "Stari Grad",
                Latitude = 43.8563,
                Longitude = 18.4131
            };
            var station2 = new MonitoringStation
            {
                Name = "Ilidža",
                City = "Sarajevo",
                District = "Ilidža",
                Latitude = 43.8300,
                Longitude = 18.3100
            };
            var station3 = new MonitoringStation
            {
                Name = "Banja Luka Centar",
                City = "Banja Luka",
                District = "Centar",
                Latitude = 44.7722,
                Longitude = 17.1910
            };

            Stations.AddRange(new[] { station1, station2, station3 });

            Readings.AddRange(new[]
            {
                new AirQualityReading { StationId = station1.Id, PM25 = 18.3, NO2Level = 42.1, OzoneLevel = 30.0, State = AirQualityState.Good },
                new AirQualityReading { StationId = station1.Id, PM25 = 55.7, NO2Level = 88.4, OzoneLevel = 60.0, State = AirQualityState.Unhealthy },
                new AirQualityReading { StationId = station2.Id, PM25 = 12.0, NO2Level = 31.5, OzoneLevel = 25.0, State = AirQualityState.Good },
                new AirQualityReading { StationId = station3.Id, PM25 = 80.0, NO2Level = 120.0, OzoneLevel = 90.0, State = AirQualityState.Hazardous }
            });

            RestoreReadingStateObjects();
        }

        private void RestoreReadingStateObjects()
        {
            foreach (var reading in Readings)
            {
                reading.RestoreStateObjectFromEnum();
            }
        }
    }
}
