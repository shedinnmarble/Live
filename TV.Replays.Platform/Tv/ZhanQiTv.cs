using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TV.Replays.Model;

namespace TV.Replays.Platform.Tv
{
    public class ZhanQiTv : ITv
    {
        public TvName Name
        {
            get { return TvName.战旗Tv; }
        }

        public IEnumerable<Live> GetDota2()
        {
            List<Live> dota2List = new List<Live>();
        start:
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync("http://www.zhanqi.tv/games/dota2");
                string html = response.Result;

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var ul = doc.GetElementbyId("hotList");
                var lis = ul.SelectNodes("li");
                foreach (var li in lis)
                {
                    try
                    {
                        if (li.SelectNodes("div")[0].SelectSingleNode("i").InnerText == "休息")
                            continue;

                        Live live = new Live(this);
                        live.RoomId = li.SelectNodes("div")[1].SelectSingleNode("a").GetAttributeValue("href", "").TrimStart('/');
                        live.RoomUrl = "http://www.zhanqi.tv/" + live.RoomId;
                        live.Title = li.SelectNodes("div")[1].SelectSingleNode("a").InnerText;
                        live.PlayerName = li.SelectNodes("div")[1].SelectSingleNode("div").SelectNodes("a")[0].InnerText;
                        live.ViewSum = li.SelectNodes("div")[1].SelectSingleNode("div").SelectSingleNode("span").SelectSingleNode("span").InnerText;
                        live.VideoIcon = li.SelectNodes("div")[0].SelectSingleNode("a").SelectSingleNode("img").GetAttributeValue("src", "");
                        live.Game = Game.Dota2;

                        dota2List.Add(live);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                goto start;
            }
            return dota2List;
        }

        public string GetVideoLink(string liveRoomId)
        {
            string id = Rooms.ContainsKey(liveRoomId) ? Rooms[liveRoomId] : liveRoomId;
            return "http://www.zhanqi.tv/live/embed?roomId=" + id;
        }

        public static Dictionary<string, string> Rooms = new Dictionary<string, string>();
        static ZhanQiTv()
        {
            Rooms.Add("buring", "2140");
            Rooms.Add("xiwa", "140");
            Rooms.Add("2009", "5161");
            Rooms.Add("achuan", "7679");
            Rooms.Add("laoshusjq", "2021");
            Rooms.Add("naigege", "737");
            Rooms.Add("eh820", "161");
            Rooms.Add("scntv", "252");
            Rooms.Add("air", "784");
            Rooms.Add("fyms", "249");
            Rooms.Add("cctv", "6341");
        }
    }
}
