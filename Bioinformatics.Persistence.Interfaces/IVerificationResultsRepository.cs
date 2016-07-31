using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;

namespace Bioinformatics.Persistence.Interfaces
{
    public interface IVerificationResultRepository
    {
        Result Save(VerificationResult vResult);
    }
}