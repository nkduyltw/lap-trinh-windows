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
    public partial class MCChatRoom : Form
    {
        public string data;
        public MCChatRoom()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        public delegate void KTResult(string s);
        public KTResult result;
        private MC mainFrom = null;
        public MCChatRoom(Form callingForm)
        {
            mainFrom = callingForm as MC;
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        private void MCChatRoom_Load(object sender, EventArgs e)
        {

        }

        private void MCChatRoom_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Close();
        }

        IPEndPoint IP;
        Socket Server;
        List<Socket> ClientList;
        /// <summary>
        /// kết nối tới sever
        /// </summary>
        void Connect()
        {
            ClientList = new List<Socket>();
            // IP là đỉa chỉ của server
            IP = new IPEndPoint(IPAddress.Any, 12345);
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
                        ClientList.Add(Client);

                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(Client);
                    }
                }
                catch
                {
                    // nếu mà tự nhiên một client thoát ra thì đột xuất thì thực hiện lại nhân ip
                    IP = new IPEndPoint(IPAddress.Any, 12345);
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
            if (client != null && txtinput.Text != string.Empty)
            {
                client.Send(Serialize(txtinput.Text));
            }

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
            lvchat.Items.Add(new ListViewItem() { Text = s });
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

        private void btnsend_Click(object sender, EventArgs e)
        {
           
                foreach (Socket item in ClientList)
                {
                    Send(item);
                }
                AddMessage(txtinput.Text);
                txtinput.Clear();
        }

        private void txtinput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
