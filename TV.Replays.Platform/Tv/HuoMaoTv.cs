using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TV.Replays.Model;

namespace TV.Replays.Platform.Tv
{
    public class HuoMaoTv : ITv
    {
        public TvName Name
        {
            get { return TvName.火猫Tv; }
        }

        public IEnumerable<Live> GetDota2()
        {
            List<Live> liveList = new List<Live>();
        start:
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync("http://www.huomaotv.com/live_list?gid=23");
                string html = response.Result;

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var divList = doc.GetElementbyId("live_list");
                foreach (HtmlNode div in divList.SelectNodes("div"))
                {
                    try
                    {
                        if (div.SelectNodes("dl")[0].SelectSingleNode("a") != null && div.SelectNodes("dl")[0].SelectSingleNode("a").InnerText == "主播正在休息")
                            continue;

                        Live live = new Live(this);
                        live.VideoIcon = "http://www.huomaotv.com" + div.SelectNodes("dl")[0].SelectSingleNode("dd").SelectSingleNode("a").SelectSingleNode("img").GetAttributeValue("src", "");
                        live.RoomUrl = "http://www.huomaotv.com" + div.SelectNodes("dl")[1].SelectSingleNode("dt").SelectSingleNode("a").GetAttributeValue("href", "");
                        live.Title = div.SelectNodes("dl")[1].SelectSingleNode("dt").SelectSingleNode("a").InnerText;
                        live.RoomId = div.SelectNodes("dl")[1].SelectSingleNode("dt").SelectSingleNode("a").GetAttributeValue("href", "").TrimStart("/live/".ToArray());
                        live.PlayerName = div.SelectNodes("dl")[1].SelectSingleNode("dd").SelectSingleNode("a").InnerText;
                        live.ViewSum = div.SelectNodes("dl")[1].SelectSingleNode("dd").SelectSingleNode("span").InnerText;
                        live.Game = Game.Dota2;

                        liveList.Add(live);
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
            return liveList;
        }

        public string GetVideoLink(string liveRoomId)
        {
            //  <iframe height=498 width=510 src='http://www.huomaotv.com/index.php?c=outplayer&live_id=15' frameborder=0 allowfullscreen></iframe>
            return "http://www.huomaotv.com/index.php?c=outplayer&live_id=" + liveRoomId;
        }
    }
}
