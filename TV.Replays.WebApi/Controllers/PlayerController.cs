using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using TV.Replays.Contract;
using TV.Replays.Model.ViewModel;

namespace TV.Replays.WebApi.Controllers
{
    public class PlayerController : ApiController
    {
        const int DefaultCount = 12;

        public IEnumerable<PlayerViewModel> Get()
        {
            IEnumerable<PlayerViewModel> players;
            using (ChannelFactory<ILiveService> channelFactory = new ChannelFactory<ILiveService>("dota2Client"))
            {
                var channel = channelFactory.CreateChannel();
                players = channel.GetPlayerViewModels()
                    .OrderByDescending(a => a.ViewSum);
            }

            if (players.Count() > DefaultCount)
                return players.Take(DefaultCount);

            return players;
        }
        public PlayerInformationViewModel Get(string id)
        {
            using (ChannelFactory<ILiveService> channelFactory = new ChannelFactory<ILiveService>("dota2Client"))
            {
                var channel = channelFactory.CreateChannel();
                return channel.GetPlayerInformationViewModel(id);
            }
        }
    }
}
