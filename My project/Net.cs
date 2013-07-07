using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Readfile;

namespace My_project
{
    
    class sck
    {
        //创建新tcplistenr监听端口
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);

        public void listen(int ThreadNum,int port)
        {
            //改变端口
            listener = new TcpListener(IPAddress.Any, port);
            try
            {
                listener.Start();

                server.text = "后台服务器启动成功";

                //MessageBox.Show (listener.LocalEndpoint.ToString());
                //创建线程
                for (int i = 1; i <= ThreadNum; i++)
                {
                    Thread thrd = new Thread(new ThreadStart(this.ls));
                    thrd.IsBackground = true;
                    thrd.Start();
                }
            }
            catch
            {
                server.text = "socket错误，未能启动";
            }


            
            
        }

        private void ls()
        {
            string GET,data;
            string[] result;
            
                    TcpClient client = listener.AcceptTcpClient();
                    //创建网络流
                    NetworkStream netStream = client.GetStream();

                    StreamReader sr = new StreamReader(client.GetStream());
                 
                    //读取第一行的东西然后关闭sr
                    GET=sr.ReadLine().ToString();

                    StreamWriter sw = new StreamWriter(client.GetStream());

                    Readfile.Readfile rf = new Readfile.Readfile();

                    result=GET.Split(' ');
                    //sw.Write("HTTP/1.1 200 OK\r\n");
                    if (result[1] == "") { ; }
                    if (result[1] == "/"|result[1] == "/index.html") 
                    { 
                        //显示首页
                        //
                        StreamReader fsr = new StreamReader(Application.StartupPath + "/UserData/default.dba");
                        while (fsr.EndOfStream == false)
                        {
                        sw.Write(fsr.ReadLine().ToString());
                        }
                        fsr.Close();
                        sw.Close();
                        sr.Close();
                        netStream.Close();
                    }
                    else
                    {
                        try
                        {//分离&
                            string[] fs = result[1].Split('&');
                            //分离=
                            string[] fr = fs[0].Split('=');

                            if (fr[1].ToString() != "")
                            {
                                if (rf.Existen(Application.StartupPath + "/UserData/wegDbfile-"
                                 + fr[1].ToString() + ".rdb"))
                                {
                                    StreamReader fsr = new StreamReader(Application.StartupPath + "/UserData/wegDbfile-"
                                     + fr[1].ToString() + ".rdb");
                                    while (fsr.EndOfStream == false)
                                    {
                                        sw.Write(fsr.ReadLine().ToString());
                                    }
                                    fsr.Close();
                                    sw.Close();
                                    sr.Close();
                                    netStream.Close();
                                }
                                else
                                {
                                    data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">数据不存在";
                                    sw.Write(data);
                                    sw.Close();
                                    sr.Close();
                                    netStream.Close();
                                }

                            }
                            else
                            {
                                data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">数据不存在";
                                sw.Write(data);
                                sw.Close();
                                sr.Close();
                                netStream.Close();
                            }


                        }
                        catch
                        {
                            data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">数据不存在";

                            sw.Write(data);
                            sw.Close();
                            sr.Close();
                            netStream.Close();
                        }
                        //MessageBox.Show(data);
                       
                    }
                    
                
            //此线程退出时创建新线程
            Thread thrd = new Thread(new ThreadStart(this.ls));
            thrd.IsBackground = true;
            thrd.Start();
        }




       

    }


}
