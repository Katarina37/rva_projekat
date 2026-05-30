using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AirQuality.Component1.ViewModels
{
    public class ChartViewModel : BaseViewModel, IReadingObserver, IDisposable
    {
        private static readonly string[] StateLabels =
        {
            AirQualityStateDisplay.ToSerbianText(AirQualityState.Good),
            AirQualityStateDisplay.ToSerbianText(AirQualityState.Moderate),
            AirQualityStateDisplay.ToSerbianText(AirQualityState.Unhealthy),
            AirQualityStateDisplay.ToSerbianText(AirQualityState.Hazardous)
        };
        private static readonly string[] StateColors = { "#39D353", "#F4D03F", "#FF8C32", "#E53935" };
        private static readonly string[] StateGradientStartColors = { "#1E9F3C", "#D4A900", "#D96611", "#B71C1C" };
        private static readonly string[] StateGradientEndColors = { "#7CFF91", "#FFF176", "#FFB36C", "#FF7A73" };
        private const double MaxVisualBarWidth = 500;

        private readonly AirQualityReadingService _readingService;
        private bool _disposed;

        public ObservableCollection<StateBarItem> StateBars { get; }

        private int _totalReadings;
        public int TotalReadings
        {
            get => _totalReadings;
            set { _totalReadings = value; OnPropertyChanged(); }
        }

        public ChartViewModel()
        {
            _readingService = AirQualityReadingService.Instance;
            StateBars = CreateStateBars();

            _readingService.Attach(this);
            Update();
            StateSimulatorService.Instance.Start();
        }

        public void Update()
        {
            UpdateChart();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _readingService.Detach(this);
            StateSimulatorService.Instance.Stop();
            _disposed = true;
        }

        private void UpdateChart()
        {
            var readings = _readingService.GetReadings();

            var counts = new double[]
            {
                readings.Count(r => r.State == AirQualityState.Good),
                readings.Count(r => r.State == AirQualityState.Moderate),
                readings.Count(r => r.State == AirQualityState.Unhealthy),
                readings.Count(r => r.State == AirQualityState.Hazardous)
            };

            TotalReadings = readings.Count;
            UpdateStateBars(counts);
        }

        private static ObservableCollection<StateBarItem> CreateStateBars()
        {
            return new ObservableCollection<StateBarItem>
            {
                new StateBarItem(StateLabels[0], StateColors[0], StateGradientStartColors[0], StateGradientEndColors[0]),
                new StateBarItem(StateLabels[1], StateColors[1], StateGradientStartColors[1], StateGradientEndColors[1]),
                new StateBarItem(StateLabels[2], StateColors[2], StateGradientStartColors[2], StateGradientEndColors[2]),
                new StateBarItem(StateLabels[3], StateColors[3], StateGradientStartColors[3], StateGradientEndColors[3])
            };
        }

        private void UpdateStateBars(double[] counts)
        {
            var maxCount = Math.Max(1, counts.Max());

            for (var i = 0; i < StateBars.Count; i++)
            {
                StateBars[i].Count = (int)counts[i];
                StateBars[i].BarWidth = MaxVisualBarWidth * counts[i] / maxCount;
            }
        }
    }

    public class StateBarItem : BaseViewModel
    {
        private int _count;
        private double _barWidth;

        public StateBarItem(string label, string color, string gradientStartColor, string gradientEndColor)
        {
            Label = label;
            Color = color;
            GradientStartColor = gradientStartColor;
            GradientEndColor = gradientEndColor;
        }

        public string Label { get; }

        public string Color { get; }

        public string GradientStartColor { get; }

        public string GradientEndColor { get; }

        public int Count
        {
            get => _count;
            set
            {
                if (_count == value) return;

                _count = value;
                OnPropertyChanged();
            }
        }

        public double BarWidth
        {
            get => _barWidth;
            set
            {
                if (_barWidth == value) return;

                _barWidth = value;
                OnPropertyChanged();
            }
        }
    }
}
