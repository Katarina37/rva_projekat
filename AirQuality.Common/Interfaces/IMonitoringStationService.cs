using AirQuality.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace AirQuality.Common.Interfaces
{
    [ServiceContract]
    public interface IMonitoringStationService
    {
        [OperationContract]
        List<MonitoringStation> GetAllStations();

        [OperationContract]
        List<AirQualityReading> GetReadingsByStationAndMonth(Guid stationId, int month, int year);
    }
}
