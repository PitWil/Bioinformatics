using System;
using System.IO;
using System.Windows.Forms;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;
using Bioinformatics.Persistence.Interfaces;

namespace Bioinformatics.Persistence.Services
{
    public class VerificationResultFileRepository : IVerificationResultRepository
    {
        private readonly string _path;

        public VerificationResultFileRepository(string path)
        {
            _path = path;
        }

        public Result Save(VerificationResult vResult)
        {
            var result = new Result();
            try
            {
                using (var sw = new StreamWriter(_path, true))
                {
                    sw.WriteLine(vResult.Sequence + " " + vResult.Result);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                MessageBox.Show(ex.Message);
                return result;
            }
            return result;
        }
    }
}