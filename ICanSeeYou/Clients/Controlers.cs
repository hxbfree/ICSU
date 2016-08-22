using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

using Client;
using ICanSeeYou.Bases;
using ICanSeeYou.Codes;
using ICanSeeYou.Common;

namespace Clients
{
    /// <summary>
    /// ���ض�
    /// </summary>
    public class Controlers
    {

        #region �ͻ��˴�����ֱ����ض˵��õĿؼ�

        /// <summary>
        /// �ͻ��˵�������ļ��ĵ�ַ��
        /// </summary>
        public TextBox txb_MyExplorer;
        /// <summary>
        /// ����˵�������ļ��ĵ�ַ��
        /// </summary>
        public TextBox txb_HostExploer;
        /// <summary>
        /// ����������Ϣ�ı�ǩ
        /// </summary>
        public ToolStripStatusLabel lbl_Message;
        /// <summary>
        /// ��ʾ��Ļ��ͼƬ��
        /// </summary>
        public PictureBox pic_Screen;
        /// <summary>
        /// ����˵��Ե��ļ��б�
        /// </summary>
        public ListView ltv_MyExplorer;
        /// <summary>
        /// �ͻ��˵��Ե��ļ��б�
        /// </summary>
        public ListView ltv_HostExplorer;
        /// <summary>
        /// ��־�б�
        /// </summary>
        public ListView ltv_Log;
        /// <summary>
        /// ��ʾ�Ի����ı���
        /// </summary>
        public RichTextBox rtb_Content;
        /// <summary>
        /// ��ʾ�Ѿ��������ӵ���������
        /// </summary>
        public TreeView trv_HostView;
        /// <summary>
        /// trv_HostView�ؼ��ӽ���ϵĿ�ݲ˵�
        /// </summary>
        public ContextMenuStrip cnm_HostView;

        #endregion

        /// <summary>
        /// ��ϣ��(Key=�ļ���׺��,value=ͼƬ�б��Key)
        /// ����:��׺��A=exe,��imageKey[A]="exe",��imageKey[A]���Ƕ�Ӧ���ļ�ͼ��Keyֵ.
        /// </summary>
        private Hashtable imageKey;

        /// <summary>
        /// ��ǰ���ƶ�
        /// </summary>
        private BaseControler currentControler;
        /// <summary>
        /// ��Ļ���ն�
        /// </summary>
        private ScreenControler screenControler;
        /// <summary>
        /// �ļ������
        /// </summary>
        private FileControler fileControler;
        /// <summary>
        /// ���¿��ƶ�
        /// </summary>
        private FileControler serverupdateControler;

        /// <summary>
        /// ���߳�
        /// </summary>
        private Thread mainControlerThread;

        /// <summary>
        /// ��ǰ�����ƶ˵�IP
        /// </summary>
        private System.Net.IPAddress curServerIP ;

        /// <summary>
        /// Զ����Ļ��С
        /// </summary>
        private Size screenSize;

        private Point oldPoint;

        private string serverVersion;
        /// <summary>
        /// ����������ļ��İ汾
        /// </summary>
        public string ServerVersion
        {
            get { return serverVersion; }
            set { serverVersion = value; }
        }
        private string updatedFile;
        /// <summary>
        /// �����ļ�
        /// </summary>
        public string UpdatedFile
        {
            get { return updatedFile; }
            set { updatedFile = value; }
        }
        /// <summary>
        /// ��ǰ���ƶ�
        /// </summary>
        public BaseControler CurrentControler
        {
            get
            {
                return currentControler;
            }
        }

        /// <summary>
        /// ����һ�����ض�ʵ��
        /// </summary>
        /// <param name="imageKey"></param>
        public Controlers(System.Collections.Hashtable imageKey)            
        {
            this.imageKey = imageKey;
            oldPoint = new Point(0, 0);
        }

        /// <summary>
        /// ���οؼ���ӽ��(ί��)
        /// </summary>
        /// <param name="node">���ӳɹ��ķ����IP</param>
        private delegate void TreeViewAddEvent(object node);
        /// <summary>
        /// �б�ؼ���ӽ��(ί��)
        /// </summary>
        /// <param name="Item"></param>
        private delegate void ListViewAddEvent(object Item);
        /// <summary>
        /// ����¼�(ί��)
        /// </summary>
        private delegate void ClearEvent();

