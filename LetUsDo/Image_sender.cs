using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.IO;


using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Drawing;
using System.Drawing.Imaging;

namespace LetUsDo
{
    public partial class Image_sender : Form
    {
        bool capture_main_screen = false;
        bool capture_black_screen = false;

        bool found_reciver = false;

        Bitmap bmp;
        Bitmap prv_bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        
        Bitmap[] p = new Bitmap[4];
        Bitmap[] c = new Bitmap[4];


        //   Mat frame, prev_frame = new Mat(), gray, diff = new Mat(), mask = new Mat();
        //  const int nNum = 2, nThreshold = 5;

        //   int sX, sY;
        int k = 0;

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            LogicalScreenWidth = 8,
            LogicalScreenHeight = 10,
            PhysicalScreenHeight = 117,
            PhysicalScreenWidth = 118,
            LOGPIXELSY = 90,
        }


        [DllImport("DpiHelper32.dll", CharSet = CharSet.Unicode)]
        public static extern int Scaling_mon(UInt32 sourceID, UInt32 dpiPercentToSet);

        //[DllImport("DpiHelper64.dll", CharSet = CharSet.Unicode)]
        //[DllImport("DpiHelper64.dll", CharSet = CharSet.Auto, SetLastError = true)]

        [DllImport("DpiHelper32.dll", CharSet = CharSet.Unicode)]
        public static extern void RestoreDPIScaling();


        public UdpClient clientSocket; //The main client socket
        public IPEndPoint p2pEndPoint;   //The EndPoint of the server
        public IPEndPoint serverEndPoint;
        public IPEndPoint tempEndPoint;

        string serverIP = "";
        int serverPort=4000;
        Data_Class dc = new Data_Class();
        Mouse_Class mc = new Mouse_Class();
        KB kb = new KB();
        String old_STR_str = "";
        String old_MOUSE_str = "";
        String old_KEYBOARD_str = "";
        String old_SYSTEM_str = "";
        String old_PROCESS_str = "";
        String old_COMMAND_str = "";
        String old_CHAT_str = "";
        String old_FILE_SYSTEM_str = "";
        String old_CLIPBOARD_string = "";

       




        String old_BLACK_SCREEN_LOGIN_string="";
        String scree_control_login_name = "";
        bool black_screen_status = false;
        String old_BLACK_SCREEN_string = "";
        Int32 old_BLACK_SCREEN_ING_LENGTH = 0;

        String old_BLACK_SCREEN_IMAGE_NAME_str = "";
        String old_DESKTOP_AS_BLACK_SCREEN_str = "";

        String old_FILE_UPLOAD_str = "";
        String old_FILE_DOWNLOAD_str="";

        Bitmap[] screen_matrix = new Bitmap[Globle_data.img_matrix * Globle_data.img_matrix];
        byte[] image_rtp_byte = new byte[Globle_data.img_matrix * Globle_data.img_matrix];

        Bitmap prv_img = null;
        Bitmap cur_img = null;

       

        Blackscreen Blackscreen_form = new Blackscreen();
       

        Byte[] img_0_webp_bytes;
        Byte[] img_1_webp_bytes;
        Byte[] img_2_webp_bytes;
        Byte[] img_3_webp_bytes;

        String file_to_upload = "";
        byte[] file_data;
        Int64 file_number_of_packet;
        Int64 file_current_packet_number;
        byte[] ft_packet_data = new byte[5120];
        String path = "";
        String file_name = "";
        Int64 file_size = 0;

        public Image_sender(int server_Port, String server_ip)
        {
            InitializeComponent();
            serverPort = server_Port;
            serverIP = server_ip;
            ClipboardMonitor.OnClipboardChange += ClipboardMonitor_OnClipboardChange;
            ClipboardMonitor.Start();
            write_log("open image sender ip " + server_ip.ToString() + " port " + serverPort);

        }

        private void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            try
            {
                String Clipboard_str = Clipboard.GetText();
                byte[] byte_data = dc.Add_Payload("CLIPBOARD", Clipboard_str);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
        }
        private void Image_sender_Load(object sender, EventArgs e)
        {
           

            IPAddress ipAddress = IPAddress.Parse(serverIP);
            serverEndPoint = new IPEndPoint(ipAddress, serverPort);
            Random r = new Random();
            int this_pc_port = r.Next(10000, 20000);
            clientSocket = new UdpClient(this_pc_port);


            if (File.Exists("log.txt"))
                File.Delete("log.txt");

            write_log("opne udp on port" + this_pc_port.ToString());
            clientSocket.Client.SendBufferSize = 1024 * 64;
            byte[] byteData = dc.Add_Payload("STR", "hi i am sender");
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);

          /*  byteData = dc.Add_Payload("STARTUP_PATH", Application.StartupPath.ToString());
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            clientSocket.Send(byteData, byteData.Length, serverEndPoint);
          */
           /* Thread t = new Thread(recive_fun);
            t.IsBackground = true;
            t.Start();*/
            Task t1 = new Task(recive_fun);
            t1.Start();

