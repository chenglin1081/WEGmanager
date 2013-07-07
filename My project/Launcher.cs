using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using Readfile;

namespace My_project
{
    //公共结构..用于传递后台服务器状况
    public struct server {
        public static  string text;
    }
    public partial class Launcher : Form
    {
        //public int i;
        MainForm mainform = new MainForm();
        
        public Launcher()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 100) 
            {
                progressBar1.Value = 0;
            }
            progressBar1.Value += 1;
            //如果线程结束则清空资源进入主窗口
            switch (progressBar1.Value)
            {
                case 1: label3.Text = "载入配置文件"; break;
                case 20: label3.Text = "载入数据库"; break;
                case 40: label3.Text = "设置后台线程"; break;
                case 60: label3.Text = server.text; break;
                case 80: label3.Text = "载入界面"; break;
            }
            if (!backgroundWorker1.IsBusy & progressBar1.Value >= 90)
            {
                //释放线程资源
                backgroundWorker1.CancelAsync();
                backgroundWorker1.Dispose();
                this.Hide();
                this.ShowInTaskbar = false;
                mainform.Show();
                timer1.Enabled = false;
            }
            
            
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            
        }
        public static string GetMd6Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8).Replace("-", "B");
            t2 += BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8).Replace("-","Z");
            t2 += BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8).Replace("-", "W");
            return t2;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //在此载入数据库
            mainform.LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            groupBox1.Enabled =false ;
            //配置文件读取类
            Readfile.Readfile cfg = new Readfile.Readfile();
            
            button1.Enabled = false;
            //检验用户名和密码
            string user=cfg.GetConfig(Application.StartupPath + "/Database/PWD.mdb", "username");
            string pwd = cfg.GetConfig(Application.StartupPath + "/Database/PWD.mdb", "userpassword");

            if (GetMd6Str(textBox1.Text) == user && GetMd6Str(textBox2.Text) == pwd)
            {
            this.Focus();
            //改变鼠标形状
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            progressBar1.Visible = true;
            button1.Visible = false;
            timer1.Enabled = true;
            backgroundWorker1.RunWorkerAsync();
            //启用socket
            if(cfg.GetConfig(Application.StartupPath + "/config.ini", "enable")=="true")
                {
                sck sock = new sck();
                //读取线程数量
                sock.listen(int.Parse(cfg.GetConfig(Application.StartupPath + "/config.ini", "ThreadNum")), 
                int.Parse(cfg.GetConfig(Application.StartupPath + "/config.ini", "port")));
                }
                else
                {
                    server.text="未开启后台服务";}
            }
            else
            {
                label3.Text = "用户名或密码错误";
                timer2.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            button1.Enabled = true;
            label3.Text = "";
            timer2.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Launcher_Paint(object sender, PaintEventArgs e)
        {
            //设定交点 
            if (textBox1.Text=="") { textBox1.Focus(); }
        }

       

      
    }
}
