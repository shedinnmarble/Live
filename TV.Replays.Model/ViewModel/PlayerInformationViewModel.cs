using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV.Replays.Model.ViewModel
{
    public class PlayerInformationViewModel
    {
        public PlayerInformationViewModel()
        {
            if (Categories == null)
                Categories = new string[0];
        }

        public string PlayerName { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string[] Categories { get; set; }
        public int ViewSum { get; set; }
        public string Url { get; set; }
    }
}
