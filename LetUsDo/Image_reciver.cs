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
using System.IO;
using System.ServiceProcess;
using System.Drawing;
using System.Drawing.Imaging;
namespace LetUsDo
{
    public partial class Image_reciver : Form
    {

        public UdpClient clientSocket; //The main client socket
        public IPEndPoint p2pEndPoint;   //The EndPoint of the server
        public IPEndPoint serverEndPoint;
        public IPEndPoint tempEndPoint;

        string serverIP = "";
        int serverPort = 4000;
        Data_Class dc = new Data_Class();
        Point old_p = new Point(0, 0);
        String old_PROCESS_str="";
        String old_COMMAND_str = "";
        String old_CHAT_str = "";
        String old_FILE_SYSTEM_string = "";
        String old_CLIPBOARD_string = "";
        String old_FILE_UPLOAD_COMPLIT_string = "";

        String old_FILE_DOWNLOAD_COMPLIT_string = "";

        String old_NO_IMAGE_CHANGE_string = "";

     
      
      
        frm_file_system ffs = new frm_file_system();
   
        String old_BLACK_SCREEN_string = "";
        String old_BLACK_SCREEN_IMAGE_LIST_string = "";
        String old_BLACK_SCREEN_IMAGE_RECIVED_string = "";

        String sender_startup_path = "";
        String file_to_download = "";

        Bitmap[] screen_matrix = new Bitmap[Globle_data.img_matrix * Globle_data.img_matrix];
        byte[] recive_image_byte = new byte[Globle_data.img_matrix * Globle_data.img_matrix];



        String file_to_upload = "";
        byte[] file_data;
        Int64 file_number_of_packet;
        Int64 file_current_packet_number;
        byte[] ft_packet_data = new byte[5120];
        String path = "";
        String file_name = "";
        Int64 file_size = 0;
        String file_to_download_ = "";
        String file_download_path = "";


        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        ToolTip tt = new ToolTip();
        public Image join_image()
        {
            Image img = null;
            try
            {
                int imgwidht=0, imgheight=0;
                int timgw = 0, timgh = 0;
                for (int i = 0; i < Globle_data.img_matrix * Globle_data.img_matrix; i++)
                {
                    if (screen_matrix[i] != null)
                    {
                        imgwidht = screen_matrix[i].Width * Globle_data.img_matrix;
                        imgheight = screen_matrix[i].Height * Globle_data.img_matrix;
                        timgw = screen_matrix[i].Width;
                        timgh = screen_matrix[i].Height;
                    }
                }

                

                Bitmap bmp = new Bitmap(imgwidht, imgheight);
                Bitmap tbmp = new Bitmap(timgw, timgh);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    int k = 0;
                    for (int i = 0; i < Globle_data.img_matrix; i++)
                    {
                        for (int j = 0; j < Globle_data.img_matrix; j++)
                        {
                            try
                            {
                                if (screen_matrix[k] == null)
                                {
                                    screen_matrix[k] = tbmp;
                                }
                                g.DrawImage(screen_matrix[k], screen_matrix[k].Width * j, screen_matrix[k].Height * i, screen_matrix[k].Width, screen_matrix[k].Height);
                                k++;
                            }
                            catch (Exception ex)
                            {
                                write_log("error :-" + ex.ToString());
                            }
                        }
                    }
                }
                img = (Image)bmp;

            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return img;

        }


