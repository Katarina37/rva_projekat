using AirQuality.Common.Models;
using AirQuality.Component1.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace AirQuality.Component1.ViewModels
{
    public class ChartViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly StateSimulatorService _simulatorService;

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

        public ChartViewModel()
        {
            _dataService = DataService.Instance;

            XAxes = new List<Axis>
            {
                new Axis
                {
                    Labels = new List<string> { "Good", "Moderate", "Unhealthy", "Hazardous" },
                    TextSize = 14
                }
            };

            UpdateChart();

            _simulatorService = new StateSimulatorService(UpdateChart);
            _simulatorService.Start();
        }

        private void UpdateChart()
        {
            var readings = _dataService.Readings;

            var counts = new double[]
            {
                readings.Count(r => r.State == AirQualityState.Good),
                readings.Count(r => r.State == AirQualityState.Moderate),
                readings.Count(r => r.State == AirQualityState.Unhealthy),
                readings.Count(r => r.State == AirQualityState.Hazardous)
            };

            Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = "Broj mjerenja",
                    Values = counts,
                    Fill = new SolidColorPaint(SKColor.Parse("#27AE60")),
                    Stroke = null,
                    MaxBarWidth = 60
                }
            };
        }
    }
}