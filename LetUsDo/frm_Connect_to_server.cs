using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LetUsDo
{
    public partial class frm_Connect_to_server : Form
    {

        int connection_index = 0;
        frm_waiting_for_user_responce frm_waiting = new frm_waiting_for_user_responce("10");
        Frm_user_responce frm_user_responce = new Frm_user_responce("10");
        String form_type = "";
        bool var_show_form = false;
        Int32 p1, p2, p3;
        String ip = "";
        int use_day;
        bool is_pesonal_password = false;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        bool connecting_to_server = false;
        int hartbit = 0;

        String[] process_need_to_delete = new String[20];

  

       

        public frm_Connect_to_server()
        {
            InitializeComponent();
        }

        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
       
        void Ontcp_read_complit(object sender, EventArgs e)
        {
            try
            {
                String[] string_from_server = Local_Client.tcp_string.Split(':');
                switch (Convert.ToInt16(string_from_server[0]))
                {
                    #region case_0 geting uid pasword and remaing day
                    case 0:
                        this.Invoke(new MethodInvoker(delegate
                        {
                            txtourID.Text = string_from_server[1];
                            txtourPassword.Text = string_from_server[2];
                            Globle_data.uid = string_from_server[1];
                            use_day = Convert.ToInt16(string_from_server[3]);
                            Globle_data.system_date = Convert.ToDateTime(string_from_server[4]);

                            String str_tcp = "6:";
                            Local_Client.send(str_tcp);
                            Thread.Sleep(500);
                            str_tcp = "9:";
                            Local_Client.send(str_tcp);
                        }));
                        break;
                    #endregion

                    #region case_1
                    case 1:
                        this.Invoke(new MethodInvoker(delegate
                        {
                            MessageBox.Show(string_from_server[1]);
                        }));
                        break;
                    #endregion

                    #region case_2 show waiting from 
                    case 2:
                        this.Invoke(new MethodInvoker(delegate
                        {
                            Thread t = new Thread(show_waiting);
                            t.IsBackground = true;
                            t.Start();
                        }));
                        break;
                    #endregion

                    #region case_3 user responce for connection
                    case 3:
                        this.Invoke(new MethodInvoker(delegate
                        {
                            frm_user_responce.i = 0;
                            DialogResult dr= frm_user_responce.ShowDialog();

                            
                            if (dr == DialogResult.Yes)
                            {
                                String str_tcp = "3:" +"allow" +":" + string_from_server[1];
                                Local_Client.send(str_tcp);
                                form_type = "a";
                                is_pesonal_password = true;
                            }
                            if (dr == DialogResult.No)
                            {
                                String str_tcp = "3:" + "reject" + ":" + string_from_server[1];
                                Local_Client.send(str_tcp);
                            }
                        }));
                        break;
                    #endregion

                    #region case_4 geting user responce and removing waiting from
                    case 4:
                        if (string_from_server[1].CompareTo("allow") == 0)
                        {
                            frm_waiting.Invoke(new MethodInvoker(delegate
                            {
                                frm_waiting.Hide();
                                form_type = "c";
                              

                            }));
                        }
                        if (string_from_server[1].CompareTo("reject") == 0)
                        {
                            frm_waiting.Invoke(new MethodInvoker(delegate
                            {
                                frm_waiting.Hide();
                                MessageBox.Show("Remote Computer rejected your connection request");
                            }));
                        }
                        
                        break;
                    #endregion

                    #region case_5 connecting form personal password
                    case 5:
                            // when connecting form personal password 
                        Thread.Sleep(1000);
                               String  str_tcp_x = "3:" +"allow" +":" + string_from_server[1];
                                Local_Client.send(str_tcp_x);
                                form_type = "a";
                        is_pesonal_password = true;
                        break;
                    #endregion

                    #region case_7 Personal Password changed message
                    case 7:
                        if (string_from_server[1].CompareTo("passward_change") == 0)
                        {
                            MessageBox.Show("Personal Password changed");
                        }
                        break;
                    #endregion

                    #region case_8 making connection 
                    case 8:
                        p1 = Convert.ToInt32(string_from_server[1]);
                        p2 = Convert.ToInt32(string_from_server[2]);
                        p3 = Convert.ToInt32(string_from_server[3]);
                        //ip = string_from_server[5];
                        ip = string_from_server[5].Replace("\0", String.Empty);
                        var_show_form = true;
                        break;
                    #endregion

                    #region case_10 old_connection
                    case 10:
                        this.Invoke((MethodInvoker)delegate
                        {
                            try
                            {
                                Array.Clear(Local_Client.tcp_data, 0, Local_Client.tcp_data.Length);
                               // dgv_old_connection.Rows.Clear();
                                String[] row_data = string_from_server[1].Split('\n');
                                for (int i = 0; i < row_data.Length-1; i++)
                                {
                                    String[] col_data = row_data[i].Split('\t');
                                    dgv_old_connection.Rows.Add(1);
                                    dgv_old_connection.Rows[dgv_old_connection.Rows.Count - 1].Cells["col_sr"].Value = dgv_old_connection.Rows.Count.ToString();
                                    dgv_old_connection.Rows[dgv_old_connection.Rows.Count - 1].Cells["col_pc_name"].Value = "-";
                                    dgv_old_connection.Rows[dgv_old_connection.Rows.Count - 1].Cells["col_uid"].Value = col_data[0];
                                    dgv_old_connection.Rows[dgv_old_connection.Rows.Count - 1].Cells["col_save"].Value = "Save";
                                    if (col_data[1].CompareTo("online") == 0)
                                    {
                                        dgv_old_connection.Rows[dgv_old_connection.Rows.Count - 1].Cells["col_status"].Value = Properties.Resources.online24;
                                    }
                                    if (col_data[1].CompareTo("offline") == 0)
                                    {
                                        dgv_old_connection.Rows[dgv_old_connection.Rows.Count - 1].Cells["col_status"].Value = Properties.Resources.offline24;
                                    }
                                    
                                    for (int j = 0; j < dgv_save_contact.Rows.Count; j++)
                                    {
                                        if (dgv_save_contact.Rows[j].Cells["col_contact_uid"].Value.ToString().CompareTo(col_data[0]) == 0)
                                        {
                                            dgv_old_connection.Rows[dgv_old_connection.Rows.Count - 1].Cells["col_save"].Value = "View";
                                            break;
                                        }
                                    }
                                }


                            }
                            catch
                            { }
                           // tabControl1.SelectedIndex = 2;
                           // set_btnbackground_null();
                           // toolStripContact.BackColor = Color.Transparent;
                        });
                        break;
                    #endregion

                    #region case_12 save contact message
                    case 12:
                        this.Invoke((MethodInvoker)delegate {
                            String str_tcp = "9:";
                            Local_Client.send(str_tcp);
                            MessageBox.Show("Contact Saved");

                        });
                        break;
                    #endregion

                    #region case_13 geting saved contact
                    case 13:
                        this.Invoke((MethodInvoker)delegate {
                           
                           // dgv_save_contact.Rows.Clear();
                            String[] row_data = string_from_server[1].Split('\n');
                            for (int i = 0; i < row_data.Length-1; i++)
                            {
                                try
                                {
                                    String[] contact_data = row_data[i].Split('\t');
                                    String contact_uid = contact_data[1];
                                    String contact_name = contact_data[2];
                                    String contact_phone_number = contact_data[3];
                                    String contact_email = contact_data[4];
                                    String contact_amount = contact_data[5];
                                    String contact_other_info_1 = contact_data[6];
                                    String contact_other_info_2 = contact_data[7];
                                    String contact_personal_password = contact_data[8];
                                    String contact_status = contact_data[9];
                                    dgv_save_contact.Rows.Add(1);
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_sr"].Value = dgv_save_contact.Rows.Count.ToString();
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_uid"].Value = contact_uid;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_name"].Value = contact_name;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_phone_number"].Value = contact_phone_number;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_email"].Value = contact_email;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_amount"].Value = contact_amount;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_other_info_1"].Value = contact_other_info_1;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_other_info_2"].Value = contact_other_info_2;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_personal_password"].Value = contact_personal_password;
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_connect"].Value = "Connect";
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_view"].Value = "View";
                                    dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_delete"].Value = "Delete";

                                    if (contact_status.CompareTo("online") == 0)
                                    {
                                        dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_status"].Value = Properties.Resources.online24;
                                    }
                                    if (contact_status.CompareTo("offline") == 0)
                                    {
                                        dgv_save_contact.Rows[dgv_save_contact.Rows.Count - 1].Cells["col_contact_status"].Value = Properties.Resources.offline24;
                                    }
                                }
                                catch
                                { }
                            }
                        });
                        break;
                    #endregion

                    #region case_16  hartbit from server
                    case 16: // hartbit from server
                        hartbit=0;
                        break;
                    #endregion

              

                }
            }
            catch (Exception ex)
            { }
           
        }

        public void show_waiting()
         {

             frm_waiting.ShowDialog();
         }
     
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
           
        }
       
       

        private void frm_Connect_to_server_Load(object sender, EventArgs e)
        {
         
                tabControl1.Appearance = TabAppearance.FlatButtons;
                tabControl1.ItemSize = new Size(0, 1);
                tabControl1.SizeMode = TabSizeMode.Fixed;
                tabControl1.SelectedIndex = 0;
                set_btnbackground_null();
                toolStripConnect.BackColor = Color.Transparent;
                try
                {
                    Local_Client.setup();
                    Local_Client.tcp_read_complit += Ontcp_read_complit;

                    if (Local_Client.Connect())
                    {
                        labconnectin_info.Text = "Ready to connect";
                        String str_tcp = "0:" + HardwareInfo.GetHDDSerialNo() + ":" + Environment.MachineName;
                        Local_Client.send(str_tcp);
                    }
                    Thread.Sleep(1000);
                    timer_auto_connect.Enabled = true;
                    
                   
                }
                catch (Exception ex)
                {
                    timer_auto_connect.Enabled = true;
                    labconnectin_info.Text = "Trying to Reconnect";
                }
          
        }
        
        private void btnconnect_Click(object sender, EventArgs e)
        {
                
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (var_show_form)
            {
                var_show_form = false;
                if (form_type.CompareTo("c") == 0)
                {
                    Image_reciver fc = new Image_reciver(p1,ip);
                    fc.Show();
                    connection_index++;
                }
                if (form_type.CompareTo("a") == 0)
                {
                    Image_sender fa = new Image_sender(p1,ip);
                    fa.Show();
                    if (is_pesonal_password == true)
                    {
                        is_pesonal_password = false;
                        fa.Visible = false;
                    }
                    connection_index++;
                }
            }
       }

        private void labpersonal_password_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var form = new frm_personal_password())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string personal_password = form.personal_password;           
                    String str_tcp = "4:" + txtourID.Text  + ":" + personal_password;
                    Local_Client.send(str_tcp);
                }
            }
        }

        public void set_btnbackground_null()
        {
            toolStripConnect.BackColor = Color.Transparent;
            toolStripContact.BackColor = Color.Transparent;
            toolStripSetting.BackColor = Color.Transparent;
            toolStripHelp.BackColor = Color.Transparent;
            toolStripFeedback.BackColor = Color.Transparent;
         
        }

        private void frm_Connect_to_server_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void toolStripConnect_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            set_btnbackground_null();
            toolStripConnect.BackColor = Color.Transparent;
        }
        private void toolStripContact_Click(object sender, EventArgs e)
        {
           
            
        }

        private void toolStripSetting_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            set_btnbackground_null();
            toolStripSetting.BackColor = Color.Transparent;
        }

       

        private void toolStripHelp_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
            set_btnbackground_null();
            toolStripHelp.BackColor = Color.Transparent;
        }

        private void toolStripFeedback_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
            set_btnbackground_null();
            toolStripFeedback.BackColor = Color.Transparent;
        }

        private void dgv_old_connection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (e.ColumnIndex == 4) // for connect button 
            {
                tabControl1.SelectedIndex = 0;
                set_btnbackground_null();
                toolStripConnect.BackColor = Color.Transparent;
                String udi_to_connect = dgv_old_connection.Rows[i].Cells[2].Value.ToString();
                txtpartnerID.Text = udi_to_connect;

            }
            if (e.ColumnIndex == 5) // for save and edit button 
            {
                String command = dgv_old_connection.Rows[i].Cells[5].Value.ToString();
                String udi_to_save = dgv_old_connection.Rows[i].Cells[2].Value.ToString();
                if (command.CompareTo("Save") == 0)
                {
                    frm_save_contact fsc = new frm_save_contact();
                    fsc.txtcontact_uid.Enabled = false;
                    fsc.contact_uid = udi_to_save;
                    fsc.contact_name = "";
                    fsc.contact_phone_number = "";
                    fsc.contact_email = "";
                    fsc.contact_amount = "";
                    fsc.contact_other_info_1 = "";
                    fsc.contact_other_info_2 = "";
                    fsc.contact_personal_password = "";


                    DialogResult dr = fsc.ShowDialog();
                    if (dr == DialogResult.Yes)
                    {
                        String str_tcp = "8:" + txtourID.Text + ":" + fsc.contact_uid + ":" + fsc.contact_name + ":" + fsc.contact_phone_number + ":" + fsc.contact_email + ":" + fsc.contact_amount + ":" + fsc.contact_other_info_1 + ":" + fsc.contact_other_info_2 + ":" + fsc.contact_personal_password;
                        Local_Client.send(str_tcp);
                    }

                   // MessageBox.Show(command);    

                }
            }
        }

        private void dgv_save_contact_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             int i = e.RowIndex;
            if (e.ColumnIndex == 9) // for connect button 
            {
                  
                    int x = dgv_save_contact.CurrentRow.Index;
                    String pp = CryptoEngine.Encrypt(dgv_save_contact.Rows[x].Cells["col_contact_personal_password"].Value.ToString(), "4589-8754-dfgfg");
                    String str_tcp = "1:" + dgv_save_contact.Rows[x].Cells["col_contact_uid"].Value.ToString() + ":" + pp;
                    Local_Client.send(str_tcp);
                

            }
            if (e.ColumnIndex == 10) // for save and edit button 
            {
                frm_save_contact fsc = new frm_save_contact();
                int x = dgv_save_contact.CurrentRow.Index;
               fsc.contact_uid=  dgv_save_contact.Rows[x].Cells["col_contact_uid"].Value.ToString();
               fsc.contact_name = dgv_save_contact.Rows[x].Cells["col_contact_name"].Value.ToString();
               fsc.contact_phone_number = dgv_save_contact.Rows[x].Cells["col_contact_phone_number"].Value.ToString();
               fsc.contact_email = dgv_save_contact.Rows[x].Cells["col_contact_email"].Value.ToString();
               fsc.contact_amount = dgv_save_contact.Rows[x].Cells["col_contact_amount"].Value.ToString();
               fsc.contact_other_info_1 = dgv_save_contact.Rows[x].Cells["col_contact_other_info_1"].Value.ToString();
               fsc.contact_other_info_2 = dgv_save_contact.Rows[x].Cells["col_contact_other_info_2"].Value.ToString();
               fsc.contact_personal_password = dgv_save_contact.Rows[x].Cells["col_contact_personal_password"].Value.ToString();
               DialogResult dr = fsc.ShowDialog();
               if (dr == DialogResult.Yes)
               {
                  String str_tcp = "11:" + txtourID.Text + ":" + fsc.contact_uid + ":" + fsc.contact_name + ":" + fsc.contact_phone_number + ":" + fsc.contact_email + ":" + fsc.contact_amount + ":" + fsc.contact_other_info_1 + ":" + fsc.contact_other_info_2 + ":" + fsc.contact_personal_password;
                   Local_Client.send(str_tcp);
               }
               // MessageBox.Show("View clike");  
            }
            if (e.ColumnIndex == 11) // for save and edit button 
            {
                 int x = dgv_save_contact.CurrentRow.Index;
                 String str_tcp = "21:" + txtourID.Text + ":" + dgv_save_contact.Rows[x].Cells["col_contact_uid"].Value.ToString();
                Local_Client.send(str_tcp);
            }
        }

        public void connect_with_server()
        {
            try
            {
                this.Invoke((MethodInvoker)delegate {
                    if (IsConnectedToInternet())
                    {
                        labconnectin_info.Text = "Trying to reconnect";
                        if (Local_Client.Connect())
                        {
                            hartbit = 0;
                            String str_tcp = "0:" + HardwareInfo.GetHDDSerialNo() + ":" + Environment.MachineName;
                            Local_Client.send(str_tcp);
                        }
                    }
                    else
                    {
                        labconnectin_info.Text = "No Internet Connection";
                        txtourID.Text = "";
                        txtourPassword.Text = "";
                    }
                    
                });
                
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate {
                    labconnectin_info.Text = "Trying to reconnect";
                    txtourID.Text = "";
                    txtourPassword.Text = "";
                });
                
            }
           // Thread.Sleep(1000);
        }
       
        private void timer_auto_connect_Tick(object sender, EventArgs e)
        {
            Local_Client.send("10:");
            if (!Local_Client.TCP_Socket.Connected)
            {
                txtourID.Text = "";
                txtourPassword.Text = "";
                Thread t = new Thread(connect_with_server);
                t.IsBackground = true;
                t.Start();
                labconnectin_info.Text = "Ready to connect";
            }
            

        }

      

        private void labconnectin_info_Click(object sender, EventArgs e)
        {

        }

        private void btnrefresh_recent_contact_Click(object sender, EventArgs e)
        {
            String str_tcp = "6:";
            Local_Client.send(str_tcp);
            dgv_old_connection.Rows.Clear();
        }

        private void btn_refresh_saved_contact_Click(object sender, EventArgs e)
        {
            String str_tcp = "9:";
            Local_Client.send(str_tcp);
            dgv_save_contact.Rows.Clear();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frm_save_contact fsc = new frm_save_contact();
            fsc.txtcontact_uid.Enabled = true;
            fsc.contact_uid = "";
            fsc.contact_name = "";
            fsc.contact_phone_number = "";
            fsc.contact_email = "";
            fsc.contact_amount = "";
            fsc.contact_other_info_1 = "";
            fsc.contact_other_info_2 = "";
            fsc.contact_personal_password = "";


            DialogResult dr = fsc.ShowDialog();
            if (dr == DialogResult.Yes)
            {
                String str_tcp = "8:" + txtourID.Text + ":" + fsc.contact_uid + ":" + fsc.contact_name + ":" + fsc.contact_phone_number + ":" + fsc.contact_email + ":" + fsc.contact_amount + ":" + fsc.contact_other_info_1 + ":" + fsc.contact_other_info_2 + ":" + fsc.contact_personal_password;
                Local_Client.send(str_tcp);
            }
        }

        private void btnsearch_recent_connection_Click(object sender, EventArgs e)
        {
          
        }

        private void btnsearch_saved_connection_Click(object sender, EventArgs e)
        {
            
        }

        private void btnConect_Click(object sender, EventArgs e)
        {
            String pp = CryptoEngine.Encrypt(txtpartnerPassword.Text, "4589-8754-dfgfg");
            String str_tcp = "1:" + txtpartnerID.Text + ":" + pp;
            Local_Client.send(str_tcp);
        }

    }
}
