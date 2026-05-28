using AirQuality.Common.Models;
using AirQuality.Component1.Services;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AirQuality.Component1.Services
{
    public class PersistenceService
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void SaveToJson(string path, List<MonitoringStation> stations, List<AirQualityReading> readings)
        {
            var data = new AppData
            {
                Stations = stations,
                Readings = readings
            };

            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(path, json);
        }

        public (List<MonitoringStation> Stations, List<AirQualityReading> Readings) LoadFromJson(string path)
        {
            if (!File.Exists(path))
                return (null, null);

            string json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<AppData>(json, _options);

            return (data?.Stations, data?.Readings);
        }
    }

    public class AppData
    {
        public List<MonitoringStation> Stations { get; set; }
        public List<AirQualityReading> Readings { get; set; }
    }
}