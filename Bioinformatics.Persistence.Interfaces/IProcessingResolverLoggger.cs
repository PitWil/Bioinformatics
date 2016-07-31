using System;

namespace Bioinformatics.Persistence.Interfaces
{
    public interface IProcessingResolverLogger : IDisposable
    {
        void ResumeWriteLine(string line);
        void ResumeLogWriteLine(string line);
        void ResumeResultWriteLine(string line);
    }
}