        public Image_reciver(int server_Port,String server_ip)
        {
            InitializeComponent();
            serverPort = server_Port;
            serverIP = server_ip;
            ClipboardMonitor.OnClipboardChange += ClipboardMonitor_OnClipboardChange;
            ClipboardMonitor.Start();
        }
        private void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            try
            {
                String Clipboard_str = Clipboard.GetText();
                byte[] byte_data = dc.Add_Payload("CLIPBOARD", Clipboard_str);
                try
                {
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                }
                catch { }
            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
        }
        private void Image_reciver_Load(object sender, EventArgs e)
        {
            tt.SetToolTip(pbsleep, "Sleep");
            tt.SetToolTip(pbshoutdown, "Shut Downl");
            tt.SetToolTip(pbrestart, "Restart");
            tt.SetToolTip(pbhibernate, "Hibernet");
            tt.SetToolTip(pblock, "Lock");
            tt.SetToolTip(pbfile_transfer, "File Transfer");
            tt.SetToolTip(pbshowblack_screen, "Show Black Screen");
            tt.SetToolTip(pbremove_blackscreen, "Remove Black Screen");
            tt.SetToolTip(pbshow_image_on_blackscreen, "Show Image On Black Screen");
            tt.SetToolTip(pbupload_image_for_blackscreen, "Upload Image Which Show on Black Screen");
            


            pictureBox1.MouseWheel += mouse_wheel;
            this.MouseWheel += mouse_wheel;
            side_panel.MouseWheel += mouse_wheel;

           /* string path = Application.StartupPath + "\\upload_file";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }*/

            this.KeyPreview = true;
            IPAddress ipAddress = IPAddress.Parse(serverIP);
            //p2pEndPoint = new IPEndPoint(
            serverEndPoint = new IPEndPoint(ipAddress, serverPort);
            Random r = new Random();
            int this_pc_port = r.Next(10000, 20000);
            try
            {
                clientSocket = new UdpClient(this_pc_port);
                clientSocket.Client.ReceiveTimeout = 10;
                clientSocket.Client.SendTimeout = 10;
            }
            catch { }

           
            write_log("open image receiver ip " + serverIP.ToString() + " port " + serverPort);

           
            

           // clientSocket.Client.SendBufferSize = 1024*64;
            byte[] byteData = dc.Add_Payload("STR", "hi i am reciver");
            try
            {
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);

                Thread.Sleep(200);

                byteData = dc.Add_Payload("LOGIN_NAME", Globle_data.login_user_name);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
                clientSocket.Send(byteData, byteData.Length, serverEndPoint);
            }
            catch { }
            timer_othre_window.Enabled = true;
          

           // Thread t = new Thread(recive_fun);
          //  t.IsBackground = true;
           // t.Start();


            Task t1 = new Task(recive_fun);
            t1.Start();

            Task c1 = new Task(check_data_comming_from_where);
            c1.Start();


            side_panel.Width = 1;
            side_panel.Left = this.Width - 5;
            arreng_pbsetting();

