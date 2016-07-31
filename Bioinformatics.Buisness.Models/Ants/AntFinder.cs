using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bioinformatics.Buisness.Models.Ants
{
    [DataContract]
    public class AntFinder : Ant
    {
        public AntFinder(double feromon) : base(feromon)
        {
        }

        public override List<ProteinNode> Explore(ProteinNode root)
        {
            VisitedNodes = new List<ProteinNode>();

            VisitedNodes.Add(root);
            while (true)
            {
                List<ProteinNode.ProteinWeightNode> neighbors = root.Neighbors[0];
                var ws1 = 0.0;
                var grain1 = Ran.NextDouble();
                var sumOfFeromon1 = 0.0;
                lock (root.SynchRoot)
                {
                    root.Neighbors.ForEach(z => sumOfFeromon1 += z.Weight);


                    var revSumOfFeromon1 = 1/sumOfFeromon1;
                    if (Math.Abs(sumOfFeromon1) < 0.0000000001)
                        sumOfFeromon1 = 0.000001;


                    for (var i = 0; i < root.Neighbors.Count; i++)
                    {
                        ws1 += 1 - root.Neighbors[i].Weight*revSumOfFeromon1;
                        if (ws1 > grain1 || Math.Abs(ws1) < 0.00001)
                        {
                            neighbors = root.Neighbors[i];
                            root.Neighbors[i] += feromon;

                            break;
                        }
                    }
                }
                var sumOfFeromon = 0.0;


                var canByAddToRoute = neighbors.FindAll(a => !VisitedNodes.Contains(a.Node));

                if (canByAddToRoute.Count == 0)
                {
                    break;
                }

                var grain = Ran.NextDouble();
                var ws = 0.0;
                canByAddToRoute.ForEach(z => sumOfFeromon += z.Weight);

                if (Math.Abs(sumOfFeromon) < 0.0000000001)
                    sumOfFeromon = 0.000001;

                var revSumOfFeromon = 1/sumOfFeromon;
                for (var i = 0; i < canByAddToRoute.Count; i++)
                {
                    ws += 1 - canByAddToRoute[i].Weight*revSumOfFeromon;
                    if (ws > grain || Math.Abs(ws) < 0.00001)
                    {
                        VisitedNodes.Add(canByAddToRoute[i].Node);
                        var proteinWeigthNode = neighbors.Find(z => z.Node == canByAddToRoute[i].Node);
                        proteinWeigthNode += feromon;
                        root = VisitedNodes[VisitedNodes.Count - 1];
                        break;
                    }
                }
            }
            return new List<ProteinNode>();
        }
    }
}