using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;
using System.Collections.ObjectModel;

namespace AirQuality.Component1.Commands
{
    public class DeleteStationCommand : IUndoableCommand
    {
        private readonly DataService dataService;
        private readonly ObservableCollection<MonitoringStation> stations;
        private readonly MonitoringStation station;

        public DeleteStationCommand(DataService dataService,
            ObservableCollection<MonitoringStation> stations,
            MonitoringStation station)
        {
            this.dataService = dataService;
            this.stations = stations;
            this.station = station;
        }

        public void Execute()
        {
            dataService.Stations.Remove(station);
            stations.Remove(station);
        }

        public void Undo()
        {
            dataService.Stations.Add(station);
            stations.Add(station);
        }
    }
}