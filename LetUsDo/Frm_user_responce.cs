using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LetUsDo
{
    public partial class Frm_user_responce : Form
    {
        public int i = 0;
        public Frm_user_responce(String connection_info)
        {
            InitializeComponent();
            //label1.Text = connection_info;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void btnallowconnection_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void Frm_user_responce_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            if(i>180)
                this.DialogResult = DialogResult.No;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
