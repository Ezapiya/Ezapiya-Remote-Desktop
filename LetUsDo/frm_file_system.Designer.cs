namespace LetUsDo
{
    partial class frm_file_system
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_file_system));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.panel5 = new System.Windows.Forms.Panel();
            this.labtotal_packet = new System.Windows.Forms.Label();
            this.labcurrent_packet = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtremote_select_status = new System.Windows.Forms.TextBox();
            this.txtthispc_select_status = new System.Windows.Forms.TextBox();
            this.btn_download = new System.Windows.Forms.Button();
            this.btn_upload = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnthispc_mycomputer = new System.Windows.Forms.Button();
            this.btnthispc_back = new System.Windows.Forms.Button();
            this.btnthispcdesktop = new System.Windows.Forms.Button();
            this.btnthispc_mydocuemt = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnback = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtthispc_path = new System.Windows.Forms.TextBox();
            this.thispc_listView = new System.Windows.Forms.ListView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.txtpath = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1174, 379);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 311);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1174, 68);
            this.panel3.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.progressBar1);
            this.panel6.Controls.Add(this.pb);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 35);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1174, 33);
            this.panel6.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1174, 33);
            this.progressBar1.TabIndex = 1;
            // 
            // pb
            // 
            this.pb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb.Location = new System.Drawing.Point(0, 0);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(1174, 33);
            this.pb.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.labtotal_packet);
            this.panel5.Controls.Add(this.labcurrent_packet);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1174, 36);
            this.panel5.TabIndex = 2;
            // 
            // labtotal_packet
            // 
            this.labtotal_packet.AutoSize = true;
            this.labtotal_packet.Location = new System.Drawing.Point(1066, 19);
            this.labtotal_packet.Name = "labtotal_packet";
            this.labtotal_packet.Size = new System.Drawing.Size(13, 13);
            this.labtotal_packet.TabIndex = 3;
            this.labtotal_packet.Text = "0";
            // 
            // labcurrent_packet
            // 
            this.labcurrent_packet.AutoSize = true;
            this.labcurrent_packet.Location = new System.Drawing.Point(94, 19);
            this.labcurrent_packet.Name = "labcurrent_packet";
            this.labcurrent_packet.Size = new System.Drawing.Size(13, 13);
            this.labcurrent_packet.TabIndex = 5;
            this.labcurrent_packet.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(983, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Total Packet :-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current Packet :-";
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::LetUsDo.Properties.Resources.background;
            this.panel2.Controls.Add(this.txtremote_select_status);
            this.panel2.Controls.Add(this.txtthispc_select_status);
            this.panel2.Controls.Add(this.btn_download);
            this.panel2.Controls.Add(this.btn_upload);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Controls.Add(this.panel9);
            this.panel2.Controls.Add(this.txtthispc_path);
            this.panel2.Controls.Add(this.thispc_listView);
            this.panel2.Controls.Add(this.txtpath);
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1174, 379);
            this.panel2.TabIndex = 0;
            // 
            // txtremote_select_status
            // 
            this.txtremote_select_status.Location = new System.Drawing.Point(559, 248);
            this.txtremote_select_status.Name = "txtremote_select_status";
            this.txtremote_select_status.Size = new System.Drawing.Size(55, 20);
            this.txtremote_select_status.TabIndex = 24;
            // 
            // txtthispc_select_status
            // 
            this.txtthispc_select_status.Location = new System.Drawing.Point(559, 212);
            this.txtthispc_select_status.Name = "txtthispc_select_status";
            this.txtthispc_select_status.Size = new System.Drawing.Size(55, 20);
            this.txtthispc_select_status.TabIndex = 25;
            // 
            // btn_download
            // 
            this.btn_download.Location = new System.Drawing.Point(559, 165);
            this.btn_download.Name = "btn_download";
            this.btn_download.Size = new System.Drawing.Size(55, 23);
            this.btn_download.TabIndex = 25;
            this.btn_download.Text = "<<";
            this.btn_download.UseVisualStyleBackColor = true;
            this.btn_download.Click += new System.EventHandler(this.btn_download_Click);
            // 
            // btn_upload
            // 
            this.btn_upload.Location = new System.Drawing.Point(559, 102);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(55, 23);
            this.btn_upload.TabIndex = 24;
            this.btn_upload.Text = ">>";
            this.btn_upload.UseVisualStyleBackColor = true;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.White;
            this.panel8.Controls.Add(this.btnthispc_mycomputer);
            this.panel8.Controls.Add(this.btnthispc_back);
            this.panel8.Controls.Add(this.btnthispcdesktop);
            this.panel8.Controls.Add(this.btnthispc_mydocuemt);
            this.panel8.Location = new System.Drawing.Point(5, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(89, 302);
            this.panel8.TabIndex = 23;
            // 
            // btnthispc_mycomputer
            // 
            this.btnthispc_mycomputer.Location = new System.Drawing.Point(0, 0);
            this.btnthispc_mycomputer.Name = "btnthispc_mycomputer";
            this.btnthispc_mycomputer.Size = new System.Drawing.Size(75, 23);
            this.btnthispc_mycomputer.TabIndex = 0;
            // 
            // btnthispc_back
            // 
            this.btnthispc_back.Location = new System.Drawing.Point(2, 2);
            this.btnthispc_back.Name = "btnthispc_back";
            this.btnthispc_back.Size = new System.Drawing.Size(82, 23);
            this.btnthispc_back.TabIndex = 6;
            this.btnthispc_back.Text = "Back";
            this.btnthispc_back.UseVisualStyleBackColor = true;
            this.btnthispc_back.Click += new System.EventHandler(this.btnthispc_back_Click);
            // 
            // btnthispcdesktop
            // 
            this.btnthispcdesktop.BackgroundImage = global::LetUsDo.Properties.Resources.desktop;
            this.btnthispcdesktop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnthispcdesktop.Location = new System.Drawing.Point(2, 191);
            this.btnthispcdesktop.Name = "btnthispcdesktop";
            this.btnthispcdesktop.Size = new System.Drawing.Size(84, 74);
            this.btnthispcdesktop.TabIndex = 12;
            this.btnthispcdesktop.UseVisualStyleBackColor = true;
            this.btnthispcdesktop.Click += new System.EventHandler(this.btnthispcdesktop_Click);
            // 
            // btnthispc_mydocuemt
            // 
            this.btnthispc_mydocuemt.BackgroundImage = global::LetUsDo.Properties.Resources.my_document;
            this.btnthispc_mydocuemt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnthispc_mydocuemt.Location = new System.Drawing.Point(2, 111);
            this.btnthispc_mydocuemt.Name = "btnthispc_mydocuemt";
            this.btnthispc_mydocuemt.Size = new System.Drawing.Size(84, 74);
            this.btnthispc_mydocuemt.TabIndex = 11;
            this.btnthispc_mydocuemt.UseVisualStyleBackColor = true;
            this.btnthispc_mydocuemt.Click += new System.EventHandler(this.btnthispc_mydocuemt_Click);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.White;
            this.panel9.Controls.Add(this.button3);
            this.panel9.Controls.Add(this.button1);
            this.panel9.Controls.Add(this.btnback);
            this.panel9.Controls.Add(this.button2);
            this.panel9.Location = new System.Drawing.Point(620, 1);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(90, 303);
            this.panel9.TabIndex = 22;
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::LetUsDo.Properties.Resources.my_computer;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Location = new System.Drawing.Point(3, 33);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 74);
            this.button3.TabIndex = 13;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::LetUsDo.Properties.Resources.desktop;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(3, 193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 74);
            this.button1.TabIndex = 15;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnback
            // 
            this.btnback.Location = new System.Drawing.Point(3, 4);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(83, 23);
            this.btnback.TabIndex = 3;
            this.btnback.Text = "Back";
            this.btnback.UseVisualStyleBackColor = true;
            this.btnback.Click += new System.EventHandler(this.btnback_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::LetUsDo.Properties.Resources.my_document;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(3, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 74);
            this.button2.TabIndex = 14;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtthispc_path
            // 
            this.txtthispc_path.Location = new System.Drawing.Point(95, 4);
            this.txtthispc_path.Name = "txtthispc_path";
            this.txtthispc_path.Size = new System.Drawing.Size(457, 20);
            this.txtthispc_path.TabIndex = 21;
            this.txtthispc_path.Text = "This PC";
            // 
            // thispc_listView
            // 
            this.thispc_listView.HideSelection = false;
            this.thispc_listView.LargeImageList = this.imageList2;
            this.thispc_listView.Location = new System.Drawing.Point(96, 27);
            this.thispc_listView.Name = "thispc_listView";
            this.thispc_listView.Size = new System.Drawing.Size(457, 277);
            this.thispc_listView.TabIndex = 20;
            this.thispc_listView.UseCompatibleStateImageBehavior = false;
            this.thispc_listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.thispc_listView_MouseClick);
            this.thispc_listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.thispc_listView_MouseDoubleClick);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Carlosjj-Microsoft-Office-2013-Word.ico");
            this.imageList2.Images.SetKeyName(1, "Driver.png");
            this.imageList2.Images.SetKeyName(2, "Folder.jpg");
            this.imageList2.Images.SetKeyName(3, "html.png");
            this.imageList2.Images.SetKeyName(4, "mycomputer.jpg");
            this.imageList2.Images.SetKeyName(5, "mydocuments.jpg");
            this.imageList2.Images.SetKeyName(6, "notepad_37173.jpg");
            this.imageList2.Images.SetKeyName(7, "UnknownFile.png");
            this.imageList2.Images.SetKeyName(8, "cdromdrive.jpg");
            this.imageList2.Images.SetKeyName(9, "rar.jpg");
            this.imageList2.Images.SetKeyName(10, "java.jpg");
            // 
            // txtpath
            // 
            this.txtpath.Location = new System.Drawing.Point(710, 5);
            this.txtpath.Name = "txtpath";
            this.txtpath.Size = new System.Drawing.Size(457, 20);
            this.txtpath.TabIndex = 19;
            this.txtpath.Text = "Remote PC";
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList2;
            this.listView1.Location = new System.Drawing.Point(710, 27);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(457, 279);
            this.listView1.TabIndex = 18;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 311);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1174, 68);
            this.panel4.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frm_file_system
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 379);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_file_system";
            this.Text = "File Transfer";
            this.Load += new System.EventHandler(this.frm_file_system_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnthispc_mycomputer;
        private System.Windows.Forms.Button btnthispc_back;
        private System.Windows.Forms.Button btnthispcdesktop;
        private System.Windows.Forms.Button btnthispc_mydocuemt;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnback;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtthispc_path;
        private System.Windows.Forms.ListView thispc_listView;
        private System.Windows.Forms.TextBox txtpath;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.TextBox txtthispc_select_status;
        private System.Windows.Forms.TextBox txtremote_select_status;
        private System.Windows.Forms.Button btn_download;
        private System.Windows.Forms.Button btn_upload;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label labtotal_packet;
        public System.Windows.Forms.Label labcurrent_packet;
    }
}