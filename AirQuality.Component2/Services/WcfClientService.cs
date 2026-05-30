using AirQuality.Common.Interfaces;
using System.ServiceModel;

namespace AirQuality.Component2.Services
{
    public class WcfClientService
    {
        private const string EndpointName = "MonitoringStationServiceEndpoint";

        public IMonitoringStationService CreateClient()
        {
            var factory = new ChannelFactory<IMonitoringStationService>(EndpointName);
            return factory.CreateChannel();
        }
    }
}
