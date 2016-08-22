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
    /// �ͻ��������޸����
    /// </summary>
    public enum PwdChangType
    {
        /// <summary>
        /// �Ѿ��޸�
        /// </summary>
        CHANGED,
        /// <summary>
        /// û���޸�
        /// </summary>
        CANCEL,
        /// <summary>
        /// �����޸Ĳ��ɹ�
        /// </summary>
        UNSUCCESED,
    }
    /// <summary>
    /// ���ô���
    /// </summary>
    public partial class frm_Option : Form
    {
        /// <summary>
        /// �����ļ���·���Ƿ�ı�
        /// </summary>
        private  bool updatedFileChanged;

        /// <summary>
        /// ����˵������޸����
        /// </summary>
        private PwdChangType serverPassWordChanged;

        /// <summary>
        /// �ͻ��������޸����
        /// </summary>
        private PwdChangType clientPassWordChanged;
       
        public frm_Option()
        {
            InitializeComponent();
            serverPassWordChanged = PwdChangType.CANCEL;
            clientPassWordChanged = PwdChangType.CANCEL;
            ReadOptionFile();
        }

        //��ȡ�����ļ�
        private void ReadOptionFile()
        {
            ICanSeeYou.Configure.Option option= new ICanSeeYou.Configure.Option();
            ICanSeeYou.Configure.OptionFile optionFile = option.OptFile;
            txt_UpdatedFile.Text = optionFile.UpdatedFile;
            mtb_Version.Text = optionFile.UpdatedVersion;
        }

        /// <summary>
        /// ����˵��˳������޸����
        /// </summary>
        public PwdChangType ServerPassWordChanged
        {
            get { return serverPassWordChanged; }
            set { serverPassWordChanged = value; }
        }
        
        /// <summary>
        /// ����˵ĸ����ļ��Ƿ��޸�
        /// </summary>
        public bool UpdatedFileChanged
        {
            get{return updatedFileChanged;}
        }

        /// <summary>
        /// �ͻ��������޸����
        /// </summary>
        public PwdChangType ClientPassWordChanged
        {
            get { return clientPassWordChanged; }
            set { clientPassWordChanged = value; }
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        public string UpdatedFile
        {
            get { return txt_UpdatedFile.Text; }
        }

        /// <summary>
        /// ����˵��˳�����
        /// </summary>
        public string ServerPassWord
        {
            get { return txt_ServerPassWord.Text; }
        }

        /// <summary>
        /// �����ļ��İ汾
        /// </summary>
        public string Version
        {
            get { return mtb_Version.Text; }
        }

        /// <summary>
        /// �ͻ��˵ĵ�½����
        /// </summary>
        public string ClientPassWord
        {
            get { return txt_ClientPassWord.Text; }
        }

        //ѡ�����˵������ļ�
        private void btn_searchpath_Click(object sender, EventArgs e)
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.CheckFileExists = true;
            fileDialog.Title = "ѡ�����˵������ļ�";
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (txt_UpdatedFile.Text != fileDialog.FileName)
                    updatedFileChanged = true;
                txt_UpdatedFile.Text = fileDialog.FileName;
            }
        }

        private void txt_PassWord_TextChanged(object sender, EventArgs e)
        {
            serverPassWordChanged = PwdChangType.CHANGED;
            lbl_ServerError.Text = "";
        }

        private void mtb_Version_TextChanged(object sender, EventArgs e)
        {
            updatedFileChanged=true;
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            if (txt_ServerPassWord.Text != txt_ServerPassWordSure.Text)
            {
                lbl_ServerError.Text = "��������ķ�������벻һ��!";
                serverPassWordChanged = PwdChangType.UNSUCCESED;
            }
            //else if (txt_ServerPassWord.Text == ""&& clientPassWordChanged== PwdChangType.CHANGED)
            //{
            //    lbl_ServerError.Text = "����ķ�������벻��Ϊ��!";
            //    serverPassWordChanged = PwdChangType.UNSUCCESED;
            //}
            else if (txt_ClientPassWord.Text != txt_ClientPassWordSure.Text)
            {
                lbl_ClientError.Text = "��������Ŀͻ������벻һ��!";
                clientPassWordChanged = PwdChangType.UNSUCCESED;
            }
            //else if(txt_ClientPassWord.Text=="")
            //{
            //    lbl_ClientError.Text = "����Ŀͻ������벻��Ϊ��!";
            //    clientPassWordChanged = PwdChangType.UNSUCCESED;
            //}
            else
                this.Close();
        }

        private void btn_Cansle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_ClientPassWordSure_TextChanged(object sender, EventArgs e)
        {
            clientPassWordChanged = PwdChangType.CHANGED;
            lbl_ClientError.Text = "";
        }

        private void frm_Option_Load(object sender, EventArgs e)
        {
            serverPassWordChanged = PwdChangType.CANCEL;
            clientPassWordChanged = PwdChangType.CANCEL;
        }
    }
}