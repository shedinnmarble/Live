using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV.Replays.Model;
using TV.Replays.Model.ViewModel;

namespace TV.Replays.Service
{
    public static class LiveExtensions
    {
        public static VideoViewModel ConvertToVideoViewModel(this Live live)
        {
            VideoViewModel vm = new VideoViewModel();
            vm.Id = Dota2LiveService.CreateLiveCacheKey(live);
            vm.PlayerName = live.PlayerName;
            vm.Title = live.Title;
            vm.VideoImage = live.VideoIcon;
            vm.ViewSum = live.ViewSumToNumber();

            return vm;
        }

    }
}
