using System.Collections.Generic;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;
using ProteinNode = Bioinformatics.Buisness.Models.ProteinNode;

namespace Bioinformatics.Buisness.Contracts
{
    public interface IResolver
    {
        DataResult<List<ProteinNode>> Resolve(List<Protein> postest, List<Protein> negtest, int AntColoniesCount,
            int AntsColonyCount, float Feromon, int IterationColonyCount, int DywersyficationTime, int AntHillSize);

        void ResolveWithStdAlgorithmAndSaveResults(List<ProteinNode> source, List<Protein> positivhProteins,
            List<Protein> negativeProteins);

        void ResolveWithoutxxxPaternAlgorithmAndSaveResults(List<ProteinNode> source, List<Protein> positivhProteins,
            List<Protein> negativeProteins);

        void ResolveWithoutNegativeRegexPaternAlgorithmAndSaveResults(List<ProteinNode> source,
            List<ProteinNode> negativeSource, List<Protein> positivhProteins, List<Protein> negativeProteins);
    }
}