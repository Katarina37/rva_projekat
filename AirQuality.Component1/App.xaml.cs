using AirQuality.Component1.Services;
using System;
using System.ServiceModel;
using System.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace AirQuality.Component1
{
    public partial class App : Application
    {
        private ServiceHost _serviceHost;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LiveCharts.Configure(config => config
             .AddSkiaSharp()
             .AddDefaultMappers()
             .AddLightTheme());
            StartWcfHost();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            StopWcfHost();
            base.OnExit(e);
        }

        private void StartWcfHost()
        {
            try
            {
                _serviceHost = new ServiceHost(typeof(MonitoringStationService));
                _serviceHost.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri pokretanju WCF servisa: {ex.Message}",
                    "WCF greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopWcfHost()
        {
            if (_serviceHost != null && _serviceHost.State == CommunicationState.Opened)
            {
                _serviceHost.Close();
            }
        }
    }
}