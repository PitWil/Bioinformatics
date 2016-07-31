using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Graph;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;
using ProteinNode = Bioinformatics.Buisness.Models.ProteinNode;

namespace Bioinformatics.Buisness.Implementations.Graph
{
    public class ProteinNodeGenerator : IProteinNodeGenerator
    {
        public DataResult<List<ProteinNode>> GenerateFromProteinSequence(string proteinSequence, int count)
        {
            var result = new DataResult<List<ProteinNode>>();
            if (count < 1)
            {
                result.ErrorMessage = "Invalid argument!";

                return result;
            }
            if (proteinSequence == null || proteinSequence.Equals(""))
            {
                result.ErrorMessage = "Incorrect protein!";
                return result;
            }
            if (count > proteinSequence.Length)
            {
                result.ErrorMessage = "Incorrect split count!";
                return result;
            }

            result.Data = new List<ProteinNode>();
            var firstCommaPointer = 0;
            var distanceBetweenComma = 1;
            for (var j = 0; j < proteinSequence.Length - 1; j++)
            {
                for (var ii = 0; ii < proteinSequence.Length - (2 + j); ii++)
                {
                    var currentResult = "";
                    for (var i = 0; i < proteinSequence.Length; i++)
                    {
                        currentResult += proteinSequence[i];
                        if ((firstCommaPointer == i || firstCommaPointer + distanceBetweenComma == i) &&
                            i < proteinSequence.Length - 1)
                        {
                            currentResult += ",";
                        }
                    }
                    result.Data.Add(new ProteinNode(currentResult));
                    firstCommaPointer++;
                }
                firstCommaPointer = 0;
                distanceBetweenComma++;
            }


            result.Successed = true;
            return result;
        }

        public DataResult<List<ProteinNode>> GenerateFromProteins(List<Protein> proteins)
        {
            var result = new DataResult<List<ProteinNode>>();
            result.Data = new List<ProteinNode>();
            foreach (var protein in proteins)
            {
                result.Data.Add(new ProteinNode(protein.Sequence));
            }
            result.Successed = true;
            return result;
        }

        public DataResult<List<ProteinNode>> GenerateProteinNodeCombination(List<ProteinNode> node)
        {
            var result = new DataResult<List<ProteinNode>>();
            if (node == null)
            {
                result.ErrorMessage = "Null arg!";
                return result;
            }
            result.Data = new List<ProteinNode>();
            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3

            for (var i = 0; i < node.Count; i++)
            {
                var a = node[i];
                for (var j = i + 1; j < node.Count; j++)
                {
                    var b = node[j];
                    result.Data.Add(new ProteinNode(a[0] + "," + b[1] + "," + a[2]));
                    result.Data.Add(new ProteinNode(a[0] + "," + b[1] + "," + b[2]));
                    result.Data.Add(new ProteinNode(a[0] + "," + a[1] + "," + b[2]));
                    result.Data.Add(new ProteinNode(b[0] + "," + a[1] + "," + a[2]));
                    result.Data.Add(new ProteinNode(b[0] + "," + a[1] + "," + b[2]));
                    result.Data.Add(new ProteinNode(b[0] + "," + b[1] + "," + a[2]));
                }
            }

            result.Successed = true;
            return result;
        }

        public DataResult<List<ProteinNode>> GenerateProteinNodeCombination(ProteinNode node1, ProteinNode node2)
        {
            var result = new DataResult<List<ProteinNode>>();
            if (node1 == null || node2 == null)
            {
                result.ErrorMessage = "Null arg!";
                return result;
            }
            result.Data = new List<ProteinNode>();
            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3


            var a = node1;
            var b = node2;
            result.Data.Add(new ProteinNode(a[0] + "," + b[1] + "," + a[2]));
            result.Data.Add(new ProteinNode(a[0] + "," + b[1] + "," + b[2]));
            result.Data.Add(new ProteinNode(a[0] + "," + a[1] + "," + b[2]));
            result.Data.Add(new ProteinNode(b[0] + "," + a[1] + "," + a[2]));
            result.Data.Add(new ProteinNode(b[0] + "," + a[1] + "," + b[2]));
            result.Data.Add(new ProteinNode(b[0] + "," + b[1] + "," + a[2]));

            result.Successed = true;
            return result;
        }
    }
}