using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LetUsDo
{
    public partial class frm_file_system : Form
    {
        String[] last_path = new String[30];
        int path_index = 0;
        bool load_driver = false;

        String[] thispc_last_path = new String[30];
        int thispc_path_index = 0;


        public String String_from_main_window { get; set; }
        public String String_from_file_system_window { get; set; }
        public String path_to_upload_file { get; set; }
        public String file_to_upload { get; set; }
        public String file_to_download { get; set; }
        TreeNode seleced_tree_node = null;
        public String file_download_path { get; set; }

        public frm_file_system()
        {
            InitializeComponent();
        }

        private void frm_file_system_Load(object sender, EventArgs e)
        {
            String_from_file_system_window = "load_drive;" + DateTime.Now.ToString();
            load_driver = false;
            last_path[0] = "load_drive";

            // this pc driver_load 
            DriveInfo[] drive = DriveInfo.GetDrives();
            foreach (DriveInfo d in drive)
            {
                // drive_list = drive_list + d.ToString() + "\n";
                ListViewItem lvi = new ListViewItem();
                String imagelist_key = d.ToString();
                String imagelist_text = d.ToString();
                lvi.Name = imagelist_key;
                lvi.Text = imagelist_text.Replace(txtpath.Text + "\\", "");
                lvi.Tag = "Driver";
                lvi.ImageIndex = 1;

                thispc_listView.Items.Add(lvi);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (String_from_main_window.CompareTo("") != 0)
                {
                    string temp = String_from_main_window;
                    String_from_main_window = "";
                    // MessageBox.Show(temp);

                    if (load_driver == false)
                    {
                        listView1.Items.Clear();
                        String[] New_string = temp.Split('\n');
                        for (int i = 0; i < New_string.Length - 1; i++)
                        {
                            ListViewItem lvi = new ListViewItem();

                            String imagelist_key = New_string[i];
                            String imagelist_text = New_string[i];
                            lvi.Name = imagelist_key;
                            lvi.Text = imagelist_text.Replace(txtpath.Text + "\\", "");

                            lvi.Tag = "Driver";
                            lvi.ImageIndex = 1;
                            //listView1.Items.Add(imagelist_key, imagelist_text, 2);
                            listView1.Items.Add(lvi);

                            // listView1.Items.Add(New_string[i], 1);
                        }
                        load_driver = true;
                    }
                    else
                    {
                        listView1.Items.Clear();
                        String[] New_string = temp.Split('\n');

                        for (int i = 0; i < New_string.Length - 1; i++)
                        {

                            ListViewItem lvi = new ListViewItem();

                            String imagelist_key = New_string[i];
                            String imagelist_text = "";
                            String[] temp_x = New_string[i].Split('\\');
                            /*imagelist_text = New_string[i].Replace(txtpath.Text, "");
                            if (imagelist_text[0] == '\\')
                            {
                                imagelist_text = imagelist_text.Substring(1, imagelist_text.Length - 1);
                            }*/
                            if (temp_x.Length > 1)
                            {
                                imagelist_text = temp_x[temp_x.Length - 1];
                            }
                            else
                            {
                                imagelist_text = temp_x[0];
                            }

                            //String imagelist_text = New_string[i].Replace(seleced_tree_node.Text, "");
                            lvi.Name = imagelist_key;
                            lvi.Text = imagelist_text;
                            if (New_string[i].Contains('.') != true)
                            {
                                lvi.Tag = "Folder";
                                lvi.ImageIndex = 2;
                                //listView1.Items.Add(imagelist_key, imagelist_text, 2);
                                listView1.Items.Add(lvi);
                            }
                            else
                            {
                                String[] fdata = New_string[i].Split('.');
                                String file_extention = fdata[1];
                                int image_index = 7;

                                if (file_extention.CompareTo("txt") == 0)
                                    image_index = 6;
                                if (file_extention.CompareTo("rtf") == 0)
                                    image_index = 6;
                                if (file_extention.CompareTo("doc") == 0)
                                    image_index = 0;
                                if (file_extention.CompareTo("docx") == 0)
                                    image_index = 0;
                                if (file_extention.CompareTo("html") == 0)
                                    image_index = 3;
                                if (file_extention.CompareTo("htm") == 0)
                                    image_index = 3;
                                if (file_extention.CompareTo("rar") == 0)
                                    image_index = 9;
                                if (file_extention.CompareTo("zip") == 0)
                                    image_index = 9;
                                if (file_extention.CompareTo("java") == 0)
                                    image_index = 10;


                                //listView1.Items.Add(imagelist_key, imagelist_text, image_index);
                                lvi.Tag = "File";
                                lvi.ImageIndex = image_index;
                                listView1.Items.Add(lvi);
                            }


                        }

                    }

                }
            }
            catch (Exception ex)
            { }
        }


      
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            this.Hide();
            e.Cancel = true;
        }

      

       

        private void btnthispc_back_Click(object sender, EventArgs e)
        {
            if (thispc_path_index <= 1)
            {
                thispc_listView.Items.Clear();
                DriveInfo[] drive = DriveInfo.GetDrives();
                foreach (DriveInfo d in drive)
                {
                    // drive_list = drive_list + d.ToString() + "\n";
                    ListViewItem lvi = new ListViewItem();
                    String imagelist_key = d.ToString();
                    String imagelist_text = d.ToString();
                    lvi.Name = imagelist_key;
                    lvi.Text = imagelist_text.Replace(txtthispc_path.Text + "\\", "");
                    lvi.Tag = "Driver";
                    lvi.ImageIndex = 1;

                    thispc_listView.Items.Add(lvi);
                }
                txtthispc_path.Text = "This pc";
            }
            else
            {
                thispc_path_index--;
                txtthispc_path.Text = thispc_last_path[thispc_path_index];
                //String_from_file_system_window = "load_folder;" + thispc_last_path[thispc_path_index] + ";" + DateTime.Now.ToString();
                thispc_listView.Items.Clear();
                // String_from_file_system_window = "load_folder;" + path + ";" + DateTime.Now.ToString();
                DirectoryInfo dirname = new DirectoryInfo(thispc_last_path[thispc_path_index]);
                foreach (FileInfo fi in dirname.GetFiles())
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Name = fi.FullName;
                    lvi.Text = fi.FullName.Replace(txtthispc_path.Text, "");
                    if (lvi.Text[0] == '\\')
                    {
                        lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                    }

                    String[] fdata = fi.FullName.Split('.');
                    String file_extention = fdata[1];
                    int image_index = 7;

                    if (file_extention.CompareTo("txt") == 0)
                        image_index = 6;
                    if (file_extention.CompareTo("rtf") == 0)
                        image_index = 6;
                    if (file_extention.CompareTo("doc") == 0)
                        image_index = 0;
                    if (file_extention.CompareTo("docx") == 0)
                        image_index = 0;
                    if (file_extention.CompareTo("html") == 0)
                        image_index = 3;
                    if (file_extention.CompareTo("htm") == 0)
                        image_index = 3;
                    if (file_extention.CompareTo("rar") == 0)
                        image_index = 9;
                    if (file_extention.CompareTo("zip") == 0)
                        image_index = 9;
                    if (file_extention.CompareTo("java") == 0)
                        image_index = 10;

                    lvi.Tag = "File";
                    lvi.ImageIndex = image_index;
                    thispc_listView.Items.Add(lvi);

                    //  node_list = node_list + fi.FullName + "\n";
                }
                foreach (DirectoryInfo di in dirname.GetDirectories())
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Name = di.FullName;
                    lvi.Text = di.FullName.Replace(txtthispc_path.Text, "");
                    if (lvi.Text[0] == '\\')
                    {
                        lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                    }

                    lvi.Tag = "Folder";
                    lvi.ImageIndex = 2;
                    thispc_listView.Items.Add(lvi);

                    //node_list = node_list + di.FullName + "\n";
                }

            }
        }

        private void btnthispc_mycomputer_Click(object sender, EventArgs e)
        {
            thispc_listView.Items.Clear();

            DriveInfo[] drive = DriveInfo.GetDrives();
            foreach (DriveInfo d in drive)
            {
                // drive_list = drive_list + d.ToString() + "\n";
                ListViewItem lvi = new ListViewItem();
                String imagelist_key = d.ToString();
                String imagelist_text = d.ToString();
                lvi.Name = imagelist_key;
                lvi.Text = imagelist_text.Replace(txtpath.Text + "\\", "");
                lvi.Tag = "Driver";
                lvi.ImageIndex = 1;

                thispc_listView.Items.Add(lvi);
            }
        }

        private void btnthispc_mydocuemt_Click(object sender, EventArgs e)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            thispc_listView.Items.Clear();

            DirectoryInfo dirname = new DirectoryInfo(path);
            foreach (FileInfo fi in dirname.GetFiles())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Name = fi.FullName;
                lvi.Text = fi.FullName.Replace(path, "");
                if (lvi.Text[0] == '\\')
                {
                    lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                }
                String[] fdata = fi.FullName.Split('.');
                String file_extention = fdata[1];
                int image_index = 7;

                if (file_extention.CompareTo("txt") == 0)
                    image_index = 6;
                if (file_extention.CompareTo("rtf") == 0)
                    image_index = 6;
                if (file_extention.CompareTo("doc") == 0)
                    image_index = 0;
                if (file_extention.CompareTo("docx") == 0)
                    image_index = 0;
                if (file_extention.CompareTo("html") == 0)
                    image_index = 3;
                if (file_extention.CompareTo("htm") == 0)
                    image_index = 3;
                if (file_extention.CompareTo("rar") == 0)
                    image_index = 9;
                if (file_extention.CompareTo("zip") == 0)
                    image_index = 9;
                if (file_extention.CompareTo("java") == 0)
                    image_index = 10;

                lvi.Tag = "File";
                lvi.ImageIndex = image_index;
                thispc_listView.Items.Add(lvi);

                //  node_list = node_list + fi.FullName + "\n";
            }
            foreach (DirectoryInfo di in dirname.GetDirectories())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Name = di.FullName;
                lvi.Text = di.FullName.Replace(path, ""); ;
                if (lvi.Text[0] == '\\')
                {
                    lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                }

                lvi.Tag = "Folder";
                lvi.ImageIndex = 2;
                thispc_listView.Items.Add(lvi);
                //node_list = node_list + di.FullName + "\n";
            }

            txtthispc_path.Text = path;
            thispc_path_index++;
            thispc_last_path[thispc_path_index] = path;
        }

        private void btnthispcdesktop_Click(object sender, EventArgs e)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            thispc_listView.Items.Clear();

            DirectoryInfo dirname = new DirectoryInfo(path);
            foreach (FileInfo fi in dirname.GetFiles())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Name = fi.FullName;
                lvi.Text = fi.FullName.Replace(path, "");
                if (lvi.Text[0] == '\\')
                {
                    lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                }
                String[] fdata = fi.FullName.Split('.');
                String file_extention = fdata[1];
                int image_index = 7;

                if (file_extention.CompareTo("txt") == 0)
                    image_index = 6;
                if (file_extention.CompareTo("rtf") == 0)
                    image_index = 6;
                if (file_extention.CompareTo("doc") == 0)
                    image_index = 0;
                if (file_extention.CompareTo("docx") == 0)
                    image_index = 0;
                if (file_extention.CompareTo("html") == 0)
                    image_index = 3;
                if (file_extention.CompareTo("htm") == 0)
                    image_index = 3;
                if (file_extention.CompareTo("rar") == 0)
                    image_index = 9;
                if (file_extention.CompareTo("zip") == 0)
                    image_index = 9;
                if (file_extention.CompareTo("java") == 0)
                    image_index = 10;

                lvi.Tag = "File";
                lvi.ImageIndex = image_index;
                thispc_listView.Items.Add(lvi);

                //  node_list = node_list + fi.FullName + "\n";
            }
            foreach (DirectoryInfo di in dirname.GetDirectories())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Name = di.FullName;
                lvi.Text = di.FullName.Replace(path, ""); ;
                if (lvi.Text[0] == '\\')
                {
                    lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                }

                lvi.Tag = "Folder";
                lvi.ImageIndex = 2;
                thispc_listView.Items.Add(lvi);
                //node_list = node_list + di.FullName + "\n";
            }

            txtthispc_path.Text = path;
            thispc_path_index++;
            thispc_last_path[thispc_path_index] = path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String_from_file_system_window = "load_drive;" + DateTime.Now.ToString();
            load_driver = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String_from_file_system_window = "document;" + DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String_from_file_system_window = "desktop;" + DateTime.Now.ToString();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected == true)
                {

                    String path = listView1.Items[i].Name;
                    // textBox1.Text = path;
                    // listView1.Items.Clear();
                    // LoadFilesAndDir(path);

                    listView1.Items.Clear();
                    String_from_file_system_window = "load_folder;" + path + ";" + DateTime.Now.ToString();
                    // seleced_tree_node = listView1.Items[i].Name;
                    txtpath.Text = path;
                    path_index++;
                    last_path[path_index] = path;
                }
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected == true)
                {
                    String path = listView1.Items[i].Name;
                    txtremote_select_status.Text = listView1.Items[i].Tag.ToString();
                    txtpath.Text = path;
                }
            }
        }

        private void thispc_listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                for (int i = 0; i < thispc_listView.Items.Count; i++)
                {
                    if (thispc_listView.Items[i].Selected == true)
                    {
                        String path = thispc_listView.Items[i].Name;

                        thispc_listView.Items.Clear();
                        // String_from_file_system_window = "load_folder;" + path + ";" + DateTime.Now.ToString();
                        DirectoryInfo dirname = new DirectoryInfo(path);
                        foreach (FileInfo fi in dirname.GetFiles())
                        {
                            ListViewItem lvi = new ListViewItem();
                            lvi.Name = fi.FullName;
                            lvi.Text = fi.FullName.Replace(path, "");
                            if (lvi.Text[0] == '\\')
                            {
                                lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                            }
                            String[] fdata = fi.FullName.Split('.');
                            String file_extention = fdata[1];
                            int image_index = 7;

                            if (file_extention.CompareTo("txt") == 0)
                                image_index = 6;
                            if (file_extention.CompareTo("rtf") == 0)
                                image_index = 6;
                            if (file_extention.CompareTo("doc") == 0)
                                image_index = 0;
                            if (file_extention.CompareTo("docx") == 0)
                                image_index = 0;
                            if (file_extention.CompareTo("html") == 0)
                                image_index = 3;
                            if (file_extention.CompareTo("htm") == 0)
                                image_index = 3;
                            if (file_extention.CompareTo("rar") == 0)
                                image_index = 9;
                            if (file_extention.CompareTo("zip") == 0)
                                image_index = 9;
                            if (file_extention.CompareTo("java") == 0)
                                image_index = 10;

                            lvi.Tag = "File";
                            lvi.ImageIndex = image_index;
                            thispc_listView.Items.Add(lvi);

                            //  node_list = node_list + fi.FullName + "\n";
                        }
                        foreach (DirectoryInfo di in dirname.GetDirectories())
                        {
                            ListViewItem lvi = new ListViewItem();
                            lvi.Name = di.FullName;
                            lvi.Text = di.FullName.Replace(path, ""); ;
                            if (lvi.Text[0] == '\\')
                            {
                                lvi.Text = lvi.Text.Substring(1, lvi.Text.Length - 1);
                            }

                            lvi.Tag = "Folder";
                            lvi.ImageIndex = 2;
                            thispc_listView.Items.Add(lvi);
                            //node_list = node_list + di.FullName + "\n";
                        }

                        txtthispc_path.Text = path;
                        thispc_path_index++;
                        thispc_last_path[thispc_path_index] = path;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void thispc_listView_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < thispc_listView.Items.Count; i++)
            {
                if (thispc_listView.Items[i].Selected == true)
                {
                    String path = thispc_listView.Items[i].Name;
                    txtthispc_select_status.Text = thispc_listView.Items[i].Tag.ToString();
                    txtthispc_path.Text = path;
                }
            }
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            if (path_index <= 1)
            {
                String_from_file_system_window = "load_drive;" + DateTime.Now.ToString();
                txtpath.Text = "Remote PC";
                load_driver = false;
            }
            else
            {
                path_index--;
                String_from_file_system_window = "load_folder;" + last_path[path_index] + ";" + DateTime.Now.ToString();
                txtpath.Text = last_path[path_index];
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            String folder_where_send_file = "";
            if (txtremote_select_status.Text.CompareTo("Folder") != 0)
            {
              folder_where_send_file = last_path[path_index];
            }
            else
            {
               folder_where_send_file = txtpath.Text;
            }
            if (txtthispc_select_status.Text.CompareTo("File") != 0)
            {
                MessageBox.Show("Plz Select file on this pc file manager");
                goto abc;
            }

            String file_to_upload = txtthispc_path.Text.Replace(thispc_last_path[thispc_path_index] + "\\", "");
            byte[] bytes = File.ReadAllBytes(txtthispc_path.Text);
            String size_of_file = bytes.Length.ToString();
            String_from_file_system_window = "file_upload_info;" + folder_where_send_file + ";" + file_to_upload + ";" + size_of_file + ";" + txtthispc_path.Text + ";" + DateTime.Now.ToString();
            
            abc:;
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            if (txtremote_select_status.Text.CompareTo("File") != 0)
            {
                MessageBox.Show("Plz Select File on remote file manager");
                goto abc;
            }
            String file_download_path = "";
            if (txtthispc_select_status.Text.CompareTo("Folder") != 0)
            {
                // MessageBox.Show("selected folder is " +thispc_last_path[thispc_path_index]);
                file_download_path = thispc_last_path[thispc_path_index];
            }
            else
            {
                // MessageBox.Show("selected folder is " + txtthispc_path.Text);
                file_download_path = txtthispc_path.Text;
            }
            String file_to_download = txtpath.Text;
            String_from_file_system_window = "file_info_request;" + file_download_path + ";" + file_to_download + ";" + DateTime.Now.ToString();
            String[] parray = txtpath.Text.Split('\\');
            String fn = parray[parray.Length - 1];
            //byte[] file_data = new byte[1024];
           // File.WriteAllBytes(file_download_path + "\\" + fn, file_data);


            abc:;
        }
    }
}
