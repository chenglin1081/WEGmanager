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
   
    public partial class MainForm : Form
    {
        private bool Edit;
        Readfile.Readfile rf = new Readfile.Readfile();
        public MainForm()
        {
            InitializeComponent();
        }
        public struct cell
        {
            public static int ColumnIndex;
            public static int RowIndex;
            public static string usertype;
        }
        private void userBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            //保存并重置数据单元
            this.Validate();
            this.userBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataDataSet);
            this.LoadData();
            this.Edit = false;
        }
        //数据加密
        public static string GetMd6Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8).Replace("-", "B");
            t2 += BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8).Replace("-", "Z");
            t2 += BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8).Replace("-", "W");
            return t2;
        }

        //填充数据库
        public void LoadData()
        {
            this.userTableAdapter.Fill (this.dataDataSet.user);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewCell c in userDataGridView.SelectedCells)
            {
                if (c.RowIndex>=0) { this.userDataGridView.Rows.RemoveAt(c.RowIndex); }
            }
            
        }
        //设定当前单元格
        private void userDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (userDataGridView.SelectedCells.Count == 1 & e.ColumnIndex >= 0 & e.RowIndex >= 0 & e.Button==MouseButtons .Right)
            {
                userDataGridView.CurrentCell = userDataGridView[e.ColumnIndex, e.RowIndex];
                
            }
         }


        private void 关于本程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox newabout= new AboutBox ();
            newabout.Show();
        }

        private void fwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(this.Edit == true)
            {

                switch (MessageBox.Show("是否保存您作的修改?", "关闭程序", MessageBoxButtons.YesNo))
                { 
                    case DialogResult.Yes:
                        this.Validate();
                        this.userBindingSource.EndEdit();
                        this.tableAdapterManager.UpdateAll(this.dataDataSet);
                        break;

                }
            }
            //else
            {
            //终止所有线程并结束程序
            Application.Exit();
            }
        }
        private void hideall()
        {
            //隐藏所有可能出现的窗体

            groupBox2.Visible = false;

            button1.Visible = false;

            groupBox1.Visible = false;


        }
        private void userDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string[] data=new string[0];
            hideall();
            this.Edit = true; 
            if (e.RowIndex >= 0)
            {
                try
                {
                    switch (e.ColumnIndex)
                    {
                        //配额  
                        case 5:
                            groupBox1.Text = "用户配额";
                            //读取数据
                            data = userDataGridView.CurrentCell.Value.ToString().Split('|');
                            if (data[0] != "")
                            {
                                water.Text = data[0];
                                elec.Text = data[1];
                                gas.Text = data[2];
                            }
                            else
                            {
                                water.Text = "0";
                                elec.Text = "0";
                                gas.Text = "0";
                            }
                            cell.ColumnIndex = e.ColumnIndex;
                            cell.RowIndex = e.RowIndex;
                            groupBox1.Visible = true;
                            button1.Visible = true;
                            //用户类型
                            cell.usertype = userDataGridView[e.ColumnIndex - 1, e.RowIndex].Value.ToString();
                            break;
                        //单价
                        case 6:
                            groupBox1.Text = "单价";
                            //读取数据
                            data = userDataGridView.CurrentCell.Value.ToString().Split('|');
                            if (data[0] != "")
                            {
                                water.Text = data[0];
                                elec.Text = data[1];
                                gas.Text = data[2];
                            }
                            else
                            {
                                water.Text = "0";
                                elec.Text = "0";
                                gas.Text = "0";
                            }
                            cell.ColumnIndex = e.ColumnIndex;
                            cell.RowIndex = e.RowIndex;
                            groupBox1.Visible = true;
                            button1.Visible = true;
                            //用户类型
                            cell.usertype = userDataGridView[e.ColumnIndex - 2, e.RowIndex].Value.ToString();
                            break;
                        //使用
                        case 7:
                            groupBox1.Text = "已经使用";
                            //读取数据
                            data = userDataGridView.CurrentCell.Value.ToString().Split('|');
                            if (data[0] != "")
                            {
                                water.Text = data[0];
                                elec.Text = data[1];
                                gas.Text = data[2];
                            }
                            else
                            {
                                water.Text = "0";
                                elec.Text = "0";
                                gas.Text = "0";
                            }
                            cell.ColumnIndex = e.ColumnIndex;
                            cell.RowIndex = e.RowIndex;
                            groupBox1.Visible = true;
                            //用户类型
                            cell.usertype = userDataGridView[e.ColumnIndex - 3, e.RowIndex].Value.ToString();
                            ; break;

                        //缴费
                        case 8:
                            recent.Text = userDataGridView[9, e.RowIndex].Value.ToString();
                            updatemoney();
                            button1.Visible = true;
                            groupBox2.Visible = true;
                            //应交费的信息
                            textBox4.Text = userDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
                            ; break;

                        default: this.userDataGridView.BeginEdit(true);break;

                    }
                    handin.Text = "";
                }
                catch
                {
                    groupBox1.Text = "";
                }
            }
            
        }

        private void 详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //详情载入
            //string type = userDataGridView.CurrentCellAddress.X();
            string userid = "wegDbfile-" + userDataGridView[1, userDataGridView.CurrentCell.RowIndex].Value.ToString();
            string path=Application.StartupPath + "/UserData/" + userid + ".rdb";
            detail de = new detail(path,userDataGridView[1, userDataGridView.CurrentCell.RowIndex].Value.ToString());
            de.Show();
            
        }

       

        private void 数据库导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName.ToString() != "")
            {
                this.dataDataSet.WriteXml(saveFileDialog1.FileName.ToString());
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton1.ShowDropDown();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //this.Search_col.Text.Trim;
            //this.Search_con.Text.Trim;
            if (this.Search_col.Text.ToString() != "" & this.Search_con.Text.ToString() != "")
            {
            userTableAdapter.Adapter.SelectCommand.CommandText = "SELECT [user].* FROM [user] WHERE [" + this.Search_col.Text.Trim() + "] like '%" + this.Search_con.Text.Trim() + "%'";
            
            this.userTableAdapter.Fill(this.dataDataSet.user);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            userTableAdapter.Adapter.SelectCommand.CommandText = "SELECT [user].* FROM [user]";

            this.userTableAdapter.Fill(this.dataDataSet.user);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        //限制输入数字
        private void water_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar)&& !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void elec_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void handin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void gas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void accept_Click(object sender, EventArgs e)
        {
            userDataGridView[cell.ColumnIndex, cell.RowIndex].Value = water.Text + '|' + elec.Text + "|" + gas.Text;
            //更新应交钱
            updatemoney(); 

            //加入历史
            string userid = "wegDbfile-" + userDataGridView[1, userDataGridView.CurrentCell.RowIndex].Value.ToString();
            string detail = DateTime.Now.ToString() + "-----应交" + userDataGridView[8, userDataGridView.CurrentCell.RowIndex].Value.ToString() + "元" + "-----" + groupBox1.Text + "改变为" + getdata() + "<br />";
            rf.AddDetail(Application.StartupPath + "/UserData/" + userid + ".rdb", detail);
            
            groupBox1.Visible = false;
        }
        //得到详细改变的内容
        private string getdata()
        {
            switch (groupBox1.Text.ToString())
            { 
                case "已经使用":
                    return userDataGridView.CurrentCell.Value.ToString();
                 
                case "用户配额":
                    return userDataGridView.CurrentCell.Value.ToString();
                   
                case "单价":
                    return userDataGridView.CurrentCell.Value.ToString();
                    
            }

            return "未知";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setdefault();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
        //+++++++++++++++++++++

        private void setdefault()
        {
            //默认值的取得
            Readfile.Readfile rf = new Readfile.Readfile();
            if (groupBox1.Text == "用户配额")
            {
                if (cell.usertype == "教师")
                {
                    water.Text = "0";
                    elec.Text = "0";
                    gas.Text = "0";
                }
                else
                {
                    water.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "waterlimit");
                    elec.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "eleclimit");
                    gas.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "gaslimit");
                }

            }
            if (groupBox1.Text == "单价")
            {

                if (cell.usertype == "教师")
                {
                    water.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "watertc");
                    elec.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "electc");
                    gas.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "gastc");


                }
                else
                {
                    water.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "waterst");
                    elec.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "elecst");
                    gas.Text = rf.GetConfig(Application.StartupPath + "/config.ini", "gasst");
                }


            }
            //rf.GetConfig();
            /*water.Text = data[0];
            elec.Text = data[1];
            gas.Text = data[2];
             * */
        
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            if (handin.Text != "")
            {
                 //修改余额
                userDataGridView[9, userDataGridView.CurrentCell.RowIndex].Value = double.Parse(recent.Text.ToString()) + double.Parse(handin.Text.ToString()) - double.Parse(textBox4.Text.ToString());
                recent.Text=userDataGridView[9, userDataGridView.CurrentCell.RowIndex].Value.ToString();
                //加入历史
                string userid = "wegDbfile-" + userDataGridView[1, userDataGridView.CurrentCell.RowIndex].Value.ToString();
                string detail = DateTime.Now.ToString() + "-----应交" + textBox4.Text.ToString() + "元" +
                    "<br />" + "----------------------缴入" + handin.Text.ToString() + "元" +
                    "<br />" + "----------------------剩余" + recent.Text.ToString() + "元"
                    ;
                rf.AddDetail(Application.StartupPath + "/UserData/" + userid + ".rdb", detail);
                //清空已经使用
                userDataGridView[userDataGridView.CurrentCell.ColumnIndex - 1, userDataGridView.CurrentCell.RowIndex].Value = "0|0|0";
                updatemoney();
                groupBox2.Visible = false;
                textBox4.Text = "0";
                
            }

        }

        private void updatemoney()
        {
            string[] used;
            string[] limit;
            string[] per;
            double money=0.0;
            double me = 0.0;
            //已经使用
            used=userDataGridView[7, userDataGridView.CurrentCell.RowIndex].Value.ToString().Split('|');
            //单价
            per = userDataGridView[6, userDataGridView.CurrentCell.RowIndex].Value.ToString().Split('|');
            //配额
            limit = userDataGridView[5, userDataGridView.CurrentCell.RowIndex].Value.ToString().Split('|');
            //用户类型
            try
                {
                    if (cell.usertype == "教师")
                        for (int i = 0; i < 3; i++)
                        {
                            money += double.Parse(used[i]) * double.Parse(per[i]);
                        }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            //如果没超过使用限额算0元
                            if (double.Parse(limit[i]) - double.Parse(used[i]) >= 0) 
                            {
                                me = 0; }
                            else {
                                me=(double.Parse(used[i]) - double.Parse(limit[i])) * double.Parse(per[i]);
                            }
                            money += me;
                        }
                    }
                    
                    if (recent.Text=="") { 
                        recent.Text = "0"; 
                    }
                        //money = money - double.Parse(recent.Text.ToString()); 
                    userDataGridView[8, userDataGridView.CurrentCell.RowIndex].Value = money.ToString();
            
                }
            catch
                {
                    userDataGridView[8, userDataGridView.CurrentCell.RowIndex].Value = "数据格式有误..请检查";
                }
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string user = rf.GetConfig(Application.StartupPath + "/Database/PWD.mdb", "username");
            string pwd = rf.GetConfig(Application.StartupPath + "/Database/PWD.mdb", "userpassword");
            if (curpwd.Text == "" || newpwd.Text == "" || newpwd2.Text == "") { MessageBox.Show("密码不能为空"); return; }
            if (newpwd.Text != newpwd2.Text) { MessageBox.Show("两次输入不同"); return; }
            if (GetMd6Str(curpwd.Text) == pwd)
            {
                //写入密码
                rf.SetPwd(Application.StartupPath + "/Database/PWD.mdb", GetMd6Str("admin"), GetMd6Str(newpwd.Text));
                MessageBox.Show("设置成功");
            }
            else
            { MessageBox.Show("原密码错误"); }
            //隐藏并清空
            groupBox3.Visible = false;
            curpwd.Text = ""; newpwd.Text = ""; newpwd2.Text = "";
        }

        private void 复制单元格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(userDataGridView.CurrentCell.Value.ToString());
        }

        private void 粘贴单元格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userDataGridView.CurrentCell.Value = Clipboard.GetText();
        }

        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void userDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }





    }

}
