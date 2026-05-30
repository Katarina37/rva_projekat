using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirQuality.Component1.ViewModels
{
    public class ChartViewModel : BaseViewModel, IReadingObserver, IDisposable
    {
        private readonly AirQualityReadingService _readingService;
        private bool _disposed;

        private ISeries[] _series;
        public ISeries[] Series
        {
            get => _series;
            set { _series = value; OnPropertyChanged(); }
        }

        private List<Axis> _xAxes;
        public List<Axis> XAxes
        {
            get => _xAxes;
            set { _xAxes = value; OnPropertyChanged(); }
        }

        private List<Axis> _yAxes;
        public List<Axis> YAxes
        {
            get => _yAxes;
            set { _yAxes = value; OnPropertyChanged(); }
        }

        public ChartViewModel()
        {
            _readingService = AirQualityReadingService.Instance;

            XAxes = new List<Axis>
            {
                new Axis
                {
                    Labels = new List<string> { "Good", "Moderate", "Unhealthy", "Hazardous" },
                    TextSize = 14
                }
            };

            YAxes = new List<Axis>
            {
                new Axis
                {
                    MinLimit = 0,
                    MinStep = 1,
                    TextSize = 14
                }
            };

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

            Series = new ISeries[]
            {
                CreateStateSeries("Good", counts[0], 0, "#27AE60"),
                CreateStateSeries("Moderate", counts[1], 1, "#F39C12"),
                CreateStateSeries("Unhealthy", counts[2], 2, "#E74C3C"),
                CreateStateSeries("Hazardous", counts[3], 3, "#8E44AD")
            };
        }

        private static ISeries CreateStateSeries(string name, double count, int stateIndex, string color)
        {
            return new StackedColumnSeries<double>
            {
                Name = name,
                Values = CreateValues(count, stateIndex),
                Fill = new SolidColorPaint(SKColor.Parse(color)),
                Stroke = null,
                MaxBarWidth = 60
            };
        }

        private static double[] CreateValues(double count, int stateIndex)
        {
            var values = new double[4];
            values[stateIndex] = count;
            return values;
        }
    }
}
