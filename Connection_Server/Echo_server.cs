using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Data;

namespace Connection_Server
{
    class Echo_server
    {
        public Socket Socket;
        public String server_name = "";
        public String server_ip = "";
        public int connection=0;
        public byte[] data = new byte[64];
        public String data_string = "";
        public void send(String data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            Socket.Send(bytes);
        }
        public void Start()
        {
            Random r = new Random();
            //connection = r.Next(0, 10);
            Console.WriteLine("Echo server is start  " + connection.ToString());

            server_ip = Socket.RemoteEndPoint.ToString().Split(':')[0];

            Thread t = new Thread(read);
            t.IsBackground = true;
            t.Start();
        }
        public void read()
        {
            while (Socket.Connected)
            {
                try
                {
                    Socket.Receive(data, SocketFlags.None);
                    if(data[0]==2)// make connection
                    {
                        byte[] tempbyte = new byte[data.Length - 1];
                        Array.Copy(data, 1, tempbyte, 0, data.Length - 1);
                        data_string = Encoding.ASCII.GetString(tempbyte);
                        Console.WriteLine(tempbyte);
                        connection++;
                    }
                    if (data[0] == 1)// close connection
                    {
                        byte[] tempbyte = new byte[data.Length - 1];
                        Array.Copy(data, 1, tempbyte, 0, data.Length - 1);
                       String port_no = Encoding.ASCII.GetString(tempbyte);
                       String[] temp_port_list = port_no.Split(':');
                       String sql = "delete from  live_connection where port_no='" + temp_port_list[0] + "'"; 
                       Program.db.run_sql(sql);


                        connection--;
                    }


                }
                catch (SocketException e)
                {
                    Console.WriteLine("Echo Server close ");
                    Socket.Close();
                    Socket.Dispose();
                    server_ip = "";
                }
                catch (Exception ex)
                { }
            }
          //  Thread t = new Thread(read);
           // t.IsBackground = true;
           // t.Start();
        }
    }
}