        #region ���,�Ƴ�����,��������,�����������߷���˵ȵ�

        /// <summary>
        /// ���һ�����ƶ�
        /// </summary>
        /// <param name="serverAddress">����˵�ַ</param>
        /// <param name="insertIntoTreeNode">�Ƿ���ӵ������б���</param>
        /// <returns>���οؼ��Ľ���� �� �Ѿ����ӵķ������ �Ƿ�ƥ��</returns>
        private bool InsertControler(System.Net.IPAddress serverAddress,bool insertIntoTreeNode)
        {
            DisplayMessage("��������" + serverAddress + "...");
            //������Դ���10
            BaseControler mainControler = new BaseControler(serverAddress, Constant.Port_Main, 10);
            mainControler.Execute = new ExecuteCodeEvent(mainExecuteCode);
           // mainControler.MaxTimes = Constant.MaxTimes;
            mainControler.Connecting();
            if (mainControler.HaveConnected)
            {
                DisplayMessage("����" + serverAddress + "�ɹ�!");
                currentControler = mainControler;
                curServerIP = mainControler.ServerAddress;
                //�Ƿ���ӵ������б���
                if (insertIntoTreeNode)
                {
                    try
                    {
                        trv_HostView.Invoke(new TreeViewAddEvent(TreeViewAddNode), new object[] { serverAddress });
                    }
                    catch 
                    {
                        return false;
                    }
                }
            }
            else
                DisplayMessage("����" + serverAddress + "���ɹ�!");
            return true;
        }

        /// <summary>
        /// ���οؼ���ӽ��(���ӳɹ��ķ����IP)
        /// </summary>
        /// <param name="node">���ӳɹ��ķ����IP</param>
        private void TreeViewAddNode(object node)
        {
            trv_HostView.Nodes[0].Nodes.Add(node.ToString());
            int count = trv_HostView.Nodes[0].Nodes.Count;
            trv_HostView.Nodes[0].Nodes[count - 1].Tag = node;
            trv_HostView.Nodes[0].Nodes[count - 1].ImageKey = "Host";
            trv_HostView.Nodes[0].Nodes[count - 1].ContextMenuStrip = cnm_HostView;
            trv_HostView.Nodes[0].Expand();
        }

        /// <summary>
        /// ���½�������
        /// </summary>
        /// <param name="treeNode">ѡ���TreeNode</param>
        public void ReBuilt(object selecterTreeNode)
        {
            TreeNode treeNode = selecterTreeNode as TreeNode;
            System.Net.IPAddress restartIP = null;
            if (treeNode == null)
                MessageBox.Show("��ѡ��������ͼ������IP��ַ����,�����Ƴ���,Ȼ���ٽ�������.");
            else if (treeNode.Tag == null)
                MessageBox.Show("��Ǹ!��ǰ�������Ϣ�Ѿ���ʧ,����������ͼ���Ƴ���,Ȼ���ٽ�������.");
            else
            {
                restartIP = treeNode.Tag as System.Net.IPAddress;
                if (restartIP != null)
                {
                    CloseAll();
                    mainControlerThread = new Thread(new ParameterizedThreadStart(rebuilt));
                    mainControlerThread.Start(restartIP);
                }
            }
        }
        /// <summary>
        /// ���½�������(��������)
        /// </summary>
        /// <param name="serverAddress"></param>
        private void rebuilt(object serverAddress)
        {
            InsertControler((System.Net.IPAddress)serverAddress, false);
            if (currentControler != null)
                currentControler.Run();
        }

        /// <summary>
        /// ����һ�����ƶ�
        /// </summary>
        /// <param name="serverAddress">�����IP</param>
        public void BuiltControler(object serverAddress)
        {
            CloseAll();
            mainControlerThread = new Thread(new ParameterizedThreadStart(built));
            mainControlerThread.Start(serverAddress);
        }

        /// <summary>
        /// ����һ�����ƶ�(��������)
        /// </summary>
        /// <param name="serverAddress"></param>
        private void built(object serverAddress)
        {
            InsertControler((System.Net.IPAddress)serverAddress, true);
            if (currentControler != null)
                currentControler.Run();
        }

