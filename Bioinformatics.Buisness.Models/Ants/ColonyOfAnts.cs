using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bioinformatics.Buisness.Models.Ants
{
    [DataContract]
    public class ColonyOfAnts
    {
        private int _antsHillSize;
        private int _diversificationTime;

        private int _interationCount;

        public ColonyOfAnts()
        {
            AntsHillSize = 1;
            DiversificationTime = 1;
            InterationCount = 1;
        }

        [DataMember]
        public List<Ant> Ants { get; set; }

        public ProteinNode AntsHill { get; set; }

        [DataMember]
        public int InterationCount
        {
            get { return _interationCount; }
            set
            {
                if (value < 1)
                    _interationCount = 1;
                _interationCount = value;
            }
        }

        [DataMember]
        public int AntsHillSize
        {
            get { return _antsHillSize; }
            set
            {
                if (value < 1)
                    _antsHillSize = 1;
                _antsHillSize = value;
            }
        }

        [DataMember]
        public int DiversificationTime
        {
            get { return _diversificationTime; }
            set
            {
                if (value < 1)
                    _diversificationTime = 1;
                _diversificationTime = value;
            }
        }

        public void SetFeromon(double f)
        {
            var localAnts = Ants;
            if (localAnts == null)
            {
                return;
            }
            foreach (var ant in localAnts)
            {
                ant.Feromon = f;
            }
        }
    }
}