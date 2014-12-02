using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TV.Replays.Model;
using TV.Replays.Platform.Tv;

namespace TV.Replays.Platform
{
    public class TvFactory
    {
        private static IEnumerable<ITv> tvList;

        public static IEnumerable<ITv> CreateTvList()
        {
            if (tvList == null)
            {
                tvList = RefectionTvList();
            }
            return tvList;
        }

        public static ITv CreateTv(TvName tvName)
        {
            return CreateTvList().SingleOrDefault(tv => tv.Name == tvName);
        }

        private static IEnumerable<ITv> RefectionTvList()
        {
            var assembly = Assembly.LoadFrom(typeof(TvFactory).Namespace + ".dll");
            var types = assembly.GetExportedTypes();
            foreach (Type type in types)
            {
                if (type.IsClass && typeof(ITv).IsAssignableFrom(type))
                {
                    ITv tv = (ITv)Activator.CreateInstance(type);
                    yield return tv;
                }
            }
        }
    }
}
