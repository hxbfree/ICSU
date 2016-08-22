using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using ICanSeeYou.Codes;

namespace INCUclient
{
    //INCU�ͻ���
    public partial class frm_Client : Form
    {
        /// <summary>
        /// �ܿ��ƶ�
        /// </summary>
        private Clients.Controlers GeneralControler;
        /// <summary>
        /// ��ǰѡ������ν��
        /// </summary>
        private TreeNode CurrentNode;
        /// <summary>
        /// ��ϣ��(Key=�ļ���׺��,value=ͼƬ�б��Key)
        /// ����:��׺��A=exe,��imageKey[A]="exe",��imageKey[A]���Ƕ�Ӧ���ļ�ͼ��Keyֵ.
        /// </summary>
        private Hashtable imageKey;

        /// <summary>
        /// �Ƿ�ʼ��ȡԶ����Ļ
        /// </summary>
        private bool ScreenOpen;

        //��ʼ��
        public frm_Client()
        {
            InitializeComponent();
            Initial();
        }       
        
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Initial()
        {
            imageKey = new Hashtable();
            System.Collections.Specialized.StringCollection keyCol = iml_ExplorerImages.Images.Keys;
            for (int i = 0; i < keyCol.Count; i++)
                if (!imageKey.Contains(keyCol[i]))
                    imageKey.Add(keyCol[i], keyCol[i]);
            //�ܿ��ƶ˳�ʼ��
            GeneralControler = new Clients.Controlers(imageKey);
            GeneralControler.pic_Screen = pic_Screen;
            GeneralControler.ltv_HostExplorer = ltv_hostexplorer;
            GeneralControler.ltv_Log = ltv_Log;
            GeneralControler.ltv_MyExplorer = ltv_myexplorer;
            GeneralControler.rtb_Content = rtb_Content;
            GeneralControler.trv_HostView = trv_HostView;
            GeneralControler.txb_HostExploer = txt_hostexplorer;
            GeneralControler.txb_MyExplorer = txt_myexplorer;
            GeneralControler.lbl_Message = lbl_Display;

            ICanSeeYou.Configure.Option option = new ICanSeeYou.Configure.Option();
            ICanSeeYou.Configure.OptionFile optionFile = option.OptFile;
            if (optionFile != null)
            {
                GeneralControler.UpdatedFile = optionFile.UpdatedFile;
                GeneralControler.ServerVersion = optionFile.UpdatedVersion;
            }

            //δ��ʼ����
            ScreenOpen = false;
            //Ĭ�Ͻ������ʱ��(һ��),�������ٶ�Ϊ��.
            ScreenTimer.Interval = 1000;
            ��MToolStripMenuItem.Checked = true;
          //  this.ShowInTaskbar = false;
        }

        #region  ��������

