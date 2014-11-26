using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV.Replays.Model.ViewModel
{
    public class VideoViewModel
    {
        public string Id { get; set; }
        public string PlayerName { get; set; }
        public string Title { get; set; }
        public int ViewSum { get; set; }
        public string VideoImage { get; set; }
    }
}