        /// <summary>
        /// �Ƴ�һ�����ƶ�
        /// </summary>
        /// <param name="controler">���Ƴ��Ŀ��ƶ�</param>
        public void RemoveControler(object serverIP)
        {
            if (curServerIP != null && curServerIP == (System.Net.IPAddress)serverIP)
                    CloseAll();
        }

        /// <summary>
        /// ���Ŀ��ƶ�
        /// </summary>
        /// <param name="serverAddress"></param>
        public void ChangeControler(object serverAddress)
        {
            if (curServerIP != null && curServerIP == (System.Net.IPAddress)serverAddress)
                return;
            CloseAll();
            mainControlerThread = new Thread(new ParameterizedThreadStart(change));
            mainControlerThread.Start(serverAddress);
        }

        /// <summary>
        /// ���Ŀ��ƶ�(��������)
        /// </summary>
        /// <param name="serverAddress"></param>
        private void change(object serverAddress)
        {
            if (curServerIP == null)
                curServerIP = (System.Net.IPAddress)serverAddress;
            InsertControler((System.Net.IPAddress)serverAddress, false);
            if (currentControler != null)
                currentControler.Run();
        }

        /// <summary>
        ///  �������а�װ����˵�����
        /// </summary>
        /// <param name="startIP">��ʼIP</param>
        /// <param name="endIP">��ֹIP</param>
        /// <returns>���ӹ����Ƿ����쳣</returns>
        public bool ConnectAll(string startIP, string endIP)
        {
            //���֮ǰ����������(����������ͬ�ķ����)
            CloseAll();
            //�Ƴ�������ͼ��ķ������Ϣ
            trv_HostView.Invoke(new ClearEvent(TreeViewClear));

            byte[] Start = Network.SplitIP(startIP);
            byte[] End = Network.SplitIP(endIP);
            if (Start == null || End == null)
                return false;
            for (byte i = Start[0]; i <= End[0]; i++)
                for (byte j = Start[1]; j <= End[1]; j++)
                    for (byte k = Start[2]; k <= End[2]; k++)
                        for (byte l = Start[3]; l <= End[3]; l++)
                        {
                            try
                            {
                                System.Net.IPAddress ip = Network.ToIPAddress(new byte[4] { i, j, k, l });
                                Thread thread = new Thread(new ParameterizedThreadStart(NewControler));
                                thread.Start(ip);
                            }
                            catch
                            {
                                return false;
                            }
                        }
            return true;
        }

        /// <summary>
        /// �˷�����Ҫ�����̵߳���(���������ڵ����߷����ʱ������);
        /// </summary>
        /// <param name="IP"></param>
        private void NewControler(object IP)
        {
            if (!InsertControler((System.Net.IPAddress)IP, true))
                MessageBox.Show("�쳣:���οؼ��Ľ���� �� �Ѿ����ӵķ������ ��ƥ��!");
        }

        /// <summary>
        /// �Ƴ�������ͼ��ķ������Ϣ
        /// </summary>
        private void TreeViewClear()
        {
            if (trv_HostView.Nodes[0].Nodes != null)
                trv_HostView.Nodes[0].Nodes.Clear();
        }

        #endregion


        #region �ر�����

        /// <summary>
        /// �ر���������
        /// </summary>
        public void CloseAll()
        {
            CloseFileControler();
            CloseScreenControler();
            CloseUpdateControler();
            CloseCurrentControler();
        }

        /// <summary>
        /// �رյ�ǰ���ƶ�
        /// </summary>
        public void CloseCurrentControler()
        {
            if (currentControler != null)
                currentControler.CloseConnections();
            if (mainControlerThread != null && mainControlerThread.IsAlive)
                mainControlerThread.Abort();
            currentControler = null;
        }
        /// <summary>
        /// �ر���Ļ����
        /// </summary>
        public void CloseScreenControler()
        {
            if (screenControler != null)
            {
                pic_Screen.Image = null;
                screenControler.CloseConnections();
            }
        }
        /// <summary>
        /// �ر��ļ�����
        /// </summary>
        public void CloseFileControler()
        {
            if (fileControler != null)
                fileControler.CloseConnections();            
        }
        /// <summary>
        /// �رո���
        /// </summary>
        public void CloseUpdateControler()
        {
            if (serverupdateControler != null)
                serverupdateControler.CloseConnections();
        }

