using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using ICanSeeYou.Common;
using ICanSeeYou.Configure;

namespace INCUserver
{
    /// <summary>
    /// INCU�����
    /// </summary>
    public partial class frm_Server : Form
    {
        //�����
        private Servers.Servers server;

        public frm_Server()
        {
            InitializeComponent();
            Run();
        }

        /// <summary>
        /// �������
        /// </summary>
        public string AssemblyTitle
        {
            get
            {
                // ��ȡ�˳����ϵ����� Title ����
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // ���������һ�� Title ����
                if (attributes.Length > 0)
                {
                    // ��ѡ���һ������
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // ���������Ϊ�ǿ��ַ��������䷵��
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // ���û�� Title ���ԣ����� Title ����Ϊһ�����ַ������򷵻� .exe ������
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }
        /// <summary>
        /// ����汾
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// �����Ʒ��
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                // ��ȡ�˳����ϵ����� Product ����
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // ��� Product ���Բ����ڣ��򷵻�һ�����ַ���
                if (attributes.Length == 0)
                    return "";
                // ����� Product ���ԣ��򷵻ظ����Ե�ֵ
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Run()
        {
            this.WindowState=FormWindowState.Minimized;
            server = new Servers.Servers(Constant.Port_Main, Constant.Port_File,  Constant.Port_Screen);
            server.ltv_Log = ltv_Log;
            server.lbl_Message = lbl_Message;
            server.Version = AssemblyVersion;
            server.ProductName = AssemblyProduct;
            try
            {
                server.Run();
            }
            catch
            {
                CloseServer();
            }
        }
        /// <summary>
        /// �رճ���
        /// </summary>
        private void CloseServer()
        {
            try
            {
                if (server != null)
                    server.Close();
                System.Environment.Exit(System.Environment.ExitCode);
                Application.ExitThread();
                Application.Exit();
            }
            catch { }
        }

        private void frm_server_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            �˳�EToolStripMenuItem1_Click(sender, e);
        }

        private void ����AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new frm_AboutINCU().Show();
        }

        private void �˳�EToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (server.ExitPassWord == "") CloseServer();
            else
            {
                frm_PassWord passwordForm = new frm_PassWord();
                DialogResult result = passwordForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (server.ExitPassWord == PassWord.MD5Encrypt(passwordForm.Password))
                    {
                        if (!System.IO.File.Exists(Constant.PassWordFilename))
                            PassWord.Save(Constant.PassWordFilename, server.ExitPassWord);
                        CloseServer();
                    }
                    else
                        MessageBox.Show("�������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void ��OToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
            if (this.Visible)
                this.WindowState = FormWindowState.Normal;
        }

        private void frm_server_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Visible = false;
        }
    }
}