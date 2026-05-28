using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AirQuality.Component1.Commands
{
    public class DeleteStationCommand : IUndoableCommand
    {
        private readonly MonitoringStation _station;
        private readonly List<MonitoringStation> _dataList;
        private readonly ObservableCollection<MonitoringStation> _observableList;

        public DeleteStationCommand(MonitoringStation station,
            List<MonitoringStation> dataList,
            ObservableCollection<MonitoringStation> observableList)
        {
            _station = station;
            _dataList = dataList;
            _observableList = observableList;
        }

        public void Execute()
        {
            _dataList.Remove(_station);
            _observableList.Remove(_station);
        }

        public void Undo()
        {
            _dataList.Add(_station);
            _observableList.Add(_station);
        }
    }
}