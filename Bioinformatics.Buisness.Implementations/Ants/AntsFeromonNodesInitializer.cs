using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Implementations.Ants
{
    public class AntsFeromonNodesInitializer : IAntsFeromonNodesInitializer
    {
        private readonly double _initialFeromon;

        public AntsFeromonNodesInitializer(double initialFeromon)
        {
            _initialFeromon = initialFeromon;
        }

        public void Initialize(List<ProteinNode> nodes)
        {
            // nodes.ForEach(z => z.NeighborsAll.ToList().ForEach(g=>g.ForEach(y => y.InitWeight(_initialFeromon))));
        }
    }
}