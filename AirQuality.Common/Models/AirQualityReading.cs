using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AirQuality.Common.Models
{
    [DataContract]
    public class AirQualityReading
    {
        [DataMember]
        public Guid StationId { get; set; }

        [DataMember]
        public DateTime ReadingTime { get; set; }

        [DataMember]
        public double PM25 { get; set; }

        [DataMember]
        public double NO2Level { get; set; }

        [DataMember]
        public double OzoneLevel { get; set; }

        [DataMember]
        public AirQualityState State { get; set; }

        public AirQualityReading()
        {
            ReadingTime = DateTime.Now;
        }
    }
}
