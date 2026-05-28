using AirQuality.Common.Models;
using AirQuality.Component1.Services;
using System;
using System.Threading;
using System.Windows.Threading;

namespace AirQuality.Component1.Services
{
    public class StateSimulatorService
    {
        private readonly DataService _dataService;
        private readonly DispatcherTimer _timer;
        private readonly Action _onStateChanged;

        public StateSimulatorService(Action onStateChanged)
        {
            _dataService = DataService.Instance;
            _onStateChanged = onStateChanged;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += OnTick;
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private void OnTick(object sender, EventArgs e)
        {
            foreach (var reading in _dataService.Readings)
            {
                reading.State = GetNextState(reading.State);
            }
            _onStateChanged?.Invoke();
        }

        private AirQualityState GetNextState(AirQualityState current)
        {
            var states = (AirQualityState[])Enum.GetValues(typeof(AirQualityState));
            int nextIndex = ((int)current + 1) % states.Length;
            return states[nextIndex];
        }
    }
}