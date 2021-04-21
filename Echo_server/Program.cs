using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;


namespace Echo_server
{
    class Program
    {
        public static Socket Connection_server;
        public static byte[] Connection_server_data = new byte[64];
        static int port1, port2, port3, con_id;
        private static String connection_server_ip = "192.168.1.185";
        private static String Echo_server_ip = "192.168.1.185";
        private static String Echo_server_name = "192.168.1.185";


        static void Main(string[] args)
        {
            String cmd = "";
            port1 = 7000;
            port2 = 7001;
            port3 = 7002;
            con_id = 2547;
          
            Console.WriteLine("Enter Connection Server public IP Address");
          //  connection_server_ip = Console.ReadLine();

            Console.WriteLine("Enter Echo Server public IP Address");
           // Echo_server_ip = Console.ReadLine();

            Console.WriteLine("Enter Echo Server Name");
           // Echo_server_name = Console.ReadLine();

            Console.WriteLine("Echo Server is Start");

            Connection_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Connection_server.NoDelay = false;
            while (true)
            {
                try
                {
                    Connection_server.Connect(connection_server_ip, 5501);
                    Console.WriteLine("connected to connection server");
                    Thread t = new Thread(read_connectin_server);
                    t.IsBackground = true;
                    t.Start();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fail to connect connection server");
                    Thread.Sleep(1000);
                }
            }
            while (true)
            {
                    cmd=Console.ReadLine();
                    if (cmd.CompareTo("Exit") == 0)
                        break;
            }
        }
       
        public static void read_connectin_server()
        {
            Connection_server.Receive(Connection_server_data, SocketFlags.None);
            String xx = Encoding.ASCII.GetString(Connection_server_data);
            Console.WriteLine(xx);
            if (xx.CompareTo("make Connection") == 0)
            {
                Random r = new Random();
                int port = r.Next(10000, 30000);


                //System.Diagnostics.Process.Start("afts.exe", Convert.ToString(port));
                Process p = new Process();
                p.Exited += new EventHandler(p_Exited);
                p.StartInfo.FileName = "afts.exe";
                p.StartInfo.Arguments = port.ToString();
                p.StartInfo.Domain = port.ToString();
                p.EnableRaisingEvents = true;
                p.Start();



                String str = Convert.ToString(port)+":7001:7002:12345:"+Echo_server_ip;
                Byte[] bdata = new byte[Encoding.ASCII.GetBytes(str).Length + 1];
                bdata[0] = 2;
                Array.Copy(Encoding.ASCII.GetBytes(str), 0, bdata, 1, Encoding.ASCII.GetBytes(str).Length);
                Connection_server.Send(bdata);
                
                

            }
            Array.Clear(Connection_server_data, 0, Connection_server_data.Length);
            Thread t = new Thread(read_connectin_server);
            t.IsBackground = true;
            t.Start();
        }
        public static void send_closeing(String msg)
        {
            Byte[] bdata = new byte[Encoding.ASCII.GetBytes(msg).Length + 1];
            bdata[0] = 1;
            Array.Copy(Encoding.ASCII.GetBytes(msg), 0, bdata, 1, Encoding.ASCII.GetBytes(msg).Length);
            Connection_server.Send(bdata);
           
        }
       public static void p_Exited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            send_closeing(p.StartInfo.Domain.ToString());
            
        }
    }
}
