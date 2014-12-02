using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
                    UpdateLiveCacheTimer.Change(15000, Timeout.Infinite);
                }, null, 0, Timeout.Infinite);
        }

        private static Dictionary<string, Live> liveCache = new Dictionary<string, Live>();
        private static Timer UpdateLiveCacheTimer;

        public IEnumerable<Live> Lives
        {
            get
            {
                return liveCache.Values;
            }
        }

        private void LoadOrUpdateLiveCache()
        {
            var tvList = TV.Replays.Platform.TvFactory.CreateTvList();
            Dictionary<string, Live> dota2Dic = new Dictionary<string, Live>();

            foreach (var dota2Live in GetDota2Lives(tvList))
            {
                string key = CreateLiveCacheKey(dota2Live);
                dota2Dic.Add(key, dota2Live);
            }

            liveCache = dota2Dic;
        }

        private static IEnumerable<Live> GetDota2Lives(IEnumerable<ITv> tvList)
        {
            List<Task<IEnumerable<Live>>> tasks = new List<Task<IEnumerable<Live>>>();
            foreach (var tv in tvList)
            {
                var task = Task.Factory.StartNew<IEnumerable<Live>>(() =>
                {
                    return tv.GetDota2();
                });
                tasks.Add(task);
            }

            List<Live> dota2Lives = new List<Live>();
            foreach (var task in tasks)
            {
                dota2Lives.AddRange(task.Result);
            }
            return dota2Lives;
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
