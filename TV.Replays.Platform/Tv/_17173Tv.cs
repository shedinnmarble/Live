using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TV.Replays.Model;

namespace TV.Replays.Platform.Tv
{
    public class _17173Tv : ITv
    {
        public TvName Name
        {
            get { return TvName._17173; }
        }

        public IEnumerable<Live> GetDota2()
        {
            List<Live> dota2LiveList = new List<Live>();
        start:
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync("http://v.17173.com/live//index/gameList.action?key=&gameId=421&pageSize=20&pageNum=0&type=2&_=1413871380162");
                string json = response.Result;

                dynamic obj = JsonConvert.DeserializeObject(json);

                foreach (var item in obj.obj[0].list)
                {
                    try
                    {
                        Live dota2Live = new Live(this);
                        dota2Live.RoomId = item.beautifulNo + "_" + item.liveRoomId;
                        dota2Live.RoomUrl = item.url;
                        dota2Live.PlayerName = item.userName;
                        dota2Live.VideoIcon = item.liveImg;
                        dota2Live.ViewSum = item.viewSum;
                        dota2Live.Title = item.liveTitle;
                        dota2Live.Game = Game.Dota2;

                        dota2LiveList.Add(dota2Live);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                goto start;
            }

            return dota2LiveList;
        }

        public string GetVideoLink(string liveRoomId)
        {
            return "http://v.17173.com/live/player/Player_stream_customOut.swf&url=http://v.17173.com/live/" + liveRoomId.Replace('_', '/');
        }
    }
}