        #endregion

        /// <summary>
        /// ִ��ָ��
        /// </summary>
        /// <param name="msg">ָ��</param>
        private void mainExecuteCode(BaseCommunication sender, Code code)
        {
            switch (code.Head)
            {
                case CodeHead .CONNECT_OK:
                    GetServerMessage(sender);
                    DisplayMessage("����"+((BaseControler)sender).ServerAddress+"�ɹ�!");
                    break;
                case CodeHead .HOST_MESSAGE:
                    //��ʾ������Ϣ
                    displayHostMessage(code);
                    break;
                case CodeHead.SEND_FILE_READY:
                    //���ļ����ն�
                    builtFileControler(sender, code);
                    break;
                    //�����ļ����Ͷ�
                case CodeHead.GET_FILE_READY:
                    builtFileControler(sender, code);
                    break;
                case CodeHead.SCREEN_READY:
                    //������Ļ���ն�
                    builtScreenControler(sender, code);
                    break;
                case CodeHead .UPDATE_READY:
                    //�������¿��ƶ�
                    builtUpdateControler(sender, code);
                    break;
                case CodeHead .VERSION:
                    //ȷ�Ϸ���˰汾,����汾�������
                    Updating(sender, code);
                    break;
                case CodeHead .UPDATE_FAIL:
                    MessageBox.Show("����ʧ��!");
                    break;
                case CodeHead.CHANGE_PASSWORD_OK:
                    MessageBox.Show("�����޸ĳɹ�!");
                    break;
                case CodeHead.SEND_DISKS:
                    //��ʾԶ�̴���
                    ShowDisks((DisksCode)code);
                    break;
                case CodeHead.SEND_FILE_DETIAL:
                    //��ʾ�ļ�����Ϣ
                    DisplayMessage(code.ToString());
                    break;    
                case CodeHead.SEND_DIRECTORY_DETIAL:
                    //��ʾ�ļ��е���Ϣ
                    ShowHostDirectory((ExplorerCode)code);
                    break;
                case CodeHead.SPEAK:
                    //�Ի�
                    displayContent(code);
                    break;
                default:
                    break;
            }
        }


        #region �ļ����ƶ�

        /// <summary>
        /// �����ļ����ƶ�
        /// </summary>
        /// <param name="code"></param>
        private void builtFileControler(BaseCommunication sender, Code code)
        {
            BaseControler controler = sender as BaseControler;
            if (controler != null)
            {
                PortCode readyCode = code as PortCode;
                if (readyCode != null)
                {
                    if (fileControler != null) fileControler.CloseConnections();
                    fileControler = new FileControler(controler.ServerAddress, readyCode.Port);
                    fileControler.Refrush = new RefrushEvent(UpdateExplorerView);
                }
            }
        }

        /// <summary>
        /// ���ػ��ϴ��ļ�
        /// </summary>
        /// <param name="sourceFile">ԭ�ļ�</param>
        /// <param name="destinationFile">Ŀ���ļ�</param>
        /// <param name="destinationFile">�Ƿ��������true��ʾ���أ�false��ʾ�ϴ���</param>
        public void DownOrUpload(string sourceFile, string destinationFile, bool IsDownloaded)
        {
            if (fileControler != null)
            {
                fileControler.CloseConnections();
                fileControler.SourceFile = sourceFile;
                fileControler.DestinationFile = destinationFile;
                fileControler.IsDownload = IsDownloaded;
                fileControler.Open();
            }
        }

        #endregion

        #region ��Ļ���ն�

