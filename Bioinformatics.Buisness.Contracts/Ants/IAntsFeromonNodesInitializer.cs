using System.Collections.Generic;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Contracts.Ants
{
    public interface IAntsFeromonNodesInitializer
    {
        void Initialize(List<ProteinNode> nodes);
    }
}