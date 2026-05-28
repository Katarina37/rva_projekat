using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AirQuality.Common.Models
{
    [DataContract]
    public class MonitoringStation : INotifyPropertyChanged
    {
        private string _name;
        private string _city;
        private string _district;
        private double _latitude;
        private double _longitude;

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        [DataMember]
        public string City
        {
            get => _city;
            set { _city = value; OnPropertyChanged(); }
        }

        [DataMember]
        public string District
        {
            get => _district;
            set { _district = value; OnPropertyChanged(); }
        }

        [DataMember]
        public double Latitude
        {
            get => _latitude;
            set { _latitude = value; OnPropertyChanged(); }
        }

        [DataMember]
        public double Longitude
        {
            get => _longitude;
            set { _longitude = value; OnPropertyChanged(); }
        }

        public MonitoringStation()
        {
            Id = Guid.NewGuid();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}