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
    /// �������ӵĶԻ���
    /// </summary>
    public partial class frm_Connection : Form
    {
        /// <summary>
        /// ��ȡҪ���ӵ�����IP
        /// </summary>
        public string ServerIP
        {
            get { return (rdb_IsIP.Checked ? ipc_ServerIP.Text : ICanSeeYou.Common.Network.GetIpAdrress(HostName)); }
        }
        /// <summary>
        /// ��ȡҪ���ӵ�������
        /// </summary>
        public string HostName
        {
            get { return txt_Hostname.Text; }
        }

        public frm_Connection()
        {
            InitializeComponent();
            ipc_ServerIP.Enabled = true;
            txt_Hostname.Enabled = false;
            ipc_ServerIP.Focus();
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdb_IsIP_CheckedChanged(object sender, EventArgs e)
        {
            txt_Hostname.Enabled = false;
            ipc_ServerIP.Enabled = true;
            ipc_ServerIP.Focus();
        }

        private void rdb_Hostname_CheckedChanged(object sender, EventArgs e)
        {
            ipc_ServerIP.Enabled = false;
            txt_Hostname.Enabled = true;
            txt_Hostname.Focus();
        }
    }
}