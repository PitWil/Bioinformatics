using System.Collections.Generic;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;
using ProteinNode = Bioinformatics.Buisness.Models.ProteinNode;

namespace Bioinformatics.Buisness.Contracts.Graph
{
    public interface IProteinNodeGenerator
    {
        DataResult<List<ProteinNode>> GenerateFromProteinSequence(string proteinSequence, int count);
        DataResult<List<ProteinNode>> GenerateFromProteins(List<Protein> proteins);
        DataResult<List<ProteinNode>> GenerateProteinNodeCombination(List<ProteinNode> node);
        DataResult<List<ProteinNode>> GenerateProteinNodeCombination(ProteinNode node1, ProteinNode node2);
    }
}