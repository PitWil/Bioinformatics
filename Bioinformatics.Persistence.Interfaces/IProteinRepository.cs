using System;
using System.Collections.Generic;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;

namespace Bioinformatics.Persistence.Interfaces
{
    public interface IProteinRepository
    {
        DataResult<List<Protein>> GetAllProtein();

        DataResult<List<Protein>> GetProteinByParametr(Func<Protein, bool> param);
    }
}