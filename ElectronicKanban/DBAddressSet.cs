using ElectronicKanban.Properties;
using System;
using System.Windows.Forms;

namespace ElectronicKanban
{
    public partial class DBAddressSet : Form
    {
        public string db1, db2, db3, db4, db5, db6, db7, db8, db9;
        public int conut = 0;
        public DBAddressSet()
        {
            InitializeComponent();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
