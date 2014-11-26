using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV.Replays.Model.ViewModel
{
    public class PlayerEditViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] TVNames { get; set; }
        public string[] Categories { get; set; }
        public bool IsRecommend { get; set; }
        public int Level { get; set; }
        public bool IsOnline { get; set; }
    }
}
