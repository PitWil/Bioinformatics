using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Bioinformatics.Buisness.Models.Ants
{
    [DataContract]
    public class Ant
    {
        protected static Random Ran = new Random((int) DateTime.Now.Ticks);

        [DataMember] protected double feromon;

        protected List<ProteinNode> VisitedNodes;

        public Ant(double feromon)
        {
            VisitedNodes = new List<ProteinNode>();
            Feromon = feromon;
        }

        public double Feromon
        {
            get { return feromon; }
            set
            {
                if (value > 0)
                {
                    feromon = value;
                }
            }
        }

        public List<ProteinNode> LastPath => VisitedNodes;

        public int LastPathLength => VisitedNodes.Count;

        public virtual List<ProteinNode> Explore(ProteinNode root)
        {
            VisitedNodes = new List<ProteinNode> {root};

            while (true)
            {
                List<ProteinNode.ProteinWeightNode> neigbors = root.Neighbors[0];
                var ws1 = 0.0;
                var grain1 = Ran.NextDouble();
                var sumOfFeromon1 = 0.0;
                var selectedIteamIndex = 0;
                lock (root.SynchRoot)
                {
                    root.Neighbors.ForEach(z => sumOfFeromon1 += z.Weight);
                    var revSumOfFeromon1 = 1/sumOfFeromon1;

                    for (var i = 0; i < root.Neighbors.Count; i++)
                    {
                        ws1 += root.Neighbors[i].Weight*revSumOfFeromon1;
                        if (ws1 > grain1)
                        {
                            neigbors = root.Neighbors[i];
                            selectedIteamIndex = i;

                            break;
                        }
                    }
                }
                var canAddToClique = neigbors.FindAll(
                    a => !VisitedNodes.Exists(x => x.Equals(a.Node)) &&
                         VisitedNodes.All(b => a.Node.NeighborsAll.ContainsKey(b.GetValueHashCode())));

                if (canAddToClique.Count == 0)
                {
                    break;
                }
                lock (root.SynchRoot)
                {
                    root.Neighbors[selectedIteamIndex] += feromon;
                }
                var sumOfFeromon = 0.0;
                canAddToClique.ForEach(z => sumOfFeromon += z.Weight);

                if (Math.Abs(sumOfFeromon) < 0.0000000001)
                    sumOfFeromon = 0.000001;

                var ws = 0.0;
                var grain = Ran.NextDouble();
                var revSumOfFeromon = 1/sumOfFeromon;
                for (var i = 0; i < canAddToClique.Count; i++)
                {
                    ws += canAddToClique[i].Weight*revSumOfFeromon;
                    if (ws > grain)
                    {
                        VisitedNodes.Add(canAddToClique[i].Node);
                        var proteinWeigthNode = neigbors.Find(z => z.Node == canAddToClique[i].Node);
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