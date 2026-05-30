using AirQuality.Common.Models;
using AirQuality.Component2.Models;
using System.Collections.Generic;

namespace AirQuality.Component2.Strategies
{
    public interface IStatisticsStrategy
    {
        string Name { get; }
        StatisticsResult Calculate(Dictionary<string, List<AirQualityReading>> data);
    }
}
