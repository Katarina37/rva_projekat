using AirQuality.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQuality.Component1.Services
{
    public class DataService
    {
        private static DataService instance;
        public static DataService Instance => instance ?? (instance = new DataService());

        public List<MonitoringStation> Stations { get; private set; }
        public List<AirQualityReading> Readings { get; private set; }

        private DataService()
        {
            Stations = new List<MonitoringStation>();
            Readings = new List<AirQualityReading>();
            SeedData();
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
                new AirQualityReading {StationId = station1.Id, PM25 = 18.3, NO2Level = 42.1, OzoneLevel = 30.0, State = AirQualityState.Good},
                new AirQualityReading {StationId = station1.Id, PM25 = 55.7, NO2Level = 88.4, OzoneLevel = 60.0, State = AirQualityState.Unhealthy},
                new AirQualityReading { StationId = station2.Id, PM25 = 12.0, NO2Level = 31.5, OzoneLevel = 25.0, State = AirQualityState.Good },
                new AirQualityReading { StationId = station3.Id, PM25 = 80.0, NO2Level = 120.0, OzoneLevel = 90.0, State = AirQualityState.Hazardous }
            });
        }
    }
}
