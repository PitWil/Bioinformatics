using System.Collections.Generic;
using System.Linq;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Buisness.Proxies.ServiceRef;
using Bioinformatics.Common.Others;
using IAntsManager = Bioinformatics.Buisness.Contracts.Ants.IAntsManager;

namespace Bioinformatics.Buisness.Proxies
{
    public class AntsManager : IAntsManager
    {
        public DataResult<AntsConfigurationModel> GetCurrentAntsSettings()
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.GetCurrentAntsSettings();
            antsManagerClient.Close();
            return result;
        }

        public Result SetAntsSettings(AntsConfigurationModel antsConfiguration)
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.SetAntsSettings(antsConfiguration);
            antsManagerClient.Close();
            return result;
        }

        public DataResult<List<ProteinNode>> GetPositiveNodes()
        {
            var result = new DataResult<List<ProteinNode>>();
            var antsManagerClient = new AntsManagerClient();
            var tmpResult = antsManagerClient.GetPositiveNodes();
            antsManagerClient.Close();
            result.Data = tmpResult.Successed ? tmpResult.Data.ToList() : null;
            result.ErrorMessage = tmpResult.ErrorMessage;
            result.Successed = tmpResult.Successed;
            return result;
        }

        public DataResult<List<ProteinNode>> GetNegativeNodes()
        {
            var result = new DataResult<List<ProteinNode>>();
            var antsManagerClient = new AntsManagerClient();
            var tmpResult = antsManagerClient.GetNegativeNodes();
            antsManagerClient.Close();
            result.Data = tmpResult.Successed ? tmpResult.Data.ToList() : null;
            result.ErrorMessage = tmpResult.ErrorMessage;
            result.Successed = tmpResult.Successed;
            return result;
        }

        public Result Start()
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.Start();
            antsManagerClient.Close();
            return result;
        }

        public Result Pause()
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.Pause();
            antsManagerClient.Close();
            return result;
        }

        public Result Finish()
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.Finish();
            antsManagerClient.Close();
            return result;
        }

        public ManagerState GetManagerState()
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.GetManagerState();
            antsManagerClient.Close();
            return result;
        }

        public StatusEntity GraphBuildStat()
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.GraphBuildStat();
            antsManagerClient.Close();
            return result;
        }

        public StatusEntity GraphSearchStat()
        {
            var antsManagerClient = new AntsManagerClient();
            var result = antsManagerClient.GraphBuildStat();
            antsManagerClient.Close();
            return result;
        }
    }
}