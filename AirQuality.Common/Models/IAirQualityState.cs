namespace AirQuality.Common.Models
{
    public interface IAirQualityState
    {
        void Handle(AirQualityReading reading);
        AirQualityState GetStateValue();
    }
}
