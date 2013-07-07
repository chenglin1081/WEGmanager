using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Readfile;

namespace BackgroundServise
{
    class Program
    {
         //创建新tcplistenr监听端口
        static TcpListener listener = new TcpListener(IPAddress.Any,0);

        public static string binpath;

        static void Main(string[] args)
        {
            binpath=System.AppDomain.CurrentDomain.BaseDirectory;
            int ThreadNum; int port;
            //初始值
            ThreadNum = -1;
            port = -1;
            try
            {
                //改变端口和线程数
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "/p") { port = int.Parse(args[i+1]); }
                    if (args[i] == "/t") { ThreadNum = int.Parse(args[i + 1]); }
                }
                if (ThreadNum == -1 || port == -1)
                {
                    Console.Beep();
                    Console.Write("用法错误 \r\n程序接口p 端口 t线程数\r\n例 BackgroundServise.exe /p 80 /t 10" +
                        "\r\n将在本地开启10个线程监听80端口\r\n超过此线程用户将无法连接\r\n");
                    return;
                }
            }
            catch
            {
                //错误捕获
                Console.Beep();
                Console.Write("用法错误 \r\n程序接口p 端口 t线程数\r\n例 BackgroundServise.exe /p 80 /t 10" +
                        "\r\n将在本地开启10个线程监听80端口\r\n超过此线程用户将无法连接\r\n");
                return;
            }
            Console.Clear();
            Console.Write("水电管理系统后台服务器\r\n版权所有:白忠魏\r\n");
            Console.Write("线程数" + ThreadNum.ToString());
            Console.Write("\r\n");
            Console.Write("端口" + port.ToString()+"\r\n");
            //改变端口
            listener = new TcpListener(IPAddress.Any, port);
            try
            {
                listener.Start();

                Console.Write("开始监听");

                //MessageBox.Show (listener.LocalEndpoint.ToString());
                //创建线程
                for (int i = 1; i <= ThreadNum; i++)
                {
                    Thread thrd = new Thread(new ThreadStart(ls));
                    thrd.IsBackground = false;
                    thrd.Start();
                }
            }
            catch
            {
                Console.Beep();
                Console.Write("socket错误，启动失败");
                return;
            }

            
        }

        static private void ls()
        {
            string GET,data;
            string[] result;
            
                    TcpClient client = listener.AcceptTcpClient();
                    //创建网络流
                    NetworkStream netStream = client.GetStream();

                    StreamReader sr = new StreamReader(client.GetStream());
                 
                    //读取第一行的东西然后关闭sr
                    GET=sr.ReadLine().ToString();
                    Console.Write("\r\n");
                    Console.Write(GET);
                    StreamWriter sw = new StreamWriter(client.GetStream());

                    Readfile.Readfile rf = new Readfile.Readfile();

                    result=GET.Split(' ');
                    //sw.Write("HTTP/1.1 200 OK\r\n");
                    if (result[1] == "") { ; }
                    if (result[1] == "/"|result[1] == "/index.html") 
                    { 
                        //显示首页
                        //
                        
                        StreamReader fsr = new StreamReader(binpath+"/UserData/default.dba");
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
                                if (rf.Existen(binpath+"/UserData/wegDbfile-"
                                 + fr[1].ToString() + ".rdb"))
                                {
                                    StreamReader fsr = new StreamReader(binpath+"/UserData/wegDbfile-"
                                     + fr[1].ToString() + ".rdb");
                                    while (fsr.EndOfStream == false)
                                    {
                                        sw.Write(fsr.ReadLine().ToString());
                                    }
                                    Console.Write("\r\n");
                                    Console.Write(fr[1].ToString()+"数据发送成功");
                                    fsr.Close();
                                    sw.Close();
                                    sr.Close();
                                    netStream.Close();
                                }
                                else
                                {
                                    data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">数据不存在";
                                    sw.Write(data);
                                    Console.Write("\r\n");
                                    Console.Write("/UserData/wegDbfile-"
                                 + fr[1].ToString() + ".rdb");
                                    Console.Write(fr[1].ToString() + "无此数据");
                                    sw.Close();
                                    sr.Close();
                                    netStream.Close();
                                }

                            }
                            else
                            {
                                data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">数据不存在";
                                sw.Write(data);
                                Console.Write("\r\n");
                                Console.Write(fr[1].ToString() + "数据发送失败");
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
            Thread thrd = new Thread(new ThreadStart(ls));
            thrd.IsBackground = false;
            thrd.Start();
        }

    }
}
