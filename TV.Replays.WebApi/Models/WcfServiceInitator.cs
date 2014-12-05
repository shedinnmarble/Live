using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TV.Replays.WebApi.Models
{
    public class WcfServiceInitator
    {
        public static int Start(string fileName, string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes == null || processes.Length <= 0)
            {
                Process.Start(fileName);
            }
            return processes.Length;
        }

        public static int Close(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes != null && processes.Length > 0)
                processes[0].Kill();
            return processes.Length;
        }
    }
}