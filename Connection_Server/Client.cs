using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Data;
//using System.Threading.Tasks;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text.pdf.fonts.cmaps;

namespace Connection_Server
{
    class Client
    {
        public Socket Socket;
        public String uid = "";
        public String pass_word = "";
        public string IP;
        public String MachineName = "";
        public String login_name = "";
        public int use_day = 0;
        public int TCPPort;
        public UdpClient UDP_Client;
        public int UDP_Port;
        public IPEndPoint UDP_Point;
        static byte[] data = new byte[256];
        public static Client instance = new Client();
        int hartbit = 0;
       
        public void start()
        {
           // Task p = new Task(process_client);
            //p.Start();
           Thread p = new Thread(process_client);
           p.IsBackground = true;
           p.Start();


           // Task t = new Task(check_isonline);
           // t.Start();
            Thread t = new Thread(check_isonline);
            t.IsBackground = true;
            t.Start();
        }
        private void process_client()
        {

            while (Socket.Connected)
            {
                try
                {
                    byte[] xdata = new byte[256];
                    Socket.Receive(xdata, SocketFlags.None);
                    string someString = Encoding.ASCII.GetString(xdata);
                    someString = someString.Replace("\0", "");
                    String[] string_from_client = someString.Split(':');
                    switch (Convert.ToInt16(string_from_client[0]))
                    {
                        #region case_0 Request for uid password
                        case 0: // request for uid password
                            try
                            {
                                IP = Socket.RemoteEndPoint.ToString().Split(':')[0];
                                IP = IP.Trim();
                                MachineName = string_from_client[2].Trim();
                                MachineName = MachineName.Replace("\0", string.Empty);
                                DataTable dt = new DataTable();
                                String hdd_no = string_from_client[1].Substring(0, 8);
                                String xx = "select * from register_pc where hdd_no='" + hdd_no + "'";
                                //Console.WriteLine(xx);
                                dt = Program.db.get_data_table(xx);
                                if (dt.Rows.Count > 0)
                                {
                                    //existing 
                                    String UID_String = dt.Rows[0]["UID"].ToString();
                                    if (Program.Clients.Where(x => x.uid == UID_String).Count() > 0)
                                    {
                                        Client client = Program.Clients.Where(x => x.uid == UID_String).First();
                                        client.close_connection(2);
                                    }
                                try_pass: ;
                                    String Password = new Random().Next(1000, 9999).ToString();
                                    uid = UID_String;

                                    String pp = CryptoEngine.Encrypt(Password, "4589-8754-dfgfg");
                                    if (pp.Contains("\'") || pp.Contains("\"") || pp.Contains(":"))
                                        goto try_pass;
                                    pass_word = Password;
                                    DateTime reg_date = Convert.ToDateTime(dt.Rows[0]["register_date"]);
                                    use_day = (DateTime.Now - reg_date).Days;
                                    String system_date = DateTime.Now.ToString("dd-MMM-yyyy");
                                    send_data("0:" + UID_String + ":" + pass_word + ":" + use_day.ToString() + ":" + system_date);
                                    Console.WriteLine("Client ip is \t" + this.IP + "\t uid is \t" + this.uid + "\t hdd_no is\t" + hdd_no);


                                }
                                else
                                {
                                    Random r = new Random();
                                    int UID_Part1 = r.Next(1000, 9999);
                                    int UID_Part2 = r.Next(10, 99);
                                    int UID_Part3 = r.Next(10, 99);
                                    String UID_String = UID_Part1.ToString() + UID_Part2.ToString() + UID_Part3.ToString();
                                try_pass: ;
                                    String Password = r.Next(1000, 9999).ToString();
                                    String Personl_Password = r.Next(100000, 999999).ToString();

                                    uid = UID_String;
                                    pass_word = Password;
                                    String pp = CryptoEngine.Encrypt(Password, "4589-8754-dfgfg");
                                    if (pp.Contains("\'") || pp.Contains("\"") || pp.Contains(":"))
                                        goto try_pass;

                                    Personl_Password = CryptoEngine.Encrypt(Personl_Password, "4589-8754-dfgfg");
                                    if (Personl_Password.Contains("\'") || Personl_Password.Contains("\"") || Personl_Password.Contains(":"))
                                        goto try_pass;


                                    String sql = "insert into  register_pc values('" + UID_String + "','" + Password + "','" + IP + "','" + MachineName + "','" + hdd_no + "','online','" + Personl_Password + "','" + DateTime.Now.ToString("dd/MMM/yyyy") + "')";
                                    Program.db.run_sql(sql);
                                    use_day = 0;
                                    String system_date = DateTime.Now.ToString("dd-MMM-yyyy");
                                    send_data("0:" + UID_String + ":" + pass_word + ":" + use_day.ToString() + ":" + system_date);
                                    Console.WriteLine("Client ip is \t" + this.IP + "\t uid is \t" + this.uid + "\t hdd_no is\t" + hdd_no);
                                }
                                //String sql1 = "update register_pc set pc_status='online' where uid='" + uid + "'";
                                // Program.db.run_sql(sql1);

                            }
                            catch (Exception ex)
                            { }

                            break;
                        #endregion

                        #region case_1 connection request
                        case 1: //connection request
                            try
                            {
                                if (use_day < 8)
                                {
                                    String patner_id = string_from_client[1];
                                    String patner_password = string_from_client[2];

                                    Console.WriteLine("connection request from " + this.uid + " to connect " + patner_id + " password is" + patner_password);


                                    patner_password = patner_password.Replace("\0", "");
                                    patner_password = CryptoEngine.Decrypt(patner_password, "4589-8754-dfgfg");
                                    if (Program.Clients.Where(x => x.uid == patner_id).Count() > 0)
                                    {
                                        Client client = Program.Clients.Where(x => x.uid == patner_id).First();
                                        if (client.pass_word.CompareTo(patner_password) == 0)
                                        {
                                            send_data("2:" + "Waiting from Remote connection");
                                            client.send_data("3:" + uid + ":" + IP + ":" + MachineName);
                                            Connection_request cr = new Connection_request();
                                            cr.connet_uid = this.uid;
                                            cr.allow_uid = patner_id;
                                            cr.connet_status = "waiting";
                                            cr.allow_status = "";

                                            Program p = new Program();
                                            p.Connection_request_list.Add(cr);
                                        }
                                        else
                                        {
                                            // check for personal password
                                            DataTable dt = new DataTable();
                                            dt = Program.db.get_data_table("select * from register_pc where uid='" + client.uid + "'");
                                            String pp = CryptoEngine.Decrypt(Convert.ToString(dt.Rows[0]["personl_password"]), "4589-8754-dfgfg");
                                            if (pp.CompareTo(patner_password) == 0)
                                            {
                                                send_data("2:" + "Waiting from Remote connection");
                                                Thread.Sleep(500);
                                                client.send_data("5:" + uid + ":" + IP + ":" + MachineName);
                                                Connection_request cr = new Connection_request();
                                                cr.connet_uid = this.uid;
                                                cr.allow_uid = patner_id;
                                                cr.connet_status = "waiting";
                                                cr.allow_status = "";

                                            }
                                            else
                                            {
                                                send_data("1:" + "Invalid Password");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        send_data("1:" + "Remote pc is offline");
                                    }
                                }
                                else
                                {
                                    if (login_name.CompareTo("") != 0)
                                    {
                                        String patner_id = string_from_client[1];
                                        String patner_password = string_from_client[2];

                                        patner_password = patner_password.Replace("\0", "");
                                        patner_password = CryptoEngine.Decrypt(patner_password, "4589-8754-dfgfg");
                                        if (Program.Clients.Where(x => x.uid == patner_id).Count() > 0)
                                        {
                                            Client client = Program.Clients.Where(x => x.uid == patner_id).First();
                                            if (client.pass_word.CompareTo(patner_password) == 0)
                                            {
                                                send_data("2:" + "Waiting from Remote connection");
                                                client.send_data("3:" + uid + ":" + IP + ":" + MachineName);
                                                Connection_request cr = new Connection_request();
                                                cr.connet_uid = this.uid;
                                                cr.allow_uid = patner_id;
                                                cr.connet_status = "waiting";
                                                cr.allow_status = "";

                                                Program p = new Program();
                                                p.Connection_request_list.Add(cr);
                                            }
                                            else
                                            {
                                                // check for personal password
                                                DataTable dt = new DataTable();
                                                dt = Program.db.get_data_table("select * from register_pc where uid='" + client.uid + "'");
                                                String pp = CryptoEngine.Decrypt(Convert.ToString(dt.Rows[0]["personl_password"]), "4589-8754-dfgfg");
                                                if (pp.CompareTo(patner_password) == 0)
                                                {
                                                    send_data("2:" + "Waiting from Remote connection");
                                                    Thread.Sleep(500);
                                                    client.send_data("5:" + uid + ":" + IP + ":" + MachineName);
                                                    Connection_request cr = new Connection_request();
                                                    cr.connet_uid = this.uid;
                                                    cr.allow_uid = patner_id;
                                                    cr.connet_status = "waiting";
                                                    cr.allow_status = "";

                                                }
                                                else
                                                {
                                                    send_data("1:" + "Invalid Password");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            send_data("1:" + "Remote pc is offline");
                                        }
                                    }
                                    else
                                    {
                                        // login needed
                                        send_data("15:" + "not login and exp.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_3 responce for connection
                        case 3: // responce for connection 
                            try
                            {
                                string patner_id1 = string_from_client[2];
                                if (string_from_client[1].CompareTo("allow") == 0)
                                {

                                    Program pm = new Program();
                                    int index = pm.get_index_of_min_connection_echo_server();

                                    pm.Echo_server_send(index, "make Connection");
                                    Thread.Sleep(500);

                                    String port_list = pm.Echo_server_get_data_string(index);

                                    send_data("8:" + port_list);

                                    for (int i = 0; i < 10000; i++)
                                    {
                                        if (Program.Clients[i].uid.CompareTo(patner_id1) == 0)
                                        {
                                            Program.Clients[i].send_data("4:" + "allow");
                                            Thread.Sleep(500);
                                            Program.Clients[i].send_data("8:" + port_list);

                                            String[] temp_port_list = port_list.Split(':');
                                            String sql = "insert into live_connection values('" + temp_port_list[0] + "','" + temp_port_list[4] + "','" + Program.Clients[i].uid + "','" + this.uid + "','" + Program.Clients[i].IP + "','" + this.IP + "')";
                                            Program.db.run_sql(sql);


                                            // sql = "select * from existing_connection where connectore_uid='" + uid + "' and connected_uid='" + Program.Clients[i].uid + "'";
                                            // dt = Program.db.get_data_table(sql);
                                            //if (dt.Rows.Count == 0)
                                            // {
                                            sql = "insert into existing_connection (connectore_mn,connectore_uid,connected_mn,connected_uid,connectore_ip,connected_ip,date) values('" + MachineName + "','" + uid + "','" + Program.Clients[i].MachineName + "','" + Program.Clients[i].uid + "','" + this.IP + "','" + Program.Clients[i].IP + "','" + DateTime.Now.ToString("dd/MMM/yyyy") + "')";
                                            Program.db.run_sql(sql);

                                            //}
                                            break;
                                        }
                                    }
                                }
                                if (string_from_client[1].CompareTo("reject") == 0)
                                {
                                    for (int i = 0; i < 10000; i++)
                                    {
                                        if (Program.Clients[i].uid.CompareTo(patner_id1) == 0)
                                        {
                                            Program.Clients[i].send_data("4:" + "reject");
                                            break;
                                        }
                                    }

                                }
                                //write code for reject 
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_4 change password
                        case 4:
                            try
                            {
                                // change password
                                String client_id = string_from_client[1];
                                String client_personal_password = string_from_client[2];
                                client_personal_password = client_personal_password.Replace("\0", "");
                                Program.db.run_sql("update register_pc  set personl_password ='" + client_personal_password + "' where uid='" + client_id + "'");
                                send_data("7:passward_change:passward_change");
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_5  invoit
                        case 5:// invoit 
                            try
                            {
                                String invoit_to_client_id = string_from_client[1];
                                String my_password = string_from_client[2];
                                if (Program.Clients.Where(x => x.uid == invoit_to_client_id).Count() > 0)
                                {
                                    Client client = Program.Clients.Where(x => x.uid == invoit_to_client_id).First();
                                    client.send_data("9:" + uid + ":" + my_password);
                                }
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_6 existing connection
                        case 6:// old connection
                            try
                            {
                                DataTable dt1 = new DataTable();
                                DataTable dt2 = new DataTable();

                                String sql_old_conncetion = "select connected_uid from existing_connection where connectore_uid='" + uid + "' group by connected_uid";
                                dt1 = Program.db.get_data_table(sql_old_conncetion);
                                sql_old_conncetion = "select connectore_uid from existing_connection where connected_uid='" + uid + "' group by connectore_uid";
                                dt2 = Program.db.get_data_table(sql_old_conncetion);
                                String result = "";
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    if (Program.Clients.Where(x => x.uid == dt1.Rows[i][0].ToString()).Count() > 0)
                                        result = result + dt1.Rows[i][0].ToString() + "\tonline\n";
                                    else
                                        result = result + dt1.Rows[i][0].ToString() + "\toffline\n";
                                    if (i % 30 == 0)
                                    {
                                        send_data("10:" + result);
                                        result = "";
                                        Thread.Sleep(50);
                                    }
                                }
                                for (int i = 0; i < dt2.Rows.Count; i++)
                                {
                                    if (Program.Clients.Where(x => x.uid == dt2.Rows[i][0].ToString()).Count() > 0)
                                        result = result + dt2.Rows[i][0].ToString() + "\tonline\n";
                                    else
                                        result = result + dt2.Rows[i][0].ToString() + "\toffline\n";

                                    if (i % 30 == 0)
                                    {
                                        send_data("10:" + result);
                                        result = "";
                                    }
                                }
                                send_data("10:" + result);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_7  check online
                        case 7:// check online
                            try
                            {
                                String[] pc_uid = string_from_client[1].Split('\t');
                                String online_list = "";
                                for (int i = 0; i < pc_uid.Length; i++)
                                {
                                    if (Program.Clients.Where(x => x.uid == pc_uid[i]).Count() > 0)
                                    {
                                        online_list = online_list + pc_uid[i] + "\t" + "online" + "\n";
                                    }
                                    else
                                    {
                                        online_list = online_list + pc_uid[i] + "\t" + "offline" + "\n";
                                    }
                                }
                                send_data("11:" + online_list);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_8 save contact
                        case 8:// save contact
                            try
                            {
                                String contact_uid = string_from_client[2];
                                String contact_name = string_from_client[3];
                                String contact_phone_number = string_from_client[4];
                                String contact_email = string_from_client[5];
                                String contact_amount = string_from_client[6];
                                String contact_other_info_1 = string_from_client[7];
                                String contact_other_info_2 = string_from_client[8];
                                String contact_personal_password = string_from_client[9];
                                contact_personal_password = contact_personal_password.Replace("\0", String.Empty);
                                String sql_save_contact = "INSERT INTO existing_contact( uid, name, phone_number, email, amount, other_info1, other_info_2, personal_password, save_by_uid)VALUES( '" + contact_uid + "', '" + contact_name + "', '" + contact_phone_number + "', '" + contact_email + "', '" + contact_amount + "', '" + contact_other_info_1 + "', '" + contact_other_info_2 + "', '" + contact_personal_password + "','" + this.uid + "')";
                                Program.db.run_sql(sql_save_contact);
                                send_data("12:" + "contact_save" + ":" + contact_uid);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_9 get existing contact
                        case 9:
                            try
                            {
                                String sql_get_existing_contact = "select * from existing_contact where save_by_uid='" + this.uid + "'";
                                DataTable ec_dt = new DataTable();
                                ec_dt = Program.db.get_data_table(sql_get_existing_contact);
                                String ec_result = "";
                                for (int i = 0; i < ec_dt.Rows.Count; i++)
                                {
                                    if (Program.Clients.Where(x => x.uid == ec_dt.Rows[i][1].ToString()).Count() > 0)
                                        ec_result = ec_result + ec_dt.Rows[i][0].ToString() + "\t" + ec_dt.Rows[i][1].ToString() + "\t" + ec_dt.Rows[i][2].ToString() + "\t" + ec_dt.Rows[i][3].ToString() + "\t" + ec_dt.Rows[i][4].ToString() + "\t" + ec_dt.Rows[i][5].ToString() + "\t" + ec_dt.Rows[i][6].ToString() + "\t" + ec_dt.Rows[i][7].ToString() + "\t" + ec_dt.Rows[i][8].ToString() + "\tonline\n";
                                    else
                                        ec_result = ec_result + ec_dt.Rows[i][0].ToString() + "\t" + ec_dt.Rows[i][1].ToString() + "\t" + ec_dt.Rows[i][2].ToString() + "\t" + ec_dt.Rows[i][3].ToString() + "\t" + ec_dt.Rows[i][4].ToString() + "\t" + ec_dt.Rows[i][5].ToString() + "\t" + ec_dt.Rows[i][6].ToString() + "\t" + ec_dt.Rows[i][7].ToString() + "\t" + ec_dt.Rows[i][8].ToString() + "\toffline\n";

                                    if (i % 10 == 0)
                                    {
                                        send_data("13:" + ec_result);
                                        ec_result = "";
                                        Thread.Sleep(50);
                                    }
                                }
                                send_data("13:" + ec_result);

                                //String result_get_existing_contact = Program.db.get_data_table_in_string(sql_get_existing_contact);
                                //send_data("13:" + result_get_existing_contact);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_10 hart bit
                        case 10://hart bit
                           // hartbit = 0;
                            //send_data("16:");

                            break;
                        #endregion

                        #region case_11 edit contact
                        case 11:// edit contact
                            try
                            {
                                String contact_uid1 = string_from_client[2];
                                String contact_name1 = string_from_client[3];
                                String contact_phone_number1 = string_from_client[4];
                                String contact_email1 = string_from_client[5];
                                String contact_amount1 = string_from_client[6];
                                String contact_other_info_11 = string_from_client[7];
                                String contact_other_info_21 = string_from_client[8];
                                String contact_personal_password1 = string_from_client[9];
                                contact_personal_password1 = contact_personal_password1.Replace("\0", String.Empty);
                                String sql_save_contact1 = "UPDATE existing_contact SET uid='" + contact_uid1 + "', name='" + contact_name1 + "', phone_number='" + contact_phone_number1 + "', email='" + contact_email1 + "', amount='" + contact_amount1 + "', other_info1='" + contact_other_info_11 + "', other_info_2='" + contact_other_info_21 + "', personal_password='" + contact_personal_password1 + "', save_by_uid='" + this.uid + "' WHERE save_by_uid='" + uid + "' and uid='" + contact_uid1 + "'";
                                Program.db.run_sql(sql_save_contact1);
                                send_data("12:" + "contact_save" + ":" + contact_uid1);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_12 login
                        case 12:// login
                            try
                            {
                                string_from_client[1] = string_from_client[1].Replace("\0", "");
                                if (Program.Clients.Where(x => x.login_name == string_from_client[1]).Count() > 0)
                                {
                                    Client client = Program.Clients.Where(x => x.login_name == string_from_client[1]).First();
                                    client.send_data("14:"); // for logout
                                }
                                this.login_name = string_from_client[1];
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_13 check save conntact  online
                        case 13:// check save conntact  online
                            try
                            {
                                String[] pc_uid1 = string_from_client[1].Split('\t');
                                String online_list1 = "";
                                for (int i = 0; i < pc_uid1.Length; i++)
                                {
                                    if (Program.Clients.Where(x => x.uid == pc_uid1[i]).Count() > 0)
                                    {
                                        online_list1 = online_list1 + pc_uid1[i] + "\t" + "online" + "\n";
                                    }
                                    else
                                    {
                                        online_list1 = online_list1 + pc_uid1[i] + "\t" + "offline" + "\n";
                                    }
                                }
                                send_data("17:" + online_list1);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_21 delete existing_contact
                        case 21:
                            try
                            {
                                String dd1 = string_from_client[1];
                                String dd2 = string_from_client[2];
                                dd2 = dd2.Replace("\0", "");
                                String dd3 = "delete from existing_contact where save_by_uid='" + dd1 + "'and uid='" + dd2 + "'";
                                Program.db.run_sql(dd3);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion


                       // #region manage website data
                        #region case_101 Sign Up new user 
                        case 101:
                            try
                            {
                                String Name = string_from_client[1];
                                String Username = string_from_client[2];
                                String Address = string_from_client[3];
                                String Mobile_No = string_from_client[4];

                                String Email = string_from_client[5];
                                String Password = string_from_client[6];
                                String Reference_Id = string_from_client[7];
                                String User_type = "first_user";
                                String date = DateTime.Now.ToString("dd/MMM/yyyy");

                                Password = CryptoEngine.Decrypt(Password, "viidf;ll");

                                DataTable dt = new DataTable();
                                dt = Program.main_db.get_data_table("select * from user_table where Username='" + Username + "'");
                                if (dt.Rows.Count == 0)
                                {
                                    String sql = "insert into user_table (Name,Username,Address,Mobile_No,Email,Password,Reference_Id,User_type,date) values('" + Name + "','" + Username + "','" + Address + "','" + Mobile_No + "','" + Email + "','" + Password + "','" + Reference_Id + "','" + User_type + "','" + date + "')";
                                    Program.db.run_sql(sql);
                                    sql = "insert into user_table (Name,Username,Address,Mobile_No,Email,Password,Reference_Id,User_type,date,url,ref_status) values('" + Name + "','" + Username + "','" + Address + "','" + Mobile_No + "','" + Email + "','" + Password + "','" + Reference_Id + "','" + User_type + "','" + date + "','"+Program.url +"','active')";

                                    Program.main_db.run_sql(sql);

                                    sql = "insert into user_status_table values('" + Username + "','inactive','-','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "')";
                                    Program.db.run_sql(sql);

                                    send_data("101:" + "Record save"); 
                                }
                                else
                                {
                                    send_data("101:" + "Username Taken");  
                                }
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion
                        #region case_102 Login
                        case 102:
                            try
                            {
                               
                                String Username = string_from_client[1];
                                String Password = string_from_client[2];
                                Password = CryptoEngine.Decrypt(Password, "fgpoirlddbfgfg");

                                DataTable dt = new DataTable();
                                dt = Program.db.get_data_table("select * from user_table where Username='" + Username + "' and Password='" + Password + "'");
                                if (dt.Rows.Count <= 0)
                                {
                                    send_data("102:" + "Invalid Login");
                                }
                                else
                                {
                                    

                                    dt = Program.db.get_data_table("select * from user_status_table where username='" + Username + "'");
                                    String account_status = "inactive";
                                    String purchase_date = dt.Rows[0][3].ToString();
                                    String expiry_date = dt.Rows[0][4].ToString();
                                    String blackscree_status = "no";

                                    String active_plan_purchse_date = "";
                                    String active_plan_activetion_date = "";
                                    String active_plan_expairy_Date = "";


                                    // check status and update it
                                    DateTime ed = Convert.ToDateTime(dt.Rows[0][4]);
                                    TimeSpan difference = ed - DateTime.Now;
                                    if (difference.Days > 0)
                                    {
                                        account_status = "active";
                                        login_name = Username;
                                        String sql = "update user_status_table  set  active_status='active' where username='" + Username + "'";
                                        Program.db.run_sql(sql);
                                        // check for blacscreen active for this month
                                        dt = Program.db.get_data_table("select * from purchase_table where username='" + Username + "'");
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {

                                            DateTime datePast=Convert.ToDateTime( dt.Rows[i][1]);
                                            DateTime dateFuture = Convert.ToDateTime(dt.Rows[i][2]);
                                            if (CheckDateRange(DateTime.Now, datePast, dateFuture) == true)
                                            {
                                                blackscree_status = dt.Rows[i][4].ToString();

                                                active_plan_purchse_date = dt.Rows[i][1].ToString();
                                                active_plan_activetion_date = dt.Rows[i][6].ToString();
                                                active_plan_expairy_Date = dt.Rows[i][2].ToString();

                                                break;
                                            }
                                        }
                                        //
                                    }
                                    else
                                    {
                                        account_status = "inactive";
                                        String sql = "update user_status_table  set  active_status='inactive' where username='" + Username + "'";
                                        Program.db.run_sql(sql);
                                    }
                                    //
                                    send_data("102:" + "Login ok:" + Username + ":" + purchase_date + ":" + expiry_date + ":" + account_status+":"+blackscree_status+":"+ active_plan_purchse_date+":"+ active_plan_activetion_date+":"+ active_plan_expairy_Date); 
                                }
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion
                        #region case_103 Logout
                        case 103:
                            try
                            {
                                this.login_name = "";
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion
                        #region case_104 request for price and gst
                        case 104:
                            try
                            {
                                DataTable dt = new DataTable();
                                dt = Program.db.get_data_table("select * from value_table");
                                send_data("104:" + dt.Rows[0][0].ToString() + ":" + dt.Rows[0][1].ToString());

                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_105 purchase this is temp setting this delete after payment getway
                        case 105:
                            try
                            {
                                String username = string_from_client[1];
                                DateTime pd = Convert.ToDateTime(string_from_client[2]);
                                DateTime ed = Convert.ToDateTime(string_from_client[3]);
                                int day = Convert.ToInt16(string_from_client[4]);
                                String blackscreen = string_from_client[5];
                                String plan_activation_Date = string_from_client[6];
                                String value = string_from_client[7];
                                String gst_value = string_from_client[8];
                                String total_value = string_from_client[9];
                                String payment_status = string_from_client[10];
                                String payment_id = string_from_client[11];

                                TimeSpan difference = ed - pd;
                                if (Convert.ToInt16(difference.Days) > 0)
                                {
                                    DataTable dt = new DataTable();
                                    dt = Program.db.get_data_table("select * from user_status_table where username='" + username + "'");
                                    DateTime expiry_date = Convert.ToDateTime(dt.Rows[0][4]);
                                    DateTime new_expiry_date = expiry_date.AddDays(day);
                                    String sql = "update user_status_table  set purchase_date='" + DateTime.Now.ToString("dd-MMM-yyyy") + "', expiry_date='" + new_expiry_date.ToString("dd-MMM-yyyy") + "', active_status='active' where username='" + string_from_client[1] + "'";
                                    Program.db.run_sql(sql);
                                }
                                else
                                {
                                    DateTime new_expiry_date = DateTime.Now.AddDays(day);
                                    String sql = "update user_status_table  set purchase_date='" + DateTime.Now.ToString("dd-MMM-yyyy") + "', expiry_date='" + new_expiry_date.ToString("dd-MMM-yyyy") + "', active_status='active' where username='" + string_from_client[1] + "'";
                                    Program.db.run_sql(sql);
                                }


                                String sql1 = "insert into purchase_table (username,purcahse_date,expiry_date,day,blackscreen,plan_activation_date,amount,gst_amount,total_amount,payment_status,payment_id) values ('" + username + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + ed.ToString("dd-MMM-yyyy") + "'," + day.ToString() + ",'" + blackscreen + "','" + plan_activation_Date + "'," + value + "," + gst_value + "," + total_value + ",'" + payment_status + "','" + payment_id + "')";
                                Program.db.run_sql(sql1);

                                String condition = "Username='" + username + "'";
                                String parent_username = Program.main_db.get_value("Reference_Id", "user_table", condition);

                                String ref_status = Program.main_db.get_value("ref_status", "user_table", condition);
                                int count_ = 0;
                                DataTable count_dt = new DataTable();
                                if (ref_status.CompareTo("active") == 0)
                                {
                                    
                                    count_dt = Program.main_db.get_data_table("select * from purchase_table where ref_by='" + parent_username + "'");
                                    count_ = count_dt.Rows.Count + 1;
                                }
                                else
                                {
                                    count_ = -1;
                                }
                                sql1 = "insert into purchase_table (username,purcahse_date,expiry_date,day,blackscreen,plan_activation_date,amount,gst_amount,total_amount,payment_status,payment_id,url,paid_status,count_,ref_by,paid_date) values ('" + username + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + ed.ToString("dd-MMM-yyyy") + "'," + day.ToString() + ",'" + blackscreen + "','" + plan_activation_Date + "'," + value + "," + gst_value + "," + total_value + ",'" + payment_status + "','" + payment_id + "','"+ Program.url + "','not paid','"+count_.ToString()+"','"+ parent_username + "','-')";
                                Program.main_db.run_sql(sql1);

                                count_dt = Program.main_db.get_data_table("select sum(day) as total_day from purchase_table where username='" + username + "'");
                                if (Convert.ToInt32(count_dt.Rows[0][0]) > 360)
                                {
                                    sql1 = "update user_table set ref_status='inactive' where Username='" + username + "'";
                                    Program.main_db.run_sql(sql1);
                                }


                                String party_name = "-";
                                String party_address = "-";
                                String other_info = "-";
                                String Email = "-";
                                String Invoice_number = "-";

                                // get user email and address and other inforamtion 

                                try
                                {
                                    DataTable dt = new DataTable();
                                    dt = Program.db.get_data_table("select * from user_table where Username='" + username + "'");

                                    party_name = dt.Rows[0][1].ToString();
                                    party_address = dt.Rows[0][2].ToString();
                                    other_info = dt.Rows[0][3].ToString();
                                    Email = dt.Rows[0][4].ToString();
                                    String condition_1 = "payment_id='" + payment_id + "'";
                                    Invoice_number = Program.main_db.get_value("id", "purchase_table", condition_1);

                                }
                                catch { }

                                //get user email and address and other inforamtion  end 


                                GeneratePDF("Ezapiya", value, DateTime.Now.ToString("dd-MMM-yyyy"), gst_value, total_value, total_value, party_name, party_address, other_info, payment_id, Email, ed.ToString("dd-MMM-yyyy"), Invoice_number);

                                String subject = "Invoice of Ezapiya";
                                String message = "Invoice of Ezapiya";
                                String pdf_file = "Invoice\\Invoice" + Invoice_number + ".pdf";
                                
                                Task.Run(() => {
                                    email_send(Email, subject, message, pdf_file);
                                });
                               

                                send_data("105:"); 
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_106 request for user profile
                        case 106:
                            try
                            {
                                String username = string_from_client[1];
                                    DataTable dt = new DataTable();
                                    dt = Program.db.get_data_table("select * from user_table where Username='" + username + "'");
                                    String temp = dt.Rows[0][0].ToString() + ":" + dt.Rows[0][1].ToString() + ":" + dt.Rows[0][2].ToString() + ":" + dt.Rows[0][3].ToString() + ":" + dt.Rows[0][4].ToString() + ":" + dt.Rows[0][6].ToString();


                                send_data("106:"+temp);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_107 Update profile
                        case 107:
                            try
                            {
                                String Name = string_from_client[1];
                                String username = string_from_client[2];
                                String Address = string_from_client[3];
                                String Mobile_No = string_from_client[4];
                                String Email = string_from_client[5];

                                String temp = "update user_table set Name='" + Name + "',Address='" + Address + "',Mobile_No='" + Mobile_No + "',Email='" + Email + "' where Username='" + username + "'";
                                Program.db.run_sql(temp);
                                send_data("107:" + temp);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_108 change password
                        case 108:
                            try
                            {
                               
                                String old_password = CryptoEngine.Decrypt(string_from_client[1], "fdfdfdfdf"); 
                                String password = CryptoEngine.Decrypt(string_from_client[2], "fdfdfdfdf"); 
                                String username = string_from_client[3];

                                DataTable dt = new DataTable();
                                dt = Program.db.get_data_table("select * from user_table where Username='" + username + "' and Password='" + old_password + "'");
                                if (dt.Rows.Count <= 0)
                                {
                                    send_data("108:" + "Old Password not match");
                                }
                                else
                                {
                                    String temp = "update user_table set Password='" + password + "' where Username='" + username + "'";
                                    Program.db.run_sql(temp);
                                    send_data("108:" + "Password Changed");
                                }

                                
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_109 request for payment history
                        case 109:
                            try
                            {
                                String sql = "select * from purchase_table where username='" + string_from_client[1] +"'";
                                DataTable dt = new DataTable();
                                dt = Program.db.get_data_table(sql);
                                String ec_result = "";
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    ec_result=ec_result+i.ToString()+"\t"+dt.Rows[i][1].ToString()+"\t"+dt.Rows[i][2].ToString()+"\t"+dt.Rows[i][3].ToString()+"\t"+dt.Rows[i][4].ToString()+"\n";
                                    if (i % 10 == 0 && i!=0)
                                    {
                                        send_data("109:" + ec_result);
                                        ec_result = "";
                                        Thread.Sleep(50);
                                    }
                                }
                                send_data("109:" + ec_result);

                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_110 request  for Reference
                        case 110:
                            try
                            {
                                String sql = "select * from user_table where Reference_Id='" + string_from_client[1] + "' order by id";
                                DataTable dt = new DataTable();
                                dt = Program.main_db.get_data_table(sql);
                                String ec_result = "";
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    ec_result = ec_result + i.ToString() + "\t" + dt.Rows[i][1].ToString() + "\t" + dt.Rows[i][0].ToString() + "\t" + dt.Rows[i][2].ToString() + "\t" + dt.Rows[i][3].ToString() + "\t" + dt.Rows[i][4].ToString() + "\t" + Convert.ToDateTime( dt.Rows[i][8]).ToString("dd-MMM-yyyy") + "\n";
                                    if (i % 10 == 0 && i != 0)
                                    {
                                        send_data("110:" + ec_result);
                                        ec_result = "";
                                        Thread.Sleep(50);
                                    }
                                }
                                send_data("110:" + ec_result);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_111 request for full_detail Reference
                        case 111:
                            try
                            {

                                DataTable main_dt = new DataTable();
                                main_dt.Columns.Add("Reference");
                                main_dt.Columns.Add("Payment_date");
                                main_dt.Columns.Add("Expair_Date");
                                main_dt.Columns.Add("day");
                                main_dt.Columns.Add("Amount");
                                main_dt.Columns.Add("bill_Amount");
                                main_dt.Columns.Add("Form");

                                if (string_from_client[1].CompareTo("") == 0)
                                {
                                    goto abc;
                                }
                                String sql = "select * from purchase_table where ref_by='" + string_from_client[1] + "' and  count_ > 0 and paid_status='not paid' order by count_";
                                DataTable dtp = new DataTable();
                                dtp = Program.main_db.get_data_table(sql);
                                for (int j = 0; j < dtp.Rows.Count; j++)
                                {
                                    if (Convert.ToInt16(dtp.Rows[j][14]) <= 5)
                                    {
                                        DataRow dr1 = main_dt.NewRow();
                                        dr1[0] = dtp.Rows[j][0].ToString();
                                        dr1[1] = dtp.Rows[j][1].ToString();
                                        dr1[2] = dtp.Rows[j][2].ToString();
                                        dr1[3] = dtp.Rows[j][3].ToString();
                                        dr1[4] = "200";
                                        dr1[5] = dtp.Rows[j][7].ToString();
                                        dr1[6] = dtp.Rows[j][12].ToString();
                                        main_dt.Rows.Add(dr1);
                                    }
                                    else
                                    {
                                        DataRow dr1 = main_dt.NewRow();
                                        dr1[0] = dtp.Rows[j][0].ToString();
                                        dr1[1] = dtp.Rows[j][1].ToString();
                                        dr1[2] = dtp.Rows[j][2].ToString();
                                        dr1[3] = dtp.Rows[j][3].ToString();
                                        dr1[4] = Convert.ToString(Convert.ToInt16(dtp.Rows[j][7]) / 2);
                                        dr1[5] = dtp.Rows[j][7].ToString();
                                        dr1[6] = dtp.Rows[j][12].ToString();
                                        main_dt.Rows.Add(dr1);
                                    }

                                }

                                abc:;

                                Int32 amt = 0;
                                String ec_result = "";
                                for (int i = 0; i < main_dt.Rows.Count; i++)
                                {
                                    ec_result = ec_result + i.ToString() + "\t" + main_dt.Rows[i][0].ToString() + "\t" + main_dt.Rows[i][1].ToString() + "\t" + main_dt.Rows[i][2].ToString() + "\t" + main_dt.Rows[i][3].ToString() + "\t" + main_dt.Rows[i][4].ToString() + "\t" + main_dt.Rows[i][5].ToString() + "\t" + main_dt.Rows[i][6].ToString() + "\n";
                                    if (i % 10 == 0 && i != 0)
                                    {
                                        send_data("111:" + ec_result);
                                        ec_result = "";
                                    }
                                    try
                                    {
                                        amt = amt + Convert.ToInt32(main_dt.Rows[i][4].ToString());
                                    }
                                    catch
                                    { }
                                }
                                send_data("111:" + ec_result);
                                Thread.Sleep(50);
                                send_data("112:" + amt.ToString());
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion
                        // #endregion
                        #region case_113 request for btnWithdraw
                        case 113:
                            try
                            {
                                String login_name = string_from_client[1];
                                String type = string_from_client[2];
                                String bank_name = string_from_client[3];
                                String ifsc_code = string_from_client[4];
                                String Withdraw_amount = string_from_client[5];
                                String upi_number = string_from_client[6];
                                String account_holder_name = string_from_client[7];
                                String account_number = string_from_client[8];

                                String req_date = DateTime.Now.ToString("dd-MMM-yyyy");
                                String sql = "insert into Withdraw_table values('" + login_name + "','" + req_date + "'," + Withdraw_amount + ",'" + type + "','" + bank_name + "','" + ifsc_code + "','" + upi_number + "','"+ account_holder_name + "','"+ account_number + "')";
                                Program.main_db.run_sql(sql);
                                
                                sql = "update purchase_table set paid_status='paid',paid_date='"+ req_date + "'  where ref_by='" + string_from_client[1] + "' and paid_status='not paid'";
                                Program.main_db.run_sql(sql);
                                




                                send_data("113:" + "-");
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_114 request  for all Reference payment
                        case 114:
                            try
                            {
                                String sql = "select count_,username,purcahse_date,day,amount,paid_status,paid_date from purchase_table where ref_by='" + string_from_client[1] + "' order by count_";
                                DataTable dt = new DataTable();
                                dt = Program.main_db.get_data_table(sql);
                                String ec_result = "";
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    ec_result = ec_result + dt.Rows[i][0].ToString() + "\t" + dt.Rows[i][1].ToString() + "\t" + dt.Rows[i][2].ToString() + "\t" + dt.Rows[i][3].ToString() + "\t" + dt.Rows[i][4].ToString() + "\t" + dt.Rows[i][5].ToString() + "\t" + dt.Rows[i][6].ToString() + "\n";
                                    if (i % 10 == 0 && i != 0)
                                    {
                                        send_data("114:" + ec_result);
                                        ec_result = "";
                                        Thread.Sleep(50);
                                    }
                                }
                                send_data("114:" + ec_result);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_115 request  for all Reference payment
                        case 115:
                            try
                            {
                                String sql = "select * from Withdraw_table where user_name='" + string_from_client[1] + "'";
                                DataTable dt = new DataTable();
                                dt = Program.main_db.get_data_table(sql);
                                String ec_result = "";
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    ec_result = ec_result +  Convert.ToDateTime( dt.Rows[i][1]).ToString("dd-MMM-yyyy") + "\t" + dt.Rows[i][2].ToString() + "\t" + dt.Rows[i][3].ToString() + "\t" + dt.Rows[i][4].ToString() + "\t" + dt.Rows[i][5].ToString() + "\t" + dt.Rows[i][6].ToString() + "\t" + dt.Rows[i][7].ToString() + "\t" + dt.Rows[i][8].ToString() + "\n";
                                    if (i % 10 == 0 && i != 0)
                                    {
                                        send_data("115:" + ec_result);
                                        ec_result = "";
                                        Thread.Sleep(50);
                                    }
                                }
                                send_data("115:" + ec_result);
                            }
                            catch (Exception ex)
                            { }
                            break;
                        #endregion

                        #region case_120 Forgot Password (change password and and send by email)
                        case 120:
                            try
                            {
                               
                               
                                String email = string_from_client[1];
                                String password = "";
                                DataTable dt = new DataTable();
                                dt = Program.db.get_data_table("select * from user_table where Email='" + email + "'");
                                if (dt.Rows.Count <= 0)
                                {
                                    email = dt.Rows[0][4].ToString();
                                }
                                Random r = new Random();
                                password = r.Next(5000, 9999).ToString();

                                String temp = "update user_table set Password='" + password + "' where Email='" + email + "'";
                                Program.db.run_sql(temp);
                                send_data("120:" + "Password send to your Email");

                                email_send(email, "Password of Ezapiya", password);


                            }
                            catch (Exception ex)
                            { }
                            break;
                            #endregion
                    }
                    Thread.Sleep(100);
                }
                catch (SocketException e)
                {
                    close_connection(0);
        
                }
                catch (Exception ex)
                {
                  
                }
            }
        
        
        }
        private static bool CheckDateRange(DateTime date, DateTime datePast, DateTime dateFuture)
        {
            if (datePast <= date && date <= dateFuture)
                    return true;
            else
                    return false;
        }  
        public void close_connection(int reson)
        {
//            String sql = "update register_pc set pc_status='offline' where uid='" + this.uid + "'";
  //          Program.db.run_sql(sql);
            if (this.uid.CompareTo("") != 0)
            {
                Console.WriteLine("Client ip is \t" + this.IP + "\t uid is \t" + this.uid + " Now offline \t"+reson);
                //Console.WriteLine(this.uid + " is now offline");
            }
            this.uid = "";
            this.IP = "";
            this.MachineName = "";
            this.pass_word = "";
            this.login_name="";
            this.hartbit = 0;
            Socket.Close();
            Socket.Dispose();

        }

        private void check_isonline()
        {
            send_data(":");
            if (Socket.Connected)
            {
                Thread.Sleep(3000);
               // Task t = new Task(check_isonline);
               // t.Start();
                Thread t = new Thread(check_isonline);
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                close_connection(1);
            }
        }
       
        private void send_data(String data)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                Socket.Send(bytes);
            }
            catch (Exception ex)
            { }
        }

        public void GeneratePDF(String Description, String Price, String Payment_Date, String GST, String Total_amt, String grand_total, String Party_Name, String Address_of_party, String other_information, String Payment_ID, String Email, String Expire_Date, String Invoice_Number)
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                iTextSharp.text.Image sigimage = iTextSharp.text.Image.GetInstance("d:\\abc.png");
                document.Add(sigimage);
                Paragraph blank_para = new Paragraph();
                blank_para.Add("\n\n");

                document.Add(blank_para);
                string text = "Invoice";
                Paragraph paragraph = new Paragraph();
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 20f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                paragraph.Add(text);
                document.Add(paragraph);

                document.Add(blank_para);


                String bill_data = "             Bill To \n";
                bill_data = bill_data + "                     \t" + Party_Name + "  \n";
                bill_data = bill_data + "                     \t" + Address_of_party + "  \n";
                bill_data = bill_data + "                     \t" + other_information + "  \n";

                PdfContentByte cb = writer.DirectContent;
                ColumnText ct = new ColumnText(cb);
                Phrase myText = new Phrase(bill_data);
                ct.SetSimpleColumn(myText, 0, 600, 250, 317, 15, Element.ALIGN_LEFT);
                ct.Go();


                String Details_data = "Details" + "\n";
                Details_data = Details_data + "        Payment ID  :- " + Payment_ID + "\n";
                Details_data = Details_data + "        Email            :- " + Email + " \n";
                Details_data = Details_data + "        Expire Date  :- " + Expire_Date + " \n";
                Details_data = Details_data + "        Invoice No.   :- " + Invoice_Number + " \n";


                ColumnText ct1 = new ColumnText(cb);
                Phrase myText1 = new Phrase(Details_data);
                ct1.SetSimpleColumn(myText1, 300, 600, 570, 317, 15, Element.ALIGN_LEFT);
                ct1.Go();


                document.Add(blank_para);
                document.Add(blank_para);

                document.Add(blank_para);

                document.Add(blank_para);

                PdfPTable t = new PdfPTable(6);
                float[] columnWidths = new float[6];
                columnWidths[0] = 50;
                columnWidths[1] = 250;
                columnWidths[2] = 100;
                columnWidths[3] = 150;
                columnWidths[4] = 100;
                columnWidths[5] = 100;
                t.SetTotalWidth(columnWidths);

                // table header 
                PdfPCell sr_heder = new PdfPCell();
                sr_heder.AddElement(new Chunk("Sr."));
                t.AddCell(sr_heder);

                PdfPCell Description_heder = new PdfPCell();
                Description_heder.AddElement(new Chunk("Description"));
                t.AddCell(Description_heder);

                PdfPCell Price_heder = new PdfPCell();
                Price_heder.AddElement(new Chunk("Price"));
                t.AddCell(Price_heder);

                PdfPCell Payment_Date_heder = new PdfPCell();
                Payment_Date_heder.AddElement(new Chunk("Payment Date"));
                t.AddCell(Payment_Date_heder);

                PdfPCell GST_heder = new PdfPCell();
                GST_heder.AddElement(new Chunk("GST"));
                t.AddCell(GST_heder);

                PdfPCell Total_heder = new PdfPCell();
                Total_heder.AddElement(new Chunk("Total"));
                t.AddCell(Total_heder);
                // table header end

                // table data 
                PdfPCell sr_data = new PdfPCell();
                sr_data.AddElement(new Chunk("1"));
                t.AddCell(sr_data);

                PdfPCell Description_data = new PdfPCell();
                Description_data.AddElement(new Chunk(Description));
                t.AddCell(Description_data);

                PdfPCell Price_data = new PdfPCell();
                Price_data.AddElement(new Chunk(Price));
                t.AddCell(Price_data);

                PdfPCell Payment_Date_data = new PdfPCell();
                Payment_Date_data.AddElement(new Chunk(Payment_Date));
                t.AddCell(Payment_Date_data);

                PdfPCell GST_data = new PdfPCell();
                GST_data.AddElement(new Chunk(GST));
                t.AddCell(GST_data);

                PdfPCell Total_data = new PdfPCell();
                Total_data.AddElement(new Chunk(Total_amt));
                t.AddCell(Total_data);

                // table data end 

                PdfPCell b = new PdfPCell();
                b.Colspan = 5;
                b.AddElement(new Chunk("total"));
                b.HorizontalAlignment = 1;
                t.AddCell(b);

                PdfPCell d = new PdfPCell();
                d.AddElement(new Chunk(grand_total));
                t.AddCell(d);

                document.Add(t);

                document.Close();
                byte[] bytes = memoryStream.ToArray();


                memoryStream.Close();
                File.WriteAllBytes("Invoice\\Invoice" + Invoice_Number+".pdf", bytes);
            }
        }

        public void email_send(String send_to_email, String subject, String mail_body, String file_to_send)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            // mail.From = new MailAddress("support@Ezapiya.com");
            mail.From = new MailAddress("vinit.ramteke@gmail.com");
            //mail.To.Add("vinit.ramteke@gmail.com");
            mail.To.Add(send_to_email);
            mail.Subject = subject;
            mail.Body = mail_body;

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(file_to_send);
            mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("vinit.ramteke@gmail.com", "Ap@131211");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
        public void email_send(String send_to_email, String subject, String mail_body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            // mail.From = new MailAddress("support@Ezapiya.com");
            mail.From = new MailAddress("vinit.ramteke@gmail.com");
            //mail.To.Add("vinit.ramteke@gmail.com");
            mail.To.Add(send_to_email);
            mail.Subject = subject;
            mail.Body = mail_body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("vinit.ramteke@gmail.com", "Ap@131211");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
        ///
    }
}
