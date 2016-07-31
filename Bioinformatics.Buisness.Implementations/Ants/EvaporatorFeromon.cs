using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Implementations.Ants
{
    public class EvaporatorFeromon : IEvaporatorFeromon
    {
        private double _a;

        public EvaporatorFeromon(double a)
        {
            _a = a;
        }

        public void Evaporate(List<ProteinNode> nodes)
        {
            ProteinNode.ProteinWeightNode.AddToCounter(1);
            // nodes.ForEach(z => z.Neighbors.ForEach(y => y *= _a));
        }
    }
}