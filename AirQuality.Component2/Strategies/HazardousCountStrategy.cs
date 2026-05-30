using AirQuality.Common.Models;
using AirQuality.Component2.Models;
using System.Collections.Generic;
using System.Linq;

namespace AirQuality.Component2.Strategies
{
    public class HazardousCountStrategy : IStatisticsStrategy
    {
        public string Name => "Hazardous Count";

        public StatisticsResult Calculate(Dictionary<string, List<AirQualityReading>> data)
        {
            var readings = data.SelectMany(item => item.Value).ToList();
            int value = readings.Count(reading => reading.State == AirQualityState.Hazardous);

            return new StatisticsResult
            {
                MethodName = Name,
                Description = "Broj merenja sa stanjem Hazardous",
                Value = value,
                Unit = "merenja"
            };
        }
    }
}
