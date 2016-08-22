using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace INCUclient
{
    /// <summary>
    /// ����Ա��½�Ի���
    /// </summary>
    public partial class frm_Login : Form
    {
        /// <summary>
        /// ��½����
        /// </summary>
        public string Password
        {
            get { return txt_password.Text; }
        }

        public frm_Login()
        {
            InitializeComponent();
            this.lbl_Check.ForeColor = Color.Red;
            this.TopMost = true;
            txt_password.Focus();
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            if (txt_password.Text == "")
                lbl_Check.Text = "���벻��Ϊ��!";
            else
                this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_password_TextChanged(object sender, EventArgs e)
        {
            lbl_Check.Text = "";
        }
    }
}