using System;
using System.Text;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Threading;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using Server;
using ICanSeeYou.Codes;
using ICanSeeYou.Hooks;
using ICanSeeYou.Bases;
using ICanSeeYou.Windows;
using ICanSeeYou.Common;

namespace Servers
{
    /// <summary>
    /// �����ƶ�
    /// </summary>
    public class Servers
    {
        /// <summary>
        /// �������
        /// </summary>
        private BaseServer mainServer;
        /// <summary>
        /// ��Ļ�����
        /// </summary>
        private ScreenServer screenServer;
        /// <summary>
        /// �ļ������
        /// </summary>
        private FileServer fileServer;
        /// <summary>
        /// �������߳�
        /// </summary>
        private Thread mainThread;
        /// <summary>
        /// �ļ������߳�
        /// </summary>
        private Thread fileThread;
        /// <summary>
        /// ��Ļ�����߳�
        /// </summary>
        private Thread screenThread;
        /// <summary>
        /// ���ַ���˿�
        /// </summary>
        private int mainPort, screenPort, filePort;

        /// <summary>
        /// ����˵ĳ�����
        /// </summary>
        private string productName;
        /// <summary>
        /// �汾
        /// </summary>
        private string version;

        /// <summary>
        ///  �˳�ʱ��Ҫ���������
        /// </summary>
        private string exitPassWord;
        
        /// <summary>
        /// ����˵ĳ�����
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        
        /// <summary>
        /// �汾
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        ///  �˳�ʱ��Ҫ���������
        /// </summary>
        public string ExitPassWord
        {
            get { return exitPassWord; }
        }

        /// <summary>
        /// ��־�б�
        /// </summary>
        public ListView ltv_Log;

        /// <summary>
        /// ��ʾ��Ϣ�ı�ǩ
        /// </summary>
        public ToolStripStatusLabel lbl_Message;