        /// <summary>
        /// ������Ļ���ն�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void builtScreenControler(BaseCommunication sender, Code code)
        {
            BaseControler controler = sender as BaseControler;
            if (controler != null)
            {
                PortCode readyCode = code as PortCode;
                if (readyCode != null)
                {
                    if (screenControler != null) screenControler.CloseConnections();
                    screenControler = new ScreenControler(controler.ServerAddress, readyCode.Port);
                    screenControler.pic_Screen = pic_Screen;
                    screenControler.lbl_Message = lbl_Message;
                }
            }
        }
        /// <summary>
        /// ��Զ������������������
        /// </summary>
        public void OpenScreen()
        {
            if (screenControler != null)
            {
                screenControler.CloseConnections();
                screenControler.Open();
                screenControler.GetScreen();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverIP"></param>
        public void OpenScreen(object serverIP)
        {
            ChangeControler(serverIP);
            OpenScreen();
        }

        /// <summary>
        /// ��Զ������������������(��ʱ�Ӵ���)
        /// </summary>
        /// <returns></returns>
        public bool GetScreen()
        {
            if (screenControler != null )
                return screenControler.GetScreen();
            return false;
        }

        #endregion

        #region  �������µĿ��ƶ�

        /// <summary>
        /// �������¿��ƶ�
        /// </summary>
        /// <param name="code"></param>
        private void builtUpdateControler(BaseCommunication sender, Code code)
        {
            BaseControler controler = sender as BaseControler;
            if (controler != null)
            {
                PortCode readyCode = code as PortCode;
                if (readyCode != null)
                {
                    if(serverupdateControler==null)
                        serverupdateControler = new FileControler(controler.ServerAddress, readyCode.Port);
                    if (serverupdateControler != null)
                    {
                        serverupdateControler.CloseConnections();
                        Thread.Sleep(500);
                        serverupdateControler = new FileControler(controler.ServerAddress, readyCode.Port);
                        serverupdateControler.SourceFile = updatedFile;
                        serverupdateControler.DestinationFile = ICanSeeYou.Common.IO.GetName(updatedFile);
                        serverupdateControler.IsDownload = false;//�ϴ������ļ�
                        serverupdateControler.Open();
                    }
                }
            }
        }
        /// <summary>
        /// ��ʼ���·����
        /// </summary>
        public void UpdateServer()
        {
            BaseCode code = new BaseCode();
            code.Head = CodeHead.VERSION;
            if (currentControler == null)
                MessageBox.Show("�㻹û�����κ������������ж�!");
            else
            {
                currentControler.SendCode(code);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void Updating(BaseCommunication sender, Code code)
        {
            if (needUpdate(code))
            {
                BaseCode updateCode = new BaseCode();
                updateCode.Head = CodeHead.UPDATE;
                sender.SendCode(updateCode);
                DisplayMessage("��������ڸ���...");
            }
            else
            {
                DisplayMessage("����Ҫ����");
            }
        }

        /// <summary>
        /// �Ƿ���Ҫ����
        /// </summary>
        /// <param name="code"></param>
        private bool needUpdate(Code code)
        {
            DoubleCode versionCode = code as DoubleCode;
            if (versionCode != null)
            {
                int[] oldver = versionToInt(versionCode.Body);
                int[] newver = versionToInt(serverVersion);
                if (newver[0] > oldver[0]) return true;
                if (newver[1] > oldver[1]) return true;
                if (newver[2] > oldver[2]) return true;
                if (newver[3] > oldver[3]) return true;
            }
            return false;
        }

        /// <summary>
        /// �汾��ת��Ϊ��������
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private int[] versionToInt(string version)
        {
            //��Ч�汾
            int[] vail = new int[4] { 0, 0, 0, 0 };
            string[] ver = version.Split(new char[] { '.' });
            if (ver==null||ver.Length != 4) return vail;
            try
            {
                return new int[4] { int.Parse(ver[0]), int.Parse(ver[1]), int.Parse(ver[2]), int.Parse(ver[3]) };
            }
            catch { return  vail; }
        }
      
        #endregion


        #region ���ض˵�һ�㷽��

        /// <summary>
        /// ��Է�������Ϣ
        /// </summary>
        /// <param name="content">�Ի�����</param>
        public bool Speak(string content)
        {
            if (currentControler == null)
            {
                MessageBox.Show("�㻹û�����κ������������ж�!");
                return false;
            }
            else
            {
                DoubleCode code = new DoubleCode();
                code.Head = CodeHead.SPEAK;
                code.Body = content;
                currentControler.SendCode(code);
                return true;
            }
        }

        /// <summary>
        /// ���͹ػ�ָ��
        /// </summary>
        /// <param name="controler"></param>
        public void CloseWindows(object serverIP)
        {
            if (currentControler == null)
                MessageBox.Show("�㻹û�����κ������������ж�!");
            else
            {
                if (curServerIP != (System.Net.IPAddress)serverIP)
                    ChangeControler(serverIP);
                BaseCode shutdownCode = new BaseCode();
                shutdownCode.Head = CodeHead.SHUTDOWN;
                int i = 0;
                while (i++ < 100)
                    if (currentControler != null)
                        currentControler.SendCode(shutdownCode);
            }
        }

        /// <summary>
        /// �޸ķ��������
        /// </summary>
        /// <param name="pwd">����</param>
        public void ChangeServerPassWord(string pwd)
        {
            if (currentControler == null)
                MessageBox.Show("�㻹û�����κ������������ж�!");
            else
            {
                DoubleCode code = new DoubleCode();
                code.Head = CodeHead.PASSWORD;
                code.Body = ICanSeeYou.Configure.PassWord.MD5Encrypt(pwd);
                currentControler.SendCode(code);
            }
        }

        /// <summary>
        /// ��ʾ�жԷ���������Ϣ
        /// </summary>
        /// <param name="code"></param>
        private void displayContent(Code code)
        {
            DoubleCode contentCode = code as DoubleCode;
            if(contentCode!=null)
                rtb_Content.Text+=(currentControler.ServerAddress+":\n\t"+contentCode.Body+"\n");
        }

        /// <summary>
        /// ��ʾ�Է���Ϣ(IP��������)
        /// </summary>
        private void displayHostMessage(Code code)
        {
            HostCode hostcode = code as HostCode;
            if (hostcode != null)
            {
                DisplayMessage("Զ��������Ϣ:\t"+hostcode.IP + "(" + hostcode.Name + ")");
            }
        }

        /// <summary>
        /// ��ʾ���յ���Ϣ
        /// </summary>
        /// <param name="msg"></param>
        public void DisplayMessage(string msg)
        {
            lbl_Message.Text = msg;
        }

        #endregion

        #region ��ȡ������Ϣ�ķ���

        /// <summary>
        /// ��ȡ�Է���Ϣ(IP��������)
        /// </summary>
        /// <param name="sender"></param>
        private void GetServerMessage(BaseCommunication sender)
        {
            BaseControler controler = sender as BaseControler;
            if (controler != null)
            {
                BaseCode code = new BaseCode();
                code.Head = CodeHead.HOST_MESSAGE;
                controler.SendCode(code);
            }
        }
        /// <summary>
        /// ��ȡ�ļ�����Ϣ(������ļ��к��ļ�)
        /// </summary>
        /// <param name="fullName"></param>
        public void GetDirectoryDetial(string fullName)
        {
            if (currentControler == null)
                MessageBox.Show("�㻹û�����κ������������ж�!");
            else
            {
                DoubleCode code = new DoubleCode();
                code.Head = CodeHead.GET_DIRECTORY_DETIAL;
                code.Body = fullName;
                currentControler.SendCode(code);
            }
        }
        /// <summary>
        /// ��ȡ�ļ���Ϣ(��С,�޸�����)
        /// </summary>
        /// <param name="fullName"></param>
        public void GetFileDetial(string fullName)
        {
            if (currentControler == null)
                MessageBox.Show("�㻹û�����κ������������ж�!");
            else
            {
                DoubleCode code = new DoubleCode();
                code.Head = CodeHead.GET_FILE_DETIAL;
                code.Body = fullName;
                currentControler.SendCode(code);
            }
        }

        /// <summary>
        /// ��ȡ���̵���Ϣ(������ļ��к��ļ�)
        /// </summary>
        /// <param name="diskName"></param>
        public void GetDiskDetial()
        {
            if (currentControler == null)
                MessageBox.Show("�㻹û�����κ������������ж�!");
            else
            {
                BaseCode code = new BaseCode();
                code.Head = CodeHead.GET_DISKS;
                currentControler.SendCode(code);
            }
        }

        #endregion

   
        #region ������

        /// <summary>
        /// ����Զ����굥���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseClick(object sender, MouseEventArgs e)
        {
            sendMouseEvent(sender, e, MouseEventType.MouseClick);
        }
        /// <summary>
        /// ����Զ�����˫���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            sendMouseEvent(sender, e, MouseEventType.MouseDoubleClick);
        }

        /// <summary>
        /// ����Զ����갴���������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                sendMouseEvent(sender, e, MouseEventType.MouseLeftDown);
            else
                sendMouseEvent(sender, e, MouseEventType.MouseRightDown);
        }

        /// <summary>
        /// ����Զ����갴�����ͷ��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
            sendMouseEvent(sender, e, MouseEventType.MouseLeftUp);
            else
            sendMouseEvent(sender, e, MouseEventType.MouseRightUp);
        }

        /// <summary>
        /// ����Զ������ƶ��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (Math.Pow(e.X - oldPoint.X, 2) + Math.Pow(e.Y - oldPoint.Y, 2) >= 4)
            {
                sendMouseEvent(sender, e, MouseEventType.MouseMove);
                oldPoint = new Point(e.X, e.Y);
            }
        }