            lab_msg.Text = "Please wait. while we are connecting...";
            sw.Start();
           
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
            /*if (pkno < file_current_packet_number)
            {
                String fpk_info = "Packet_recived;" + pkno.ToString();
                file_current_packet_number++;
                byte[] file_system_data_x = dc.Add_Payload("FT_INFO", fpk_info);

                clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                goto abc;
            }*/
            if (pkno != file_current_packet_number)
                goto abc;
            if (file_data.Length <= 5120)
            {
                // only one packet
                Array.Copy(byte_data, 8, file_data, 0, byte_data.Length - 8);
                // save file 
                String[] parray = file_to_download_.Split('\\');
                String fn = parray[parray.Length - 1];
                File.WriteAllBytes(file_download_path +"\\"+ fn, file_data);
                byte[] file_system_data = dc.Add_Payload("FT_INFO", "file_upload_ok");
                this.Invoke((MethodInvoker)delegate {
                    MessageBox.Show("Download complit");
                });
               
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
                    String[] parray = file_to_download_.Split('\\');
                    String fn = parray[parray.Length - 1];
                    File.WriteAllBytes(file_download_path + "\\" + fn, file_data);
                    byte[] file_system_data_x = dc.Add_Payload("FT_INFO", "file_upload_ok");
                    clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                    // clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                    // clientSocket.Send(file_system_data_x, file_system_data_x.Length, p2pEndPoint);
                    this.Invoke((MethodInvoker)delegate {
                        MessageBox.Show("Download complit");
                    });
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
                this.Invoke((MethodInvoker)delegate
                {
                    try 
                    {
                        ffs.progressBar1.Value = (int)file_current_packet_number;
                        ffs.labtotal_packet.Text = file_number_of_packet.ToString();
                        ffs.labcurrent_packet.Text = file_current_packet_number.ToString();
                    } catch
                    { }
                });
            }
            abc:;
        }
        public void recive_fun()
        {
            byte[] data;

            try
            {
                data = clientSocket.Receive(ref tempEndPoint);
                //data = clientSocket.Receive(

            }
            catch(Exception ex)
            {
                write_log("error :-" + ex.ToString());
                goto xyz;
            }

            String command="";
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
                    this.Invoke((MethodInvoker)delegate {
                        try
                        {
                            ffs.progressBar1.Value = Convert.ToInt32(file_current_packet_number);
                        }
                        catch (Exception ex)
                        { }
                    });
                }
                if (data_d[0].CompareTo("file_size") == 0)
                {
                    file_size = Convert.ToInt64(data_d[1]);
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

                    this.Invoke((MethodInvoker)delegate {
                        ffs.progressBar1.Maximum = (int)file_number_of_packet;
                    });
                    
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

            #region NO_IMAGE_CHANGE

            if (command.CompareTo("NO_IMAGE_CHANGE") == 0)
            {
                try
                {
                    if (old_NO_IMAGE_CHANGE_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_NO_IMAGE_CHANGE_string = Encoding.ASCII.GetString(byte_data);

                       /* Thread thread = new Thread(() => Process_NO_IMAGE_CHANGE(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_NO_IMAGE_CHANGE(byte_data));
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }

            #endregion

            #region MIC
            if (command.CompareTo("MIC") == 0)
            {
                try
                {
                   
                   /* Thread thread = new Thread(() => Process_MIC(byte_data));
                    thread.IsBackground = true;
                    thread.Start();*/
                    Task.Run(() => Process_MIC(byte_data));
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }

            #endregion

            #region FILE_UPLOAD_COMPLIT
            if (command.CompareTo("FILE_UPLOAD_COMPLIT") == 0)
            {
                try
                {
                    if (old_FILE_UPLOAD_COMPLIT_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_FILE_UPLOAD_COMPLIT_string = Encoding.ASCII.GetString(byte_data);
                       
                       /* Thread thread = new Thread(() => Process_FILE_UPLOAD_COMPLIT(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_FILE_UPLOAD_COMPLIT(byte_data));
                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion


            #region FILE_DOWNLOAD_COMPLIT
            if (command.CompareTo("FILE_DOWNLOAD_COMPLIT") == 0)
            {
                try
                {
                    if (old_FILE_DOWNLOAD_COMPLIT_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_FILE_DOWNLOAD_COMPLIT_string = Encoding.ASCII.GetString(byte_data);



                       /* Thread thread = new Thread(() => Process_FILE_DOWNLOAD_COMPLIT(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_FILE_DOWNLOAD_COMPLIT(byte_data));

                    }

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region NEW_IMG
            if (command.CompareTo("NEW_IMG") == 0)
            {
                try
                {
                    
                   /* Thread thread = new Thread(() => Process_NEW_IMG(byte_data));
                    thread.IsBackground = true;
                    thread.Start();*/
                    Task.Run(() => Process_NEW_IMG(byte_data));

                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region IMG
            if (command.CompareTo("IMG") == 0)
            {
                Task.Run(() => process_IMG(byte_data));

                /*Thread thread = new Thread(() => process_IMG(byte_data));
                thread.IsBackground = true;
                thread.Start(); */
            }
            #endregion

            #region STR
            if (command.CompareTo("STR") == 0)
            {
                try
                {
                    
                   /* Thread thread = new Thread(() => Process_STR(byte_data));
                    thread.IsBackground = true;
                    thread.Start();*/
                    Task.Run(() => Process_STR(byte_data));
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
                        
                       /* Thread thread = new Thread(() => Process_PROCESS(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_PROCESS(byte_data));
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
                       /* Thread thread = new Thread(() => Process_COMMAND(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_COMMAND(byte_data));
                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region CHAT
            if (command.CompareTo("CHAT") == 0)
            {
                try
                {
                    if (old_CHAT_str.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_CHAT_str = Encoding.ASCII.GetString(byte_data);
                        

                       /* Thread thread = new Thread(() => Process_CHAT(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_CHAT(byte_data));

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
                    if (old_FILE_SYSTEM_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_FILE_SYSTEM_string = Encoding.ASCII.GetString(byte_data);
                       

                        /*Thread thread = new Thread(() => Process_FILE_SYSTEM(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_FILE_SYSTEM(byte_data));
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
                        

                       /* Thread thread = new Thread(() => Process_CLIPBOARD(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_CLIPBOARD(byte_data));
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
                      
                      /*  Thread thread = new Thread(() => Process_BLACK_SCREEN(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_BLACK_SCREEN(byte_data));


                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString());
                }
            }
            #endregion

            #region BLACK_SCREEN_IMAGE_LIST
            if (command.CompareTo("BLACK_SCREEN_IMAGE_LIST") == 0)
            {
                try
                {
                    if (old_BLACK_SCREEN_IMAGE_LIST_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_BLACK_SCREEN_IMAGE_LIST_string = Encoding.ASCII.GetString(byte_data);
                      
                       

                    }
                }
                catch (Exception ex)
                {
                    write_log("error :-" + ex.ToString()); 
                }
            }
            #endregion

            #region STARTUP_PATH
            if (command.CompareTo("STARTUP_PATH") == 0)
            {
                try
                {
                    sender_startup_path = Encoding.ASCII.GetString(byte_data); 

                }
                catch (Exception ex)
                { }
            }
            #endregion

            #region BLACK_SCREEN_IMAGE_RECIVED
            if (command.CompareTo("BLACK_SCREEN_IMAGE_RECIVED") == 0)
            {
                try
                {
                    if (old_BLACK_SCREEN_IMAGE_RECIVED_string.CompareTo(Encoding.ASCII.GetString(byte_data)) != 0)
                    {
                        old_BLACK_SCREEN_IMAGE_RECIVED_string = Encoding.ASCII.GetString(byte_data);
                        

                       /* Thread thread = new Thread(() => Process_BLACK_SCREEN_IMAGE_RECIVED(byte_data));
                        thread.IsBackground = true;
                        thread.Start();*/
                        Task.Run(() => Process_BLACK_SCREEN_IMAGE_RECIVED(byte_data));

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
                        MessageBox.Show("Remote PC Close this session");

                        ffs.Close();
                        ffs.Dispose();

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
        xyz: ;
        Thread.Sleep(10);
           /* Thread t = new Thread(recive_fun);
            t.IsBackground=true;
            t.Start();*/
        Task t1 = new Task(recive_fun);
        t1.Start();
            
        }

       

        #region Process_NO_IMAGE_CHANGE
        public void Process_NO_IMAGE_CHANGE(Byte[] byte_data)
        {
            sw.Stop();
            sw.Reset();
            sw.Start();
        }
        #endregion

        #region Process_MIC
        public void Process_MIC(Byte[] byte_data)
        {
            using (Stream s = new MemoryStream(byte_data))
            {
                System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(s);
                myPlayer.Play();
            }
        }
        #endregion

        #region Process_FILE_UPLOAD_COMPLIT
        public void Process_FILE_UPLOAD_COMPLIT(Byte[] byte_data)
        {
            MessageBox.Show("File Successfully   Upload");
        }
        #endregion

        #region Process_FILE_DOWNLOAD_COMPLIT
        public void Process_FILE_DOWNLOAD_COMPLIT(Byte[] byte_data)
        {

            

        }
        #endregion

        #region Process_NEW_IMG
        public void Process_NEW_IMG(Byte[] byte_data)
        {
           /* for (int i = 0; i < Globle_data.img_matrix * Globle_data.img_matrix; i++)
            {
                recive_image_byte[i] = 0;
            }*/

            for (int i = 0; i < Globle_data.img_matrix * Globle_data.img_matrix; i++)
            {
                if ((int)byte_data[i] == 1)
                {
                    recive_image_byte[i] = 1;
                }
                else
                {
                    recive_image_byte[i] = 0;
                }
            }

        }
        #endregion

        #region process_IMG
        public void process_IMG(Byte[] byte_data)
        {
            try
            {
                int pic_no = (int)byte_data[0];
                byte[] imgdata = new byte[byte_data.Length - 1];
                Array.Copy(byte_data, 1, imgdata, 0, byte_data.Length - 1);
                Image img_x = null;
                using (WebP webp = new WebP())
                {
                    img_x = webp.Decode(imgdata);
                }
                write_log("image " + pic_no.ToString() + " receive  size " + imgdata.Length.ToString());
                screen_matrix[pic_no] = (Bitmap)img_x;
                recive_image_byte[pic_no] = 1;
                this.Invoke((MethodInvoker)delegate
                {
                   
                    //if (sw.ElapsedMilliseconds < 750)
                    //{
                    if (recive_image_byte[0] == 1 && recive_image_byte[1] == 1 && recive_image_byte[2] == 1 && recive_image_byte[3] == 1)
                    {
                        lab_msg.Text = "";
                        lab_msg.Visible = false;
                        sw.Stop();
                        sw.Reset();
                        sw.Start();
                        try
                        {

                            pictureBox1.Image = join_image();
                            write_log("all image receive join and show on screen");
                        }
                        catch (Exception ex)
                        {
                            write_log("error :-" + ex.ToString());
                        }
                    }
                    //}
                    //else
                    // {
                    //    pictureBox1.Image = join_image();
                    //    write_log("all image receive join and show on screen");
                    //}
                });
            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
        }
        #endregion

        #region Process_STR
        public void Process_STR(Byte[] byte_data)
        {
            /*String msg = Encoding.ASCII.GetString(byte_data);
            textBox1.Invoke((MethodInvoker)delegate
            {
                textBox1.Text = msg;
            });*/
        }
        #endregion

        #region Process_PROCESS
        public void Process_PROCESS(Byte[] byte_data)
        {
          
        }
        #endregion

        #region Process_COMMAND
        public void Process_COMMAND(Byte[] byte_data)
        {
          
        }
        #endregion

        #region Process_CHAT
        public void Process_CHAT(Byte[] byte_data)
        {
          
        }
        #endregion

        #region Process_FILE_SYSTEM
        public void Process_FILE_SYSTEM(Byte[] byte_data)
        {
            ffs.String_from_main_window = old_FILE_SYSTEM_string;

        }
        #endregion

        #region Process_CLIPBOARD
        public void Process_CLIPBOARD(Byte[] byte_data)
        {
            this.Invoke((MethodInvoker)delegate
            {
                Clipboard.SetText(old_CLIPBOARD_string);
            });
        }
        #endregion

        #region Process_BLACK_SCREEN
        public void Process_BLACK_SCREEN(Byte[] byte_data)
        {

            if (old_BLACK_SCREEN_string.CompareTo("BlackScreen Showing") == 0)
            {
                MessageBox.Show("BlackScreen is Showing on Remote pc");
            }

        }
        #endregion

        #region Process_BLACK_SCREEN_IMAGE_LIST
        public void Process_BLACK_SCREEN_IMAGE_LIST(Byte[] byte_data)
        { }
        #endregion

        #region Process_STARTUP_PATH
        public void Process_STARTUP_PATH(Byte[] byte_data)
        { }
        #endregion

        #region Process_BLACK_SCREEN_IMAGE_RECIVED
        public void Process_BLACK_SCREEN_IMAGE_RECIVED(Byte[] byte_data)
        {
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show(old_BLACK_SCREEN_IMAGE_RECIVED_string + " Uploaded successfully");
            });
        }
        #endregion




      


        public int get_actual_position(int orignal, int pic, int m)
        {
            int actual = 0;
            actual = (orignal * m) / pic;
            return actual;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            /*try
            {
                MouseEventArgs me = (MouseEventArgs)e;
                int orignal_width = pictureBox1.Image.Width;
                int orignal_height = pictureBox1.Image.Height;
                int pic_width = pictureBox1.Width;
                int pic_height = pictureBox1.Height;

                int actual_x = get_actual_position(orignal_width, pic_width, me.X);
                int actual_y = get_actual_position(orignal_height, pic_height, me.Y);

                if (me.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    String mouse_string = "1:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                    byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                }

                if (me.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    String mouse_string = "2:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                    byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                }

               
            }
            catch (Exception ex)
            {
                 write_log("error :-" + ex.ToString()); 
            }
          */

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MouseEventArgs me = (MouseEventArgs)e;
                int orignal_width = pictureBox1.Image.Width;
                int orignal_height = pictureBox1.Image.Height;
                int pic_width = pictureBox1.Width;
                int pic_height = pictureBox1.Height;

                int actual_x = get_actual_position(orignal_width, pic_width, me.X);
                int actual_y = get_actual_position(orignal_height, pic_height, me.Y);

                if (me.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    String mouse_string = "4:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                    byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                    try
                    {
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    }
                    catch { }
                }
                if (me.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    String mouse_string = "5:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                    byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                    try
                    {
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    }
                    catch { }
                    }
            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                MouseEventArgs me = (MouseEventArgs)e;
                int orignal_width = pictureBox1.Image.Width;
                int orignal_height = pictureBox1.Image.Height;
                int pic_width = pictureBox1.Width;
                int pic_height = pictureBox1.Height;

                Point p = e.Location;
                if (old_p.X != e.X || old_p.Y != e.Y)
                {
                    int actual_x = get_actual_position(orignal_width, pic_width, me.X);
                    int actual_y = get_actual_position(orignal_height, pic_height, me.Y);


                    String mouse_string = "3:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                    old_p.X = e.X;
                    old_p.Y = e.Y;

                    byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                    try
                    {
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        /* clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                         clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                         clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                         clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);*/
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                MouseEventArgs me = (MouseEventArgs)e;
                int orignal_width = pictureBox1.Image.Width;
                int orignal_height = pictureBox1.Image.Height;
                int pic_width = pictureBox1.Width;
                int pic_height = pictureBox1.Height;

                int actual_x = get_actual_position(orignal_width, pic_width, me.X);
                int actual_y = get_actual_position(orignal_height, pic_height, me.Y);

                if (me.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    String mouse_string = "6:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                    byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                    try
                    {
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    }
                    catch { }
                }
                if (me.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    String mouse_string = "7:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                    byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                    try
                    {
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                        clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
        }

        private void Image_reciver_KeyPress(object sender, KeyPressEventArgs e)
        {
            byte[] msg1 = new byte[20];
            String xx = e.KeyChar.ToString();
            if (xx.CompareTo("+") == 0)
            {
                String yy = "KB:" + "add";
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }
            if (xx.CompareTo("^") == 0)
            {
                String yy = "KB:" + "cart";
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo("%") == 0)
            {
                String yy = "KB:" + "per";
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo("~") == 0)
            {
                String yy = "KB:" + "tilde";
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo("(") == 0)
            {
                String yy = "KB:" + "left_b";
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo(")") == 0)
            {
                String yy = "KB:" + "right_b";
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo("{") == 0)
            {
                String yy = "KB:" + "left_b1" ;
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo("}") == 0)
            {
                String yy = "KB:" + "right_b1" ;
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo("[") == 0)
            {
                String yy = "KB:" + "left_b2" ;
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            if (xx.CompareTo("]") == 0)
            {
                String yy = "KB:" + "right_b2";
                msg1 = Encoding.ASCII.GetBytes(yy);
                goto abc;
            }

            String yy1 = "KB:" + Convert.ToString(e.KeyChar);
            msg1 = Encoding.ASCII.GetBytes(yy1);
            
        abc: ;
            byte[] data = dc.Add_Payload("KEYBOARD", msg1);
            try
            {
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
            }
            catch { }
            e.Handled = true;
           
        }

        private void Image_reciver_KeyUp(object sender, KeyEventArgs e)
        {
            byte[] msg1 = new byte[20];
            if (e.KeyCode == Keys.Up)
            {
                String yy = "KB:" + "UP" ;
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.Down)
            {
                String yy = "KB:" + "DOWN";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.Left)
            {
                String yy = "KB:" + "LEFT";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.Right)
            {
                String yy = "KB:" + "RIGHT";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.Pause)
            {
                String yy = "KB:" + "Pause";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.PageUp)
            {
                String yy = "KB:" + "PageUp";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.PageDown)
            {
                String yy = "KB:" + "PageDown";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.Scroll)
            {
                String yy = "KB:" + "Scroll";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.Home)
            {
                String yy = "KB:" + "Home";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.End)
            {
                String yy = "KB:" + "End";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.PrintScreen)
            {
                String yy = "KB:" + "PrintScreen";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }
            if (e.KeyCode == Keys.Insert)
            {
                String yy = "KB:" + "Insert";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }

            if (e.KeyCode == Keys.Delete)
            {
                String yy = "KB:" + "Delete";
                msg1 = Encoding.ASCII.GetBytes(yy);
            }

            byte[] data = dc.Add_Payload("KEYBOARD", msg1);
            try
            {
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
                clientSocket.Send(data, data.Length, p2pEndPoint);
            }
            catch { }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void Image_reciver_FormClosing(object sender, FormClosingEventArgs e)
        {
            byte[] to_send = dc.Add_Payload("Exit", "Exit");
            try
            {
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            }
            catch { }
        }

        public void send_black_scree_img(String sfile_name)
        {
           

        }

        private void timer_othre_window_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ffs.String_from_file_system_window.CompareTo("") != 0)
                {
                    byte[] to_send = dc.Add_Payload("FILE_SYSTEM", ffs.String_from_file_system_window);
                    String temp = ffs.String_from_file_system_window;
                    ffs.String_from_file_system_window = "";
                    try
                    {
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                    }
                    catch { }
                    old_FILE_SYSTEM_string = "";
                    // this code need to coment 
                   
                    String[] temp_x = temp.Split(';');
                    if (temp_x[0].CompareTo("file_upload_info") == 0)
                    {
                        String full_file_name = temp_x[4];
                        file_to_upload = temp_x[2];
                        file_data = File.ReadAllBytes(full_file_name);

                        if (file_data.Length <= 5120)
                        {
                            file_number_of_packet = 1;
                        }
                        else
                        {
                            file_number_of_packet = file_data.Length / 5120;
                            if (file_data.Length % 5120 != 0)
                            {
                                file_number_of_packet++;
                            }
                        }
                        ffs.progressBar1.Maximum = Convert.ToInt32(file_number_of_packet);
                        ffs.progressBar1.Value = 0;
                        file_current_packet_number = 1;
                    }

                    if (temp_x[0].CompareTo("file_info_request") == 0)
                    {
                       
                       file_download_path = temp_x[1];
                       file_to_download_ = temp_x[2];

                    }
                    // this code need to coment 
                }

            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }
            try
            {
                if (ffs.path_to_upload_file.CompareTo("") != 0)
                {
                   

                }

            }
            catch (Exception ex)
            {
                write_log("error :-" + ex.ToString());
            }

            try
            {
                if (ffs.file_to_download.CompareTo("") != 0)
                {
                    file_to_download = ffs.file_to_download;
                    ffs.file_to_download = "";
                    byte[] to_send = dc.Add_Payload("FILE_DOWNLOAD", file_to_download);
                    try
                    {
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                        clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                    }
                    catch { }

                }
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

       

       

       

        private void Image_reciver_Resize(object sender, EventArgs e)
        {
            arreng_pbsetting();
        }

        private void timer_send_rtp_Tick(object sender, EventArgs e)
        {
            byte[] to_send = dc.Add_Payload("IMG_RTP", recive_image_byte);
            try
            {
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
                write_log("send rtp" + Encoding.ASCII.GetString(recive_image_byte));
            }
            catch
            { }

            if (sw.ElapsedMilliseconds > 7000)
            {
                sw.Stop();
                sw.Reset();
                sw.Start();
                lab_msg.Text = "Connection lost. Reconnecting...";
                lab_msg.Visible = true;
                byte[] data = dc.Add_Payload("RESET_CONNECTION", "");
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
        }
        public void arreng_pbsetting()
        {
            pbsetting.Top = (this.Height / 2) - 30;
            //pbsetting.Left = (this.Width - 45);
            pbsetting.Left = (this.Width - side_panel.Width - 40);
            side_panel.Height = this.Height;
            side_panel.Left = this.Width - side_panel.Width;
        }

        

        public void write_log(String log)
        {
            try
            {
              //  String log_msg = log + "\t at :-" + DateTime.Now.ToString() + "\t\n";
               // File.AppendAllText("log.txt", log_msg);
            }
            catch (Exception ex)
            { }
        }
        public void mouse_wheel(object sender, MouseEventArgs e)
        {
            // textBox1.Text = e.Delta.ToString();
            MouseEventArgs me = (MouseEventArgs)e;
            int orignal_width = pictureBox1.Image.Width;
            int orignal_height = pictureBox1.Image.Height;
            int pic_width = pictureBox1.Width;
            int pic_height = pictureBox1.Height;

            Point p = e.Location;

            int actual_x = get_actual_position(orignal_width, pic_width, me.X);
            int actual_y = get_actual_position(orignal_height, pic_height, me.Y);



            old_p.X = e.X;
            old_p.Y = e.Y;


            if (e.Delta > 0)
            {
                String mouse_string = "8:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                try
                {
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                }
                catch { }
            }
            
            else
            {
                String mouse_string = "9:" + Convert.ToString(actual_x) + ":" + Convert.ToString(actual_y);
                byte[] byte_data = dc.Add_Payload("MOUSE", mouse_string);
                try
                {
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                }
                catch { }
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
            /* byte[] byteData = dc.Add_Payload("STR", "p2p");
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

        private void timer_ft_Tick(object sender, EventArgs e)
        {
            byte[] to_send = dc.Add_Payload("FT_DATA", ft_packet_data);
            try
            {
                clientSocket.Send(to_send, to_send.Length, p2pEndPoint);
            }
            catch { }
        }

        private void pbsleep_Click(object sender, EventArgs e)
        {
            String yy = "cmd:" + "hibernate";
            byte[] byte_data = dc.Add_Payload("SYSTEM", yy);
            try
            {
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch { }
            
        }

        private void pbhibernate_Click(object sender, EventArgs e)
        {
            String yy = "cmd:" + "hibernate";
            byte[] byte_data = dc.Add_Payload("SYSTEM", yy);
            try
            {
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch { }
        }

        private void pblock_Click(object sender, EventArgs e)
        {
            String yy = "cmd:" + "hibernate";
            byte[] byte_data = dc.Add_Payload("SYSTEM", yy);
            try
            {
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch { }
        }

        private void pbshoutdown_Click(object sender, EventArgs e)
        {
            String yy = "cmd:" + "shutdown";
            byte[] byte_data = dc.Add_Payload("SYSTEM", yy);
            try
            {
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch { }
        }

        private void pbrestart_Click(object sender, EventArgs e)
        {
            String yy = "cmd:" + "restart";
            byte[] byte_data = dc.Add_Payload("SYSTEM", yy);
            try
            {
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch { }
        }

        private void pbfile_transfer_Click(object sender, EventArgs e)
        {
            ffs.Show();
        }

        private void pbshowblack_screen_Click(object sender, EventArgs e)
        {
            Thread.Sleep(200);
            byte[] byte_data = dc.Add_Payload("BLACK_SCREEN", "show");
            try
            {
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch { }
        }

        private void pbremove_blackscreen_Click(object sender, EventArgs e)
        {
            byte[] byte_data = dc.Add_Payload("BLACK_SCREEN", "remove");
            try
            {
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
            }
            catch { }
        }

        private void pbshow_image_on_blackscreen_Click(object sender, EventArgs e)
        {
            if (Globle_data.blackscree_status == true)
            {
                byte[] byte_data = dc.Add_Payload("DESKTOP_AS_BLACK_SCREEN", DateTime.Now.ToShortTimeString());
                try
                {
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                    clientSocket.Send(byte_data, byte_data.Length, p2pEndPoint);
                }
                catch { }
            }
            else
            {
                Task.Run(() =>
                {
                    MessageBox.Show("This service is available with Add-on. ");
                });
            }
        }

        private void pbupload_image_for_blackscreen_Click(object sender, EventArgs e)
        {
            if (Globle_data.blackscree_status == true)
            {
                openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF";
                DialogResult dr = this.openFileDialog1.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        Thread thread = new Thread(() => send_black_scree_img(openFileDialog1.FileName));
                        thread.Start();
                    }
                    catch (Exception ex)
                    {
                        write_log("error :-" + ex.ToString());
                        MessageBox.Show("Image size is more then packet size");
                    }
                }
            }
            else
            {
                Task.Run(() =>
                {
                    MessageBox.Show("This service is available with Add-on. ");
                });
            }
        }

        private void pbsetting_Click(object sender, EventArgs e)
        {
            if (side_panel.Width < 70)
            {
                side_panel.Width = 70;
                side_panel.Left = this.Width - 70;
                side_panel.Height = this.Height;
                side_panel.Top = 0;
                pbsetting.Top = (this.Height / 2) - 30;
                pbsetting.Left = (this.Width - side_panel.Width - 30);
            }
            else
            {
                side_panel.Width = 1;
                side_panel.Left = this.Width - 5;
                pbsetting.Left = (this.Width - 45);
            }
        }

      
      

       

      

      
    }
}
