using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace LetUsDo
{
    class Local_Client
    {
        

        public static event EventHandler tcp_read_complit;

      //  public static Local_Client instance = new Local_Client();

        public static Socket TCP_Socket;
        public static int TCP_Port = 5500;
        public static string LocalIP = "localhost";
        public static string ServerIP = "103.212.120.85";
       // public static string ServerIP = "192.168.1.185";
      
        
        public static int UDP_Port;
        public static IPEndPoint UDP_Point;
       
        public static byte[] tcp_data = new byte[1024];
        public static String tcp_string = "";
        public static bool connection_status = false;

        public static void setup()
        {
            TCP_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        protected virtual  void Ontcp_read_complit(EventArgs e)
        {
            EventHandler handler = tcp_read_complit;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        public static bool Connect()
        {
            try
            {
                TCP_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //TCP_Socket.Connect(ServerIP, TCP_Port);
                IAsyncResult result= TCP_Socket.BeginConnect(ServerIP, TCP_Port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(1000, true);
                if (TCP_Socket.Connected)
                {
                    TCP_Socket.EndConnect(result);
                    Thread t = new Thread(tcp_recived);
                    t.IsBackground = true;
                    t.Start();
                    return true;
                }
                else
                {
                    TCP_Socket.Close();
                    TCP_Socket.Dispose();
                    return false;
                }
            }
            catch(Exception ex)
            {
                TCP_Socket.Close();
                TCP_Socket.Dispose();
                return false;
            }
        }
        
        public static void send(String data)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                TCP_Socket.Send(bytes);
            }
            catch (Exception ex)
            { }

        }
        private static void send_complit(IAsyncResult ar)
        {

        }
        static void ConnectCallback(IAsyncResult result)
        {
           
        }
        static void Dis_ConnectCallback(IAsyncResult result)
        {

        }
        private static void tcp_recived()
        {
            try
            {
                TCP_Socket.Receive(tcp_data, SocketFlags.None);
                tcp_string = Encoding.ASCII.GetString(tcp_data);
                Local_Client lc = new Local_Client();
                lc.Ontcp_read_complit(EventArgs.Empty);
                Array.Clear(tcp_data, 0, tcp_data.Length);
            }
            catch
            { }
            Thread t = new Thread(tcp_recived);
            t.IsBackground = true;
            t.Start();
        }
       
       
    }
}