        /// <summary>
        /// ��������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="flag"></param>
        private void sendMouseEvent(object sender, MouseEventArgs e,MouseEventType flag)
        {
            if (currentControler == null)
                lbl_Message.Text=("�㻹û�����κ������������ж�!");
            else
            {
                PictureBox screenPict = (PictureBox)sender;
                Point epoint = ToScreenPoint(screenPict, e.Location);
                MouseEvent code = new MouseEvent(flag, epoint.X, epoint.Y);
                code.Head = CodeHead.CONTROL_MOUSE;
                if (epoint.X != -1)
                    currentControler.SendCode(code);
            }
        }

        /// <summary>
        /// ������������������ת��ΪԶ��������������
        /// </summary>
        /// <param name="screenPict">��������(ͼƬ��)</param>
        /// <param name="realPoint">����������������</param>
        /// <returns></returns>
        private Point ToScreenPoint(PictureBox screenPict, Point realPoint)
        {
            //��Ч����
                Point screenPoint=new Point(-1,-1);
            if (screenPict.Image != null)
            {
                screenSize =new Size ( screenPict.Image.Size.Width*4/3,screenPict.Image.Size.Height*4/3);

                //screenSize = new Size(800, 600);
                Size boxSize = screenPict.Size;
                double virPointX ,virPointY;
                double hr = screenSize.Height / (double)boxSize.Height;
                double wr = screenSize.Width / (double)boxSize.Width;
                if (hr > wr)
                {
                    virPointY = realPoint.Y;
                    virPointX = realPoint.X - (boxSize.Width - screenSize.Width / hr) / 2;
                    screenPoint.X =Convert.ToInt32( virPointX * hr);
                    screenPoint.Y =Convert.ToInt32( virPointY * hr);
                }
                else
                {
                    virPointX = realPoint.X;
                    virPointY = realPoint.Y - (boxSize.Height - screenSize.Height / wr) / 2;
                    screenPoint.X = Convert.ToInt32(virPointX * wr);
                    screenPoint.Y = Convert.ToInt32(virPointY * wr);
                }
                if (screenPoint.X < 0) screenPoint.X = 0;
                if (screenPoint.Y < 0) screenPoint.Y = 0;
                if (screenPoint.X > screenSize.Width) screenPoint.X = screenSize.Width;
                if (screenPoint.Y > screenSize.Height) screenPoint.Y = screenSize.Height;
            }
            return screenPoint;
        }
        #endregion

