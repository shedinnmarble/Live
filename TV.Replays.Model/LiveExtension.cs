using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV.Replays.Model
{
    public static class LiveExtension
    {
        public static void IsOnline(this IEnumerable<Live> lives, Player player)
        {
            foreach (Live live in lives)
            {
                LiveRoom playerLiveRoom = player.LiveRooms.FirstOrDefault(a => a.Name == live.TvName.ToString());

                if (playerLiveRoom != null && live.Equals(playerLiveRoom.Url))
                {
                    player.Live = live;
                    playerLiveRoom.IsOnline = true;
                }
            }
        }
        public static int ViewSumToNumber(this Live live)
        {
            string result = string.Empty;
            string original = live.ViewSum;
            string[] sprit = original.Split('.');

            if (sprit.Length > 1)
            {
                result = original.Replace(".", "")
                    .Replace("万", "000");
            }
            else
            {
                result = original.Replace("万", "0000");
            }

            return int.Parse(result);
        }
    }
}
