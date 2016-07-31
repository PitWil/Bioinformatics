using System.Runtime.Serialization;

namespace Bioinformatics.Common.Others
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public bool Successed { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}