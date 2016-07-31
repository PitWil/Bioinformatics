using System.Collections.Generic;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Contracts
{
    public interface IRegexGenerator
    {
        string GetRegexFromListOfProteinNode(List<ProteinNode> nodes);
    }
}