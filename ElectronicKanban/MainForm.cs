using CCWin.SkinClass;
using CCWin.SkinControl;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicKanban
{
    public partial class MainForm : Form  //Skin_VS
    {
        private float X;//当前窗体的宽度
        private float Y;//当前窗体的高度

        #region 构建plc结构体

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class DataforView
        {
            public short iCurrentCutTimeHH; //(*本次坡口时间H*)
            public short iCurrentCutTimeMM; //(*本次坡口时间M*)
            public short iCurrentCutTimeSS;//(*本次坡口时间S*)
            public short iCurrentStartTimeHH;//(*本次开机时间H*)
            public short iCurrentStartTimeMM;//(*本次开机时间M*)
            public short iCurrentStartTimeSS;//(*本次开机时间S*)
            public short iTotalCutTimeHH;//(*累计坡口时间H*)
            public short iTotalCutTimeMM;//(*累计坡口时间M*)
            public short iTotalCutTimeSS;//(*累计坡口时间S*)
            public short iTotalStartTimeHH;//(*累计开机时间H*)
            public short iTotalStartTimeMM;//(*累计开机时间M*)
            public short iTotalStartTimeSS;//(*累计开机时间S*)
            public short iTotalCutTimes;//(*累计坡口次数*)
            public short iThisDayCutTimes;//(*当天坡口次数*)
            public short iTotalStartTimes;//(*累计启动次数*)
            public short iThisDayStartTimes;//(*当天启动次数*)
            public int udiSysTotalDIDsp;//(*累计产能*)
        }

        #endregion 构建plc结构体

        private readonly string NetId;
        private readonly int port;
        private TwinCAT.Ads.TcAdsClient adsClient = null;
        private int HandleVar_DataforView = 0;

        #region 窗体初始化

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            // 设置背景图片自动适应
            this.skinPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            SetChart();
            //AdsInit();
            //Task.Run(() => StartTh());
        }

        public MainForm(LoginIn _loginIn)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            // 设置背景图片自动适应
            this.skinPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            NetId = _loginIn.txt1.Text;
            port = _loginIn.txt2.Text.ToInt32();
            SetChart();
            AdsInit();

            Task.Run(() => StartTh());
        }

        private void 上传照片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            photoUpLoad photoUpLoad = new photoUpLoad(this);
            photoUpLoad.Show();
        }

        #endregion 窗体初始化

        #region 获取照片路径

        public void GetImages(SkinPictureBox skinPictureBox, int count)
        {
            if (count == 1)//切割工位
            {
                skinPictureBox4.BackgroundImage = null;
                this.skinPictureBox4.ImageLocation = skinPictureBox.ImageLocation;
            }
            if (count == 2)//端口工位
            {
                skinPictureBox5.BackgroundImage = null;
                this.skinPictureBox5.ImageLocation = skinPictureBox.ImageLocation;
            }
        }

        #endregion 获取照片路径

        #region chart设置

        private void SetChart()
        {
            try
            {
                chartDemo.ChartAreas[0].BackColor = Color.FromArgb(30, Color.LightBlue);
                chartDemo.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Yellow;//设置刻度的颜色
                chartDemo.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Yellow;
                chartDemo.Series[0].Label = "#VAL";//显示每个柱状图的数值
                object[] dates = new object[15];
                for (int i = 0; i < 15; i++)
                {
                    dates[i] = DateTime.Now.AddDays(-i - 1).ToString("MM-dd");//取当前日期的前15天
                }
                double[] yval = { 5, 6, 4, 6, 3, 12, 5, 6, 7, 8, 9, 3, 13, 18, 56 };

                chartDemo.Series[0].Points.DataBindXY(dates, yval);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion chart设置

        #region Load事件

        private void MainForm_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            AutoSetControlSize.setTag(this);
            timer1.Start();

            #region 全屏

            this.ShowInTaskbar = false;
            //this.TopMost = false;
            this.WindowState = FormWindowState.Maximized;

            #endregion 全屏
        }

        #endregion Load事件

        #region 根据窗体大小调整控件大小

        private void MainForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X; //窗体宽度缩放比例
            float newy = (this.Height) / Y;//窗体高度缩放比例
            AutoSetControlSize.setControls(newx, newy, this);//随窗体改变控件大小
        }

        #endregion 根据窗体大小调整控件大小

        #region 关闭窗体

        private void 关闭程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        #endregion 关闭窗体

        #region 时间显示

        private void timer1_Tick(object sender, EventArgs e)
        {
            skinLabel5.Text = System.DateTime.Now.ToString();
        }

        #endregion 时间显示

        //private void GetDB()
        //{
        //    try
        //    {
        //        #region 注释掉
        //        //Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db1), lbl1));
        //        //Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db2), lbl2));
        //        // Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db3), lbl3));
        //        //Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db4), lbl4));
        //        //Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db5), lbl5));
        //        //Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db6), lbl6));
        //        //Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db7), lbl7));
        //        //Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db8), lbl8));
        //        // Task.Run(() => ReadResultRender(mainsiemensTcpNet.ReadInt32(db9), lbl9));
        //        #endregion

        //        Task.Run(() => Read());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        #region 开启多线程

        private void StartTh()
        {
            while (true)
            {
                Thread th = new Thread(Read);
                th.IsBackground = true;
                th.Start();
                Thread.Sleep(1000);
            }
        }

        #endregion 开启多线程

        private void AdsInit()
        {
            try
            {
                adsClient = new TwinCAT.Ads.TcAdsClient();
                adsClient.Connect(NetId, port);
                // Add Variables  此处为结构体
                HandleVar_DataforView = adsClient.CreateVariableHandle(".DataforView");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
            }
        }

        private void Read()
        {
            DataforView dfv = adsClient.ReadAny(HandleVar_DataforView, typeof(DataforView)) as DataforView;
            lbl4.Text = Convert.ToString(dfv.iThisDayCutTimes);//当天坡口次数
            lbl7.Text = Convert.ToString(dfv.udiSysTotalDIDsp);//坡口累计产能
            lbl1.Text = Convert.ToString(dfv.udiSysTotalDIDsp);//坡口工位稼动率

            lbl5.Text = Convert.ToString(dfv.iThisDayCutTimes);//当天切割次数
            lbl8.Text = Convert.ToString(dfv.udiSysTotalDIDsp);//切割累计产能
            lbl2.Text = Convert.ToString(dfv.udiSysTotalDIDsp);//切割工位稼动率
        }
    }
}
