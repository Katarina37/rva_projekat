using System;
using System.Linq;
using System.Windows.Threading;

namespace AirQuality.Component1.Services
{
    public class StateSimulatorService
    {
        private static StateSimulatorService _instance;
        public static StateSimulatorService Instance => _instance ?? (_instance = new StateSimulatorService());

        private readonly AirQualityReadingService _readingService;
        private readonly DispatcherTimer _timer;

        private StateSimulatorService()
        {
            _readingService = AirQualityReadingService.Instance;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += OnTick;
        }

        public void Start()
        {
            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }
        }

        public void Stop()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            foreach (var reading in _readingService.GetReadings().ToList())
            {
                reading.ChangeToNextState();
            }

            _readingService.Notify();
        }
    }
}