        /// <summary>
        /// ����Servers(�����ƶ�)��һ��ʵ��
        /// </summary>
        /// <param name="mainPort">������˿�</param>
        /// <param name="filePort">�ļ�����˿�</param>
        /// <param name="screenPort">��Ļ����˿�</param>
        public Servers(int mainPort,int filePort,int screenPort)
        {
            this.mainPort = mainPort;
            this.screenPort = screenPort;
            this.filePort = filePort;
            this.version = "1.0.0.0";
            this.exitPassWord =ICanSeeYou.Configure.PassWord.Read(ICanSeeYou.Common.Constant.PassWordFilename);
            if (exitPassWord == null)
                exitPassWord = "";
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Run()
        {
            try
            {
                OpenMainServer();
                OpenScreenServer();
                OpenFileServer();
            }
            catch
            {
                Close();
                MessageBox.Show("ͨѶ�˿ڱ�ռ��!");
            }
        }

        /// <summary>
        /// �ر���������
        /// </summary>
        public void Close()
        {
            if (fileServer != null)
                fileServer.CloseConnections();
            if (screenServer != null)
                screenServer.CloseConnections();
            if (mainServer != null)
                mainServer.CloseConnections();
            if (fileThread!=null&& fileThread.IsAlive) fileThread.Abort();
            if (screenThread != null && screenThread.IsAlive) screenThread.Abort();
            if (mainThread != null && mainThread.IsAlive) mainThread.Abort();
        }

        /// <summary>
        /// �������������
        /// </summary>
        private void ReStart()
        {
            Close();
            Thread.Sleep(1000);
            Thread NewThread = new Thread(new ThreadStart(Run));
            NewThread.Start();
        }

        /// <summary>
        /// ���������
        /// </summary>
        private void OpenMainServer()
        {
                mainServer = new BaseServer(mainPort);
                mainServer.Execute = new ExecuteCodeEvent(mainExecuteCode);
                mainThread = new Thread(new ThreadStart(mainServer.Run));
                mainThread.Start();            
        }

        /// <summary>
        /// ���ļ������
        /// </summary>
        /// <param name="code"></param>
        private void OpenFileServer()
        {
            fileServer = new FileServer(filePort);
            fileThread = new Thread(new ThreadStart(fileServer.Run));
            fileThread.Start();            
        }

        /// <summary>
        /// ����Ļ�����
        /// </summary>
        /// <param name="code"></param>
        private void OpenScreenServer()
        {
            screenServer = new ScreenServer(screenPort);
            screenThread = new Thread(new ThreadStart(screenServer.Run));
            screenThread.Start();
        }

        /// <summary>
        /// ִ��ָ��
        /// </summary>
        /// <param name="msg">ָ��</param>
        private void mainExecuteCode(BaseCommunication sender, Code code)
        {
            switch (code.Head)
            {
                case CodeHead.CONNECT_OK:
                    //���ӳɹ�
                    displayMessage(code);
                    break;
                case CodeHead.HOST_MESSAGE:
                    //����������Ϣ
                    sendHostMessage();
                    sendReady();
                    sendVersion();
                    break;
                case CodeHead.SHUTDOWN:
                    //�ػ�
                    WindowsManager.ShutDown();
                    break;
                case CodeHead.REBOOT:
                    WindowsManager.Reboot();
                    // ���������.
                    break;
                case CodeHead.SPEAK:
                    //�Ի�
                  displayMessage(code);
                    break;
                case CodeHead .CLOSE_APPLICATION:
                    //�رճ���
                     Close();
                     Application.ExitThread();
                     Application.Exit();
                     break;
                case CodeHead.CONNECT_RESTART:
                    //������������
                    ReStart();
                    break;
                case CodeHead.GET_DISKS:
                    //��ȡ���ش���
                    sendDisks(sender);
                    break;
                case CodeHead.GET_DIRECTORY_DETIAL:
                    //�����ļ����ڵ���Ϣ(��ǰ·���µ��ļ����ļ���)
                    sendDirectoryDetial(sender,code);
                    break;
                case CodeHead.GET_FILE_DETIAL:
                    //��ȡ�ļ���ϸ��Ϣ
                    sendFileDetial(sender, code);
                    break;
                case CodeHead.CONTROL_MOUSE:
                    //������
                    doMouseEvent(code);
                    break;
                case CodeHead.CONTROL_KEYBOARD:
                    //���̿���
                    doKeyBoardEvent(code);
                    break;
                case CodeHead .VERSION:
                    //���Ͱ汾��Ϣ
                    sendVersion();
                    break;
                case CodeHead .UPDATE:
                    //���и��³���
                    builtUpdateServer();
                    break;
                case CodeHead.PASSWORD:
                    savePassWord(sender, code);
                    break;
                default:
                    break;
            }
            lbl_Message.Text = code.ToString();
        }


        /// <summary>
        /// ִ������¼�
        /// </summary>
        /// <param name="code"></param>
        private void doMouseEvent(Code code)
        {
           MouseEvent mouseCode = code as MouseEvent;
           MouseHook hook = new MouseHook();
            if (mouseCode != null)
            {
                switch (mouseCode.Type)
                {
                    case MouseEventType.MouseMove:
                        hook.MouseWork(mouseCode);
                        break;
                    case MouseEventType.MouseClick:
                        hook.MouseWork(mouseCode);
                        break;
                    default:
                        hook.MouseWork(mouseCode);
                        break;
                }
            }
        }

        /// <summary>
        /// ִ�м����¼�
        /// </summary>
        /// <param name="code"></param>
        private void doKeyBoardEvent(Code code)
        {
            KeyBoardEvent keyboardCode = code as KeyBoardEvent;
            KeyBoardHook hook = new KeyBoardHook();
            if (keyboardCode != null)
            {
                switch (keyboardCode.Type)
                {
                    case KeyBoardType.Key_Down:
                        KeyBoardHook.KeyDown(keyboardCode.KeyCode);
                        break;
                    case KeyBoardType.Key_Up:
                        KeyBoardHook.KeyUp(keyboardCode.KeyCode);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// ��ʾͨѶ����
        /// </summary>
        /// <param name="code"></param>
        private void displayMessage(Code code)
        {            
            DoubleCode contentCode = code as DoubleCode;
            if (contentCode != null)
            {
                switch (code.Head)
                {
                    case CodeHead.SPEAK:
                        showClientMessage(contentCode.Body);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// ��ʾ����Ա����Ϣ
        /// </summary>
        /// <param name="content"></param>
        private void showClientMessage(string content)
        {
            string IP= "����ԱIP";
            string[] record = new string[3] { DateTime.Now.ToString(), IP,"��Ϣ:"+ content };
            ListViewItem item = new ListViewItem(record);
            UpdateListView(item);
            Thread showMessageThread = new Thread(new ParameterizedThreadStart(show));
            showMessageThread.Start(content);
        }

        private void show(object content)
        {
            ShowMessage.ShowMessageForm showMessage = new ShowMessage.ShowMessageForm();
            showMessage.Message = "\t  ����Ա��Ϣ\n   "+content.ToString();
            showMessage.ShowDialog();
        }
        private delegate void ListViewAddEvent(object Item);

        private void ListViewAddItem(object Item)
        {
            ltv_Log.Items.Add((ListViewItem)Item);
        }

        private void UpdateListView(ListViewItem Item)
        {
            ltv_Log.Invoke(new ListViewAddEvent(ListViewAddItem), new object[] { Item });
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        private void sendHostMessage()
        {
            string hostName = ICanSeeYou.Common.Network.GetHostName();
            string hostIP = ICanSeeYou.Common.Network.GetIpAdrress(hostName);
            HostCode code = new HostCode();
            code.Head = CodeHead.HOST_MESSAGE;
            code.Name = hostName;
            code.IP = hostIP;
            mainServer.SendCode(code);
        }

        /// <summary>
        /// ���з�����Ѿ�׼����(�������Ǵ򿪵Ķ˿ڵ����ƶ�)
        /// </summary>
        private void sendReady()
        {
            PortCode readyCode = new PortCode();
            readyCode.Head = CodeHead.SEND_FILE_READY;
            readyCode.Port = Constant.Port_File;
            mainServer.SendCode(readyCode);

            readyCode.Head = CodeHead.SCREEN_READY;
            readyCode.Port = Constant.Port_Screen;
            mainServer.SendCode(readyCode);
        }

        /// <summary>
        /// ���ͷ���˰汾
        /// </summary>
        private void sendVersion()
        {
            DoubleCode versionCode = new DoubleCode();
            versionCode.Head = CodeHead.VERSION;
            versionCode.Body = version;
            mainServer.SendCode(versionCode);            
        }

        /// <summary>
        ///��������������,��������������,������߿��ƶ˸���ʧ��
        /// </summary>
        private void builtUpdateServer()
        {
            string path = Directory.GetCurrentDirectory() + "\\Update.exe";
            //���Update�����Ѿ�����,�ȹر���.
            ServerUpdater.CloseApplication("update");           
            if (!File.Exists(path))
            {
                BaseCode code = new BaseCode();
                code.Head = CodeHead.UPDATE_FAIL;
                mainServer.SendCode(code);
            }
            else
            {
                Thread.Sleep(300);
                //����Update����
                Thread updateThread = new Thread(new ThreadStart(runUpdateApp));
                updateThread.Start();
                //���߿��ƶ�Update�����Ѿ�����.
                Thread.Sleep(100);
                PortCode code = new PortCode();
                code.Head = CodeHead.UPDATE_READY;
                code.Port = Constant.Port_Update;
                mainServer.SendCode(code);
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        private void runUpdateApp()
        {           
            string path =Directory.GetCurrentDirectory() + "\\Update.exe";
            System.Diagnostics.Process.Start(path, productName + ".exe");
        }

        /// <summary>
        /// ���ͱ��ش�����Ϣ
        /// </summary>
        /// <param name="sender"></param>
        private void sendDisks(BaseCommunication sender)
        {
            try
            {
                DisksCode diskscode = ICanSeeYou.Common.IO.GetDisks();
                sender.SendCode(diskscode);
            }
            catch
            {
            }
        }

        /// <summary>
        /// �����ļ����ڵ���Ϣ(��ǰ·���µ��ļ����ļ���)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void sendDirectoryDetial(BaseCommunication sender, Code code)
        {
            DoubleCode tempcode = code as DoubleCode;
            if (tempcode != null)
            {
                if (tempcode.Body != "")
                {
                    ExplorerCode explorer = new ExplorerCode();
                    explorer.Enter(tempcode.Body);
                    sender.SendCode(explorer);
                }
            }
        }

        /// <summary>
        /// �����ļ����ڵ���Ϣ(��ǰ·���µ��ļ����ļ���)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void sendFileDetial(BaseCommunication sender, Code code)
        {
            DoubleCode tempcode = code as DoubleCode;
            if (tempcode != null)
            {
                DoubleCode filedetialcode = new DoubleCode();
                filedetialcode.Head = CodeHead.SEND_FILE_DETIAL;
                filedetialcode.Body=ICanSeeYou.Common.IO.GetFileDetial(tempcode.Body);
                sender.SendCode(filedetialcode);
            }
        }

        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void savePassWord(BaseCommunication sender, Code code)
        {
            DoubleCode tempcode = code as DoubleCode;
            if (tempcode != null)
            {
                if (ICanSeeYou.Configure.PassWord.Save(Constant.PassWordFilename, tempcode.Body))
                {
                    this.exitPassWord = tempcode.Body;
                    BaseCode ok = new BaseCode();
                    ok.Head = CodeHead.CHANGE_PASSWORD_OK;
                    sender.SendCode(ok);
                }
            }
        }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="msg"></param>
        private void displayMessage(string msg)
        {
            lbl_Message.Text = msg;
        }
    }
}
