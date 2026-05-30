using AirQuality.Common.Models;
using AirQuality.Component2.Models;
using System.Collections.Generic;

namespace AirQuality.Component2.Strategies
{
    public class StatisticsProcessor
    {
        private IStatisticsStrategy _strategy;

        public void SetStrategy(IStatisticsStrategy strategy)
        {
            _strategy = strategy;
        }

        public StatisticsResult Process(Dictionary<string, List<AirQualityReading>> data)
        {
            if (_strategy == null)
            {
                return new StatisticsResult
                {
                    MethodName = string.Empty,
                    Description = "Statisticka metoda nije izabrana.",
                    Value = 0,
                    Unit = string.Empty
                };
            }

            return _strategy.Calculate(data);
        }
    }
}
