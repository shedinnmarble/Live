using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using TV.Replays.Contract;
using TV.Replays.Model.ViewModel;

namespace TV.Replays.WebApi.Controllers
{
    public class VideoController : ApiController
    {
        public IEnumerable<VideoViewModel> Get()
        {
            using (ChannelFactory<ILiveService> channelFactory = new ChannelFactory<ILiveService>("dota2Client"))
            {
                var channel = channelFactory.CreateChannel();
                return channel.GetVideoVideoModels()
                    .OrderByDescending(a => a.ViewSum);
            }
        }
    }
}
