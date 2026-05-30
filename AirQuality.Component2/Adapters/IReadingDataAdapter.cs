using AirQuality.Common.Models;
using System;
using System.Collections.Generic;

namespace AirQuality.Component2.Adapters
{
    public interface IReadingDataAdapter
    {
        Dictionary<string, List<AirQualityReading>> GetReadings(Guid stationId, int month, int year);
    }
}