            Task c1 = new Task(check_data_comming_from_where);
            c1.Start();

            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            Globle_data.LogicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenWidth);
            Globle_data.LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenHeight);
            Globle_data.PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenWidth);
            Globle_data.PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenHeight);

           
            bmp = new Bitmap(Globle_data.PhysicalScreenWidth, Globle_data.PhysicalScreenHeight);
            p[0] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
            p[1] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
            p[2] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
            p[3] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);

            c[0] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
            c[1] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
            c[2] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
            c[3] = new Bitmap(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);

            capture_main_screen = true;
            main_screen_fun();
        }
        public void check_data_comming_from_where()
        {
            Thread.Sleep(5000);
            String t = "";
            String p = "";
            if (tempEndPoint != null)
            {
                t = tempEndPoint.ToString();
            }
            if (p2pEndPoint != null)
            {
                p = p2pEndPoint.ToString();
            }
            if (t.CompareTo(p) != 0)
                p2pEndPoint = serverEndPoint;

            Thread.Sleep(5000);
            found_reciver = true;
        }

        public void get_ft_packet_data()
        {
            try
            {
                if (file_data.Length <= 5120)
                {
                    // first and only packet
                    ft_packet_data = new byte[file_data.Length + 8];

                    Int64 pkno = 1;
                    byte[] len_byte = BitConverter.GetBytes(pkno);
                    ft_packet_data[0] = len_byte[0];
                    ft_packet_data[1] = len_byte[1];
                    ft_packet_data[2] = len_byte[2];
                    ft_packet_data[3] = len_byte[3];
                    /* ft_packet_data[4] = len_byte[4];
                     ft_packet_data[5] = len_byte[5];
                     ft_packet_data[6] = len_byte[6];
                     ft_packet_data[7] = len_byte[7];*/
                    Array.Copy(file_data, 0, ft_packet_data, 8, file_data.Length);


                }
                else
                {
                    if (file_current_packet_number == 1)
                    {
                        ft_packet_data = new byte[5120 + 8];

                        Int32 pkno = 1;
                        byte[] len_byte = BitConverter.GetBytes(pkno);
                        ft_packet_data[0] = len_byte[0];
                        ft_packet_data[1] = len_byte[1];
                        ft_packet_data[2] = len_byte[2];
                        ft_packet_data[3] = len_byte[3];
                        /*ft_packet_data[4] = len_byte[4];
                        ft_packet_data[5] = len_byte[5];
                        ft_packet_data[6] = len_byte[6];
                        ft_packet_data[7] = len_byte[7];*/
                        Array.Copy(file_data, 0, ft_packet_data, 8, 5120);
                    }
                    if (file_current_packet_number == file_number_of_packet)
                    {
                        // last packet 
                        ft_packet_data = new byte[5120 + 8];
                        Int64 pkno = file_current_packet_number;
                        byte[] len_byte = BitConverter.GetBytes(pkno);
                        ft_packet_data[0] = len_byte[0];
                        ft_packet_data[1] = len_byte[1];
                        ft_packet_data[2] = len_byte[2];
                        ft_packet_data[3] = len_byte[3];
                        /* ft_packet_data[4] = len_byte[4];
                         ft_packet_data[5] = len_byte[5];
                         ft_packet_data[6] = len_byte[6];
                         ft_packet_data[7] = len_byte[7];*/

                        Int64 starting_byte = 0;
                        if (file_current_packet_number > 1)
                        {
                            starting_byte = (file_current_packet_number - 1) * 5120;
                        }
                        else
                        {
                            starting_byte = 0;
                        }

                        Int64 len = file_data.Length - starting_byte;
                        Array.Copy(file_data, starting_byte, ft_packet_data, 8, len);
                    }
                    else
                    {
                        // normal packet 
                        ft_packet_data = new byte[5120 + 8];
                        Int64 pkno = file_current_packet_number;
                        byte[] len_byte = BitConverter.GetBytes(pkno);
                        ft_packet_data[0] = len_byte[0];
                        ft_packet_data[1] = len_byte[1];
                        ft_packet_data[2] = len_byte[2];
                        ft_packet_data[3] = len_byte[3];
                        /* ft_packet_data[4] = len_byte[4];
                         ft_packet_data[5] = len_byte[5];
                         ft_packet_data[6] = len_byte[6];
                         ft_packet_data[7] = len_byte[7];*/
                        Int64 starting_byte = (file_current_packet_number - 1) * 5120;
                        Array.Copy(file_data, starting_byte, ft_packet_data, 8, 5120);
                    }

                }
            }
            catch (Exception ex)
            { }
        }
        public void store_data_from_packet(byte[] byte_data)
        {
            Int64 pkno = BitConverter.ToInt32(byte_data, 0);
            if (pkno != file_current_packet_number)
                goto abc;
            if (file_data.Length <= 5120)
            {
                // only one packet
                Array.Copy(byte_data, 8, file_data, 0, byte_data.Length - 8);
                // save file 
                File.WriteAllBytes(path + "\\" + file_name, file_data);
                byte[] file_system_data = dc.Add_Payload("FT_INFO", "file_upload_ok");

                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);

            }
            else
            {

                if (file_current_packet_number == 1)
                {
                    // fisrt packet
                    Array.Copy(byte_data, 8, file_data, 0, byte_data.Length - 8);
                }
                if (file_current_packet_number == file_number_of_packet)
                {
                    //last packet
                    Int64 cb = (file_current_packet_number - 1) * 5120;
                    Int64 len = file_data.Length - cb;
                    Array.Copy(byte_data, 8, file_data, cb, len);
                    //Array.Copy(byte_data, 8, file_data, cb, byte_data.Length - 8);
                    File.WriteAllBytes(path + "\\" + file_name, file_data);
                    byte[] file_system_data_x = dc.Add_Payload("FT_INFO", "file_upload_ok");
                    clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                    // clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                    // clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);

                }
                else
                {
                    // normal packet
                    Int64 cb = (file_current_packet_number - 1) * 5120;
                    Array.Copy(byte_data, 8, file_data, cb, byte_data.Length - 8);
                }
                String fpk_info = "Packet_recived;" + file_current_packet_number.ToString();
                file_current_packet_number++;
                byte[] file_system_data = dc.Add_Payload("FT_INFO", fpk_info);

                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);

            }
            abc:;
        }
        public void recive_fun()
        {

            byte[] data;
            try
            {
                data = clientSocket.Receive(ref tempEndPoint);
               
            }
            catch(Exception ex)
            {
                write_log("error :-" + ex.ToString());
                goto xyz;
            }

            String command = "";
            byte[] byte_data = dc.Get_Payload(data, ref command);

            #region FT_INFO
            if (command.CompareTo("FT_INFO") == 0)
            {
                String ep = Encoding.ASCII.GetString(byte_data);
                String[] data_d = ep.Split(';');
                if (data_d[0].CompareTo("file_upload_request_recive") == 0)
                {
                    /////////// start sending file 
                    get_ft_packet_data();
                    this.Invoke((MethodInvoker)delegate {
                        timer_ft.Start();
                    });
                }
                if (data_d[0].CompareTo("file_upload_ok") == 0)
                {
                    /////////// start sending file 
                    //get_ft_packet_data();
                    this.Invoke((MethodInvoker)delegate {
                        timer_ft.Stop();
                        MessageBox.Show("File Uploaded");
                    });
                }
                if (data_d[0].CompareTo("Packet_recived") == 0 && Convert.ToInt64(data_d[1]) == file_current_packet_number)
                {
                    file_current_packet_number++;
                    get_ft_packet_data();
                    
                }
            }
            #endregion

            #region FT_DATA
            if (command.CompareTo("FT_DATA") == 0)
            {
                store_data_from_packet(byte_data);
            }
            #endregion

            #region P2P_ADDRESS
            if (command.CompareTo("P2P_ADDRESS") == 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    String ep = Encoding.ASCII.GetString(byte_data);
                    String[] data_d = ep.Split(':');
                    //txtip.Text = data_d[0];
                    //txtport.Text = data_d[1];

                    Thread.Sleep(500);
                    IPAddress ipAddress = IPAddress.Parse(data_d[0]);
                    p2pEndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(data_d[1]));
                    timer_send_to_p2p.Start();
                });
            }
            #endregion

           

            #region STR
            if (command.CompareTo("STR") == 0)
            {
                try
                {
                    String msg = Encoding.ASCII.GetString(byte_data);
                    if (old_STR_str.CompareTo(msg) != 0)
                    {
                        old_STR_str = msg;
                        textBox1.Invoke((MethodInvoker)delegate
                        {
                            textBox1.Text = msg;
                        });
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region FILE_UPLOAD
            if (command.CompareTo("FILE_UPLOAD") == 0)
            {
                try
                {
                    if (old_FILE_UPLOAD_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_FILE_UPLOAD_str = Encoding.ASCII.GetString(byte_data);
                        String[] file_str = old_FILE_UPLOAD_str.Split(';');
                        
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region FILE_DOWNLOAD
            if (command.CompareTo("FILE_DOWNLOAD") == 0)
            {
                try
                {
                    if (old_FILE_DOWNLOAD_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_FILE_DOWNLOAD_str = Encoding.ASCII.GetString(byte_data);

                        

                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region MOUSE
            if (command.CompareTo("MOUSE") == 0)
            {
                try
                {
                    if (old_MOUSE_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_MOUSE_str = Encoding.ASCII.GetString(byte_data);
                        String[] mouse_str = Encoding.ASCII.GetString(byte_data).Split(':');
                        if (mouse_str[2].Contains(" "))
                            mouse_str[2] = mouse_str[2].Split(' ')[0];
                        try
                        {
                            mc.process_mouse(Convert.ToInt16(mouse_str[0]), Convert.ToInt16(mouse_str[1]), Convert.ToInt16(mouse_str[2]));
                        }
                        catch (Exception ex)
                        { }
                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region KEYBOARD
            if (command.CompareTo("KEYBOARD") == 0)
            {
                try
                {
                    if (old_KEYBOARD_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_KEYBOARD_str = Encoding.ASCII.GetString(byte_data);
                        kb.process_Keyboard(byte_data);
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region SYSTEM
            if (command.CompareTo("SYSTEM") == 0)
            {
                try
                {
                    if (old_SYSTEM_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_SYSTEM_str = Encoding.ASCII.GetString(byte_data);
                        String[] str_system = old_SYSTEM_str.Split(':');
                        if (str_system[1].CompareTo("hibernate") == 0)
                        {
                            Process.Start("shutdown", "/h /f"); 
                        }
                        if (str_system[1].CompareTo("shutdown") == 0)
                        {
                            Process.Start("shutdown", "/r /t 0"); 
                        }
                        if (str_system[1].CompareTo("restart") == 0)
                        {
                            Process.Start("shutdown", "/s /t 0");
                        }
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region PROCESS
            if (command.CompareTo("PROCESS") == 0)
            {
                try
                {
                    if (old_PROCESS_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_PROCESS_str = Encoding.ASCII.GetString(byte_data);
                        String[] xxx = old_PROCESS_str.Split(':');
                        String str = xxx[0];
                        if (str.CompareTo("get_process") == 0)
                        {
                            String process_list = "";
                            Process[] processlist = Process.GetProcesses();

                            foreach (Process theprocess in processlist)
                            {
                                process_list += theprocess.Id.ToString() + "\t";
                                process_list += theprocess.ProcessName.ToString() + "\t";
                                process_list += theprocess.MainWindowTitle.ToString() + "\t";
                                process_list += theprocess.Responding.ToString() + "\t";
                                process_list += Convert.ToString(theprocess.WorkingSet) + "\t";
                                process_list += "\n";
                            }

                            byte[] data1 = dc.Add_Payload("PROCESS", process_list);
                            try
                            {
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                //byte[] msg_k = Encoding.ASCII.GetBytes("process_manger:" + process_list);
                                //tcp_send_socket.Send(msg_k);
                            }
                            catch { }
                        }
                        if (str.CompareTo("kill_process") == 0)
                        {
                            String pid = xxx[1];
                            Process proc = Process.GetProcessById(Convert.ToInt16(pid));
                            proc.Kill();
                        }

                    }
                }
                catch (Exception ex)
                {
                   write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region COMMAND
            if (command.CompareTo("COMMAND") == 0)
            {
                try
                {
                    if (old_COMMAND_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_COMMAND_str = Encoding.ASCII.GetString(byte_data);
                        String str = old_COMMAND_str;
                        Process process = new Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = "/c " + str; // Note the /c command (*)
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        string err = process.StandardError.ReadToEnd();
                        process.WaitForExit();
                        if (output.Length > 0)
                        {                            
                            byte[] data1 = dc.Add_Payload("COMMAND", output);
                            try
                            {
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                            }
                            catch { }
                        }
                        if (err.Length > 0)
                        {
                            byte[] data1 = dc.Add_Payload("COMMAND", err);
                            try
                            {
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                                clientSocket.Send(data1, data1.Length, p2pEndPoint);
                            }
                            catch { }
                        }
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

         

            #region FILE_SYSTEM
            if (command.CompareTo("FILE_SYSTEM") == 0)
            {
                try
                {
                    if (old_FILE_SYSTEM_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_FILE_SYSTEM_str = Encoding.ASCII.GetString(byte_data);
                        String[] file_system_str = old_FILE_SYSTEM_str.Split(';');
                        if (file_system_str[0].CompareTo("load_drive") == 0)
                        {
                            String drive_list = "";
                            DriveInfo[] drive = DriveInfo.GetDrives();
                            foreach (DriveInfo d in drive)
                            {
                                drive_list = drive_list + d.ToString() + "\n";
                            }
                            byte[] file_system_data = dc.Add_Payload("FILE_SYSTEM", drive_list);
                            try
                            {
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            }
                            catch { }
                        }
                        if (file_system_str[0].CompareTo("load_folder") == 0)
                        {
                            String node_list = "";
                            DirectoryInfo dirname = new DirectoryInfo(file_system_str[1]);
                            foreach (FileInfo fi in dirname.GetFiles())
                            {
                                node_list = node_list + fi.FullName + "\n";
                            }
                            foreach (DirectoryInfo di in dirname.GetDirectories())
                            {
                                node_list = node_list + di.FullName + "\n";
                            }
                            byte[] file_system_data = dc.Add_Payload("FILE_SYSTEM", node_list);
                            try
                            {
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            }
                            catch { }
                        }
                        if (file_system_str[0].CompareTo("document") == 0)
                        {
                            String node_list = "";
                            DirectoryInfo dirname = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                            foreach (FileInfo fi in dirname.GetFiles())
                            {
                                node_list = node_list + fi.FullName + "\n";
                            }
                            foreach (DirectoryInfo di in dirname.GetDirectories())
                            {
                                node_list = node_list + di.FullName + "\n";
                            }
                            byte[] file_system_data = dc.Add_Payload("FILE_SYSTEM", node_list);
                            try
                            {
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            }
                            catch { }
                        }
                        if (file_system_str[0].CompareTo("desktop") == 0)
                        {
                            String node_list = "";
                            DirectoryInfo dirname = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                            foreach (FileInfo fi in dirname.GetFiles())
                            {
                                node_list = node_list + fi.FullName + "\n";
                            }
                            foreach (DirectoryInfo di in dirname.GetDirectories())
                            {
                                node_list = node_list + di.FullName + "\n";
                            }
                            byte[] file_system_data = dc.Add_Payload("FILE_SYSTEM", node_list);
                            try
                            {
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            }
                            catch { }
                        }
                        if (file_system_str[0].CompareTo("file_upload_info") == 0)
                        {
                            path = file_system_str[1];
                            file_name = file_system_str[2];
                            file_size = Convert.ToInt64(file_system_str[3]);
                            file_data = new byte[file_size];
                            if (file_size <= 5120)
                            {
                                file_number_of_packet = 1;
                            }
                            else
                            {
                                file_number_of_packet = file_size / 5120;
                                if (file_data.Length % 5120 != 0)
                                {
                                    file_number_of_packet++;
                                }
                            }
                            file_current_packet_number = 1;
                            byte[] file_system_data = dc.Add_Payload("FT_INFO", "file_upload_request_recive");

                            clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);

                            //File.WriteAllBytes(path+"\\"+ file_name, file_data);
                        }
                        if (file_system_str[0].CompareTo("file_info_request") == 0)
                        {
                            String file_download_path = file_system_str[1];
                            String file_to_download = file_system_str[2];
                            byte[] bytes = File.ReadAllBytes(file_to_download);
                            String size_of_file = bytes.Length.ToString();

                            String xx = "file_size;" + size_of_file;
                            byte[] file_system_data = dc.Add_Payload("FT_INFO", xx);
                            file_current_packet_number = 1;
                            clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                            clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);



                        }
                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region CLIPBOARD
            if (command.CompareTo("CLIPBOARD") == 0)
            {
                try
                {
                    if (old_CLIPBOARD_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_CLIPBOARD_string = Encoding.ASCII.GetString(byte_data);
                        this.Invoke((MethodInvoker)delegate
                        {
                            try
                            {
                                Clipboard.SetText(old_CLIPBOARD_string);
                            }
                            catch(Exception ex) 
                            {
                                write_log("error :-" + ex.ToString());
                            }
                        });
                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region LOGIN_NAME
            if (command.CompareTo("LOGIN_NAME") == 0)
            {
                try
                {
                    if (old_BLACK_SCREEN_LOGIN_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_BLACK_SCREEN_LOGIN_string = Encoding.ASCII.GetString(byte_data);
                        scree_control_login_name = Encoding.ASCII.GetString(byte_data);

                        Thread bc = new Thread(check_black_screen_login);
                        bc.IsBackground = true;
                        bc.Start();
                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region BLACK_SCREEN
            if (command.CompareTo("BLACK_SCREEN") == 0)
            {
                try
                {
                    if (old_BLACK_SCREEN_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_BLACK_SCREEN_string = Encoding.ASCII.GetString(byte_data);
                        if (old_BLACK_SCREEN_string.Contains(':'))
                        {
                            String[] temp_str = old_BLACK_SCREEN_string.Split(':');
                            if (temp_str[0].CompareTo("delete_img") == 0)
                            {
                                File.Delete("C:\\Users\\Public\\" + "\\temp_img\\" + temp_str[1]);
                            }
                        }
                        else
                        {
                            if (old_BLACK_SCREEN_string.CompareTo("Activet") == 0)
                            {
                              

                            }
                            if (old_BLACK_SCREEN_string.CompareTo("show") == 0)
                            {


                                byte[] file_system_data = dc.Add_Payload("BLACK_SCREEN", "BlackScreen Showing");
                                try
                                {
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                }
                                catch { }
                                try
                                {
                                    Scaling_mon(0, 100);
                                    Thread.Sleep(1000);
                                    Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                                    IntPtr desktop = g.GetHdc();
                                    Globle_data.LogicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenWidth);
                                    Globle_data.LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenHeight);
                                    Globle_data.PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenWidth);
                                    Globle_data.PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenHeight);

                                    int nW = Globle_data.PhysicalScreenWidth, nH = Globle_data.PhysicalScreenHeight;
                                   // sX = nW / nNum;
                                   // sY = nH / nNum;
                                    bmp = new Bitmap(Globle_data.PhysicalScreenWidth, Globle_data.PhysicalScreenHeight);

                                }
                                catch (Exception ex)
                                { }
                                this.Invoke((MethodInvoker)delegate
                                {

                                    Blackscreen_form.timer1.Start();
                                    Blackscreen_form.timer2.Start();
                                    Blackscreen_form.Show();

                                    capture_main_screen = false;
                                    capture_black_screen = true;
                                    black_screen_fun();
                                });

                            }
                            if (old_BLACK_SCREEN_string.CompareTo("remove") == 0)
                            {



                                byte[] file_system_data = dc.Add_Payload("BLACK_SCREEN", "BlackScreen Removing");
                                try
                                {
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                    clientSocket.Send(file_system_data, file_system_data.Length, p2pEndPoint);
                                }
                                catch { }
                                    try
                                {
                                    RestoreDPIScaling();

                                    Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                                    IntPtr desktop = g.GetHdc();
                                    Globle_data.LogicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenWidth);
                                    Globle_data.LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenHeight);
                                    Globle_data.PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenWidth);
                                    Globle_data.PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenHeight);

                                    int nW = Globle_data.PhysicalScreenWidth, nH = Globle_data.PhysicalScreenHeight;
                                    //sX = nW / nNum;
                                   // sY = nH / nNum;
                                    bmp = new Bitmap(Globle_data.PhysicalScreenWidth, Globle_data.PhysicalScreenHeight);

                                }
                                catch
                                { }
                                this.Invoke((MethodInvoker)delegate
                                {
                                    Blackscreen_form.Hide();
                                    Blackscreen_form.timer1.Stop();
                                    Blackscreen_form.timer2.Stop();

                                    capture_main_screen = true;
                                    capture_black_screen = false;
                                    main_screen_fun();
                                });
                            }
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region BLACK_SCREEN_ING
            if (command.CompareTo("BLACK_SCREEN_ING") == 0)
            {
                try
                {
                    if (old_BLACK_SCREEN_ING_LENGTH != byte_data.Length)
                    {
                        old_BLACK_SCREEN_ING_LENGTH = byte_data.Length;
                        Image img_x = null;
                        using (WebP webp = new WebP())
                        {
                            img_x = webp.Decode(byte_data);
                        }
                        Blackscreen_form.pictureBox1.Image = (Bitmap)img_x;
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region BLACK_SCREEN_IMAGE_NAME
            if (command.CompareTo("BLACK_SCREEN_IMAGE_NAME") == 0)
            {
                try
                {
                    if (old_BLACK_SCREEN_IMAGE_NAME_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_BLACK_SCREEN_IMAGE_NAME_str = Encoding.ASCII.GetString(byte_data);
                        
                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region Exit
            if (command.CompareTo("Exit") == 0)
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        Blackscreen_form.timer1.Stop();
                        Blackscreen_form.timer2.Stop();
                        Blackscreen_form.Dispose();
                      
                        capture_black_screen = false;
                        capture_main_screen = false;
                        MessageBox.Show("Remote PC Close this session");
                        this.Close();
                        this.Dispose();
                    });
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region DESKTOP_AS_BLACK_SCREEN
            if (command.CompareTo("DESKTOP_AS_BLACK_SCREEN") == 0)
            {
                try
                {
                    if (old_DESKTOP_AS_BLACK_SCREEN_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_DESKTOP_AS_BLACK_SCREEN_str = Encoding.ASCII.GetString(byte_data);
                        Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                        IntPtr desktop = g.GetHdc();
                        Globle_data.LogicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenWidth);
                        Globle_data.LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenHeight);
                        Globle_data.PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenWidth);
                        Globle_data.PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenHeight);
                        Rectangle bounds = new Rectangle(0, 0, Globle_data.PhysicalScreenWidth, Globle_data.PhysicalScreenHeight);
                        Bitmap result = new Bitmap(bounds.Width, bounds.Height);
                        try
                        {
                            using (Graphics gg = Graphics.FromImage(result))
                            {
                                gg.CopyFromScreen(bounds.Location, System.Drawing.Point.Empty, bounds.Size);
                            }
                            Blackscreen_form.pictureBox1.Image = result;
                        }
                        catch (Exception ex)
                        {
                            write_log("error :-" + ex.ToString());
                        }

                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region IMG_RTP
            if (command.CompareTo("IMG_RTP") == 0)
            {
                try
                {
                    image_rtp_byte = byte_data;
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion
        xyz: ;
           /* Thread t = new Thread(recive_fun);
            t.IsBackground = true;
            t.Start();*/

            Task t1 = new Task(recive_fun);
            t1.Start();

        }

        public void check_black_screen_login()
        {
            if (scree_control_login_name.CompareTo("") != 0 || scree_control_login_name != String.Empty)
            {
                using (var client = new WebClient())
                {
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;

                    var postData = new System.Collections.Specialized.NameValueCollection();
                    postData.Add("username", scree_control_login_name);
                    byte[] response = client.UploadValues("https://LetUsDo.com//rd_check_login_for_black_screen.php", "POST", postData);
                    var responsebody = Encoding.UTF8.GetString(response);
                    // MessageBox.Show(responsebody.ToString());
                    if (Convert.ToInt16(responsebody) == 0)
                    {
                        black_screen_status = false;
                        
                    }
                    else
                    {
                        black_screen_status = true;
                       
                    }
                }
            }
            else
            {
                black_screen_status = false;
               
            }
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        public void Empty(DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch(Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            {
                try
                {
                    subDirectory.Delete(true);
                }
                catch(Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = dc.Add_Payload("STR", textBox1.Text);
            try
            {
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
            }
            catch { }
        }

        private void Image_sender_FormClosing(object sender, FormClosingEventArgs e)
        {

            byte[] to_send = dc.Add_Payload("Exit", "Exit");
            clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
        }

        private void timer_hartbit_Tick(object sender, EventArgs e)
        {
            byte[] to_send = dc.Add_Payload("HARTBIT", "HB");
            try
            {
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            }
            catch { }
        }

        private void btnns_Click(object sender, EventArgs e)
        {
           
        }

        private void btnbs_Click(object sender, EventArgs e)
        {
          

        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timercheck_reciver_Tick(object sender, EventArgs e)
        {
            if (found_reciver == false)
            {
                MessageBox.Show("Reciver not found");
                this.Dispose();
            }
        }

        private void timer_send_to_sesrver_Tick(object sender, EventArgs e)
        {
            byte[] byteData = dc.Add_Payload("STR", "hi");
            try
            {
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            }
            catch { }
            }

        private void timer_send_to_p2p_Tick(object sender, EventArgs e)
        {
            /*byte[] byteData = dc.Add_Payload("STR", "p2p");
            try
            {
                clientSocket.Send(byteData, byteData.Length, p2pEndPoint);
            }
            catch { }*/
            try
            {
                if (p2pEndPoint != serverEndPoint)
                {
                    byte[] byteData = dc.Add_Payload("STR", "p2p");
                    clientSocket.Send(byteData, byteData.Length, p2pEndPoint);
                  
                }
                else
                {
                    byte[] byteData = dc.Add_Payload("STR", "server");
                    clientSocket.Send(byteData, byteData.Length, p2pEndPoint);
                   

                }
            }
            catch
            { }
        }

        private void timer_send_rtp_image_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < screen_matrix.Length; i++)
                {
                    if (image_rtp_byte[i] == 0)
                    // if (image_rtp_byte[i] == 1)
                    {

                        switch (i)
                        {
                            case 0:
                                byte[] to_send_0 = dc.Add_Payload("IMG", img_0_webp_bytes);
                                try
                                {
                                    clientSocket.Send(to_send_0, to_send_0.Length, p2pEndPoint);
                                }
                                catch { }
                                    break;
                            case 1:
                                byte[] to_send_1 = dc.Add_Payload("IMG", img_1_webp_bytes);
                                try
                                {
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                }
                                catch { }
                                    break;
                            case 2:
                                byte[] to_send_2 = dc.Add_Payload("IMG", img_2_webp_bytes);
                                try
                                {
                                    clientSocket.Send(to_send_2, to_send_2.Length, p2pEndPoint);
                                }
                                catch { }
                                    break;
                            case 3:
                                byte[] to_send_3 = dc.Add_Payload("IMG", img_3_webp_bytes);
                                try
                                {
                                    clientSocket.Send(to_send_3, to_send_3.Length, p2pEndPoint);
                                }
                                catch { }
                                    break;

                        }
                        // Globle_data.image_sending_time = DateTime.UtcNow.Millisecond;
                        // Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
       

        public void write_log(String log)
        {
            if (!log.Contains("error :-"))
            {
                try
                {
                   // String log_msg = log + "\t at :-" + DateTime.Now.ToString() + "\t\n";
                   // File.AppendAllText("log.txt", log_msg);
                }
                catch (Exception ex)
                { }
            }
        }

        private void ScreenShot()
        {
            

            Rectangle bounds = new Rectangle(0, 0, Globle_data.PhysicalScreenWidth, Globle_data.PhysicalScreenHeight);
            //Bitmap result = new Bitmap(bounds.Width, bounds.Height);
            try
            {
                using (Graphics gg = Graphics.FromImage(bmp))
                {
                    gg.CopyFromScreen(bounds.Location, System.Drawing.Point.Empty, bounds.Size);
                }
            }
            catch (Exception ex)
            {

            }
           // return result;
        }

        public void main_screen_fun()
        {

            Thread thread = new Thread(t =>
            {
                while (capture_main_screen)
                {

                    ScreenShot();
                   
                    Byte[] image_change = new Byte[4];
                    image_change[0] = (byte)1;
                    image_change[1] = (byte)1;
                    image_change[2] = (byte)1;
                    image_change[3] = (byte)1;
                    k = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Rectangle bounds_0 = new Rectangle(0, 0, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_0, bmp.PixelFormat);
                                break;
                            case 1:
                                Rectangle bounds_1 = new Rectangle(Globle_data.PhysicalScreenWidth / 2, 0, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_1, bmp.PixelFormat);
                                break;
                            case 2:
                                Rectangle bounds_2 = new Rectangle(0, Globle_data.PhysicalScreenHeight / 2, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_2, bmp.PixelFormat);
                                break;
                            case 3:
                                Rectangle bounds_3 = new Rectangle(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_3, bmp.PixelFormat);
                                break;

                        }


                        if (c[i].PercentageDifference(p[i], 5) != 0 || found_reciver == false)
                        {
                            p[i] = c[i];
                            k++;
                            byte[] rawWebP;
                            using (WebP webp = new WebP())
                            {
                                rawWebP = webp.EncodeLossy(c[i], 10);
                            }
                            byte[] tempbyte = new byte[rawWebP.Length + 1];
                            tempbyte[0] = (Byte)i;
                            //Array.Copy(tempbyte, 1, rawWebP, 0, rawWebP.Length);
                            Array.Copy(rawWebP, 0, tempbyte, 1, rawWebP.Length);
                            switch (i)
                            {
                                case 0:
                                    img_0_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_0_webp_bytes, 0, tempbyte.Length);
                                    image_change[0] = (byte)0;
                                    break;
                                case 1:
                                    img_1_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_1_webp_bytes, 0, tempbyte.Length);
                                    image_change[1] = (byte)0;
                                    break;
                                case 2:
                                    img_2_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_2_webp_bytes, 0, tempbyte.Length);
                                    image_change[2] = (byte)0;
                                    break;
                                case 3:
                                    img_3_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_3_webp_bytes, 0, tempbyte.Length);
                                    image_change[3] = (byte)0;
                                    break;
                            }
                        }
                    }


                   

                    if (found_reciver == false)
                    {
                        image_change[0] = (byte)0;
                        image_change[1] = (byte)0;
                        image_change[2] = (byte)0;
                        image_change[3] = (byte)0;


                        Array.Clear(image_rtp_byte, 0, image_rtp_byte.Length);
                        image_rtp_byte[0] = image_change[0];
                        image_rtp_byte[1] = image_change[1];
                        image_rtp_byte[2] = image_change[2];
                        image_rtp_byte[3] = image_change[3];

                        byte[] to_send = dc.Add_Payload("NEW_IMG", image_change);
                        try { 
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        }
                        catch { }
                        Thread.Sleep(k * 400);

                    }
                    else
                    { 
                    if (k != 0)
                    {
                        Array.Clear(image_rtp_byte, 0, image_rtp_byte.Length);
                        image_rtp_byte[0] = image_change[0];
                        image_rtp_byte[1] = image_change[1];
                        image_rtp_byte[2] = image_change[2];
                        image_rtp_byte[3] = image_change[3];

                        byte[] to_send = dc.Add_Payload("NEW_IMG", image_change);
                            try
                            {
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                            }
                            catch
                            { }
                        Thread.Sleep(k * 100);
                    }
                    else
                    {
                        try
                        {
                            Array.Clear(image_rtp_byte, 0, image_rtp_byte.Length);
                            image_rtp_byte[0] = image_change[0];
                            image_rtp_byte[1] = image_change[1];
                            image_rtp_byte[2] = image_change[2];
                            image_rtp_byte[3] = image_change[3];

                            byte[] to_send_1 = dc.Add_Payload("NO_IMAGE_CHANGE", DateTime.Now.Millisecond.ToString());
                                try
                                {
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                }
                                catch
                                { }
                        }
                        catch (Exception ex)
                        { }


                        Thread.Sleep(100);
                    }
                    }

                    if (timer_send_rtp_image.Enabled == false)
                        timer_send_rtp_image.Enabled = true;
                }
            })
            { IsBackground = true };
            thread.Start();
        }
        public void black_screen_fun()
        {

            Thread thread = new Thread(t =>
            {
                while (capture_black_screen)
                {
                    bmp =(Bitmap) Blackscreen_form.bitmap_image;
                    if(bmp!=null)
                    {
                     Byte[] image_change = new Byte[4];
                    image_change[0] = (byte)1;
                    image_change[1] = (byte)1;
                    image_change[2] = (byte)1;
                    image_change[3] = (byte)1;
                    k = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Rectangle bounds_0 = new Rectangle(0, 0, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_0, bmp.PixelFormat);
                                break;
                            case 1:
                                Rectangle bounds_1 = new Rectangle(Globle_data.PhysicalScreenWidth / 2, 0, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_1, bmp.PixelFormat);
                                break;
                            case 2:
                                Rectangle bounds_2 = new Rectangle(0, Globle_data.PhysicalScreenHeight / 2, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_2, bmp.PixelFormat);
                                break;
                            case 3:
                                Rectangle bounds_3 = new Rectangle(Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2, Globle_data.PhysicalScreenWidth / 2, Globle_data.PhysicalScreenHeight / 2);
                                c[i] = bmp.Clone(bounds_3, bmp.PixelFormat);
                                break;

                        }


                        if (c[i].PercentageDifference(p[i], 5) != 0 || found_reciver == false)
                        {
                            p[i] = c[i];
                            k++;
                            byte[] rawWebP;
                            using (WebP webp = new WebP())
                            {
                                rawWebP = webp.EncodeLossy(c[i], 10);
                            }
                            byte[] tempbyte = new byte[rawWebP.Length + 1];
                            tempbyte[0] = (Byte)i;
                            //Array.Copy(tempbyte, 1, rawWebP, 0, rawWebP.Length);
                            Array.Copy(rawWebP, 0, tempbyte, 1, rawWebP.Length);
                            switch (i)
                            {
                                case 0:
                                    img_0_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_0_webp_bytes, 0, tempbyte.Length);
                                    image_change[0] = (byte)0;
                                    break;
                                case 1:
                                    img_1_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_1_webp_bytes, 0, tempbyte.Length);
                                    image_change[1] = (byte)0;
                                    break;
                                case 2:
                                    img_2_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_2_webp_bytes, 0, tempbyte.Length);
                                    image_change[2] = (byte)0;
                                    break;
                                case 3:
                                    img_3_webp_bytes = new Byte[tempbyte.Length];
                                    Array.Copy(tempbyte, 0, img_3_webp_bytes, 0, tempbyte.Length);
                                    image_change[3] = (byte)0;
                                    break;
                            }
                        }
                    }


                   

                    if (found_reciver == false)
                    {
                        image_change[0] = (byte)0;
                        image_change[1] = (byte)0;
                        image_change[2] = (byte)0;
                        image_change[3] = (byte)0;


                        Array.Clear(image_rtp_byte, 0, image_rtp_byte.Length);
                        image_rtp_byte[0] = image_change[0];
                        image_rtp_byte[1] = image_change[1];
                        image_rtp_byte[2] = image_change[2];
                        image_rtp_byte[3] = image_change[3];

                        byte[] to_send = dc.Add_Payload("NEW_IMG", image_change);
                        try { 
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        }
                        catch { }
                        Thread.Sleep(k * 400);

                    }
                    else
                    { 
                    if (k != 0)
                    {
                        Array.Clear(image_rtp_byte, 0, image_rtp_byte.Length);
                        image_rtp_byte[0] = image_change[0];
                        image_rtp_byte[1] = image_change[1];
                        image_rtp_byte[2] = image_change[2];
                        image_rtp_byte[3] = image_change[3];

                        byte[] to_send = dc.Add_Payload("NEW_IMG", image_change);
                            try
                            {
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                            }
                            catch
                            { }
                        Thread.Sleep(k * 100);
                    }
                    else
                    {
                        try
                        {
                            Array.Clear(image_rtp_byte, 0, image_rtp_byte.Length);
                            image_rtp_byte[0] = image_change[0];
                            image_rtp_byte[1] = image_change[1];
                            image_rtp_byte[2] = image_change[2];
                            image_rtp_byte[3] = image_change[3];

                            byte[] to_send_1 = dc.Add_Payload("NO_IMAGE_CHANGE", DateTime.Now.Millisecond.ToString());
                                try
                                {
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                    clientSocket.Send(to_send_1, to_send_1.Length, p2pEndPoint);
                                }
                                catch
                                { }
                        }
                        catch (Exception ex)
                        { }


                        Thread.Sleep(100);
                    }
                    }

                    if (timer_send_rtp_image.Enabled == false)
                        timer_send_rtp_image.Enabled = true;

                    }
                }

                

            })
            { IsBackground = true };
            thread.Start();
        }

        private void timer_ft_Tick(object sender, EventArgs e)
        {
            byte[] to_send = dc.Add_Payload("FT_DATA", ft_packet_data);
            try
            {
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            }
            catch { }
        }
    }
}
