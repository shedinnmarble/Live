using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using TV.Replays.Contract;
using TV.Replays.Model;
using TV.Replays.Model.ViewModel;

namespace TV.Replays.WebApi.Controllers
{
    public class CategoryController : ApiController
    {
        const int DefaultCount = 8;

        public IEnumerable<LiveViewModel> Get()
        {
            using (ChannelFactory<ILiveService> channelFactory = new ChannelFactory<ILiveService>("dota2Client"))
            {
                var channel = channelFactory.CreateChannel();
                return channel.GetLiveViewModels().OrderByDescending(a => a.ViewSum);
            }
        }

        public IEnumerable<LiveViewModel> Get(string id)
        {
            IEnumerable<LiveViewModel> result = Get();
            if (result != null)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    result = result.Where(a => a.Categories.Contains(id));
                }
            }

            if (result.Count() > DefaultCount)
                return result.Take(DefaultCount);

            return result;
        }
    }
}