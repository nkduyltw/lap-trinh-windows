using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MC
{
    public partial class LiveStream : Form
    {
        public LiveStream()
        {
            InitializeComponent();
        }
        Thread thread;
        Socket receiveSocket;
        IPEndPoint hostIpEndPoint;
        List<Socket> ClientList;
        private Bitmap GetScreen()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            return bitmap;
        }
        //
        private void threadimage()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GetScreen().Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);   //Here I use the BMP format
                byte[] b = ms.ToArray();
                foreach (Socket item in ClientList)
                {
                    item.Send(b);
                }
                ms.Close();
            }
            catch (Exception ee)
            {
            }
            //Thread.Sleep(1000);
            threadimage();
        }
        //
        private void connectSocket()
        {
            ClientList = new List<Socket>();
            receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            hostIpEndPoint = new IPEndPoint(IPAddress.Any, 10001);
            //Connection node
            receiveSocket.Bind(hostIpEndPoint);
            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        receiveSocket.Listen(100);
                        MessageBox.Show("start");
                        Socket hostSocket = receiveSocket.Accept();
                        ClientList.Add(hostSocket);
                        thread = new Thread(new ThreadStart(threadimage));
                        thread.IsBackground = true;
                        thread.Start();
                    }
                }
                catch
                {
                    receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    hostIpEndPoint = new IPEndPoint(IPAddress.Any, 10001);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }
        private void LiveStream_Load(object sender, EventArgs e)
        {
            connectSocket();
            this.Hide(); // Hide Form
        }
    }
}
