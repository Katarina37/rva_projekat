using AirQuality.Common.Interfaces;
using AirQuality.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirQuality.Component1.Services
{
    public class MonitoringStationService : IMonitoringStationService
    {
        public List<MonitoringStation> GetAllStations()
        {
            return DataService.Instance.Stations;
        }

        public List<AirQualityReading> GetReadingsByStationAndMonth(Guid stationId, int month, int year)
        {
            return DataService.Instance.Readings
                .Where(r => r.StationId == stationId
                         && r.ReadingTime.Month == month
                         && r.ReadingTime.Year == year)
                .ToList();
        }
    }
}