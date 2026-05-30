namespace AirQuality.Common.Models
{
    public class UnhealthyState : IAirQualityState
    {
        public void Handle(AirQualityReading reading)
        {
            reading.SetStateObject(new HazardousState());
        }

        public AirQualityState GetStateValue()
        {
            return AirQualityState.Unhealthy;
        }
    }
}
