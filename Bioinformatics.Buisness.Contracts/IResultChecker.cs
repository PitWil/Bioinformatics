using System;
using System.Collections.Generic;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Contracts
{
    public interface IResultChecker
    {
        void Validate(List<ProteinNode> positiveNodes,
            List<ProteinNode> counterNodes,
            Func<ProteinNode, bool> verPositiveFunc,
            Func<ProteinNode, bool> verCounterFunc);
    }
}