using System.Globalization;

namespace AirQuality.Component2.Models
{
    public class StatisticsResult
    {
        public string MethodName { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(MethodName))
            {
                return Description;
            }

            if (string.IsNullOrWhiteSpace(Unit))
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "{0}: {1} = {2:0.##}", MethodName, Description, Value);
            }

            return string.Format(CultureInfo.InvariantCulture,
                "{0}: {1} = {2:0.##} {3}", MethodName, Description, Value, Unit);
        }
    }
}