        //�������а�װ����˵�����
        private void ��������AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Enabled = false;
            frm_ConnectAll Connection = new frm_ConnectAll();
            DialogResult result = Connection.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!GeneralControler.ConnectAll(Connection.StartIP, Connection.EndIP))
                    MessageBox.Show("���ܽ�������!");
            }
        }

        //��ָ��������������
        private void ָ������SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Enabled = false;
            frm_Connection Connection = new frm_Connection();
            DialogResult result = Connection.ShowDialog();
            if (result == DialogResult.OK)
            { 
                System.Net.IPAddress serverIP;
                try
                {
                   serverIP = ICanSeeYou.Common.Network.ToIPAddress(Connection.ServerIP);
                }
                catch
                {
                    MessageBox.Show("IP��ַ����!", "IP��ַ����", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                GeneralControler.BuiltControler(serverIP);
            }
        }

        //��������
        private void ��������RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Enabled = false;
            TreeNode tn = trv_HostView.SelectedNode;
            if (tn != null)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(GeneralControler.ReBuilt));
                thread.Start(tn);
            }
            else
                MessageBox.Show("��ѡ��������ͼ������IP��ַ����,�����Ƴ���,Ȼ���ٽ�������.");
        }

        #endregion

        #region �ļ�����

        //�򿪱��ص����ϵ�·��
        private void btn_myexplorer_Click(object sender, EventArgs e)
        {
            ICanSeeYou.Common.IO.OpenDirectory(txt_myexplorer.Text, ltv_myexplorer, imageKey);
        }

        //�򿪱��ص����ϵ�·��
        private void ltv_myexplorer_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem selectItem = null;
            BaseFile basefile = null;
            try
            {
                selectItem = ltv_myexplorer.FocusedItem;
                if (selectItem != null)
                {
                    basefile = selectItem.Tag as BaseFile;
                    if (basefile != null)
                        if (basefile.Flag != FileFlag.File)
                        {
                            string path = (basefile.Flag == FileFlag.Directory ? basefile.Name : basefile.Name + @"\");
                            lbl_Display.Text = path;
                            ICanSeeYou.Common.IO.OpenDirectory(path, ltv_myexplorer, imageKey);
                            txt_myexplorer.Text = path;
                        }
                }
                else
                {
                    MessageBox.Show(" ��ѡ��һ���ļ��У�");
                }
            }
            catch
            {
            }
        }

        //��ȡ���ص������ļ�����Ϣ
        private void ltv_myexplorer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ltv_myexplorer.FocusedItem != null)
            {
                BaseFile basefile = ltv_myexplorer.FocusedItem.Tag as BaseFile;
                if (basefile != null)
                    if (basefile.Flag == FileFlag.Directory)
                        lbl_Display.Text = basefile.Name;
                    else if (basefile.Flag == FileFlag.File)
                        lbl_Display.Text = ICanSeeYou.Common.IO.GetFileDetial(basefile.Name);
            }
        }

        //�򿪱��ص����ϵ�·��
        private void txt_myexplorer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ICanSeeYou.Common.IO.OpenDirectory(txt_myexplorer.Text, ltv_myexplorer, imageKey);
        }

        //�򿪱��ص����ϵ�·��
        private void ��OToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ltv_myexplorer_DoubleClick(sender, e);
        }

        //ˢ�±��ص���
        private void ˢ��RtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_myexplorer_Click(sender, e);
        }

        //��Զ�̵����ϵ�·��
        private void ��OToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            lvs_hostexplorer_DoubleClick(sender, e);
        }

        //��Զ�̵����ϵ�·��
        private void btn_hostexplorer_Click(object sender, EventArgs e)
        {
            string path = txt_hostexplorer.Text;
            if (path != "")
                GeneralControler.GetDirectoryDetial(path);
            else
                GeneralControler.GetDiskDetial();
        }

        //��Զ�̵����ϵ�·��
        private void lvs_hostexplorer_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ltv_hostexplorer.FocusedItem != null)
                {
                    BaseFile basefile = ltv_hostexplorer.FocusedItem.Tag as BaseFile;
                    if (basefile != null)
                        if (basefile.Flag != FileFlag.File)
                        {
                            string path = (basefile.Flag == FileFlag.Directory ? basefile.Name : basefile.Name + @"\");
                            if (path != "")
                            {
                                lbl_Display.Text = path;
                                GeneralControler.GetDirectoryDetial(path);
                                txt_hostexplorer.Text = path;
                            }
                            else
                            {
                                GeneralControler.GetDiskDetial();
                                lbl_Display.Text = "Զ�̵��Եĸ�Ŀ¼";
                            }
                        }
                }
                else
                {
                    MessageBox.Show(" ��ѡ��һ���ļ��У�");
                }
            }
            catch { }
        }

        //��Զ�̵����ϵ�·��
        private void txt_hostexplorer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_hostexplorer_Click(sender, e);
        }

        private void ˢ��RToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btn_hostexplorer_Click(sender, e);
        }

        //��ȡԶ�̵������ļ�����Ϣ
        private void lvs_hostexplorer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BaseFile basefile = ltv_hostexplorer.FocusedItem.Tag as BaseFile;
                if (basefile != null)
                    if (basefile.Flag == FileFlag.File)
                        GeneralControler.GetFileDetial(basefile.Name);
                    else if (basefile.Flag == FileFlag.Directory)
                        lbl_Display.Text = basefile.Name;
            }
            catch { }

        }

        //Զ���ļ�����
        private void ����DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ltv_hostexplorer.FocusedItem != null)
                {
                    BaseFile basefile = ltv_hostexplorer.FocusedItem.Tag as BaseFile;
                    if (basefile != null)
                        if (basefile.Flag == FileFlag.File)
                        {
                            string savePath = ltv_myexplorer.Tag as string;
                            if (savePath != null)
                            {
                                string fileName = ICanSeeYou.Common.IO.GetName(basefile.Name);
                                savePath += (savePath.EndsWith(@"\") ? fileName : @"\" + fileName);
                                if (savePath != "")
                                {
                                    if (System.IO.File.Exists(savePath))
                                    {
                                        DialogResult result = MessageBox.Show("\t�ļ�\'" + fileName + "\'�Ѿ�����!\n\t�Ƿ�ѡ������һ��Ŀ¼����?", "ѡ������һ��Ŀ¼����", MessageBoxButtons.YesNo);
                                        if (result == DialogResult.Yes)
                                        {
                                            SaveFileDialog filechooser = new SaveFileDialog();
                                            filechooser.FileName = savePath;
                                            filechooser.Filter = "(" + ICanSeeYou.Common.IO.GetFileType(fileName) + ")|*." + ICanSeeYou.Common.IO.GetFileType(fileName);
                                            DialogResult saveResult = filechooser.ShowDialog();
                                            if (saveResult == DialogResult.OK)
                                            {
                                                savePath = filechooser.FileName;
                                            }
                                        }
                                    }
                                    if (savePath != null && savePath != "")
                                        GeneralControler.DownOrUpload(basefile.Name, savePath, true);
                                }
                                else
                                    MessageBox.Show("��ǰ����·��" + savePath + "��Ч!");
                            }
                        }
                        else
                            MessageBox.Show("ֻ�������ļ�");
                }
                else
                {
                    MessageBox.Show(" ��ѡ��һ���ļ����أ�");
                }
            }
            catch { }
        }

        //�ϴ������ļ��������
        private void �ϴ�UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ltv_myexplorer.FocusedItem != null)
                {
                    BaseFile basefile = ltv_myexplorer.FocusedItem.Tag as BaseFile;
                    if (basefile != null)
                        if (basefile.Flag == FileFlag.File)
                        {
                            string savePath = ltv_hostexplorer.Tag as string;
                            if (savePath != null)
                                if (savePath != "")
                                {
                                    string fileName = ICanSeeYou.Common.IO.GetName(basefile.Name);
                                    savePath += (savePath.EndsWith(@"\") ? fileName : @"\" + fileName);
                                    GeneralControler.DownOrUpload(basefile.Name, savePath, false);
                                }
                                else
                                    MessageBox.Show("��ǰ����·��" + savePath + "��Ч!");
                        }
                        else
                            MessageBox.Show("ֻ���ϴ��ļ�");
                }
                else
                {
                    MessageBox.Show(" ��ѡ��һ���ļ��ϴ���");
                }
            }
            catch { }
        }

        #endregion

        #region  Զ��������������

        private void pic_Screen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ScreenOpen)
                GeneralControler.MouseDoubleClick(sender, e);
        }

        private void pic_Screen_MouseDown(object sender, MouseEventArgs e)
        {
            if (ScreenOpen)
                GeneralControler.MouseDown(sender, e);
        }

        private void pic_Screen_MouseMove(object sender, MouseEventArgs e)
        {
            if (ScreenOpen)
                GeneralControler.MouseMove(sender, e);
        }

        private void pic_Screen_MouseUp(object sender, MouseEventArgs e)
        {
            if (ScreenOpen)
                GeneralControler.MouseUp(sender, e);
        }
        #endregion

        #region Զ����������Ļ��ȡ

        //��ȡԶ����Ļ
        private void ScreenTimer_Tick(object sender, EventArgs e)
        {
            if (!GeneralControler.GetScreen())
            {
                ScreenTimer.Enabled = false;
                MessageBox.Show("Զ����Ļ��ȡʧ��!", "Զ����Ļ��ȡʧ��", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //��Զ����Ļ(���˵��͹�����)
        private void ��OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Enabled = false;
            if (!ScreenOpen)
            {
                if (GeneralControler.CurrentControler != null)
                {
                    tabs.SelectedTab = tab_Desktop;
                    GeneralControler.OpenScreen();
                    //��ͣƬ��,����Ļ���ƶ˺ͷ��������ʱ�佨������
                    Thread.Sleep(300);
                    ScreenOpen = true;
                    ScreenTimer.Enabled = true;
                }
            }
            else
                MessageBox.Show("�Ѿ���Զ�̽���������");
        }

        //��Զ����Ļ(������ͼ���Ҽ��˵�)
        private void ��Ļ����PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = trv_HostView.SelectedNode;
            if (tn != null)
            {
                ScreenTimer.Enabled = false;
                tabs.SelectedTab = tab_Desktop;
                if (tn != CurrentNode)
                {
                    CurrentNode = tn;
                    GeneralControler.OpenScreen(tn.Tag);
                    //��ͣƬ��,����Ļ���ƶ˺ͷ��������ʱ�佨������
                    Thread.Sleep(300);
                    ScreenOpen = true;
                }
                ScreenTimer.Enabled = true;
            }
        }
        //��ͣ��ȡԶ����Ļ
        private void ��ͣPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Enabled = false;
            ScreenOpen = false;
        }

        //�رս�ȡԶ����Ļ
        private void �ر�CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Enabled = false;
            ScreenOpen = false;
            GeneralControler.CloseScreenControler();
        }

        #endregion

       
        //�����˶Ի�
        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (rtb_Speak.Text == "")
                MessageBox.Show("���ܷ��Ϳ���Ϣ!", "��ֹ��", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            else
            {
                if (GeneralControler.Speak(rtb_Speak.Text))
                {
                    rtb_Content.Text += ("����Ա" + ":\n\t" + rtb_Speak.Text + "\n");
                    rtb_Speak.Text = "";
                }
            }
        }

        //�����˶Ի�
        private void rtb_Speak_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
                btn_Send_Click(sender, e);
        }

        //�ڹرճ���ǰ�ر����е�����
        private void frm_client_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();            
        }

        private void �ػ�SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = trv_HostView.SelectedNode;
            if (tn != null && tn.Tag != null)
            {
                DialogResult result = MessageBox.Show("ȷ���ر�Զ������" + tn.Tag.ToString() + "��?", "�ر�ȷ��", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                    GeneralControler.CloseWindows(tn.Tag);
            }
            else
            {
                MessageBox.Show("��ǰ�����Ѿ��Ͽ�������,���Ƴ���,Ȼ��������");
            }
        }

        private void �Ƴ�MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = trv_HostView.SelectedNode;
            if (tn != null)
            {
                GeneralControler.RemoveControler(tn.Tag);
                tn.Remove();
            }
        }

        private void trv_HostView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            trv_HostView.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Left && e.Node != null && e.Node != trv_HostView.Nodes[0])
                if (e.Node.Tag != null)
                {
                     GeneralControler.ChangeControler(e.Node.Tag);
                }
                else
                    lbl_Display.Text = "��ǰ�����Ѿ��Ͽ�������,���Ƴ���,Ȼ��������";
           
        }

        private void trv_HostView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ��������RToolStripMenuItem_Click(sender, e);
        }

        private void ��������NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ָ������SToolStripMenuItem_Click(sender, e);
        }

        private void �Ƴ�����RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trv_HostView.Nodes[0].Nodes != null)
                trv_HostView.Nodes[0].Nodes.Clear();
            if (GeneralControler != null)
                GeneralControler.CloseAll();
        }

        private void �ر�����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            �Ƴ�����RToolStripMenuItem_Click(sender, e);
        }

        private void �˳�EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GeneralControler != null)
                GeneralControler.CloseAll();
            System.Environment.Exit(System.Environment.ExitCode);
            Application.Exit();
        }

        private void frm_client_FormClosed(object sender, FormClosedEventArgs e)
        {
            // �˳�EToolStripMenuItem_Click(sender, e);
        }

        #region �����¼�
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //if(ScreenOpen && tabs.SelectedTab == tab_Desktop)
                GeneralControler.KeyDown(e.KeyCode);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (ScreenOpen && tabs.SelectedTab == tab_Desktop)
                GeneralControler.KeyUp(e.KeyCode);
           // base.OnKeyUp(e);
        }

        #endregion

        //��ʾ������
        private void ��OToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Show();
        }

        private void �ر�����CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            �ر�����ToolStripMenuItem_Click(sender, e);
        }

        private void ����AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new frm_AboutINCU().Show();
        }

        private void �˳�EToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            �˳�EToolStripMenuItem_Click(sender, e);
        }

        //��������
        private void ����CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(ICanSeeYou.Common.Constant.HelpFilename))
                    System.Diagnostics.Process.Start(ICanSeeYou.Common.Constant.HelpFilename);
                else
                    MessageBox.Show("�����ļ���ʧ��");
            }
            catch
            {
                MessageBox.Show("�����ļ��޷���ȡ�������Ѿ��𻵡�");
            }
        }

        private void ����AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ����AToolStripMenuItem1_Click(sender, e);
        }


        private void ��QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Interval = 500;
            ��MToolStripMenuItem.Checked = false;
            ��QToolStripMenuItem.Checked = true;
            ��SToolStripMenuItem.Checked = false;

        }

        private void ��MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Interval = 1000;
            ��MToolStripMenuItem.Checked = true;
            ��QToolStripMenuItem.Checked = false;
            ��SToolStripMenuItem.Checked = false;

        }

        private void ��SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTimer.Interval = 1500;
            ��MToolStripMenuItem.Checked = false;
            ��QToolStripMenuItem.Checked = false;
            ��SToolStripMenuItem.Checked = true;

        }

        //�߼�����
        private void �߼�HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Option option = new frm_Option();
            DialogResult result= option.ShowDialog();
            if (result == DialogResult.OK)
            {
                while (result == DialogResult.OK && option.ServerPassWordChanged == PwdChangType.UNSUCCESED)
                {
                    MessageBox.Show("����������޸Ĳ��ɹ�");
                    if (option != null)
                        result = option.ShowDialog();
                }
                if (option.ServerPassWordChanged == PwdChangType.CHANGED)
                {
                    GeneralControler.ChangeServerPassWord(option.ServerPassWord);
                }
                if (option.UpdatedFileChanged)
                {
                    ICanSeeYou.Configure.OptionManager.ChangeUpdatedFile(option.UpdatedFile, option.Version);
                    GeneralControler.UpdatedFile = option.UpdatedFile;
                    GeneralControler.ServerVersion = option.Version;
                }
                while (result == DialogResult.OK && option.ClientPassWordChanged == PwdChangType.UNSUCCESED)
                {
                    MessageBox.Show("�ͻ��������޸Ĳ��ɹ�");
                    if(option !=null)
                        result = option.ShowDialog();
                }
                if (option.ClientPassWordChanged==PwdChangType.CHANGED)
                {
                    string Md5Pwd = ICanSeeYou.Configure.PassWord.MD5Encrypt(option.ClientPassWord);
                    if (Md5Pwd != "")
                        ICanSeeYou.Configure.OptionManager.ChangePassWord(Md5Pwd);
                    else
                        MessageBox.Show("�ͻ��˵����벻�ܱ�����!�������޸�!");
                }
                
            }
        }

        //���������
        private void ���������UtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICanSeeYou.Configure.Option option = new ICanSeeYou.Configure.Option(ICanSeeYou.Common.Constant.OptionFilename);
            if(!option.Read()) MessageBox.Show("�����ļ���ʧ,����������.");
            if (option.OptFile == null)MessageBox.Show("�����ļ���������,����������.");
            else{
                string UpdatedFile = option.OptFile.UpdatedFile;
                if (UpdatedFile != null && System.IO.File.Exists(UpdatedFile))
                    GeneralControler.UpdateServer();
                else
                    MessageBox.Show("�����ļ���������������ļ���ʧ.");
            }
        }

        //˫��������ʾ������������
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                ��OToolStripMenuItem3_Click(sender, e);
            }
            else
                this.WindowState = FormWindowState.Minimized;
        }

       
    }
}