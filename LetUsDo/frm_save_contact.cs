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
    public partial class frm_save_contact : Form
    {
        public string contact_uid{get;set;}
        public string contact_name{get;set;}
        public string contact_phone_number{get;set;}
        public string contact_email{get;set;}
        public string contact_amount{get;set;}
        public string contact_other_info_1{get;set;}
        public string contact_other_info_2{get;set;}
        public string contact_personal_password { get; set; }

        public frm_save_contact()
        {
            InitializeComponent();
        }

        private void frm_save_contact_Load(object sender, EventArgs e)
        {
            txtcontact_uid.Text  = contact_uid;
            txtcontact_name.Text = this.contact_name;
            txtcontact_phone_number.Text = this.contact_phone_number;
            txtcontact_email.Text = this.contact_email;
            txtcontact_amount.Text = this.contact_amount;
            txtcontact_other_info_1.Text = this.contact_other_info_1;
            txtcontact_other_info_2.Text = this.contact_other_info_2;
            txtcontact_personal_password.Text = this.contact_personal_password;

        }

        private void btncancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            this.contact_uid = txtcontact_uid.Text;
            this.contact_name=txtcontact_name.Text;
            this.contact_phone_number=txtcontact_phone_number.Text ;
            this.contact_email=txtcontact_email.Text ;
            this.contact_amount=txtcontact_amount.Text;
            this.contact_other_info_1=txtcontact_other_info_1.Text;
            this.contact_other_info_2=txtcontact_other_info_2.Text;
            this.contact_personal_password = txtcontact_personal_password.Text;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
