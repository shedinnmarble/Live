using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TV.Replays.Service;

namespace TV.Replays.Dota2Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ServiceHost host = new ServiceHost(typeof(Dota2LiveService)))
                {
                    host.Opened += delegate
                    {
                        Console.WriteLine("Dota2服务开启 ! ");
                    };
                    host.Open();

                    while (true)
                    {
                        Console.WriteLine("输入'exc'停止服务！");
                        string input = Console.ReadLine();

                        if (input.Equals("exc"))
                            return;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
