using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TV.Replays.Model
{
    public class Player
    {
        public Player()
        {
            LiveRooms = new List<LiveRoom>();
            if (string.IsNullOrEmpty(Id))
                Id = MongoDB.Oid.NewOid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }

        [Required]
        public string Game { get; set; }

        [Required]
        public string[] Categories { get; set; }

        public bool Recommend { get; set; }

        public int Level { get; set; }

        public string Description { get; set; }

        public List<LiveRoom> LiveRooms { get; set; }

        public bool IsOnline()
        {
            return LiveRooms.Select(a => a.IsOnline).Contains(true);
        }

        public Live Live { get; set; }
    }
}
