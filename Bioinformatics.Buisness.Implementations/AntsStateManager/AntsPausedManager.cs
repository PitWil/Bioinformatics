using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Implementations.AntsStateManager
{
    public class AntsPausedManager : IAntsManager
    {
        private readonly AntsManager _antsManager;

        public AntsPausedManager(AntsManager antsManager)
        {
            _antsManager = antsManager;
        }

        public DataResult<AntsConfigurationModel> GetCurrentAntsSettings()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Paused)
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
                if (_antsManager.State != ManagerState.Paused)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result
                {
                    Successed = false,
                    ErrorMessage = "Searching should be stoped before update settings!"
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

        public Result Start()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Paused)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result {Successed = false, ErrorMessage = "Searching should be stoped before restart!"};
            }
        }

        public Result Pause()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Paused)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result {Successed = false, ErrorMessage = "Searching current is stoped!"};
            }
        }

        public Result Finish()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Paused)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return null;
            }
        }

        public ManagerState GetManagerState()
        {
            return _antsManager.State;
        }

        public StatusEntity GraphBuildStat()
        {
            return new StatusEntity {Percentage = _antsManager.ProteinGraphGenerator.DoneStat, TimeToFinish = 0};
        }

        public StatusEntity GraphSearchStat()
        {
            return new StatusEntity {Percentage = _antsManager.CliqueResolver.DoneStat, TimeToFinish = 0};
        }
    }
}