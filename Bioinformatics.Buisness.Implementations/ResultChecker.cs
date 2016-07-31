using System;
using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts;
using Bioinformatics.Persistence.Entities;
using Bioinformatics.Persistence.Interfaces;
using ProteinNode = Bioinformatics.Buisness.Models.ProteinNode;

namespace Bioinformatics.Buisness.Implementations
{
    public class ResultChecker : IResultChecker
    {
        private readonly IVerificationResultRepository _verificationResultRepository;

        public ResultChecker(IVerificationResultRepository verificationResultRepository)
        {
            _verificationResultRepository = verificationResultRepository;
        }

        public void Validate(
            List<ProteinNode> positiveNodes,
            List<ProteinNode> counterNodes,
            Func<ProteinNode, bool> verPositiveFunc,
            Func<ProteinNode, bool> verCounterFunc)
        {
            if (positiveNodes != null)
                foreach (var t in positiveNodes)
                {
                    var result = verPositiveFunc(t);
                    _verificationResultRepository.Save(
                        new VerificationResult
                        {
                            Sequence = t.Value + "True",
                            Result = result
                        });
                }

            if (counterNodes == null)
            {
                return;
            }
            foreach (var t in counterNodes)
            {
                var result = verCounterFunc(t);
                _verificationResultRepository.Save(
                    new VerificationResult
                    {
                        Sequence = t.Value + "False",
                        Result = result
                    });
            }
        }
    }
}