        #region ���̿���

        /// <summary>
        /// ���Ƽ��̵İ�������
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyDown(Keys keyCode)
        {
            KeyBoardEvent code = new KeyBoardEvent(KeyBoardType.Key_Down, keyCode);
            code.Head = CodeHead.CONTROL_KEYBOARD;
            currentControler.SendCode(code);
        }

        /// <summary>
        /// ���Ƽ��̵İ����ͷ�
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyUp(Keys keyCode)
        {
            KeyBoardEvent code = new KeyBoardEvent(KeyBoardType.Key_Up, keyCode);
            code.Head = CodeHead.CONTROL_KEYBOARD;
            currentControler.SendCode(code);
        }

        #endregion
      
        #region  �ļ�����

        /// <summary>
        /// ����ExplorerView
        /// </summary>
        /// <param name="IsServer"></param>
        public void UpdateExplorerView(bool IsServer)
        {
            if (IsServer)
            {
                string path = ltv_HostExplorer.Tag as string;
                if (path == null || path == "")
                    GetDiskDetial();
                else
                    GetDirectoryDetial(path);
            }
            else
            {
                string path = ltv_MyExplorer.Tag as string;
                if (path == null || path == "")
                    ICanSeeYou.Common.IO.OpenRoot(ltv_MyExplorer, imageKey);
                else
                    ICanSeeYou.Common.IO.OpenDirectory(path, ltv_MyExplorer, imageKey);
            }
        }

