using System.IO;
using Bioinformatics.Persistence.Interfaces;

namespace Bioinformatics.Persistence.Services
{
    public class ProcesingFileResolverLogger : IProcessingResolverLogger
    {
        private readonly StreamWriter _resume;
        private readonly StreamWriter _resumeLog;
        private readonly StreamWriter _resumeResult;

        public ProcesingFileResolverLogger(string resumeFileName, string resumeLogFileName, string resumeResultFileName)
        {
            _resume = new StreamWriter(resumeFileName);
            _resumeLog = new StreamWriter(resumeLogFileName);
            _resumeResult = new StreamWriter(resumeResultFileName, true);
            _resumeResult.WriteLine("Program;P;R;F1;Acc;AUC;MCC;tp;fp;fn;tn");
        }

        public void ResumeWriteLine(string line)
        {
            _resume.WriteLine(line);
        }

        public void ResumeLogWriteLine(string line)
        {
            _resumeLog.WriteLine(line);
        }

        public void ResumeResultWriteLine(string line)
        {
            _resumeResult.WriteLine(line);
                   }

        public void Dispose()
        {
            _resume.Flush();
            _resume.Close();
            _resumeLog.Flush();
            _resumeLog.Close();
            _resumeResult.Flush();
            _resumeResult.Close();
        }
    }
}