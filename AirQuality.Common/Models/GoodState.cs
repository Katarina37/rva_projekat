namespace AirQuality.Common.Models
{
    public class GoodState : IAirQualityState
    {
        public void Handle(AirQualityReading reading)
        {
            reading.SetStateObject(new ModerateState());
        }

        public AirQualityState GetStateValue()
        {
            return AirQualityState.Good;
        }
    }
}
