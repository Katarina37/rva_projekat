namespace AirQuality.Common.Models
{
    public class HazardousState : IAirQualityState
    {
        public void Handle(AirQualityReading reading)
        {
            reading.SetStateObject(new GoodState());
        }

        public AirQualityState GetStateValue()
        {
            return AirQualityState.Hazardous;
        }
    }
}
