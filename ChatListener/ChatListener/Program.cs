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
        static string trigger = ">";
        static string name = "Optimus Prime";
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

                    string command = msg.Body.Remove(0, trigger.Length).ToLower();
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
            string data = web.DownloadString("http://www.colaska.com/skype/login.php?user=" + user);
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
                string data = web.DownloadString("http://www.colaska.com/skype/command.php?name=" + commanda[1] + "&user=" + EncodeTo64(name) + "&command=message" + "&instruction=" + message);
                reply = "Command sent to " + commanda[1];
            }
            else
            {
                WebClient web = new WebClient();
                string data = web.DownloadString("http://www.colaska.com/skype/command.php?name=global" + "&user=" + EncodeTo64(name) + "&command=message" + "&instruction=" + message);
                data = web.DownloadString("http://www.colaska.com/skype/" + name + "/online.txt");
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
                case "list":
                    result = ":D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D:D";
                    break;
                case "command":
                    result = "Commands are - \n.hello \n.help \n.date .time";
                    break;
                case "hello":
                    result = "Hello!";
                    break;
                case "help":
                    result = "Please read the HF help docs";
                    break;
                case "date":
                    result = "Current Date is: " +
                             DateTime.Now.ToLongDateString();
                    break;
                case "time":
                    result = "Current Time is: " +
                             DateTime.Now.ToLongTimeString();
                    break;
                default:
                    result = "Sorry, I do not recognize your command";
                    break;

            }
            return result;

        }

    }
}
