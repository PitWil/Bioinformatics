using System.Collections.Generic;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Contracts.Ants
{
    public interface IEvaporatorFeromon
    {
        void Evaporate(List<ProteinNode> nodes);
    }
}