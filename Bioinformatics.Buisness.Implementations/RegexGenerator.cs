using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Implementations
{
    public class RegexGenerator : IRegexGenerator
    {
        public string GetRegexFromListOfProteinNode(List<ProteinNode> nodes)
        {
            var currentAvaiablePhrases = new List<List<string>>();

            for (var i = 0; i < nodes[0].Length; i++)
            {
                currentAvaiablePhrases.Add(new List<string>());
                nodes.ForEach(z =>
                {
                    if (!currentAvaiablePhrases[i].Contains(z[i]))
                        currentAvaiablePhrases[i].Add(z[i]);
                });
            }
            var result = "";

            for (var i = 0; i < currentAvaiablePhrases.Count; i++)
            {
                result += "(";
                for (var j = 0; j < currentAvaiablePhrases[i].Count; j++)
                {
                    result += currentAvaiablePhrases[i][j];
                    if (j != currentAvaiablePhrases[i].Count - 1)
                    {
                        result += "|";
                    }
                }
                result += ")";

            }

            return result;
        }
    }
}