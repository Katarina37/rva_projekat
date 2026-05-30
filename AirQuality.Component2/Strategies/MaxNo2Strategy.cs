using AirQuality.Common.Models;
using AirQuality.Component2.Models;
using System.Collections.Generic;
using System.Linq;

namespace AirQuality.Component2.Strategies
{
    public class MaxNo2Strategy : IStatisticsStrategy
    {
        public string Name => "Maksimalna vrednost NO2";

        public StatisticsResult Calculate(Dictionary<string, List<AirQualityReading>> data)
        {
            var readings = data.SelectMany(item => item.Value).ToList();
            double value = readings.Count == 0 ? 0 : readings.Max(reading => reading.NO2Level);

            return new StatisticsResult
            {
                MethodName = Name,
                Description = "Maksimalna vrednost NO2",
                Value = value,
                Unit = "μg/m³"
            };
        }
    }
}
