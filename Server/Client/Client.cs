using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Client
{
    public partial class Client : Form
    {   //khơi tạo biến gửi đi kết quả đúng or sai
        string _Yes = "1";
        string _NO = "0";
        // khởi tọa biến kiểm tra câu hỏi thứ mấy
        int Intcount = 0;
        //khởi tạo đỉa chỉ tên
        string fileName;
        // khởi tạo biến kt kết nối
        public int KtConnect = 0;
        // khởi tạo chứa kết quả đáp án
        public string DA = null;
        // khởi tạo biến kiểm tra thời gian
        public bool Time = false;
        //khởi tọa biến kiểm tra thời gian của trợ giúp 50/50
        public bool b50 = true;
        // khởi tạo biến kiểm tra thời gian của trợ giúp gọi điện thoại cho người thân
        public bool bgoi = true;
        // khởi tọa biến kiểm tra thời gian của trợ  giúp hỏi ý kiến khán giả trường quay
        public bool bkhangia = true;
        //
        public Client()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            SoundPlayer chaomung = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\nhacnen\begin.wav");
            chaomung.Play();
            tmThoiGianTraLoi.Start();
            EnabledFalsebtn();
        }
        // khởi tọa socket client
        IPEndPoint IP;
        Socket client;
        // khoi tao socket video
        Socket sendsocket;
        IPEndPoint ipendpiont;
        
        private void trreadimage()
        {
            int dataSize;
            string imageName = "Image-" + System.DateTime.Now.Ticks + ".JPG";
            try
            {

                dataSize = 0;
                byte[] b = new byte[1024 * 10000];  //Picture of great
                dataSize = sendsocket.Receive(b);
                if (dataSize > 0)
                {
                    MemoryStream ms = new MemoryStream(b);
                    Image img = Image.FromStream(ms);
                    img.Save(imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    videoBox.Image = img;
                    ms.Close();
                }

            }
            catch (Exception ee)
            {
            }
            System.Threading.Thread.Sleep(500);
            trreadimage();
        }
        //
        // khởi tạo biến kt đăng nhập thành công hay không
        /// <summary>
        /// kết nối tới sever
        /// </summary>
        void Connect(string s)
        {
            // IP là đỉa chỉ của server
            try
            {
                IP = new IPEndPoint(IPAddress.Parse(s), 5000);
            }
            catch
            {
            }
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
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
            client.Close();
        }
        /// <summary>
        /// Gửi tin
        /// </summary>
        void Send(string s)
        {
            if (s != "")
            {
                client.Send(Serialize(s));
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
                    btnA.Visible = true;
                    btnB.Visible = true;
                    btnC.Visible = true;
                    btnD.Visible = true;
                    byte[] message = new byte[1024 * 5000];
                    client.Receive(message);
                    string data = (string)Deserialize(message);
                    DA = data.Substring(0, data.IndexOf("@"));

                    data = data.Remove(0, data.IndexOf("@") + 1);
                    lblQuestion.Text = data.Substring(0, data.IndexOf("@"));


                    data = data.Remove(0, data.IndexOf("@") + 1);
                    btnA.Text = data.Substring(0, data.IndexOf("@"));

                    data = data.Remove(0, data.IndexOf("@") + 1);
                    btnB.Text = data.Substring(0, data.IndexOf("@"));

                    data = data.Remove(0, data.IndexOf("@") + 1);
                    btnC.Text = data.Substring(0, data.IndexOf("@"));

                    data = data.Remove(0, data.IndexOf("@") + 1);
                    btnD.Text = data.Substring(0, data.IndexOf("@"));
                    data = data.Remove(0, data.IndexOf("@") + 1);
                    Intcount = int.Parse(data);
                    scores(Intcount);
                    BackColorbtnScores(Intcount);
                    EnabledTruebtn();
                    ResetColorbtn();
                    Time = true;
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



        //
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
            //Connect();
        }

        public int STEP = 1000;
        public int TimeDown = 10000;
        public int Interval = 1000;
        #region
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            _Close();
        }

        private void lblQuestion_Click(object sender, EventArgs e)
        {
        }
        #endregion
        SoundPlayer NhacChon = null;
        int DemTG = 0;
        string chon = "";
        
        /// <summary>
        /// tắt thao tác trên các btn
        /// </summary>
        private void EnabledFalsebtn()
        {
            btnA.Enabled = false;
            btnB.Enabled = false;
            btnC.Enabled = false;
            btnD.Enabled = false;
        }
        /// <summary>
        /// mở thao tác trên các btn
        /// </summary>
        private void EnabledTruebtn()
        {
            btnA.Enabled = true;
            btnB.Enabled = true;
            btnC.Enabled = true;
            btnD.Enabled = true;
        }
        /// <summary>
        /// trả về màu mặc định của btn các đáp án
        /// </summary>
        private void ResetColorbtn()
        {
            btnA.ForeColor = Color.PaleGreen;
            btnB.ForeColor = Color.PaleGreen;
            btnC.ForeColor = Color.PaleGreen;
            btnD.ForeColor = Color.PaleGreen;
        }
            

        private void btnA_Click(object sender, EventArgs e)
        {
            DialogResult _result = MessageBox.Show("Bạn có chắc chắn chọn đáp án A ?", "Thông Báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (_result == DialogResult.OK)
            {
                NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\chon\a.wav");
                NhacChon.Play();
                chon = "A";
                tmDemTG.Start();
                EnabledFalsebtn();
                btnA.ForeColor = Color.Red;
            }
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            DialogResult _result = MessageBox.Show("Bạn có chắc chắn chọn đáp án B ?", "Thông Báo",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (_result == DialogResult.OK)
            {
                NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\chon\b.wav");
                NhacChon.Play();
                chon = "B";
                tmDemTG.Start();
                EnabledFalsebtn();
                btnB.ForeColor = Color.Red;
            }
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            DialogResult _result = MessageBox.Show("Bạn có chắc chắn chọn đáp án C ?", "Thông Báo",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (_result == DialogResult.OK)
            {
                NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\chon\c.wav");
                NhacChon.Play();
                chon = "C";
                tmDemTG.Start();
                EnabledFalsebtn();
                btnC.ForeColor = Color.Red;
            }
        }

        private void btnD_Click(object sender, EventArgs e)
        {
            DialogResult _result = MessageBox.Show("Bạn có chắc chắn chọn đáp án D ?", "Thông Báo",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (_result == DialogResult.OK)
            {
                NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\chon\d.wav");
                NhacChon.Play();
                chon = "D";
                tmDemTG.Start();
                EnabledFalsebtn();
                btnD.ForeColor = Color.Red;
            }
        }
        //khởi tạo số lượng đáp án đúng;
        static int SLDAD = 0;
        void CheckDA()
        {
            if (chon == DA)
            {
                NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\dap\dung.wav");
                NhacChon.Play();
                chon = "";
                Send(_Yes);
                if (chon == "A")
                {
                    btnA.ForeColor = Color.Blue;
                }else if(chon == "B")
                {
                    btnB.ForeColor = Color.Blue;
                }else if(chon == "C")
                {
                    btnC.ForeColor = Color.Blue;
                }else if(chon == "D")
                {
                    btnD.ForeColor = Color.Blue;
                }
                SLDAD++;
                
            }
            else
            {
                if (DA == "A")
                {
                    NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\dap\asai.wav");
                    btnA.ForeColor = Color.Blue;
                }

                else
                {
                    if (DA == "B")
                    {
                        NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\dap\bsai.wav");
                        btnB.ForeColor = Color.Blue;
                    }
                    else
                    {
                        if (DA == "C")
                        {
                            NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\dap\csai.wav");
                            btnC.ForeColor = Color.Blue;
                        }
                        else
                        {
                            NhacChon = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\dap\dsai.wav");
                            btnD.ForeColor = Color.Blue;
                            this.Opacity = 10;
                        }
                    }
                }
                chon = "";
                Send(_NO);
                NhacChon.Play();

                /*SAi  lam o day*/
                DialogResult _KQ = MessageBox.Show("Bạn Đã Thua Cuộc, Kết thúc lượt chơi tại đây, hẹn gặp bạn ở số tiếp theo!!!!", "Thong bao",MessageBoxButtons.OK);
                if(_KQ == DialogResult.OK)
                {
                    Close();
                    _Close();
                }
            }

            Time = false;
            lblTG.Text = "60";
        }



        private void btnChat_Click(object sender, EventArgs e)
        {
            ChatRoom frm = new ChatRoom(this);
            frm.result(txtIp.Text);
            frm.Show();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            lblTG.Visible = false;
            gbHelp.Visible = false;
            gbHoiYKien.Visible = false;
            gbScore.Visible = false;
            groupBox1.Visible = false;
            btnChat.Visible = false;
            videoBox.Visible = false;
            gbGoiDien.Visible = false;
            gbKieuPhong.Visible = false;
            gbHuTruc.Visible = false;
            gbDoanDu.Visible = false;
            gbDungchoi.Visible = false;
        }

        /// <summary>
        /// Hiển thị Điểm trên BtnDiem
        /// </summary>
        /// <param name="cau"></param>
        public void scores(int cau)
        {
            switch (cau)
            {
                case 1: btnDiem.Text = "100.000"; break;
                case 2: btnDiem.Text = "200.000"; break;
                case 3: btnDiem.Text = "300.000"; break;
                case 4: btnDiem.Text = "500.000"; break;
                case 5: btnDiem.Text = "1.000.000"; break;
                case 6: btnDiem.Text = "2.000.000"; break;
                case 7: btnDiem.Text = "3.600.000"; break;
                case 8: btnDiem.Text = "6.000.000"; break;
                case 9: btnDiem.Text = "9.000.000"; break;
                case 10: btnDiem.Text = "15.000.000"; break;
                case 11: btnDiem.Text = "25.000.000"; break;
                case 12: btnDiem.Text = "35.000.000"; break;
                case 13: btnDiem.Text = "50.000.000"; break;
                case 14: btnDiem.Text = "80.000.000"; break;
                case 15: btnDiem.Text = "120.000.000"; break;
            }
        }

        /// <summary>
        /// thay dổi màu nền bảng hiện thỉ điểm
        /// </summary>
        /// <param name="cau"></param>
        public void BackColorbtnScores(int cau)
        {
            switch (cau)
            {
                case 1:
                    btn1.BackColor = Color.Yellow;
                    break;
                case 2:
                    btn2.BackColor = Color.Yellow;
                    btn1.BackColor = Color.Gainsboro;
                    break;
                case 3:
                    btn3.BackColor = Color.Yellow;
                    btn2.BackColor = Color.Gainsboro;

                    break;
                case 4:
                    btn4.BackColor = Color.Yellow;
                    btn3.BackColor = Color.Gainsboro;

                    break;
                case 5:
                    btn5.BackColor = Color.Yellow;
                    btn4.BackColor = Color.Gainsboro;

                    break;
                case 6:
                    btn6.BackColor = Color.Yellow;
                    btn5.BackColor = Color.Gainsboro;

                    break;
                case 7:
                    btn7.BackColor = Color.Yellow;
                    btn6.BackColor = Color.Gainsboro;

                    break;
                case 8:
                    btn8.BackColor = Color.Yellow;
                    btn7.BackColor = Color.Gainsboro;

                    break;
                case 9:
                    btn9.BackColor = Color.Yellow;
                    btn8.BackColor = Color.Gainsboro;

                    break;
                case 10:
                    btn10.BackColor = Color.Yellow;
                    btn9.BackColor = Color.Gainsboro;

                    break;
                case 11:
                    btn11.BackColor = Color.Yellow;
                    btn10.BackColor = Color.Gainsboro;

                    break;
                case 12:
                    btn12.BackColor = Color.Yellow;
                    btn11.BackColor = Color.Gainsboro;

                    break;
                case 13:
                    btn13.BackColor = Color.Yellow;
                    btn12.BackColor = Color.Gainsboro;

                    break;
                case 14:
                    btn14.BackColor = Color.Yellow;
                    btn13.BackColor = Color.Gainsboro;
                    break;
                case 15:
                    btn15.BackColor = Color.Yellow;
                    btn14.BackColor = Color.Gainsboro;
                    break;
            }
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void ptrProfile_Click(object sender, EventArgs e)
        {

        }

        private void btnlogon_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Connect(txtIp.Text);
            if (KtConnect == 1 && txtHovaTen.Text != "" && txtIp.Text != "")
            {
                lbName.Text = txtHovaTen.Text;
                if (fileName != null)
                {
                    ptrProfile.Image = Bitmap.FromFile(fileName);
                }
                panel1.Visible = false;
                gbHelp.Visible = true;
                gbScore.Visible = true;
                groupBox1.Visible = true;
                btnChat.Visible = true;
                videoBox.Visible = true;
                lblTG.Visible = true;
            }
            else
            {
                MessageBox.Show("Bạn đã điền thiếu thông tin hoặc sai", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnchosseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                ptImage.Image = Bitmap.FromFile(fileName);
            }
        }

        private void tmDemTg_Tick(object sender, EventArgs e)
        {
            if (DemTG >= 4)
            {
                tmDemTG.Stop();
                DemTG = 0;
                CheckDA();
            }
            else DemTG++;
        }

        private void tmThoiGianTraLoi_Tick(object sender, EventArgs e)
        {
            if (Time == true)
            {
                if (int.Parse(lblTG.Text) <= 0)
                {
                    lblTG.Text = "60";
                    Time = false;
                    CheckDA();
                }
                else
                {
                    lblTG.Text = (int.Parse(lblTG.Text) - 1).ToString();
                }
            }
        }
        

        private void btnKhanGia_Click(object sender, EventArgs e)
        {
            btnKhanGia.Enabled = false;
            bkhangia = true;
            btnKhanGia.BackColor = Color.SlateGray;
            gbHoiYKien.Visible = true;
            tmThoiGianTraLoi.Stop();
            tmKhanGia.Start();
            tmRangBuoc.Stop();
            btnKhanGia.Visible = false;
           
        }

        private void btn50_Click(object sender, EventArgs e)
        {
            b50 = true;
            btn50.Enabled = false;
            btn50.BackColor = Color.SlateGray;
            SoundPlayer nam = new SoundPlayer(@Application.StartupPath + @"\resource\Sounds\trogiup\50\trogiup50.wav");

            nam.Play();
            tm50.Start();
            tmThoiGianTraLoi.Stop();
            btn50.Visible = false;
        }

        private void btnGoiDien_Click(object sender, EventArgs e)
        {
            btnGoiDien.Enabled = false;
            bgoi = true;
            btnGoiDien.BackColor = Color.SlateGray;
            tmThoiGianTraLoi.Stop();
            gbGoiDien.Visible = true;
            btnGoiDien.Visible = false;
        }
        int B = 5;
        private void tm50_Tick(object sender, EventArgs e)
        {
            if (B == 0)
            {
                if (DA == "A")
                {
                    btnB.Visible = false;
                    btnC.Visible = false;

                }

                if (DA == "B")
                {
                    btnA.Visible = false;
                    btnD.Visible = false;

                }

                if (DA == "C")
                {
                    btnA.Visible = false;
                    btnD.Visible = false;
                }

                if (DA == "D")
                {
                    btnB.Visible = false;
                    btnA.Visible = false;
                }

                tm50.Stop();
                tmThoiGianTraLoi.Start();
            }
            B--;
        }
        int c = 37;
        private void tmGoiDienThoai_Tick(object sender, EventArgs e)
        {
            if(c==37)
            {
                SoundPlayer nam = new SoundPlayer(Application.StartupPath + @"\resource\Sounds\trogiup\nt\danhcho30s.wav");
                nam.Play();
            }
            if(c==32)
            {
                tmCuaTao.Start();
            }
            if (c == 30)
            {
                if(gbKieuPhong.Visible==true)
                {
                    string s = "Câu hỏi này có vẻ khó với Kiều Phong Ta quá,Đợi ta suy nghĩ 1 tí nha";
                    txtkieuphongtraloi.Text = s;
                }
                if (gbHuTruc.Visible ==true)
                {
                    string s = "Câu hỏi này có vẻ khó với Hư Trúc Ta quá,Đợi ta suy nghĩ 1 tí nha";
                    txthutructraloi.Text = s;
                }
                if (gbDoanDu.Visible == true)
                {
                    string s = "Câu hỏi này có vẻ khó với Đoàn dự ta quá à,,Đợi ta suy nghĩ 1 tí nha";
                    txtdoandutraloi.Text = s;
                }
            }
            if (c == 20)
            {
                if (gbKieuPhong.Visible == true)
                {
                    string s = "Câu này ta biết nhưng lâu quá ta cũng không nắm rõ nữa ngươi đợi ta tí nha";
                    txtkieuphongtraloi.Text = s;
                }
                if (gbHuTruc.Visible == true)
                {
                    string s = "Câu này ta biết nhưng lâu quá ta cũng không nắm rõ nữa ngươi đợi ta tí nha";
                    txthutructraloi.Text = s;
                }
                if (gbDoanDu.Visible == true)
                {
                    string s =  "Câu này ta biết nhưng lâu quá ta cũng không nắm rõ nữa ngươi đợi ta tí nha";
                    txtdoandutraloi.Text = s;
                }
            }
            if(c==10)
            {
                if (gbKieuPhong.Visible == true)
                {
                    string s = "Ta nhớ ra rồi đáp án cuối cùng của ta là: ";
                    txtkieuphongtraloi.Text = s;
                }
                if (gbHuTruc.Visible == true)
                {
                    string s = "Ta nhớ ra rồi đáp án cuối cùng của ta là: ";
                    txthutructraloi.Text = s;
                }
                if (gbDoanDu.Visible == true)
                {
                    string s = "Ta nhớ ra rồi đáp án cuối cùng của ta là: ";
                    txtdoandutraloi.Text = s;
                }
            }
            if(c==4)
            {
                if (gbKieuPhong.Visible == true)
                {
                    if (DA == "A")
                    {
                        string s = "A";
                        txthutructraloi.Text = s;
                    }
                    if(DA == "B")
                    {
                        string s = "B";
                        txthutructraloi.Text = s;
                    }
                    if (DA == "C")
                    {
                        string s = "C";
                        txthutructraloi.Text = s;
                    }
                    if (DA == "D")
                    {
                        string s = "D";
                        txthutructraloi.Text = s;
                    }
                }
                if (gbHuTruc.Visible == true)
                {
                    if (DA == "A")
                    {
                        txthutructraloi.Text = "A";
                    }
                    if (DA == "B")
                    {
                        txthutructraloi.Text = "B";
                    }
                    if (DA == "C")
                    {
                        txthutructraloi.Text = "C";
                    }
                    if (DA == "D")
                    {
                        txthutructraloi.Text = "D";
                    }
                }
                if (gbDoanDu.Visible == true)
                {
                    if (DA == "A")
                    {
                        txtdoandutraloi.Text = "A";
                    }
                    if (DA == "B")
                    {
                        txtdoandutraloi.Text = "B";
                    }
                    if (DA == "C")
                    {
                        txtdoandutraloi.Text = "C";
                    }
                    if (DA == "D")
                    {
                        txtdoandutraloi.Text = "D";
                    }
                }
            }
            if(c==0)
            {
                tmGoiDienThoai.Stop();
                tmThoiGianTraLoi.Start();
            }
            c--;
        }

        private void btnHoiYKien_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            if (btnA.Visible = true && btnB.Visible == true && btnC.Visible == true && btnD.Visible == true)
            {
                int A = rd.Next(1, 100);
                int B = rd.Next(1, 100 - A);
                int C = rd.Next(1, 100 - A - B);
                int D = 100 - A - B - C;
                this.chart1.Series["Age"].Points.AddXY("A", A);
                this.chart1.Series["Age"].Points.AddXY("B", B);
                this.chart1.Series["Age"].Points.AddXY("C", C);
                this.chart1.Series["Age"].Points.AddXY("D", D);
            }
            else
            {
                if (DA == "A")
                {
                    int a = rd.Next(1, 100);
                    int d = 100 - a;
                    this.chart1.Series["Age"].Points.AddXY("A", a);
                    this.chart1.Series["Age"].Points.AddXY("D", d);
                }
                if (DA == "B")
                {
                    int b = rd.Next(1, 100);
                    int c = 100 - b;
                    this.chart1.Series["Age"].Points.AddXY("B", b);
                    this.chart1.Series["Age"].Points.AddXY("C", c);
                }
                if (DA == "C")
                {
                    int c = rd.Next(1, 100);
                    int b = 100 - c;
                    this.chart1.Series["Age"].Points.AddXY("C", c);
                    this.chart1.Series["Age"].Points.AddXY("B", b);
                }
                if (DA == "D")
                {
                    int d = rd.Next(1, 100);
                    int c = 100 - d;
                    this.chart1.Series["Age"].Points.AddXY("D", d);
                    this.chart1.Series["Age"].Points.AddXY("C", c);
                }
            }
        }

        private void btnXemxong_Click(object sender, EventArgs e)
        {
            gbHoiYKien.Visible = false;
            tmThoiGianTraLoi.Start();
        }

        private void btnGoiKieuPhong_Click(object sender, EventArgs e)
        {
            tmGoiDienThoai.Start();
            gbGoiDien.Visible = false;
            gbKieuPhong.Visible = true;
            tmThoiGianTraLoi.Stop();
        }


        private void btncamonkieuphong_Click(object sender, EventArgs e)
        {
            gbKieuPhong.Visible = false;
            tmThoiGianTraLoi.Start();
        }

        private void btnGoiHuTruc_Click(object sender, EventArgs e)
        {
            tmGoiDienThoai.Start();
            gbGoiDien.Visible = false;
            gbHuTruc.Visible = true;
            tmThoiGianTraLoi.Stop();
        }

        private void btnGoiDoanDu_Click(object sender, EventArgs e)
        {
            tmGoiDienThoai.Start();
            gbGoiDien.Visible = false;
            gbDoanDu.Visible = true;
            tmThoiGianTraLoi.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gbHuTruc.Visible = false;
            tmThoiGianTraLoi.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gbDoanDu.Visible = false;
            tmThoiGianTraLoi.Start();
        }

        private void btnHuongDan_Click(object sender, EventArgs e)
        {
            Instruction a = new Instruction();
            a.Show();
        }
        int CuaTao = 29;
        private void tmCuaTao_Tick(object sender, EventArgs e)
        {
            if(CuaTao<0)
            {
                tmCuaTao.Enabled = false;
            }
            else
            {
                if (gbKieuPhong.Enabled == true)
                    lblKieuPhong.Text = CuaTao.ToString();
                if (gbDoanDu.Enabled == true)
                    lblDoanDu.Text = CuaTao.ToString();
                if (gbHuTruc.Enabled == true)
                    lblHuTruc.Text = CuaTao.ToString();
            }
            CuaTao--;
        }

        private void btnDungchoi_Click(object sender, EventArgs e)
        {
            gbDungchoi.Visible = true;
            tmThoiGianTraLoi.Stop();
            switch (SLDAD)
            {
                case 1: btnTien.Text = "100.000"; break;
                case 2: btnTien.Text = "200.000"; break;
                case 3: btnTien.Text = "300.000"; break;
                case 4: btnTien.Text = "500.000"; break;
                case 5: btnTien.Text = "1.000.000"; break;
                case 6: btnTien.Text = "2.000.000"; break;
                case 7: btnTien.Text = "3.600.000"; break;
                case 8: btnTien.Text = "6.000.000"; break;
                case 9: btnTien.Text = "9.000.000"; break;
                case 10: btnTien.Text = "15.000.000"; break;
                case 11: btnTien.Text = "25.000.000"; break;
                case 12: btnTien.Text = "35.000.000"; break;
                case 13: btnTien.Text = "50.000.000"; break;
                case 14: btnTien.Text = "80.000.000"; break;
                case 15: btnTien.Text = "120.000.000"; break;
            }

        }

        private void btnTiepTuc_Click(object sender, EventArgs e)
        {
            gbDungchoi.Visible = false;
            tmThoiGianTraLoi.Start();
        }

        private void btnDung_Click(object sender, EventArgs e)
        {
            DialogResult _result = MessageBox.Show("Bạn đã dừng cuộc chơi tại đây nhấn OK để thoát", "Thông báo dừng chơi",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (_result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void goLive_Click(object sender, EventArgs e)
        {
            sendsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipendpiont = new IPEndPoint(IPAddress.Parse(txtIp.Text), 10001);
            sendsocket.Connect(ipendpiont);
            Thread th = new Thread(new ThreadStart(trreadimage));
            th.IsBackground = true;
            th.Start();
        }
    }
}