using System;
using System.Threading;
using System.Windows.Forms;

using ICanSeeYou.Bases;
using ICanSeeYou.Codes;
using ICanSeeYou.Common;

namespace Client
{
    /// <summary>
    /// �����ļ���ͼ��ί��
    /// </summary>
    /// <param name="IsServer">�Ƿ����˵��ļ���ͼ</param>
   public delegate void RefrushEvent(bool IsServer);

    /// <summary>
    /// �ļ�������
    /// </summary>
    public class FileControler : BaseControler
    {
        /// <summary>
        /// �ļ����ƶ��߳�
        /// </summary>
        private Thread fileThread;

        private string sourceFile;
        private string destinationFile;        
        private bool isDownload;

        /// <summary>
        /// �����ļ���ͼ�¼�
        /// </summary>
        private RefrushEvent refrush;


        /// <summary>
        /// ֵΪtrueʱ��ʾ����,false��ʾ�ϴ�
        /// </summary>
        public bool IsDownload
        {
            get { return isDownload; }
            set { isDownload = value; }
        }

        /// <summary>
        /// ԭ�ļ�
        /// </summary>
        public string SourceFile
        {
            get { return sourceFile; }
            set { sourceFile = value; }
        }

        /// <summary>
        /// Ŀ���ļ�
        /// </summary>
        public string DestinationFile
        {
            get { return destinationFile; }
            set { destinationFile = value; }
        }

        /// <summary>
        /// �����ļ���ͼ�¼�
        /// </summary>
        public RefrushEvent Refrush
        {
            get { return refrush; }
            set { refrush = value; }
        }

        /// <summary>
        /// ����һ���ļ����ƶ˵�ʵ��
        /// </summary>
        /// <param name="serverAddress">�ļ������IP</param>
        /// <param name="port">�˿�</param>
        public FileControler(System.Net.IPAddress serverAddress, int port)
            : base(serverAddress, port)
        {
            base.Execute = new ExecuteCodeEvent(fileExecuteCode);
        }

        /// <summary>
        /// ִ��ָ��
        /// </summary>
        /// <param name="msg">ָ��</param>
        private void fileExecuteCode(BaseCommunication sender, Code code)
        {
            switch (code.Head)
            {
                case CodeHead.CONNECT_OK:
                    if (isDownload)
                        IO.DownloadFile(sender, sourceFile, destinationFile);
                    else
                        IO.ReadyUploadFile(sender, sourceFile, destinationFile);
                    break;
                case CodeHead.SEND_FILE:
                    IO.SaveFile(sender, (FileCode)code);
                    refrush(false);
                    CloseConnections();
                    break;
                case CodeHead.GET_FILE:
                  //  FileManager.UploadFile(sender, (FileStructCode)code);
                    break;
                case CodeHead.FILE_TRAN_END:
                    refrush(true);
                    CloseConnections();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///  ���ļ����ƶ�
        /// </summary>
        public void Open()
        {
            if (fileThread != null && fileThread.IsAlive)
            {
               DialogResult result= MessageBox.Show("��ǰ�ļ��߳�û�ر�!�Ƿ�ر�?","�ر��߳�",MessageBoxButtons.YesNo);
               if (result == DialogResult.No) 
                   return;
            }
            CloseConnections();
            ThreadStart threadStart = new ThreadStart(base.Connecting);
            threadStart += new ThreadStart(base.Run);
            fileThread = new Thread(threadStart);
            fileThread.Start();
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        public override void CloseConnections()
        {
            base.CloseConnections();
            if (fileThread != null && fileThread.IsAlive)
                fileThread.Abort();
        }
    }
}

