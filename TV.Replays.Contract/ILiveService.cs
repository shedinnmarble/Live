using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TV.Replays.Model.ViewModel;

namespace TV.Replays.Contract
{
    [ServiceContract]
    public interface ILiveService
    {
        [OperationContract]
        IEnumerable<LiveViewModel> GetLiveViewModels();

        [OperationContract]
        IEnumerable<PlayerViewModel> GetPlayerViewModels();

        [OperationContract]
        IEnumerable<VideoViewModel> GetVideoVideoModels();

        [OperationContract]
        IEnumerable<PlayerEditViewModel> GetPlayerEditViewModels();

        [OperationContract]
        PlayerInformationViewModel GetPlayerInformationViewModel(string id);
    }
}
