using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using System.Threading;

namespace Connection_Server
{
    class Program
    {
   
        public static Client[] Clients = new Client[10000];
        public  Echo_server[] Echo_Server = new Echo_server[100];
        private Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket connection_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               
        public static Program instance = new Program();
        private int UDPPort = 20000;
        public  List<Connection_request> Connection_request_list = new List<Connection_request>();

        public static DB db = new DB();
        public static Main_db main_db = new Main_db();

        public static String url = "ezapiya.com";

        static void Main(string[] args)
        {
            

           for (int i = 0; i < 10000; i++)
            {
                Clients[i] = new Client();
            }
           for (int i = 0; i <100; i++)
           {
               instance.Echo_Server[i] = new Echo_server();
           }
            Console.WriteLine("Server Start\n");

            Thread siid = new Thread(set_information_in_database);
            siid.IsBackground = true;
            siid.Start();

            instance.connection_server.Bind(new IPEndPoint(IPAddress.Any, 5501));
            instance.connection_server.Listen(10);
            instance.connection_server.BeginAccept(new AsyncCallback(instance.OnConnectionserver_connect),null);
            

            instance.server.Bind(new IPEndPoint(IPAddress.Any, 5500));
            instance.server.Listen(10);
            instance.server.BeginAccept(new AsyncCallback(instance.OnClientConnect), null);
            while (true)
            {
                String cmd = Console.ReadLine();
                if (cmd.CompareTo("Exit") == 0)
                {
                    break;
                }
                if (cmd.CompareTo("min_echo") == 0)
                {
                    Console.WriteLine(Convert.ToString(instance.get_index_of_min_connection_echo_server()));
                }
               


            }
        }
        public void Echo_server_send(int index, String data)
        {
            instance.Echo_Server[index].send(data);
        }
        public String Echo_server_get_data_string(int index)
        {
            return instance.Echo_Server[index].data_string;
        }
        public int get_index_of_min_connection_echo_server()
        {
            int index = 0, min = instance.Echo_Server[0].connection;
            for (int i = 0; i < 100; i++)
            {
                if (instance.Echo_Server[i].connection < min && instance.Echo_Server[i].server_ip.CompareTo("")!=0)
                {
                    min = instance.Echo_Server[i].connection;
                    index = i;
                }
            }
            return index;
        }
        void OnConnectionserver_connect(IAsyncResult result)
        {
            Socket _socket = connection_server.EndAccept(result);
            Console.WriteLine("Connection Server is connected");
            connection_server.BeginAccept(new AsyncCallback(instance.OnConnectionserver_connect), null);
            for (int i = 0; i < 100; i++)
            {
                if (Echo_Server[i].server_ip.CompareTo("") == 0)
                {
                    Echo_Server[i].Socket = _socket;
                    Echo_Server[i].Start();
                    break;
                }
            }
        }
       
       
        void OnClientConnect(IAsyncResult result)
        {
            Thread.Sleep(30);
            Socket socket = server.EndAccept(result);
            server.BeginAccept(new AsyncCallback(OnClientConnect), null);
            String ex_ip = socket.RemoteEndPoint.ToString().Split(':')[0];
            if (ex_ip.CompareTo("73.185.97.186") == 0 || ex_ip.CompareTo("68.194.193.41") == 0 || ex_ip.CompareTo("99.124.89.93") == 0)
                goto abc;
            for (int i = 0; i < 10000; i++)
            {
              
                if (Clients[i].uid.CompareTo("") == 0)
                {
                    Clients[i].Socket = socket;
                    Clients[i].IP = socket.RemoteEndPoint.ToString().Split(':')[0];
                    Clients[i].start();
                    return;
                }
            }
        abc: ;
        }

        public static void set_information_in_database()
        {
            Thread.Sleep(90000);
            db.run_sql("delete from online_client");
            for (int i = 0; i < 10000; i++)
            {
                if (Clients[i].uid.CompareTo("") != 0)
                {
                    String sql = "insert into online_client values('" + Clients[i].IP + "','" + Clients[i].uid + "','" + Clients[i].MachineName + "')";
                    db.run_sql(sql);
                }
            }
            Thread siid = new Thread(set_information_in_database);
            siid.IsBackground = true;
            siid.Start();
        }
    }


}
