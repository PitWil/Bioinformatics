using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Bioinformatics.Buisness.Models.Ants;

namespace Bioinformatics.Buisness.Models
{
    [DataContract]
    public class AntsConfigurationModel
    {
        [DataMember]
        public List<ColonyOfAnts> Colonies { get; set; }

        [DataMember]
        public List<ProteinNode> PositiveNodes { get; set; }

        [DataMember]
        public List<ProteinNode> CounterNodes { get; set; }

        public List<ProteinNode> PositiveNodesToVerif { get; set; }

        [DataMember]
        public List<ProteinNode> NegativeNodesToVerif { get; set; }

        public DateTime StartedTime { get; set; }
    }
}