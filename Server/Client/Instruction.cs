using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Instruction : Form
    {
        public Instruction()
        {
            InitializeComponent();
           
        }
        //private Logon mainFrom = null;
        //public Instruction(Form callingForm)
        //{
        //    mainFrom = callingForm as Logon;
        //    InitializeComponent();

        //}
        private void Instruction_Load(object sender, EventArgs e)
        {
            HuongDan();
        }
        public void HuongDan()
        {
            string[] lines = File.ReadAllLines("E:\\Lập Trình Windows\\demo04\\Server\\Client\\HuongDan.txt");
            foreach(string s in lines)
            {
                lvHuongDan.Items.Add(s);
            }

        }
    }
}
