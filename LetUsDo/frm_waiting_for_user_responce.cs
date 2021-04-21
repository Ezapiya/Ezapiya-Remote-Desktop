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



namespace LetUsDo
{
    public partial class frm_waiting_for_user_responce : Form
    {
        int i = 0;
        String con_id = "";
        public frm_waiting_for_user_responce(String connnection_id)
        {
            InitializeComponent();
            con_id = connnection_id;
        }

       

        private void frm_waiting_for_user_responce_Load(object sender, EventArgs e)
        {
            i = 0;  
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // this.Dispose();
            this.Hide();
        }

        private void frm_waiting_for_user_responce_Activated(object sender, EventArgs e)
        {
            i = 0;  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
           // if (i > 10)
             //   this.Hide();
        }
    }
}
