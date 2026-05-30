namespace AirQuality.Component1.Interfaces
{
    public interface IReadingSubject
    {
        void Attach(IReadingObserver observer);
        void Detach(IReadingObserver observer);
        void Notify();
    }
}
