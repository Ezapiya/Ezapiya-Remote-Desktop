namespace LetUsDo
{
    partial class Image_sender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Image_sender));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnns = new System.Windows.Forms.Button();
            this.btnbs = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer_hartbit = new System.Windows.Forms.Timer(this.components);
            this.timer_send_rtp_image = new System.Windows.Forms.Timer(this.components);
            this.timercheck_reciver = new System.Windows.Forms.Timer(this.components);
            this.timer_send_to_sesrver = new System.Windows.Forms.Timer(this.components);
            this.timer_send_to_p2p = new System.Windows.Forms.Timer(this.components);
            this.timer_ft = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(258, 42);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(322, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 42);
            this.button1.TabIndex = 2;
            this.button1.Text = ">>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(397, 264);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(397, 222);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnns);
            this.panel2.Controls.Add(this.btnbs);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 222);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(397, 42);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // btnns
            // 
            this.btnns.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnns.Location = new System.Drawing.Point(277, 0);
            this.btnns.Name = "btnns";
            this.btnns.Size = new System.Drawing.Size(22, 42);
            this.btnns.TabIndex = 4;
            this.btnns.Text = "NS";
            this.btnns.UseVisualStyleBackColor = true;
            this.btnns.Click += new System.EventHandler(this.btnns_Click);
            // 
            // btnbs
            // 
            this.btnbs.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnbs.Location = new System.Drawing.Point(299, 0);
            this.btnbs.Name = "btnbs";
            this.btnbs.Size = new System.Drawing.Size(23, 42);
            this.btnbs.TabIndex = 3;
            this.btnbs.Text = "BS";
            this.btnbs.UseVisualStyleBackColor = true;
            this.btnbs.Click += new System.EventHandler(this.btnbs_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer_hartbit
            // 
            this.timer_hartbit.Enabled = true;
            this.timer_hartbit.Interval = 3000;
            this.timer_hartbit.Tick += new System.EventHandler(this.timer_hartbit_Tick);
            // 
            // timer_send_rtp_image
            // 
            this.timer_send_rtp_image.Enabled = true;
            this.timer_send_rtp_image.Interval = 40;
            this.timer_send_rtp_image.Tick += new System.EventHandler(this.timer_send_rtp_image_Tick);
            // 
            // timercheck_reciver
            // 
            this.timercheck_reciver.Interval = 20000;
            this.timercheck_reciver.Tick += new System.EventHandler(this.timercheck_reciver_Tick);
            // 
            // timer_send_to_sesrver
            // 
            this.timer_send_to_sesrver.Interval = 1000;
            this.timer_send_to_sesrver.Tick += new System.EventHandler(this.timer_send_to_sesrver_Tick);
            // 
            // timer_send_to_p2p
            // 
            this.timer_send_to_p2p.Interval = 1000;
            this.timer_send_to_p2p.Tick += new System.EventHandler(this.timer_send_to_p2p_Tick);
            // 
            // timer_ft
            // 
            this.timer_ft.Interval = 50;
            this.timer_ft.Tick += new System.EventHandler(this.timer_ft_Tick);
            // 
            // Image_sender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 264);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Image_sender";
            this.Text = "LetUsDo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Image_sender_FormClosing);
            this.Load += new System.EventHandler(this.Image_sender_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer_hartbit;
        private System.Windows.Forms.Button btnns;
        private System.Windows.Forms.Button btnbs;
        private System.Windows.Forms.Timer timer_send_rtp_image;
        private System.Windows.Forms.Timer timercheck_reciver;
        private System.Windows.Forms.Timer timer_send_to_sesrver;
        private System.Windows.Forms.Timer timer_send_to_p2p;
        private System.Windows.Forms.Timer timer_ft;
    }
}