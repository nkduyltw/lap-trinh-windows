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
    public partial class MC : Form
    {   // khởi tạo biến đếm gửi câu hỏi
        int IntCount = 0;
        // khởi tạo biến kiểm tra kết nối
        int KtConnect = 0;
        // khởi tạo đường dẫn hình ảnh
        public string fileName;
        // khởi tọa list chứa câu hỏi
        List<Questions> list = new List<Questions>() ;
        List<Questions> list02 = new List<Questions> ();
        //
        bool kt = false;
        // khởi tạo giá trị kế tiếp
        int Value_NextQs = -1;
        int Value_NextQs02 = -1;
        public MC()
        {
            InitializeComponent();
        }

        IPEndPoint IP;
        Socket Client;
        /// <summary>
        /// kết nối tới sever
        /// </summary>
        void Connect(string s)
        {
            try
            {
                IP = new IPEndPoint(IPAddress.Parse(s), 5000);
            }
            catch
            {
            }
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                Client.Connect(IP);
                KtConnect = 1;
            }
            catch
            {

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
        void Send(string s)
        {
            if (s != "")
            {
                Client.Send(Serialize(s));
            }
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
                    if(message == "1")
                    {
                        btnSend.Enabled = true;
                        RemoveQsSend();
                    }
                    else if (message =="0")
                    {
                        btnSend.Enabled = true;
                    }
                }
            }
            catch
            {
                _Close();
            }
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
        #region
        private void btnConnect_Click(object sender, EventArgs e)
        {
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadQuestions_Click(object sender, EventArgs e)
        {
        }
       
        private void btnNext_Click(object sender, EventArgs e)
        {
        }
        #endregion
        private void MC_Load(object sender, EventArgs e)
        {
            txtA.Visible = false;
            txtB.Visible = false;
            txtC.Visible = false;
            txtD.Visible = false;
            txtDA.Visible = false;
            ptrImageMc.Visible = false;
            lbtenmc.Visible = false;
            lbD.Visible = false;
            lbA.Visible = false;
            lbB.Visible = false;
            lbC.Visible = false;
            lbda.Visible = false;
            lbinputda.Visible = false;
            lbinputQs.Visible = false;
            btnNext.Visible = false;
            btnChat.Visible = false;
            btnSend.Visible = false;
            btnLoadQuestions.Visible = false;
            txtQuestion.Visible = false;
            btn02.Visible = false;
            lblDe.Visible = false;
            lblKho.Visible = false;
            //Load Câu hỏi lên List và list02
            LoadQs();

        }
        #region
        private void btnChat_Click(object sender, EventArgs e)
        {
        }

        private void MC_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_Close();
        }

        private void rtbQuestion_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                ptcLogonImage.Image = Bitmap.FromFile(fileName);
            }
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Connect(txtIplogon.Text);
            if (KtConnect == 1 && txtNameLogon.Text != "" && txtIplogon.Text != "")
            {
                lbtenmc.Text = txtNameLogon.Text;
                if (fileName != null)
                {
                    ptrImageMc.Image = Bitmap.FromFile(fileName);
                }
                panel1.Visible = false;
                //
                txtA.Visible = true;
                txtB.Visible = true;
                txtC.Visible = true;
                txtD.Visible = true;
                txtDA.Visible = true;
                ptrImageMc.Visible = true;
                lbtenmc.Visible = true;
                lbD.Visible = true;
                lbA.Visible = true;
                lbB.Visible = true;
                lbC.Visible = true;
                lbda.Visible = true;
                lbinputda.Visible = true;
                lbinputQs.Visible = true;
                btnNext.Visible = true;
                btnChat.Visible = true;
                btnSend.Visible = true;
                btnLoadQuestions.Visible = true;
                txtQuestion.Visible = true;
                btn02.Visible = true;
                //
                lblDe.Visible = true;
               lblKho.Visible = true;
                CheckNumberQS();

            }
            else
            {
                MessageBox.Show("Bạn đã điền thiếu thông tin hoặc sai", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChat_Click_1(object sender, EventArgs e)
        {
            MCChatRoom frm = new MCChatRoom(this);

            frm.Show();
        }
        // Load câu hỏi vào sẵn 2 list
        public void LoadQs()
        {
            // load de
            Questions temp = null;
            string[] lines = File.ReadAllLines("D:\\demo07\\Server\\MC\\TextFile1.txt");
            foreach (string s in lines)
            {
                if (s.StartsWith("@@"))
                {
                    temp = new Questions();
                    temp._TrueAnswer = s.Substring(2);
                }
                if (s.StartsWith("**"))
                {
                    list.Add(temp);
                }
                if (s.StartsWith("A."))
                {
                    temp._ListAnswer._answerA = s.Substring(2);
                }
                if (s.StartsWith("B."))
                {
                    temp._ListAnswer._answerB = s.Substring(2);
                }
                if (s.StartsWith("C."))
                {
                    temp._ListAnswer._answerC = s.Substring(2);
                }
                if (s.StartsWith("D."))
                {
                    temp._ListAnswer._answerD = s.Substring(2);
                }
                if (s.StartsWith("Q."))
                {
                    temp._Question = s.Substring(2);
                }
            }
            // Load kho'
            Questions temp02 = null;
            lines = File.ReadAllLines("D:\\demo07\\Server\\MC\\Muc02.txt");
            foreach (string s in lines)
            {
                if (s.StartsWith("@@"))
                {
                    temp02 = new Questions();
                    temp02._TrueAnswer = s.Substring(2);
                }
                if (s.StartsWith("**"))
                {
                    list02.Add(temp02);
                }
                if (s.StartsWith("A."))
                {
                    temp02._ListAnswer._answerA = s.Substring(2);
                }
                if (s.StartsWith("B."))
                {
                    temp02._ListAnswer._answerB = s.Substring(2);
                }
                if (s.StartsWith("C."))
                {
                    temp02._ListAnswer._answerC = s.Substring(2);
                }
                if (s.StartsWith("D."))
                {
                    temp02._ListAnswer._answerD = s.Substring(2);
                }
                if (s.StartsWith("Q."))
                {
                    temp02._Question = s.Substring(2);
                }
            }
        }
        private void btnNext_Click_1(object sender, EventArgs e)
        {
            if (kt == false)
            {
                if (Value_NextQs < list.Count)
                {
                    txtQuestion.Text = list[Value_NextQs]._Question;
                    txtA.Text = list[Value_NextQs]._ListAnswer._answerA;
                    txtB.Text = list[Value_NextQs]._ListAnswer._answerB;
                    txtC.Text = list[Value_NextQs]._ListAnswer._answerC;
                    txtD.Text = list[Value_NextQs]._ListAnswer._answerD;
                    txtDA.Text = list[Value_NextQs]._TrueAnswer;
                    Value_NextQs++;
                }
                if (Value_NextQs >= list.Count)
                {
                    Value_NextQs = 0;
                }
            }
            else
            {
                if (Value_NextQs < list02.Count)
                {
                    txtQuestion.Text = list02[Value_NextQs]._Question;
                    txtA.Text = list02[Value_NextQs]._ListAnswer._answerA;
                    txtB.Text = list02[Value_NextQs]._ListAnswer._answerB;
                    txtC.Text = list02[Value_NextQs]._ListAnswer._answerC;
                    txtD.Text = list02[Value_NextQs]._ListAnswer._answerD;
                    txtDA.Text = list02[Value_NextQs]._TrueAnswer;
                    Value_NextQs++;
                }
                if (Value_NextQs >= list02.Count)
                {
                    Value_NextQs = 0;
                }
            }
        }

        private void btnLoadQuestions_Click_1(object sender, EventArgs e)
        {
            if (list.Count > 0)
            {
                kt = false;
                Value_NextQs++;
                if (Value_NextQs <= list.Count - 1)
                {
                    txtQuestion.Text = list[Value_NextQs]._Question;
                    txtA.Text = list[Value_NextQs]._ListAnswer._answerA;
                    txtB.Text = list[Value_NextQs]._ListAnswer._answerB;
                    txtC.Text = list[Value_NextQs]._ListAnswer._answerC;
                    txtD.Text = list[Value_NextQs]._ListAnswer._answerD;
                    txtDA.Text = list[Value_NextQs]._TrueAnswer;
                }
                else
                {
                    Value_NextQs = 0;
                    txtQuestion.Text = list[Value_NextQs]._Question;
                    txtA.Text = list[Value_NextQs]._ListAnswer._answerA;
                    txtB.Text = list[Value_NextQs]._ListAnswer._answerB;
                    txtC.Text = list[Value_NextQs]._ListAnswer._answerC;
                    txtD.Text = list[Value_NextQs]._ListAnswer._answerD;
                    txtDA.Text = list[Value_NextQs]._TrueAnswer;
                }
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            kt = true;
            Value_NextQs02++;
            if (Value_NextQs02 <= list02.Count - 1)
            {
                txtQuestion.Text = list02[Value_NextQs02]._Question;
                txtA.Text = list02[Value_NextQs02]._ListAnswer._answerA;
                txtB.Text = list02[Value_NextQs02]._ListAnswer._answerB;
                txtC.Text = list02[Value_NextQs02]._ListAnswer._answerC;
                txtD.Text = list02[Value_NextQs02]._ListAnswer._answerD;
                txtDA.Text = list02[Value_NextQs02]._TrueAnswer;
            }
            else
            {
                Value_NextQs02 = 0;
                txtQuestion.Text = list02[Value_NextQs02]._Question;
                txtA.Text = list02[Value_NextQs02]._ListAnswer._answerA;
                txtB.Text = list02[Value_NextQs02]._ListAnswer._answerB;
                txtC.Text = list02[Value_NextQs02]._ListAnswer._answerC;
                txtD.Text = list02[Value_NextQs02]._ListAnswer._answerD;
                txtDA.Text = list02[Value_NextQs02]._TrueAnswer;
            }
        }
        //Ham xoa cau hoi ra khoi List cau hoi khi da gui
        public void RemoveQsSend()
        {
            // Danh cho cau hoi de
            if (kt == false)
            {
                list.RemoveAt(Value_NextQs);
                if (list.Count != 0)
                {
                    if (Value_NextQs != list.Count - 1)
                    {
                        txtQuestion.Text = list[Value_NextQs]._Question;
                        txtA.Text = list[Value_NextQs]._ListAnswer._answerA;
                        txtB.Text = list[Value_NextQs]._ListAnswer._answerB;
                        txtC.Text = list[Value_NextQs]._ListAnswer._answerC;
                        txtD.Text = list[Value_NextQs]._ListAnswer._answerD;
                        txtDA.Text = list[Value_NextQs]._TrueAnswer;
                    }
                    else
                    {
                        Value_NextQs = 0;
                        txtQuestion.Text = list[Value_NextQs]._Question;
                        txtA.Text = list[Value_NextQs]._ListAnswer._answerA;
                        txtB.Text = list[Value_NextQs]._ListAnswer._answerB;
                        txtC.Text = list[Value_NextQs]._ListAnswer._answerC;
                        txtD.Text = list[Value_NextQs]._ListAnswer._answerD;
                        txtDA.Text = list[Value_NextQs]._TrueAnswer;
                    }
                }
                else
                {
                    txtQuestion.Text = "";
                    txtA.Text = "";
                    txtB.Text = "";
                    txtC.Text = "";
                    txtD.Text = "";
                    txtDA.Text = "";
                    btnLoadQuestions.Enabled = false;
                }
            }
            // Danh cho cau hoi kho
            if (kt == true)
            {
                list02.RemoveAt(Value_NextQs02);
                if (list02.Count != 0)
                {
                    if (Value_NextQs02 != list02.Count - 1)
                    {
                        txtQuestion.Text = list02[Value_NextQs02]._Question;
                        txtA.Text = list02[Value_NextQs02]._ListAnswer._answerA;
                        txtB.Text = list02[Value_NextQs02]._ListAnswer._answerB;
                        txtC.Text = list02[Value_NextQs02]._ListAnswer._answerC;
                        txtD.Text = list02[Value_NextQs02]._ListAnswer._answerD;
                        txtDA.Text = list02[Value_NextQs02]._TrueAnswer;
                    }
                    else
                    {
                        Value_NextQs02 = 0;
                        txtQuestion.Text = list[Value_NextQs02]._Question;
                        txtA.Text = list02[Value_NextQs02]._ListAnswer._answerA;
                        txtB.Text = list02[Value_NextQs02]._ListAnswer._answerB;
                        txtC.Text = list02[Value_NextQs02]._ListAnswer._answerC;
                        txtD.Text = list02[Value_NextQs02]._ListAnswer._answerD;
                        txtDA.Text = list02[Value_NextQs02]._TrueAnswer;
                    }
                }
                else
                {
                    txtQuestion.Text = "";
                    txtA.Text = "";
                    txtB.Text = "";
                    txtC.Text = "";
                    txtD.Text = "";
                    txtDA.Text = "";
                    btn02.Enabled = false;
                }
            }
        }
        // kiểm tra thông tin đày đủ trước khi gửi đi
        public bool CheckBeforeSend()
        {
            if (string.IsNullOrEmpty(txtQuestion.Text) == true)
                return false;
            if (string.IsNullOrEmpty(txtA.Text) == true)
                return false;
            if (string.IsNullOrEmpty(txtB.Text) == true)
                return false;
            if (string.IsNullOrEmpty(txtC.Text) == true)
                return false;
            if (string.IsNullOrEmpty(txtD.Text) == true)
                return false;
            if (string.IsNullOrEmpty(txtDA.Text) == true)
                return false;
            return true;
        }
        private void btnSend_Click_1(object sender, EventArgs e)
        {
            if (CheckBeforeSend() == true)
            {
                IntCount++;
                lbcount.Text = IntCount.ToString();
                string question = txtQuestion.Text;
                string a = txtA.Text;
                string b = txtB.Text;
                string c = txtC.Text;
                string d = txtD.Text;
                string da = txtDA.Text;
                string data = string.Format("{0}" + "@" + "{1}" + "@" + "{2}" + "@" + "{3}" + "@" + "{4}" + "@" + "{5}" + "@" + "{6}", da, question, a, b, c, d, IntCount);
                Send(data);
                //RemoveQsSend();
                CheckNumberQS();
                btnSend.Enabled = false;
            }
            else
            {
                MessageBox.Show("Thiếu thông tin câu hỏi !", "Lỗi");
            }
        }
        // Số câu hỏi còn trong danh sách
        public void CheckNumberQS()
        {
            lblDe.Text = list.Count.ToString();
            lblKho.Text = list02.Count.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LiveStream frm = new LiveStream();
            frm.Show();
        }
    }

}
