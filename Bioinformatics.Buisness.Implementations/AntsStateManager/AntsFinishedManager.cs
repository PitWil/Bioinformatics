using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Implementations.AntsStateManager
{
    public class AntsFinishedManager : IAntsManager
    {
        private readonly AntsManager _antsManager;

        public AntsFinishedManager(AntsConfigurationModel antsAntsConfigurationModel, AntsManager antsManager)
        {
            _antsManager.AntsConfigurationModel = antsAntsConfigurationModel;
            _antsManager = antsManager;
        }

        public DataResult<AntsConfigurationModel> GetCurrentAntsSettings()
        {
            lock (_antsManager.SynchRoot)
            {
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
                if (_antsManager.State != ManagerState.Finished)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                _antsManager.SetAntsSettings(antsConfiguration);
                return new Result {Successed = true};
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
                if (_antsManager.State != ManagerState.Finished)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                _antsManager.State = ManagerState.Started;
                _antsManager.SetManager(new AntsReadyManager(_antsManager));
                return _antsManager.Start();
            }
        }

        public Result Pause()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Finished)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result {Successed = false, ErrorMessage = "Searching current is finished!"};
            }
        }

        public Result Finish()
        {
            lock (_antsManager.SynchRoot)
            {
                if (_antsManager.State != ManagerState.Finished)
                {
                    return new Result {ErrorMessage = "Refresh"};
                }
                return new Result {Successed = false, ErrorMessage = "Searching current is finished!"};
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
            return new StatusEntity {Percentage = 1, TimeToFinish = 0};
        }
    }
}