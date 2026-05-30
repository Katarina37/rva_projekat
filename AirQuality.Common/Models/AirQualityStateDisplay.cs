namespace AirQuality.Common.Models
{
    public static class AirQualityStateDisplay
    {
        public static string ToSerbianText(AirQualityState state)
        {
            switch (state)
            {
                case AirQualityState.Moderate:
                    return "Umereno";
                case AirQualityState.Unhealthy:
                    return "Nezdravo";
                case AirQualityState.Hazardous:
                    return "Opasno";
                default:
                    return "Dobro";
            }
        }
    }
}
