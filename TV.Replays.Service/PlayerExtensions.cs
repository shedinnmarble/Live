using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV.Replays.Model;
using TV.Replays.Model.ViewModel;

namespace TV.Replays.Service
{
    public static class PlayerExtensions
    {
        public static LiveViewModel ConvertLiveViewModel(this Player player)
        {
            LiveViewModel vm = new LiveViewModel();
            vm.Id = player.Id;
            vm.PlayerName = player.Name;
            vm.Categories = player.Categories;
            vm.IsOnline = player.IsOnline();

            if (player.Live != null)
            {
                vm.ViewSum = player.Live.ViewSumToNumber();
                vm.Title = player.Live.Title;
                vm.VideoImage = player.Live.VideoIcon;
            }
            else
            {
                vm.Title = player.Name;
                vm.ViewSum = 0;
                vm.VideoImage = player.Icon;
            }

            return vm;
        }
        public static PlayerViewModel ConvertPlayerViewModel(this Player player)
        {
            PlayerViewModel vm = new PlayerViewModel();
            vm.Id = player.Id;
            vm.Name = player.Name;
            vm.Icon = player.Icon;
            vm.IsOnline = player.IsOnline();
            if (player.Live != null)
                vm.ViewSum = player.Live.ViewSumToNumber();

            return vm;
        }
        public static PlayerInformationViewModel ConvertPlayerInformationViewModel(this Player player)
        {
            PlayerInformationViewModel vm = new PlayerInformationViewModel();
            if (player.Live != null)
            {
                vm.Title = player.Live.Title;
                vm.ViewSum = player.Live.ViewSumToNumber();
                vm.Url = player.Live.GetVideoLink();
            }
            vm.PlayerName = player.Name;
            vm.Icon = player.Icon;
            vm.Description = player.Description;
            vm.Categories = player.Categories;

            return vm;
        }
        public static PlayerEditViewModel ConvertPlayerEditViewModel(this Player player)
        {
            PlayerEditViewModel vm = new PlayerEditViewModel();
            vm.Id = player.Id;
            vm.Name = player.Name;
            vm.Categories = player.Categories;
            if (player.LiveRooms != null)
                vm.TVNames = player.LiveRooms.Select(a => a.Name).ToArray();
            vm.Level = player.Level;
            vm.IsRecommend = player.Recommend;
            vm.IsOnline = player.IsOnline();

            return vm;
        }
    }
}
