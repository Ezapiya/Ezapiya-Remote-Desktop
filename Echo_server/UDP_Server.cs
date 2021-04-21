using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Echo_server
{
    class UDP_Server
    {
        UdpClient serverSocket;
        IPEndPoint client_1_endpoint;
        IPEndPoint client_2_endpoint;
        String last_read_endpoint = "";
        IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
        int port = 0;
        public UDP_Server(int Server_port)
        {
            serverSocket = new UdpClient(Server_port);
            serverSocket.Client.ReceiveBufferSize = 64 * 1024;
            port = Server_port;
        }
        public void Start()
        {
            Console.WriteLine("Server Start");
            DateTime endTime = DateTime.Now.AddSeconds(45);
            while (DateTime.Now < endTime)
            {
                byte[] byteData = serverSocket.Receive(ref ipeSender);
                if (last_read_endpoint.CompareTo(ipeSender.ToString()) != 0)
                {
                    last_read_endpoint = ipeSender.ToString();

                    Console.WriteLine("Read");
                    if (client_1_endpoint == null)
                    {
                        client_1_endpoint = ipeSender;
                        Console.WriteLine("Client 1 connected");
                    }
                    else
                    {
                        client_2_endpoint = ipeSender;
                        Console.WriteLine("Client 2 connected");
                    }
                }
                if (client_1_endpoint != null && client_2_endpoint != null)
                    break;
            }
            if (DateTime.Now >= endTime)
            {
                Console.WriteLine("Connection Fail");
                Program p = new Program();
               // p.send_closeing(Convert.ToString(port));
                try
                {
                    byte[] byteData = new Byte[5];
                    byteData[0] = 25;

                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);


                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);

                }
                catch
                { }
            }
            else
            {

                serverSocket.Client.ReceiveTimeout = 10000;
                Thread t = new Thread(data_comunication);
                t.IsBackground = true;
                t.Start();
            }
           


        }
     

        public void data_comunication()
        {
            
            try
            {
                byte[] byteData = serverSocket.Receive(ref ipeSender);

                Int32 data_lenth = BitConverter.ToInt32(byteData, 1);

                if (ipeSender.ToString().CompareTo(client_1_endpoint.ToString()) == 0)
                {
                    if ((int)byteData[0] == 0)
                    {
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        
                        write_log("IMG "+Convert.ToString((int)byteData[5]) +" recive from " + client_1_endpoint.ToString() + " data size " + data_lenth.ToString());
                        Console.WriteLine("IMG " + (int)byteData[5] + " data size " + data_lenth.ToString());
                    }
                    else {
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        //write_log("recive from " + client_1_endpoint.ToString() + " data size " + data_lenth.ToString());
                    }
                    
                }
                else
                {
                    if ((int)byteData[0] == 0)
                    {
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        
                        write_log("IMG " + Convert.ToString((int)byteData[5]) + " recive from " + client_2_endpoint.ToString() + " data size " + data_lenth.ToString());
                        Console.WriteLine("IMG " + (int)byteData[5] + " data size " + data_lenth.ToString());
                    }
                    else {
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        //write_log("recive from " + client_2_endpoint.ToString() + " data size " + data_lenth.ToString());
                    }
                    
                }
                if ((int)byteData[0] == 25)
                {
                    if (ipeSender.ToString().CompareTo(client_1_endpoint.ToString()) == 0)
                    {
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_2_endpoint);
                        Program p = new Program();
                       // p.send_closeing(Convert.ToString(port));
                    }
                    else
                    {
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        serverSocket.Send(byteData, data_lenth, client_1_endpoint);
                        Program p = new Program();
                       // p.send_closeing(Convert.ToString(port));
                    }
                    Console.WriteLine("Connection Close ");
                    Thread.Sleep(1000);
                    serverSocket.Close();
                    //serverSocket.Client.Close();
                    goto abc;
                }
               

            }
            catch (SocketException e)
            {
                try
                {
                    byte[] byteData = new Byte[5];
                    byteData[0] = 25;
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);
                    serverSocket.Send(byteData, 5, client_2_endpoint);

                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    serverSocket.Send(byteData, 5, client_1_endpoint);
                    Console.WriteLine("Both connection close ");
                    Console.WriteLine("Stop Reading");
                    Program p = new Program();
                   // p.send_closeing(Convert.ToString(port));

                    goto abc;

                }
                catch
                { }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error " + ex.ToString());
                write_log("error " +ex.ToString());
            }
            Thread t = new Thread(data_comunication);
            t.IsBackground = true;
            t.Start();
        abc: ;
        }
        public void write_log(String log)
        {
            if (!log.Contains("error :-"))
            {
                try
                {
                    String log_msg = log + "\t at :-" + DateTime.Now.ToString() + "\t\n";
                    File.AppendAllText("log.txt", log_msg);
                }
                catch (Exception ex)
                { }
            }
        }
    }
}
