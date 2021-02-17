using System;
using System.Windows.Forms;

namespace ElectronicKanban
{
    public partial class LoginIn : Form
    {
     
        public int count = 0;

        public LoginIn()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 连接plc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm(this);
            mainForm.Show();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            //子窗体向父窗体传值
            DBAddressSet dBAddressSet = new DBAddressSet();
            dBAddressSet.ShowDialog(this);
        }
    }
}
