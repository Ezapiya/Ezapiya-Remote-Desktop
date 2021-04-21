using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LetUsDo
{
    public partial class frm_personal_password : Form
    {
        public string personal_password { get; set; }

        public frm_personal_password()
        {
            InitializeComponent();
        }

        private void frm_personal_password_Load(object sender, EventArgs e)
        {

        }

        private void btncancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            if (txtpersonal_password.Text.Length < 6)
            {
                MessageBox.Show("Password length must be more then 6");
                goto abc;
            }
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (regexItem.IsMatch(txtpersonal_password.Text))
            {
                MessageBox.Show("Week Password");
                goto abc;
            }
            String pp= CryptoEngine.Encrypt(txtpersonal_password.Text,"4589-8754-dfgfg");
            if (pp.Contains("\'") || pp.Contains("\"") || pp.Contains(":"))
            {
                MessageBox.Show("Invalid Password");
                goto abc;
            }
            if (txtpersonal_password.Text.CompareTo(txtre_personal_password.Text) == 0)
            {
                this.personal_password = pp;

                //this.personal_password = CryptoEngine.Encrypt(txtpersonal_password.Text, Globle_data.crypto_key);
                if (this.personal_password.Contains(';') == true)
                {
                    MessageBox.Show("This Password is not compatible");
                    goto abc;
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Both Password is not same");
            }
        abc: ;
        }
    }
}
