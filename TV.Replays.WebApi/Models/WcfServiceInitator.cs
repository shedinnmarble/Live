using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TV.Replays.WebApi.Models
{
    public class WcfServiceInitator
    {
        public static void Start(string fileName, string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes == null || processes.Length <= 0)
            {
                Process.Start(fileName);
            }
        }
    }
}