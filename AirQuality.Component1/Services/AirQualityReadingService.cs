using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AirQuality.Component1.Services
{
    public class AirQualityReadingService : IReadingSubject
    {
        private static AirQualityReadingService _instance;
        public static AirQualityReadingService Instance => _instance ?? (_instance = new AirQualityReadingService());

        private readonly DataService _dataService;
        private readonly List<IReadingObserver> observers;

        private AirQualityReadingService()
        {
            _dataService = DataService.Instance;
            observers = new List<IReadingObserver>();
        }

        public void AddReading(AirQualityReading reading)
        {
            if (!_dataService.Readings.Contains(reading))
            {
                reading.RestoreStateObjectFromEnum();
                _dataService.Readings.Add(reading);
            }

            Notify();
        }

        public void DeleteReading(AirQualityReading reading)
        {
            _dataService.Readings.Remove(reading);
            Notify();
        }

        public void EditReading(AirQualityReading reading, double pm25, double no2, double ozone, AirQualityState state)
        {
            reading.PM25 = pm25;
            reading.NO2Level = no2;
            reading.OzoneLevel = ozone;
            SetReadingStateWithoutNotify(reading, state);
            Notify();
        }

        public void ChangeReadingState(AirQualityReading reading)
        {
            reading.ChangeToNextState();
            Notify();
        }

        public void SetReadingState(AirQualityReading reading, AirQualityState state)
        {
            SetReadingStateWithoutNotify(reading, state);
            Notify();
        }

        public List<AirQualityReading> GetReadings()
        {
            return _dataService.Readings;
        }

        public void Attach(IReadingObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void Detach(IReadingObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in observers.ToList())
            {
                observer.Update();
            }
        }

        private void SetReadingStateWithoutNotify(AirQualityReading reading, AirQualityState state)
        {
            reading.SetStateObject(AirQualityStateFactory.Create(state));
        }
    }
}
