using System.Collections.Generic;

namespace Bioinformatics.Persistence.Entities
{
    public class ProteinNode
    {
        public string[] Value { get; set; }
        public virtual List<ProteinWeigthNode> Neighbors { get; set; }

        public class ProteinWeigthNode
        {
            public double Weigth { get; set; }

            public ProteinNode Neighbors { get; set; }
        }
    }
}