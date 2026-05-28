using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AirQuality.Common.Models
{
    [DataContract]
    public class AirQualityReading : INotifyPropertyChanged
    {
        private AirQualityState _state;

        [DataMember] public Guid StationId { get; set; }
        [DataMember] public DateTime ReadingTime { get; set; }
        [DataMember] public double PM25 { get; set; }
        [DataMember] public double NO2Level { get; set; }
        [DataMember] public double OzoneLevel { get; set; }

        [DataMember]
        public AirQualityState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public AirQualityReading()
        {
            ReadingTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}