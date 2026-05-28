using AirQuality.Common.Models;
using AirQuality.Component1.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AirQuality.Component1.Commands
{
    public class DeleteReadingCommand : IUndoableCommand
    {
        private readonly AirQualityReading _reading;
        private readonly List<AirQualityReading> _dataList;
        private readonly ObservableCollection<AirQualityReading> _observableList;

        public DeleteReadingCommand(AirQualityReading reading,
            List<AirQualityReading> dataList,
            ObservableCollection<AirQualityReading> observableList)
        {
            _reading = reading;
            _dataList = dataList;
            _observableList = observableList;
        }

        public void Execute()
        {
            _dataList.Remove(_reading);
            _observableList.Remove(_reading);
        }

        public void Undo()
        {
            _dataList.Add(_reading);
            _observableList.Add(_reading);
        }
    }
}