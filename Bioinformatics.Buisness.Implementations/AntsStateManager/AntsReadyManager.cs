using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Implementations.AntsStateManager
{
    public class AntsReadyManager : IAntsManager
    {
        private readonly AntsManager _antsManager;

        public AntsReadyManager(AntsManager antsManager)
        {
            _antsManager = antsManager;
        }

        public DataResult<AntsConfigurationModel> GetCurrentAntsSettings()
        {
            lock (_antsManager.SynchRoot)
            {
                return new DataResult<AntsConfigurationModel>
                {
                    Data = _antsManager.AntsConfigurationModel,
                    Successed = _antsManager.AntsConfigurationModel != null,
                    ErrorMessage = _antsManager.AntsConfigurationModel == null ? "Configuration has not been set" : ""
                };
            }
        }

        public DataResult<List<ProteinNode>> GetPositiveNodes()
        {
            return _antsManager.GetPositiveNodes();
        }

        public DataResult<List<ProteinNode>> GetNegativeNodes()
        {
            return _antsManager.GetNegativeNodes();
        }

        public Result SetAntsSettings(AntsConfigurationModel antsConfiguration)
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Ready)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }

                if (antsConfiguration == null)
                {
                    return new Result {ErrorMessage = "Invalid argument"};
                }
                _antsManager.AntsConfigurationModel = antsConfiguration;

                return new Result {Successed = true};
            }
        }

        public Result Start()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Ready)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                if (!GetCurrentAntsSettings().Successed)
                {
                    return new Result {ErrorMessage = "Invalid Setting!"};
                }
                var thread = new Thread(StartAntsSearching) {IsBackground = true};
                thread.Start();

                _antsManager.SetManager(new AntsStartedManager(_antsManager));
                return new Result {Successed = true};
            }
        }

        public Result Pause()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Ready)
                {
                    return new DataResult<bool> {ErrorMessage = "Refresh"};
                }
                return new DataResult<bool> {ErrorMessage = "Has not been started!"};
            }
        }

        public Result Finish()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Ready)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result {ErrorMessage = "Has not been started!"};
            }
        }

        public StatusEntity GraphBuildStat()
        {
            return new StatusEntity {Percentage = _antsManager.ProteinGraphGenerator.DoneStat, TimeToFinish = 0};
        }

        public StatusEntity GraphSearchStat()
        {
            return new StatusEntity {Percentage = _antsManager.ProteinGraphGenerator.DoneStat, TimeToFinish = 0};
        }

        public ManagerState GetManagerState()
        {
            return _antsManager.State;
        }

        private void StartAntsSearching()
        {
            var png = _antsManager.ProteinNodeGenerator;
            var lpn = new List<ProteinNode>();
            _antsManager.AntsConfigurationModel
                .PositiveNodes.ForEach(z => lpn.AddRange(png.GenerateFromProteinSequence(z.Value, 3).Data));

            var pgg = _antsManager.ProteinGraphGenerator;

            var g = pgg.CreateGraph(AutoMapper.Mapper.Map<List<ProteinNode>>(lpn),
                _antsManager.AntsConfigurationModel.CounterNodes);
            var result = _antsManager.Resolve(g.Data);


            var regex = _antsManager.RegexGenerator.GetRegexFromListOfProteinNode(result.Data);

            var reg = new Regex(regex);
            _antsManager
                .ResultChecker
                .Validate(
                    _antsManager.AntsConfigurationModel.PositiveNodesToVerif,
                    _antsManager.AntsConfigurationModel.NegativeNodesToVerif,
                    a => reg.IsMatch(a.Value),
                    b => reg.IsMatch(b.Value));
        }
    }
}