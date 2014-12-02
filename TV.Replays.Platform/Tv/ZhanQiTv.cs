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
            //string url = "http://www.zhanqi.tv/" + liveRoomId;
            //HttpClient client = new HttpClient();
            //var result = client.GetStringAsync(url);
            //string html = result.Result;

            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);

            //HtmlNode element = doc.GetElementbyId("js-flash-layer");
            //var roomLink = element.SelectNodes("div")[0].SelectNodes("div")[0].SelectSingleNode("a").GetAttributeValue("href", "");
            //string[] sprit = roomLink.Split('?');
            //string roomId = String.Empty;
            //if (sprit.Length > 0)
            //    roomId = sprit[1];

            //return "http://www.zhanqi.tv/live/embed?" + roomId;

            return "http://www.zhanqi.tv/live/embed?roomId=" + liveRoomId;
        }
    }
}
