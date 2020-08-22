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

namespace Client
{
    public partial class ChatRoom : Form
    {
        string data;
        public ChatRoom()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        public delegate void KTResult(string s);
        public KTResult result;
        private Client mainFrom = null;
        public ChatRoom(Form callingForm)
        {
            mainFrom = callingForm as Client;
            InitializeComponent();
            result = new KTResult(GetIP);
            
        }
        void GetIP(string s)
        {
            data = s;
            if(data != "")
            {
                CheckForIllegalCrossThreadCalls = false;
                Connect();
            }
        }
        private void ChatRoom_Load(object sender, EventArgs e)
        {

        }

        private void btnsend_Click(object sender, EventArgs e)
        {
            Send();
        }

        IPEndPoint IP;
        Socket Client;
        /// <summary>
        /// kết nối tới sever
        /// </summary>
        void Connect()
        {
            // IP là đỉa chỉ của server
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                Client.Connect(IP);
            }
            catch
            {
                MessageBox.Show("Không thể kết nối đến server!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();

        }
        /// <summary>
        /// đóng kết nối hiện thời lại
        /// </summary>
        void _Close()
        {
            Client.Close();
        }
        /// <summary>
        /// Gửi tin
        /// </summary>
        void Send()
        {
            if (txtinput.Text != string.Empty)
            {
                Client.Send(Serialize(txtinput.Text));
            }
            lvchat.Items.Add(txtinput.Text);
            txtinput.Clear();
        }
        /// <summary>
        /// nhận tin
        /// </summary>
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    Client.Receive(data);
                    string message = (string)Deserialize(data);
                    AddMessage(message);
                }
            }
            catch
            {
                _Close();
            }
        }

        /// <summary>
        /// add message vào khung chat
        /// </summary>
        /// <param name="s"></param>
        void AddMessage(string s)
        {
            lvchat.Items.Add(new ListViewItem() { Text = s });
            txtinput.Clear();
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

        /// <summary>
        /// Đóng kết nối khi đóng Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatRoom_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Close();
        }
    }
}
