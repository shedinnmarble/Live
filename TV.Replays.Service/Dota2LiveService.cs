using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TV.Replays.Contract;
using TV.Replays.DAL;
using TV.Replays.IDAL;
using TV.Replays.Model;
using TV.Replays.Model.ViewModel;

namespace TV.Replays.Service
{
    public class Dota2LiveService : ILiveService
    {
        public IPlayerDal PlayerDal { get; set; }
        public Dota2LiveService()
        {
            PlayerDal = new PlayerDal();
            if (UpdateLiveCacheTimer == null)
                UpdateLiveCacheTimer = new Timer(state =>
                {
                    LoadOrUpdateLiveCache();
                }, null, 0, 30000);
        }

        private static ConcurrentDictionary<string, Live> liveCache;
        private static Timer UpdateLiveCacheTimer;

        public IEnumerable<Live> Lives
        {
            get
            {
                if (liveCache == null)
                {
                    liveCache = new ConcurrentDictionary<string, Live>();
                    LoadOrUpdateLiveCache();
                }
                return liveCache.Values;
            }
        }

        private void LoadOrUpdateLiveCache()
        {
            var tvList = TvFactory.CreateTvList();
            foreach (var tv in tvList)
            {
                var dota2List = tv.GetDota2();
                foreach (var dota2Live in dota2List)
                {
                    string key = CreateLiveCacheKey(dota2Live);
                    liveCache.AddOrUpdate(key, dota2Live, (k, v) => dota2Live);
                }
            }
        }

        public static string CreateLiveCacheKey(Live live)
        {
            switch (live.TvName)
            {
                case TvName.斗鱼Tv:
                    return "dy_" + live.RoomId;
                case TvName.战旗Tv:
                    return "zq_" + live.RoomId;
                case TvName.火猫Tv:
                    return "hm_" + live.RoomId;
                case TvName._17173:
                    return live.RoomId;
                case TvName.YY:
                    return "YY_" + live.RoomId;
                default:
                    return "";
            }
        }

        private IEnumerable<Player> GetPlayers()
        {
            var playerList = PlayerDal.Get();
            foreach (var player in playerList)
            {
                //因为引导tools.replays.net路径到dota2.replays.net/live  
                //修改上传的头像icon路径
                if (!String.IsNullOrEmpty(player.Icon))
                    player.Icon = "http://tools.replays.net" + player.Icon;

                Lives.IsOnline(player);
            }
            return playerList;
        }

        public IEnumerable<LiveViewModel> GetLiveViewModels()
        {
            var players = GetPlayers()
                .Where(a => a.Game == Game.Dota2.ToString())
                .Where(a => a.Recommend == false)
                .OrderByDescending(a => a.Level);

            foreach (Player player in players)
                yield return player.ConvertLiveViewModel();

        }

        public IEnumerable<PlayerViewModel> GetPlayerViewModels()
        {
            var players = GetPlayers()
                .Where(a => a.Game == Game.Dota2.ToString())
                .Where(a => a.Recommend == true)
                .OrderByDescending(a => a.Level);

            foreach (Player player in players)
                yield return player.ConvertPlayerViewModel();

        }

        public IEnumerable<VideoViewModel> GetVideoVideoModels()
        {
            foreach (Live live in Lives)
                yield return live.ConvertToVideoViewModel();
        }

        public IEnumerable<PlayerEditViewModel> GetPlayerEditViewModels()
        {
            var players = GetPlayers();
            foreach (Player player in players)
                yield return player.ConvertPlayerEditViewModel();
        }


        public PlayerInformationViewModel GetPlayerInformationViewModel(string id)
        {
            var player = GetPlayers().FirstOrDefault(p => p.Id == id);

            if (player != null)
                return player.ConvertPlayerInformationViewModel();

            return null;
        }
    }
}
