using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TV.Replays.Contract;

namespace TV.Replays.WcfClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            Console.WriteLine("loading...");
            using (ChannelFactory<ILiveService> ChannelFactory = new ChannelFactory<ILiveService>("dota2Client"))
            {
                var channel = ChannelFactory.CreateChannel();
                var videos = channel.GetVideoVideoModels();
                foreach (var item in videos)
                {
                    Console.WriteLine("-------");
                    Console.WriteLine(item.PlayerName);
                    Console.WriteLine(item.Title);
                    Console.WriteLine(item.ViewSum);
                    Console.WriteLine();
                }
            }
            Console.WriteLine("end");
            Console.ReadKey();
        }
    }
}
