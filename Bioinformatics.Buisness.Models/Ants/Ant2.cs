using System;
using System.Collections.Generic;

namespace Bioinformatics.Buisness.Models.Ants
{
    public class Ant2 : Ant
    {
        public Ant2(double feromon) : base(feromon)
        {
        }

        public override List<ProteinNode> Explore(ProteinNode root)
        {
            VisitedNodes = new List<ProteinNode>();
            if (root == null)
            {
                var a = 1;
                a += 1;
            }
            VisitedNodes.Add(root);
            while (true)
            {
                List<ProteinNode.ProteinWeightNode> neightbors = root.Neighbors[0];
                var ws1 = 0.0;
                var grain1 = Ran.NextDouble();
                var sumOfFeromon1 = 0.0;
                lock (root.SynchRoot)
                {
                    root.Neighbors.ForEach(z => sumOfFeromon1 += z.Weight);


                    var revSumOfFeromon1 = 1 / sumOfFeromon1;
                    if (Math.Abs(sumOfFeromon1) < 00001)
                        sumOfFeromon1 = 0.000001;

                    ws1 = 1;
                    for (var i = 0; i < root.Neighbors.Count; i++)
                    {
                        ws1 -= root.Neighbors[i].Weight * revSumOfFeromon1;
                        if (ws1 < grain1 || Math.Abs(ws1) < 0.00001)
                        {
                            neightbors = root.Neighbors[i];
                            root.Neighbors[i] += feromon;

                            break;
                        }
                    }
                }
                var sumOfFeromon = 0.0;


                var canByAddToRoute = neightbors.FindAll(a => !VisitedNodes.Contains(a.Node));

                if (canByAddToRoute.Count == 0)
                {
                    break;
                }

                canByAddToRoute.ForEach(z => sumOfFeromon += z.Weight);
                if (Math.Abs(sumOfFeromon) < 0.0000000001)
                    sumOfFeromon = 0.000001;

                var ws = 0.0;
                var grain = Ran.NextDouble();
                var revSumOfFeromon = 1 / sumOfFeromon;
                for (var i = 0; i < canByAddToRoute.Count; i++)
                {
                    ws += canByAddToRoute[i].Weight * revSumOfFeromon;
                    if (ws > grain)
                    {
                        VisitedNodes.Add(canByAddToRoute[i].Node);
                        var proteinWeigthNode = neightbors.Find(z => z.Node == canByAddToRoute[i].Node);
                        proteinWeigthNode += feromon;
                        root = VisitedNodes[VisitedNodes.Count - 1];
                        break;
                    }
                }
            }
            return VisitedNodes;
        }
    }
}