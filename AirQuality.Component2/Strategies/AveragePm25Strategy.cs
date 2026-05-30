using AirQuality.Common.Models;
using AirQuality.Component2.Models;
using System.Collections.Generic;
using System.Linq;

namespace AirQuality.Component2.Strategies
{
    public class AveragePm25Strategy : IStatisticsStrategy
    {
        public string Name => "Average PM2.5";

        public StatisticsResult Calculate(Dictionary<string, List<AirQualityReading>> data)
        {
            var readings = data.SelectMany(item => item.Value).ToList();
            double value = readings.Count == 0 ? 0 : readings.Average(reading => reading.PM25);

            return new StatisticsResult
            {
                MethodName = Name,
                Description = "Prosecna koncentracija PM2.5",
                Value = value,
                Unit = "μg/m³"
            };
        }
    }
}
