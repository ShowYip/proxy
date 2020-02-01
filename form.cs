using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        


        public Form1()
        {
            InitializeComponent();
        }


        [DllImport(@"wininet",
        SetLastError = true,
        CharSet = CharSet.Auto,
        EntryPoint = "InternetSetOption",
        CallingConvention = CallingConvention.StdCall)]
        public static extern bool InternetSetOption
        (
            int hInternet,
            int dmOption,
            IntPtr lpBuffer,
            int dwBufferLength
        );

        private void button1_Click(object sender, EventArgs e)
        {
            //cmdkey /add:ip /user:用户名 /pass:密码

            string A = "cmdkey /add:" + comboBox1.Text + " /user:" + textBox2.Text + " /pass:" + textBox1.Text;
            //string A = @"start c:\";
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(A);

            //StreamWriter sw = new StreamWriter(@"./url.ini", true, Encoding.ASCII);
            //sw.Write(Environment.NewLine + comboBox1.Text);
            //sw.Flush();
            //sw.Close();

            timer1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button4.PerformClick();
            timer1.Enabled = true;
            textBox2.Text = "itccts_proxy";
            comboBox1.Text = textBox3.Text;
            button6.PerformClick();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();

            string ReadLine;
            string[] array;
            string Path = @"./url.ini";
            StreamReader reader = new StreamReader(Path,System.Text.Encoding.GetEncoding("GB2312"));
            while (reader.Peek() >= 0)
            {
                try
                {
                    ReadLine = reader.ReadLine();
                    if (ReadLine != "")
                    {
                        ReadLine = ReadLine.Replace("/", "");
                        array = ReadLine.Split(',');
                        if (array.Length == 0)
                        {
                            MessageBox.Show("error:00");
                            return;
                        }
                        this.comboBox1.Items.Add(array[0]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
            reader.Close();

            timer1.Enabled = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(@"./url.ini", true, Encoding.ASCII);
            sw.Write(Environment.NewLine + comboBox1.Text);
            sw.Flush();
            sw.Close();
            timer1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            rk.SetValue("ProxyEnable", 1);
            rk.SetValue("ProxyServer", comboBox1.Text+":8080");
            rk.Flush(); 
            rk.Close();
            //激活
            InternetSetOption(0, 39, IntPtr.Zero, 0);
            InternetSetOption(0, 37, IntPtr.Zero, 0);

            button4.PerformClick();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try{
                RegistryKey rsg = null;                    //声明变量
                rsg = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true); //true表可修改
                if (rsg.GetValue("ProxyServer") != null)  //如果值不为空
                    {
                        this.textBox3.Text = rsg.GetValue("ProxyServer").ToString();                                                            //读取值
                    }
                else
                //this.textBox3.Text = "该键不存在";
                rsg.Close();                            //关闭
            }
            catch (Exception ex)                        //捕获异常
            {
                this.textBox3.Text = ex.Message;            //显示异常信息
            }

            String str; 
            str = textBox3.Text; //获取文本框中的文本赋与字符串变量
            str = str.Substring(0, str.Length - 5); //提取去除最后一个字符的子字符串(参数：0(从零Index处开始)，str.Lenght-1(提取几个字符))
            textBox3.Text = str;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            var fileName = Path.GetFullPath(@"d:/proxy/proxy.pro");
            var isHavePwd = false;
            var str = "";
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                StreamReader reader = new StreamReader(stream, Encoding.Default);
                StringBuilder builder = new StringBuilder();
                string strLine = "";

                while ((strLine = reader.ReadLine()) != null)
                {
                    str += strLine;
                }
                var arrStr = str.Replace("\n", "").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arrStr)
                {
                    if (isHavePwd)
                    {
                        textBox1.Text = item;
                        break;
                    }
                    isHavePwd = item == "itccts_proxy";
                }
                stream.Dispose();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                timer2.Enabled = true;
            }
            else
            {
                timer2.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            button6.PerformClick();
            button1.PerformClick();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}
