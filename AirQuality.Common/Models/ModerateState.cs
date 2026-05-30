namespace AirQuality.Common.Models
{
    public class ModerateState : IAirQualityState
    {
        public void Handle(AirQualityReading reading)
        {
            reading.SetStateObject(new UnhealthyState());
        }

        public AirQualityState GetStateValue()
        {
            return AirQualityState.Moderate;
        }
    }
}
