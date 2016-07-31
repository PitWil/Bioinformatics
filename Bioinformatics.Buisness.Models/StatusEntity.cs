using System.Runtime.Serialization;

namespace Bioinformatics.Buisness.Models
{
    [DataContract]
    public class StatusEntity
    {
        [DataMember]
        public double Percentage { get; set; }

        [DataMember]
        public double TimeToFinish { get; set; }
    }
}