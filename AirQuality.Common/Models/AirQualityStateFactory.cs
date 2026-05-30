namespace AirQuality.Common.Models
{
    public static class AirQualityStateFactory
    {
        public static IAirQualityState Create(AirQualityState state)
        {
            switch (state)
            {
                case AirQualityState.Moderate:
                    return new ModerateState();
                case AirQualityState.Unhealthy:
                    return new UnhealthyState();
                case AirQualityState.Hazardous:
                    return new HazardousState();
                default:
                    return new GoodState();
            }
        }
    }
}
