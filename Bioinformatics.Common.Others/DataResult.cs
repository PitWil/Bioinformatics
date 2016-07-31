using System.Runtime.Serialization;

namespace Bioinformatics.Common.Others
{
    [DataContract]
    public class DataResult<T> : Result
    {
        [DataMember]
        public T Data { get; set; }
    }
}