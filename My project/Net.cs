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
        //������tcplistenr�����˿�
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);

        public void listen(int ThreadNum,int port)
        {
            //�ı�˿�
            listener = new TcpListener(IPAddress.Any, port);
            try
            {
                listener.Start();

                server.text = "��̨�����������ɹ�";

                //MessageBox.Show (listener.LocalEndpoint.ToString());
                //�����߳�
                for (int i = 1; i <= ThreadNum; i++)
                {
                    Thread thrd = new Thread(new ThreadStart(this.ls));
                    thrd.IsBackground = true;
                    thrd.Start();
                }
            }
            catch
            {
                server.text = "socket����δ������";
            }


            
            
        }

        private void ls()
        {
            string GET,data;
            string[] result;
            
                    TcpClient client = listener.AcceptTcpClient();
                    //����������
                    NetworkStream netStream = client.GetStream();

                    StreamReader sr = new StreamReader(client.GetStream());
                 
                    //��ȡ��һ�еĶ���Ȼ��ر�sr
                    GET=sr.ReadLine().ToString();

                    StreamWriter sw = new StreamWriter(client.GetStream());

                    Readfile.Readfile rf = new Readfile.Readfile();

                    result=GET.Split(' ');
                    //sw.Write("HTTP/1.1 200 OK\r\n");
                    if (result[1] == "") { ; }
                    if (result[1] == "/"|result[1] == "/index.html") 
                    { 
                        //��ʾ��ҳ
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
                        {//����&
                            string[] fs = result[1].Split('&');
                            //����=
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
                                    data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">���ݲ�����";
                                    sw.Write(data);
                                    sw.Close();
                                    sr.Close();
                                    netStream.Close();
                                }

                            }
                            else
                            {
                                data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">���ݲ�����";
                                sw.Write(data);
                                sw.Close();
                                sr.Close();
                                netStream.Close();
                            }


                        }
                        catch
                        {
                            data = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\">���ݲ�����";

                            sw.Write(data);
                            sw.Close();
                            sr.Close();
                            netStream.Close();
                        }
                        //MessageBox.Show(data);
                       
                    }
                    
                
            //���߳��˳�ʱ�������߳�
            Thread thrd = new Thread(new ThreadStart(this.ls));
            thrd.IsBackground = true;
            thrd.Start();
        }




       

    }


}
