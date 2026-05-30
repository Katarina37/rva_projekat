using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AirQuality.Common.Models
{
    [DataContract]
    public class AirQualityReading : INotifyPropertyChanged
    {
        private double _pm25;
        private double _no2Level;
        private double _ozoneLevel;
        private AirQualityState _state;
        private IAirQualityState _currentState;

        [DataMember]
        public Guid StationId { get; set; }

        [DataMember]
        public DateTime ReadingTime { get; set; }

        [DataMember]
        public double PM25
        {
            get => _pm25;
            set { _pm25 = value; OnPropertyChanged(); }
        }

        [DataMember]
        public double NO2Level
        {
            get => _no2Level;
            set { _no2Level = value; OnPropertyChanged(); }
        }

        [DataMember]
        public double OzoneLevel
        {
            get => _ozoneLevel;
            set { _ozoneLevel = value; OnPropertyChanged(); }
        }

        [DataMember]
        public AirQualityState State
        {
            get => _state;
            set
            {
                _state = value;
                _currentState = AirQualityStateFactory.Create(value);
                OnPropertyChanged();
            }
        }

        public AirQualityReading()
        {
            ReadingTime = DateTime.Now;
            RestoreStateObjectFromEnum();
        }

        public void SetStateObject(IAirQualityState state)
        {
            if (state == null) return;

            _currentState = state;
            _state = state.GetStateValue();
            OnPropertyChanged(nameof(State));
        }

        public void ChangeToNextState()
        {
            if (_currentState == null)
            {
                RestoreStateObjectFromEnum();
            }

            _currentState.Handle(this);
        }

        public void RestoreStateObjectFromEnum()
        {
            _currentState = AirQualityStateFactory.Create(State);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            RestoreStateObjectFromEnum();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
