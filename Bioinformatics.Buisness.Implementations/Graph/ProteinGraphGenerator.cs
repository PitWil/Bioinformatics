using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bioinformatics.Buisness.Contracts.Graph;
using Bioinformatics.Buisness.Contracts.State;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Implementations.Graph
{
    public class ProteinGraphGenerator : IProteinGraphGenerator
    {
        private readonly IProteinNodeGenerator _proteinNodeGenerator;
        private readonly object _synchBuildContainerRoot = new object();
        private readonly object _synchRoot = new object();
        private List<BuildGraphContainer> _buildGraphContainiers;

        public ProteinGraphGenerator(IProteinNodeGenerator proteinNodeGenerator)
        {
            State = PerformingState.Ready;
            _proteinNodeGenerator = proteinNodeGenerator;
        }

        public PerformingState State { get; private set; }

        public double DoneStat
        {
            get
            {
                if (_buildGraphContainiers == null)
                    return 0;

                var sum = 0.0;
                lock (_synchBuildContainerRoot)
                {
                    sum = _buildGraphContainiers.Sum(t => t.DoneStat);
                }

                return sum/_buildGraphContainiers.Count;
            }
        }

        public DataResult<List<ProteinNode>> CreateGraph(List<ProteinNode> nodes, List<ProteinNode> counterexamples)
        {
            lock (_synchRoot)
            {
                if (State != PerformingState.Ready)
                {
                    return new DataResult<List<ProteinNode>> {ErrorMessage = "Refresh"};
                }
                State = PerformingState.Started;
            }

            var result = new DataResult<List<ProteinNode>>();
            if (nodes == null || counterexamples == null)
            {
                result.ErrorMessage = "Incorrect args!";
                return result;
            }
            if (nodes.Count > 0)
            {
                var dictionaryCounterExamples = new Dictionary<string, ProteinNode>();
                counterexamples.ForEach(z => dictionaryCounterExamples.Add(z.RealValue, z));
                var threadCount = 100;
                var threads = new Thread[threadCount];
                lock (_synchBuildContainerRoot)
                {
                    _buildGraphContainiers = new List<BuildGraphContainer>();
                    var count = nodes.Count/threadCount;

                    for (var i = 0; i < threads.Length; i++)
                    {
                        threads[i] = new Thread(BuildGraph) {IsBackground = true};
                        var buildGraphContainer = new BuildGraphContainer
                        {
                            Id = i,
                            ProteinNodes = nodes,
                            ProteinCounterNodes = dictionaryCounterExamples,
                            Count = count,
                            MaxId = i == threads.Length - 1 ? (i + 1)*count + threadCount : (i + 1)*count
                        };

                        _buildGraphContainiers.Add(buildGraphContainer);
                        threads[i].Start(buildGraphContainer);
                    }
                }
                foreach (var t in threads)
                {
                    t.Join();
                }
            }

            result.Successed = true;
            result.Data = nodes;
            return result;
        }

        public DataResult<List<ProteinNode>> GetAllNodesFromGraph(ProteinNode rootNode)
        {
            var result = new DataResult<List<ProteinNode>>();
            if (rootNode == null)
            {
                result.ErrorMessage = "Incorrect arg!";
                return result;
            }
            var resultList = new List<ProteinNode>();

            resultList.Add(rootNode);
            GetAllNodesFromGraphRec(rootNode, ref resultList);
            result.Data = resultList;
            result.Successed = true;
            return result;
        }

        public void Pause()
        {
            lock (_synchRoot)
            {
                if (State != PerformingState.Started)
                {
                    return;
                }

                _buildGraphContainiers.ForEach(z => z.Pause());
                State = PerformingState.Paused;
            }
        }

        public void Resume()
        {
            lock (_synchRoot)
            {
                if (State != PerformingState.Paused)
                {
                    return;
                }
                _buildGraphContainiers.ForEach(z => z.Resume());
                State = PerformingState.Started;
            }
        }

        public void Stop()
        {
            lock (_synchRoot)
            {
                if (State == PerformingState.Started)
                {
                    State = PerformingState.Stopped;
                }
            }
        }

        private void BuildGraph(object obj)
        {
            var buildGraphContainer = (BuildGraphContainer) obj;
            var maxCount = buildGraphContainer.MaxId;
            buildGraphContainer.I = buildGraphContainer.Id*buildGraphContainer.Count;
            for (var i = buildGraphContainer.Id*buildGraphContainer.Count;
                i < maxCount;
                i++, buildGraphContainer.J = 0, buildGraphContainer.I++)
            {
                for (var j = i + 1; j < buildGraphContainer.ProteinNodes.Count; j++)
                {
                    buildGraphContainer.J = j;
                    if (State == PerformingState.Paused)
                    {
                        buildGraphContainer.Pause();
                    }
                    else if (State == PerformingState.Stopped)
                    {
                        return;
                    }

                    var proteinNodesCombinationsResult = _proteinNodeGenerator
                        .GenerateProteinNodeCombination(buildGraphContainer.ProteinNodes[i],
                            buildGraphContainer.ProteinNodes[j]);
                    if (!proteinNodesCombinationsResult.Successed)
                    {
                        return;
                    }
                    ProteinNode pn = null;
                    if (
                        !proteinNodesCombinationsResult.Data.Any(
                            q => buildGraphContainer.ProteinCounterNodes.TryGetValue(q.RealValue, out pn)))
                    {
                        buildGraphContainer.ProteinNodes[i].AddNeighbor(buildGraphContainer.ProteinNodes[j]);
                        buildGraphContainer.ProteinNodes[j].AddNeighbor(buildGraphContainer.ProteinNodes[i]);
                    }
                }
            }
            buildGraphContainer.J = 0;
        }

        protected void GetAllNodesFromGraphRec(ProteinNode rootNode, ref List<ProteinNode> result)
        {
            var p = new List<ProteinNode>();
            p.Add(rootNode);
            while (p.Count > 0)
            {
                rootNode = p[0];
                p.RemoveAt(0);
                for (var i = 0; i < rootNode.Neighbors.Count; i++)
                {
                    //         if (!result.Contains(rootNode.Neighbors[i].Node))
                    {
                        //           result.Add(rootNode.Neighbors[i].Node);
                        //         p.Add(rootNode.Neighbors[i].Node);
                    }
                }
            }
        }

        private class BuildGraphContainer
        {
            private readonly EventWaitHandle _waitHandle;

            public BuildGraphContainer()
            {
                _waitHandle = new AutoResetEvent(false);
            }

            public int Id { get; set; }
            public int Count { get; set; }
            public int MaxId { get; set; }
            public List<ProteinNode> ProteinNodes { get; set; }
            public Dictionary<string, ProteinNode> ProteinCounterNodes { get; set; }
            public int I { get; set; }
            public int J { get; set; }

            public double DoneStat
            {
                get
                {
                    var a = (I - (double) (Id*Count))/Count +
                            1.0/Count*(J/(double) ProteinNodes.Count);
                    return a > 1 ? 1 : a;
                }
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