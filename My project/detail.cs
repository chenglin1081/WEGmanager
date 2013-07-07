using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace My_project
{
    public partial class detail : Form
    {
        public detail(string url,string id)
        {
            InitializeComponent();
            webBrowser1.Url = new Uri(url);
            this.Text = "用户"+id+"历史详单";
        }

        private void detail_Load(object sender, EventArgs e)
        {
            
        }


    }
}
