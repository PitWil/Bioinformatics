using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.State;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Contracts.Graph
{
    public interface IProteinGraphGenerator
    {
        PerformingState State { get; }
        double DoneStat { get; }
        DataResult<List<ProteinNode>> CreateGraph(List<ProteinNode> nodes, List<ProteinNode> counterexamples);
        DataResult<List<ProteinNode>> GetAllNodesFromGraph(ProteinNode rootNode);
        void Pause();
        void Resume();
        void Stop();
    }
}