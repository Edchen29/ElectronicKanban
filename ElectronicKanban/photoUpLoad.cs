using System;
using System.IO;
using System.Windows.Forms;

namespace ElectronicKanban
{
    public partial class photoUpLoad : Form
    {
        public MainForm _mainForm;

        public photoUpLoad()
        {
            InitializeComponent();
           
        }
        public photoUpLoad(MainForm mainForm)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            _mainForm = mainForm;
        }
        private void btn2_Click(object sender, EventArgs e)
        {
            //创建一个对话框对象
            OpenFileDialog ofd = new OpenFileDialog();
            //为对话框设置标题
            ofd.Title = "请选择上传的图片";
            //设置筛选的图片格式
            ofd.Filter = "*.jpg|*.jpg|*.png|*.png|*.bmp|*.bmp|*.tiff|*.tiff";
            //设置是否允许多选
            ofd.Multiselect = false;
            //如果你点了“确定”按钮
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得文件的完整路径（包括名字后后缀）
                string filePath = ofd.FileName;

                //找到文件名比如“1.jpg”前面的那个“\”的位置
                int position = filePath.LastIndexOf("\\");
                //从完整路径中截取出来文件名“1.jpg”
                string fileName = filePath.Substring(position + 1);
                //读取选择的文件，返回一个流
                using (Stream stream = ofd.OpenFile())
                {
                    //创建一个流，用来写入得到的文件流（注意：创建一个名为“Images”的文件夹，如果是用相对路径，必须在这个程序的Degug目录下创建
                    //如果是绝对路径，放在那里都行，我用的是相对路径）
                    using (FileStream fs = new FileStream(@"./Images/" + fileName, FileMode.CreateNew))
                    {
                        //将得到的文件流复制到写入流中
                        stream.CopyTo(fs);
                        //将写入流中的数据写入到文件中
                        fs.Flush();
                    }
                    //PictrueBOx 显示该图片，此时这个图片已经被复制了一份在Images文件夹下，就相当于上传
                    //至于上传到别的地方你再更改思路就行，这里只是演示过程
                    pbShow.ImageLocation = @"./Images/" + fileName;
                    MessageBox.Show("上传成功,照片已保存至程序目录bin/Debug/Images文件夹中!");
                    //return;
                   
                    
                }
            }
        }
        int count = 0;
        #region 切割工位照片选择
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
           
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                pbShow.ImageLocation = openfile.FileName;
                count = 1;
            }
            openfile.Dispose();
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openfile = new OpenFileDialog();

            //if (openfile.ShowDialog() == DialogResult.OK)
            //{
            //    pbShow.ImageLocation = openfile.FileName;
            //    count = 2;
            //}
            //openfile.Dispose();
        }


        #region 端口工位照片选择
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                pbShow.ImageLocation = openfile.FileName;
                count = 2;
            }
            openfile.Dispose();
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            _mainForm.GetImages(pbShow,count);
            this.Close();
        }
    }
}
