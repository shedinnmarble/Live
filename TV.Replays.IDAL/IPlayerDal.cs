using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV.Replays.Model;

namespace TV.Replays.IDAL
{
    public interface IPlayerDal
    {
        void Update(Player player);
        void Delete(string id);
        void Insert(Player player);
        Player Get(string id);
        IEnumerable<Player> Get();
        IEnumerable<Player> Get(int pageIndex, int pageSize, out int total);
    }
}
