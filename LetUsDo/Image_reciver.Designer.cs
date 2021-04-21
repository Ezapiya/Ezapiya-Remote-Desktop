namespace LetUsDo
{
    partial class Image_reciver
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Image_reciver));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbsetting = new System.Windows.Forms.PictureBox();
            this.side_panel = new System.Windows.Forms.Panel();
            this.pbupload_image_for_blackscreen = new System.Windows.Forms.PictureBox();
            this.pbshow_image_on_blackscreen = new System.Windows.Forms.PictureBox();
            this.pbremove_blackscreen = new System.Windows.Forms.PictureBox();
            this.pbshowblack_screen = new System.Windows.Forms.PictureBox();
            this.pbfile_transfer = new System.Windows.Forms.PictureBox();
            this.pblock = new System.Windows.Forms.PictureBox();
            this.pbhibernate = new System.Windows.Forms.PictureBox();
            this.pbrestart = new System.Windows.Forms.PictureBox();
            this.pbshoutdown = new System.Windows.Forms.PictureBox();
            this.pbsleep = new System.Windows.Forms.PictureBox();
            this.lab_msg = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timer_othre_window = new System.Windows.Forms.Timer(this.components);
            this.timer_hartbit = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer_send_rtp = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timer_send_to_p2p = new System.Windows.Forms.Timer(this.components);
            this.timer_send_to_sesrver = new System.Windows.Forms.Timer(this.components);
            this.timer_ft = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbsetting)).BeginInit();
            this.side_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbupload_image_for_blackscreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbshow_image_on_blackscreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbremove_blackscreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbshowblack_screen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbfile_transfer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pblock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbhibernate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbrestart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbshoutdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbsleep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(963, 701);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pbsetting);
            this.panel3.Controls.Add(this.side_panel);
            this.panel3.Controls.Add(this.lab_msg);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(963, 666);
            this.panel3.TabIndex = 2;
            // 
            // pbsetting
            // 
            this.pbsetting.Image = global::LetUsDo.Properties.Resources.settings;
            this.pbsetting.Location = new System.Drawing.Point(928, 290);
            this.pbsetting.Name = "pbsetting";
            this.pbsetting.Size = new System.Drawing.Size(32, 32);
            this.pbsetting.TabIndex = 7;
            this.pbsetting.TabStop = false;
            this.pbsetting.Click += new System.EventHandler(this.pbsetting_Click);
            // 
            // side_panel
            // 
            this.side_panel.Controls.Add(this.pbupload_image_for_blackscreen);
            this.side_panel.Controls.Add(this.pbshow_image_on_blackscreen);
            this.side_panel.Controls.Add(this.pbremove_blackscreen);
            this.side_panel.Controls.Add(this.pbshowblack_screen);
            this.side_panel.Controls.Add(this.pbfile_transfer);
            this.side_panel.Controls.Add(this.pblock);
            this.side_panel.Controls.Add(this.pbhibernate);
            this.side_panel.Controls.Add(this.pbrestart);
            this.side_panel.Controls.Add(this.pbshoutdown);
            this.side_panel.Controls.Add(this.pbsleep);
            this.side_panel.Location = new System.Drawing.Point(663, 12);
            this.side_panel.Name = "side_panel";
            this.side_panel.Size = new System.Drawing.Size(75, 535);
            this.side_panel.TabIndex = 4;
            // 
            // pbupload_image_for_blackscreen
            // 
            this.pbupload_image_for_blackscreen.Image = global::LetUsDo.Properties.Resources.upload_image;
            this.pbupload_image_for_blackscreen.Location = new System.Drawing.Point(5, 588);
            this.pbupload_image_for_blackscreen.Name = "pbupload_image_for_blackscreen";
            this.pbupload_image_for_blackscreen.Size = new System.Drawing.Size(57, 50);
            this.pbupload_image_for_blackscreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbupload_image_for_blackscreen.TabIndex = 27;
            this.pbupload_image_for_blackscreen.TabStop = false;
            this.pbupload_image_for_blackscreen.Visible = false;
            this.pbupload_image_for_blackscreen.Click += new System.EventHandler(this.pbupload_image_for_blackscreen_Click);
            // 
            // pbshow_image_on_blackscreen
            // 
            this.pbshow_image_on_blackscreen.Image = global::LetUsDo.Properties.Resources.Image_on_blackscreen;
            this.pbshow_image_on_blackscreen.Location = new System.Drawing.Point(5, 532);
            this.pbshow_image_on_blackscreen.Name = "pbshow_image_on_blackscreen";
            this.pbshow_image_on_blackscreen.Size = new System.Drawing.Size(57, 50);
            this.pbshow_image_on_blackscreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbshow_image_on_blackscreen.TabIndex = 26;
            this.pbshow_image_on_blackscreen.TabStop = false;
            this.pbshow_image_on_blackscreen.Visible = false;
            this.pbshow_image_on_blackscreen.Click += new System.EventHandler(this.pbshow_image_on_blackscreen_Click);
            // 
            // pbremove_blackscreen
            // 
            this.pbremove_blackscreen.Image = global::LetUsDo.Properties.Resources.remove_blackscreen;
            this.pbremove_blackscreen.Location = new System.Drawing.Point(5, 476);
            this.pbremove_blackscreen.Name = "pbremove_blackscreen";
            this.pbremove_blackscreen.Size = new System.Drawing.Size(57, 50);
            this.pbremove_blackscreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbremove_blackscreen.TabIndex = 25;
            this.pbremove_blackscreen.TabStop = false;
            this.pbremove_blackscreen.Click += new System.EventHandler(this.pbremove_blackscreen_Click);
            // 
            // pbshowblack_screen
            // 
            this.pbshowblack_screen.Image = global::LetUsDo.Properties.Resources.blackscreen;
            this.pbshowblack_screen.Location = new System.Drawing.Point(5, 420);
            this.pbshowblack_screen.Name = "pbshowblack_screen";
            this.pbshowblack_screen.Size = new System.Drawing.Size(57, 50);
            this.pbshowblack_screen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbshowblack_screen.TabIndex = 24;
            this.pbshowblack_screen.TabStop = false;
            this.pbshowblack_screen.Click += new System.EventHandler(this.pbshowblack_screen_Click);
            // 
            // pbfile_transfer
            // 
            this.pbfile_transfer.Image = global::LetUsDo.Properties.Resources.file_transfer;
            this.pbfile_transfer.Location = new System.Drawing.Point(5, 323);
            this.pbfile_transfer.Name = "pbfile_transfer";
            this.pbfile_transfer.Size = new System.Drawing.Size(57, 50);
            this.pbfile_transfer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbfile_transfer.TabIndex = 23;
            this.pbfile_transfer.TabStop = false;
            this.pbfile_transfer.Click += new System.EventHandler(this.pbfile_transfer_Click);
            // 
            // pblock
            // 
            this.pblock.Image = global::LetUsDo.Properties.Resources._lock;
            this.pblock.Location = new System.Drawing.Point(5, 232);
            this.pblock.Name = "pblock";
            this.pblock.Size = new System.Drawing.Size(57, 50);
            this.pblock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pblock.TabIndex = 22;
            this.pblock.TabStop = false;
            this.pblock.Click += new System.EventHandler(this.pblock_Click);
            // 
            // pbhibernate
            // 
            this.pbhibernate.Image = global::LetUsDo.Properties.Resources.hibernate;
            this.pbhibernate.Location = new System.Drawing.Point(5, 176);
            this.pbhibernate.Name = "pbhibernate";
            this.pbhibernate.Size = new System.Drawing.Size(57, 50);
            this.pbhibernate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbhibernate.TabIndex = 21;
            this.pbhibernate.TabStop = false;
            this.pbhibernate.Click += new System.EventHandler(this.pbhibernate_Click);
            // 
            // pbrestart
            // 
            this.pbrestart.Image = global::LetUsDo.Properties.Resources.restart;
            this.pbrestart.Location = new System.Drawing.Point(5, 120);
            this.pbrestart.Name = "pbrestart";
            this.pbrestart.Size = new System.Drawing.Size(57, 50);
            this.pbrestart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbrestart.TabIndex = 20;
            this.pbrestart.TabStop = false;
            this.pbrestart.Click += new System.EventHandler(this.pbrestart_Click);
            // 
            // pbshoutdown
            // 
            this.pbshoutdown.Image = global::LetUsDo.Properties.Resources.shoutdonw;
            this.pbshoutdown.Location = new System.Drawing.Point(5, 64);
            this.pbshoutdown.Name = "pbshoutdown";
            this.pbshoutdown.Size = new System.Drawing.Size(57, 50);
            this.pbshoutdown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbshoutdown.TabIndex = 19;
            this.pbshoutdown.TabStop = false;
            this.pbshoutdown.Click += new System.EventHandler(this.pbshoutdown_Click);
            // 
            // pbsleep
            // 
            this.pbsleep.Image = global::LetUsDo.Properties.Resources.sleep;
            this.pbsleep.Location = new System.Drawing.Point(5, 8);
            this.pbsleep.Name = "pbsleep";
            this.pbsleep.Size = new System.Drawing.Size(57, 50);
            this.pbsleep.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbsleep.TabIndex = 18;
            this.pbsleep.TabStop = false;
            this.pbsleep.Click += new System.EventHandler(this.pbsleep_Click);
            // 
            // lab_msg
            // 
            this.lab_msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_msg.Location = new System.Drawing.Point(254, 271);
            this.lab_msg.Name = "lab_msg";
            this.lab_msg.Size = new System.Drawing.Size(500, 61);
            this.lab_msg.TabIndex = 5;
            this.lab_msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(963, 666);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 666);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(963, 35);
            this.panel2.TabIndex = 1;
            this.panel2.Visible = false;
            // 
            // timer_othre_window
            // 
            this.timer_othre_window.Tick += new System.EventHandler(this.timer_othre_window_Tick);
            // 
            // timer_hartbit
            // 
            this.timer_hartbit.Enabled = true;
            this.timer_hartbit.Interval = 3000;
            this.timer_hartbit.Tick += new System.EventHandler(this.timer_hartbit_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer_send_rtp
            // 
            this.timer_send_rtp.Enabled = true;
            this.timer_send_rtp.Interval = 40;
            this.timer_send_rtp.Tick += new System.EventHandler(this.timer_send_rtp_Tick);
            // 
            // timer_send_to_p2p
            // 
            this.timer_send_to_p2p.Interval = 1000;
            this.timer_send_to_p2p.Tick += new System.EventHandler(this.timer_send_to_p2p_Tick);
            // 
            // timer_send_to_sesrver
            // 
            this.timer_send_to_sesrver.Interval = 1000;
            this.timer_send_to_sesrver.Tick += new System.EventHandler(this.timer_send_to_sesrver_Tick);
            // 
            // timer_ft
            // 
            this.timer_ft.Interval = 50;
            this.timer_ft.Tick += new System.EventHandler(this.timer_ft_Tick);
            // 
            // Image_reciver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 701);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Image_reciver";
            this.Text = "LetUsDo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Image_reciver_FormClosing);
            this.Load += new System.EventHandler(this.Image_reciver_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Image_reciver_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Image_reciver_KeyUp);
            this.Resize += new System.EventHandler(this.Image_reciver_Resize);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbsetting)).EndInit();
            this.side_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbupload_image_for_blackscreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbshow_image_on_blackscreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbremove_blackscreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbshowblack_screen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbfile_transfer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pblock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbhibernate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbrestart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbshoutdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbsleep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lab_msg;
        private System.Windows.Forms.Timer timer_othre_window;
        private System.Windows.Forms.Timer timer_hartbit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer_send_rtp;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel side_panel;
        private System.Windows.Forms.Timer timer_send_to_p2p;
        private System.Windows.Forms.Timer timer_send_to_sesrver;
        private System.Windows.Forms.Timer timer_ft;
        private System.Windows.Forms.PictureBox pbsleep;
        private System.Windows.Forms.PictureBox pbrestart;
        private System.Windows.Forms.PictureBox pbshoutdown;
        private System.Windows.Forms.PictureBox pbfile_transfer;
        private System.Windows.Forms.PictureBox pblock;
        private System.Windows.Forms.PictureBox pbhibernate;
        private System.Windows.Forms.PictureBox pbupload_image_for_blackscreen;
        private System.Windows.Forms.PictureBox pbshow_image_on_blackscreen;
        private System.Windows.Forms.PictureBox pbremove_blackscreen;
        private System.Windows.Forms.PictureBox pbshowblack_screen;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pbsetting;
    }
}