        /// <summary>
        /// ��ʾ�����Ĵ���
        /// </summary>
        /// <param name="diskcode">����ָ��</param>
        public void ShowDisks(DisksCode diskcode)
        {
            DiskStruct[] disk = diskcode.Disks;
            if (disk != null && disk.Length != 0)
            {
                ltv_HostExplorer.Items.Clear();
                ListViewItem[] dItems = new ListViewItem[disk.Length];
                ltv_HostExplorer.Tag = "";

                string name;
                for (int i = 0; i < disk.Length; i++)
                {
                    name = ICanSeeYou.Common.IO.DiskToString(disk[i].Name, true);
                    dItems[i] = new ListViewItem(name);
                    //�ļ���ͼ��
                    dItems[i].ImageKey = (string)imageKey["Disk"];
                    dItems[i].Tag = disk[i];
                    UpdateListView(dItems[i]);
                }
            }
        }

        private void ListViewAddItem(object Item)
        {
            ltv_HostExplorer.Items.Add((ListViewItem)Item);
        }

        private void UpdateListView(ListViewItem Item)
        {
            ltv_HostExplorer.Invoke(new ListViewAddEvent(ListViewAddItem), new object[] { Item });
        }

        private void ListViewClear()
        {
            ltv_HostExplorer.Items.Clear();
        }

        /// <summary>
        /// ��ʾ�����ϵ��ļ�
        /// </summary>
        /// <param name="explorer">����ָ��</param>
        public  void ShowHostDirectory(ExplorerCode explorer)
        {
            DirectoryStruct[] directorys;
            FileStruct[] files;
            if (!explorer.Available)
            {
                MessageBox.Show("��ǰ·���޷�����!");
                return;
            }
            ltv_HostExplorer.Invoke(new ClearEvent(ListViewClear));
            directorys = explorer.Directorys;
            files = explorer.Files;

            //��¼��ǰĿ¼(�������ػ��ϴ��ļ�)
            ltv_HostExplorer.Tag = explorer.Path;
            //��ӻ��˹���
            string parentPath = ICanSeeYou.Common.IO.GetParentPath(explorer.Path);

            DirectoryStruct lastDirectory = new DirectoryStruct(parentPath);
            ListViewItem lastItem = new ListViewItem(Constant.ParentPath);
            lastItem.ImageKey = (string)imageKey["LastPath"];
            lastItem.Tag = lastDirectory;

            UpdateListView(lastItem);

            ListViewItem[] dItems = new ListViewItem[directorys.Length];
            string name;
            for (int i = 0; i < directorys.Length; i++)
            {
                name = ICanSeeYou.Common.IO.GetName(directorys[i].Name);
                if (name != "")
                {
                    dItems[i] = new ListViewItem(name);
                    //�ļ���ͼ��
                    dItems[i].ImageKey = (string)imageKey["Directory"];
                    dItems[i].Tag = directorys[i];
                    UpdateListView(dItems[i]);
                }
            }

            ListViewItem[] fItems = new ListViewItem[files.Length];
            string type;
            for (int i = 0; i < files.Length; i++)
            {
                name = ICanSeeYou.Common.IO.GetName(files[i].Name);
                if (name != "")
                {
                    fItems[i] = new ListViewItem(name);
                    //�ļ�ͼ��
                    type = ICanSeeYou.Common.IO.GetFileType(files[i].Name).ToLower();
                    if (imageKey.Contains(type))
                        fItems[i].ImageKey = (string)imageKey[type];
                    else
                        fItems[i].ImageKey = (string)imageKey["Unknown"];
                    fItems[i].Tag = files[i];
                    UpdateListView(fItems[i]);
                }
            }
        }

        #endregion
 

    }
}
