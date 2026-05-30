using AirQuality.Common.Interfaces;
using AirQuality.Common.Models;
using AirQuality.Component2.Services;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AirQuality.Component2.Adapters
{
    public class WcfReadingDataAdapter : IReadingDataAdapter
    {
        private readonly WcfClientService _clientService;

        public WcfReadingDataAdapter(WcfClientService clientService)
        {
            _clientService = clientService;
        }

        public Dictionary<string, List<AirQualityReading>> GetReadings(Guid stationId, int month, int year)
        {
            IMonitoringStationService client = _clientService.CreateClient();

            try
            {
                List<AirQualityReading> readings =
                    client.GetReadingsByStationAndMonth(stationId, month, year) ?? new List<AirQualityReading>();

                string key = string.Format("{0}-{1:00}-{2}", stationId, month, year);

                var data = new Dictionary<string, List<AirQualityReading>>();
                data[key] = readings;

                CloseClient(client);
                return data;
            }
            catch
            {
                AbortClient(client);
                throw;
            }
        }

        private void CloseClient(IMonitoringStationService client)
        {
            var channel = client as IClientChannel;
            if (channel != null)
            {
                channel.Close();
            }
        }

        private void AbortClient(IMonitoringStationService client)
        {
            var channel = client as IClientChannel;
            if (channel != null)
            {
                channel.Abort();
            }
        }
    }
}
