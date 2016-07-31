using System.Collections.Generic;
using System.ServiceModel;
using Bioinformatics.Buisness.Contracts;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Contracts.Graph;
using Bioinformatics.Buisness.Implementations.Resolver;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Interfaces;

namespace Bioinformatics.Buisness.Implementations.AntsStateManager
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AntsManager : IAntsManager
    {
        private readonly IProteinRepository _proteinRepository;
        public readonly IProteinGraphGenerator ProteinGraphGenerator;
        public readonly IProteinNodeGenerator ProteinNodeGenerator;
        public readonly IRegexGenerator RegexGenerator;
        public readonly IResultChecker ResultChecker;
        private IAntsManager _antsManager;


        public AntsManager(IAntsFeromonNodesInitializer antsFeromonNodesInitializer,
            IEvaporatorFeromon evaporatorFeromon,
            IProteinRepository proteinRepository,
            IProteinNodeGenerator proteinNodeGenerator,
            IProteinGraphGenerator proteinGraphGenerator, IResultChecker resultChecker, IRegexGenerator regexGenerator)
        {
            SynchRoot = new object();
            CliqueResolver = new AntsCliqueResolver(antsFeromonNodesInitializer, evaporatorFeromon);
            _proteinRepository = proteinRepository;
            ProteinNodeGenerator = proteinNodeGenerator;
            ProteinGraphGenerator = proteinGraphGenerator;
            ResultChecker = resultChecker;
            RegexGenerator = regexGenerator;
            _antsManager = new AntsReadyManager(this);
        }

        public AntsConfigurationModel AntsConfigurationModel { get; set; }
        public object SynchRoot { get; }
        public ManagerState State { get; set; }
        public AntsCliqueResolver CliqueResolver { get; }

        public virtual DataResult<AntsConfigurationModel> GetCurrentAntsSettings()
        {
            return _antsManager.GetCurrentAntsSettings();
        }

        public virtual Result SetAntsSettings(AntsConfigurationModel antsConfiguration)
        {
            return _antsManager.SetAntsSettings(antsConfiguration);
        }

        public DataResult<List<ProteinNode>> GetPositiveNodes()
        {
            var protein = _proteinRepository.GetProteinByParametr(z => z.Experimental);
            if (!protein.Successed)
            {
                return new DataResult<List<ProteinNode>> {ErrorMessage = protein.ErrorMessage};
            }

            return ProteinNodeGenerator.GenerateFromProteins(protein.Data);
        }

        public DataResult<List<ProteinNode>> GetNegativeNodes()
        {
            var protein = _proteinRepository.GetProteinByParametr(z => !z.Experimental);
            if (!protein.Successed)
            {
                return new DataResult<List<ProteinNode>> {ErrorMessage = protein.ErrorMessage};
            }

            return ProteinNodeGenerator.GenerateFromProteins(protein.Data);
        }

        public virtual Result Start()
        {
            return _antsManager.Start();
        }

        public virtual Result Pause()
        {
            return _antsManager.Pause();
        }

        public virtual Result Finish()
        {
            return _antsManager.Finish();
        }

        ManagerState IAntsManager.GetManagerState()
        {
            return State;
        }

        public StatusEntity GraphBuildStat()
        {
            return new StatusEntity {Percentage = ProteinGraphGenerator.DoneStat, TimeToFinish = 0};
        }

        public StatusEntity GraphSearchStat()
        {
            return new StatusEntity {Percentage = CliqueResolver.DoneStat, TimeToFinish = 0};
        }


        public DataResult<List<ProteinNode>> Resolve(List<ProteinNode> nodes)
        {
            CliqueResolver.Colonies = AntsConfigurationModel.Colonies;
            var w = CliqueResolver.Resolve(nodes);
            return w;
        }

        public void SetManager(IAntsManager antsManager)
        {
            if (antsManager != null)
            {
                _antsManager = antsManager;
            }
        }
    }
}