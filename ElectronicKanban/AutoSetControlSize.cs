using System;
using System.Drawing;
using System.Windows.Forms;

namespace ElectronicKanban
{
    internal class AutoSetControlSize
    {
        #region 根据窗体大小调整控件大小

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        public static void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }

        //根据窗体大小调整控件大小
        public static void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                try
                {
                    if (con.Tag != null)
                    {
                        string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                        float a = System.Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                        con.Width = (int)a;//宽度
                        a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                        con.Height = (int)(a);
                        a = System.Convert.ToSingle(mytag[2]) * newx;//左边距离
                        con.Left = (int)(a);
                        a = System.Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                        con.Top = (int)(a);
                        Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                        con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                        if (con.Controls.Count > 0)
                        {
                            setControls(newx, newy, con);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //private void MainForm_Resize(object sender, EventArgs e)
        //{
        //    float newx = (this.Width) / X; //窗体宽度缩放比例
        //    float newy = (this.Height) / Y;//窗体高度缩放比例
        //    setControls(newx, newy, this);//随窗体改变控件大小
        //}

        #endregion 根据窗体大小调整控件大小
    }
}
