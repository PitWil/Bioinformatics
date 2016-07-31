using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.State;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Contracts
{
    public interface ICliqueResolver
    {
        PerformingState State { get; }
        double DoneStat { get; }
        DataResult<List<ProteinNode>> Resolve(List<ProteinNode> nodes);
        void Pause();
        void Resume();
        void Stop();
    }
}