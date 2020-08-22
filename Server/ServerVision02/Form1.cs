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

namespace ServerVision02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        IPEndPoint IP;
        Socket Server;
        List<Socket> ClientList;
        int n = 0;
        /// <summary>
        /// kết nối tới sever
        /// </summary>
        void Connect()
        {
            ClientList = new List<Socket>();
            // IP là đỉa chỉ của server
            IP = new IPEndPoint(IPAddress.Any, 5000);
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            Server.Bind(IP);

            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Server.Listen(100);
                        Socket Client = Server.Accept();
                        n = n + 1;
                        AddMessage("Người chơi thứ " + n + " đã kết nối");
                        ClientList.Add(Client);

                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(Client);
                    }
                }
                catch
                {
                    // nếu mà tự nhiên một client thoát ra thì đột xuất thì thực hiện lại nhân ip
                    IP = new IPEndPoint(IPAddress.Any, 5000);
                    Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }
        /// <summary>
        /// đóng kết nối hiện thời lại
        /// </summary>
        void _Close()
        {
            Server.Close();
        }
        /// <summary>
        /// Gửi tin
        /// </summary>
        void Send(Socket client)
        {
            //if (client != null && txtinput.Text != string.Empty)
            //{
            //    client.Send(Serialize(txtinput.Text));
            //}

        }
        /// <summary>
        /// nhận tin
        /// </summary>
        void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string message = (string)Deserialize(data);

                    foreach (Socket item in ClientList)
                    {
                        if (item != null && item != client)
                        {
                            item.Send(Serialize(message));
                        }
                    }
                    //for(int i=0;i<ClientList.Count;i++)
                    //{
                    //    ClientList[i].Send(Serialize(message));
                    //}
                    AddMessage(message);
                }
            }
            catch
            {
                ClientList.Remove(client);
                client.Close();
            }
        }

        /// <summary>
        /// add message vào khung chat
        /// </summary>
        /// <param name="s"></param>
        void AddMessage(string s)
        {
            lvOutPut.Items.Add(new ListViewItem() { Text = s });
        }
        /// <summary>
        /// Phân mảnh
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Close();
        }
    }
}
