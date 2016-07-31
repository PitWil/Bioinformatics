using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bioinformatics.Buisness.Contracts;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Contracts.State;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Buisness.Models.Ants;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Implementations.Resolver
{
    public class AntsCliqueResolver : ICliqueResolver
    {
        private static readonly Random Ran = new Random((int) DateTime.Now.Ticks);
        private readonly IAntsFeromonNodesInitializer _antsFeromonNodesInitializer;
        private readonly IEvaporatorFeromon _evaporatorFeromon;
        private readonly object _synchRoot = new object();
        private List<ProteinNode> _bestSolution = new List<ProteinNode>();
        private List<ColonyCliqueSearchContainer> _colonyCliqueSearchContainers;
        private List<ProteinNode> _nodes;

        public AntsCliqueResolver(IAntsFeromonNodesInitializer antsFeromonNodesInitializer,
            IEvaporatorFeromon evaporatorFeromon)
        {
            _antsFeromonNodesInitializer = antsFeromonNodesInitializer;
            _evaporatorFeromon = evaporatorFeromon;
            State = PerformingState.Ready;
        }

        public List<ColonyOfAnts> Colonies { get; set; }

        public PerformingState State { get; set; }

        public double DoneStat
        {
            get
            {
                if (_colonyCliqueSearchContainers == null)
                    return 0;

                var sum = 0.0;
                lock (_synchRoot)
                {
                    sum = _colonyCliqueSearchContainers.Sum(t => t.DoneStat);
                }

                return sum/_colonyCliqueSearchContainers.Count;
            }
        }

        public DataResult<List<ProteinNode>> Resolve(List<ProteinNode> nodes)
        {
            var result = new DataResult<List<ProteinNode>>();
            if (nodes == null)
            {
                return result;
            }
            if (State != PerformingState.Ready)
            {
                result.ErrorMessage = "Please finish current task";
                return result;
            }

            State = PerformingState.Started;
            _nodes = nodes;
            _antsFeromonNodesInitializer.Initialize(_nodes);
            _colonyCliqueSearchContainers = new List<ColonyCliqueSearchContainer>();
            var threads = new Thread[Colonies.Count];
            for (var i = Colonies.Count - 1; i > -1; --i)
            {
                threads[i] = new Thread(ColonyCliqueSearch) {IsBackground = true};

                var colonyCliqueSearchContainer = new ColonyCliqueSearchContainer {Colony = Colonies[i]};
                _colonyCliqueSearchContainers.Add(colonyCliqueSearchContainer);
                threads[i].Start(colonyCliqueSearchContainer);
                Thread.Sleep(80);
            }
            for (var i = 0; i < Colonies.Count; i++)
            {
                threads[i].Join();
            }

            result.Data = _bestSolution;
            result.Successed = true;
            return result;
        }

        public void Resume()
        {
            lock (_synchRoot)
            {
                if (State != PerformingState.Paused)
                {
                    return;
                }
                _colonyCliqueSearchContainers.ForEach(z => z.Resume());
                State = PerformingState.Started;
            }
        }

        public void Pause()
        {
            lock (_synchRoot)
            {
                if (State != PerformingState.Started)
                {
                    return;
                }
                _colonyCliqueSearchContainers.ForEach(z => z.Pause());
                State = PerformingState.Paused;
            }
        }

        public void Stop()
        {
            lock (_synchRoot)
            {
                if (State != PerformingState.Started)
                {
                    return;
                }
                State = PerformingState.Stopped;
            }
        }

        public static int NextGaussian(double mean, double stddev)
        {
            var x1 = 1 - Ran.NextDouble();
            var x2 = 1 - Ran.NextDouble();

            var y1 = Math.Sqrt(-2.0*Math.Log(x1))*Math.Cos(2.0*Math.PI*x2);
            var result = (int) (y1*stddev + mean);
            return result > 0 ? result : -result;
        }

        private void ColonyCliqueSearch(object colonyObj)
        {
            var colonyContainer = (ColonyCliqueSearchContainer) colonyObj;
            colonyContainer.Colony.AntsHill = _nodes[Ran.Next(0, _nodes.Count - 1)];

            colonyContainer.I = 0;
            colonyContainer.J = 0;
            for (var j = 0; j < colonyContainer.Colony.InterationCount; j++, colonyContainer.J++, colonyContainer.I = 0)
            {
                if (State == PerformingState.Paused)
                {
                    colonyContainer.Pause();
                }
                else if (State == PerformingState.Stopped)
                {
                    return;
                }
                for (var i = 0; i < colonyContainer.Colony.Ants.Count; i++, colonyContainer.I++)
                {
                    var t = NextGaussian(colonyContainer.Colony.AntsHillSize/2, colonyContainer.Colony.AntsHillSize/2);
                    var startNode = colonyContainer.Colony.AntsHill;
                    for (var k = 0; k < t; k++)
                    {
                        var id = Ran.Next(0, startNode.Neighbors.Count - 1);
                        var ide = Ran.Next(0, startNode.Neighbors[id].Count - 1);
                        startNode = startNode.Neighbors[id][ide].Node;
                    }
                    AntCliqueSearch(colonyContainer.Colony.Ants[i], startNode);
                }

                _evaporatorFeromon.Evaporate(_nodes);
                if (j%colonyContainer.Colony.DiversificationTime == 0)
                {
                    colonyContainer.Colony.AntsHill = _nodes[Ran.Next(0, _nodes.Count - 1)];
                }
            }
        }


        private void AntCliqueSearch(Ant ant, ProteinNode node)
        {
            var result = ant.Explore(node);
            lock (_synchRoot)
            {
                if (result.Count > _bestSolution.Count)
                {
                    _bestSolution = result;
                }
            }
        }

        private class ColonyCliqueSearchContainer
        {
            private readonly EventWaitHandle _waitHandle;

            public ColonyCliqueSearchContainer()
            {
                _waitHandle = new AutoResetEvent(false);
            }

            public ColonyOfAnts Colony { get; set; }

            public int I { get; set; }
            public int J { get; set; }

            public double DoneStat
            {
                get { return J/(double) Colony.InterationCount + I/(double) Colony.Ants.Count/Colony.InterationCount; }
            }

            public void Pause()
            {
                _waitHandle.WaitOne();
            }

            public void Resume()
            {
                _waitHandle.Set();
            }
        }
    }
}