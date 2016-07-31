using System.Collections.Generic;
using System.ServiceModel;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;

namespace Bioinformatics.Buisness.Contracts.Ants
{
    [ServiceContract]
    public interface IAntsManager
    {
        [OperationContract]
        DataResult<AntsConfigurationModel> GetCurrentAntsSettings();

        [OperationContract]
        Result SetAntsSettings(AntsConfigurationModel antsConfiguration);

        [OperationContract]
        DataResult<List<ProteinNode>> GetPositiveNodes();

        [OperationContract]
        DataResult<List<ProteinNode>> GetNegativeNodes();

        [OperationContract]
        Result Start();

        [OperationContract]
        Result Pause();

        [OperationContract]
        Result Finish();

        [OperationContract]
        ManagerState GetManagerState();

        [OperationContract]
        StatusEntity GraphBuildStat();

        [OperationContract]
        StatusEntity GraphSearchStat();
    }
}