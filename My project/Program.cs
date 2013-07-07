using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace My_project
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //以launcher创建新窗口

            Launcher frm2 = new Launcher();
            //启动此窗口
            Application.Run(frm2);
        }
    }
}