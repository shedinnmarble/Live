using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace TV.Replays.Model
{
    public class TvFactory
    {
        private static readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tv");
        private static IEnumerable<ITv> tvList;

        static TvFactory()
        {
#if DEBUG
            path = @"E:\工作文件夹\TV.Replays\Tv";
#endif
        }

        public static IEnumerable<ITv> CreateTvList()
        {
            if (tvList == null)
            {
                tvList = ReflectionTvList();
            }
            return tvList;
        }

        public static ITv CreateTv(TvName tvName)
        {
            return CreateTvList().SingleOrDefault(tv => tv.Name == tvName);
        }

        private static IEnumerable<ITv> ReflectionTvList()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            var files = dirInfo.GetFiles("*.dll");

            foreach (var file in files)
            {
                var assembly = Assembly.LoadFrom(file.FullName);
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.IsClass && typeof(ITv).IsAssignableFrom(type))
                    {
                        ITv instance = (ITv)Activator.CreateInstance(type);
                        yield return instance;
                    }
                }
            }
        }
    }
}
