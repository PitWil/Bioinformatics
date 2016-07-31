using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Implementations.AntsStateManager
{
    public class AntsStartedManager : IAntsManager
    {
        private readonly AntsManager _antsManager;

        public AntsStartedManager(AntsManager antsManager)
        {
            _antsManager = antsManager;
        }

        public DataResult<AntsConfigurationModel> GetCurrentAntsSettings()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Started)
                {
                    return new DataResult<AntsConfigurationModel> {ErrorMessage = "Refresh"};
                }
                return new DataResult<AntsConfigurationModel>
                {
                    Successed = true,
                    Data = _antsManager.AntsConfigurationModel
                };
            }
        }

        public Result SetAntsSettings(AntsConfigurationModel antsConfiguration)
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Started)
                {
                    return new DataResult<bool> {ErrorMessage = "Refresh"};
                }
                return new DataResult<bool> {ErrorMessage = "Stop searching!"};
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

        public Result Start()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Started)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result {ErrorMessage = "Has not been started!"};
            }
        }

        public Result Pause()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Started)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result {ErrorMessage = "Has not been started!"};
            }
        }

        public Result Finish()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Started)
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
            return new StatusEntity {Percentage = _antsManager.CliqueResolver.DoneStat, TimeToFinish = 0};
        }

        public ManagerState GetManagerState()
        {
            return _antsManager.State;
        }
    }
}