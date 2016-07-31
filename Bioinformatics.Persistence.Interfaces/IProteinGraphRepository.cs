using System.Collections.Generic;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;

namespace Bioinformatics.Persistence.Interfaces
{
    public interface IProteinGraphRepository
    {
        Result SaveProteinGraph(Graph g);
        DataResult<Graph> GetProteinGraphById(string id);
        DataResult<List<Graph>> GetAllProteinGraph();
    }
}