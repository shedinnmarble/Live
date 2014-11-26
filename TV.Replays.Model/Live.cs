using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV.Replays.Model
{
    public class Live
    {
        public Live(ITv tv)
        {
            this._tv = tv;
        }

        public ITv _tv;

        public TvName TvName
        {
            get { return _tv.Name; }
        }

        public Game Game { get; set; }

        public string RoomUrl { get; set; }

        public string RoomId { get; set; }

        public string Title { get; set; }

        public string PlayerName { get; set; }

        public string VideoIcon { get; set; }

        public string ViewSum { get; set; }

        public string GetVideoLink()
        {
            return _tv.GetVideoLink(this.RoomId);
        }

        public bool Equals(string url)
        {
            if (!url.StartsWith("http://"))
                url = "http://" + url;

            if (url.EndsWith("/"))
                url = url.TrimEnd('/');

            return RoomUrl.Equals(url);
        }
    }
}
