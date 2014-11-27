using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV.Replays.Model.ViewModel
{
    public class PlayerViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsOnline { get; set; }
        public int ViewSum { get; set; }
    }
}
