using System;
using System.IO;
using System.Diagnostics;

using ICanSeeYou.Codes;
using ICanSeeYou.Bases;

namespace  Server
{
    /// <summary>
    /// �ر��¼�ί��
    /// </summary>
    public delegate void CloseMeEvent();

    /// <summary>
    /// ����˸�����
    /// </summary>
    public class ServerUpdater : BaseServer
    {
        /// <summary>
        /// ��ʱ�ļ�
        /// </summary>
        private string tempFile;

        /// <summary>
        /// ����˳�����
        /// </summary>
        private string appName;

        /// <summary>
        /// �رճ����ί��
        /// </summary>
        private CloseMeEvent close;

        /// <summary>
        /// �رճ����ί��
        /// </summary>
        public CloseMeEvent Close
        {
            get { return close; }
            set { close = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        /// <summary>
        /// ����һ���ļ������ʵ��
        /// </summary>
        /// <param name="port">�ļ�����˿�</param>
        public ServerUpdater(int port)
            : base(port)
        {
            base.Execute = new ExecuteCodeEvent(updaterExecuteCode);
        }

        /// <summary>
        /// ִ��ָ��
        /// </summary>
        /// <param name="msg">ָ��</param>
        private void updaterExecuteCode(BaseCommunication sender, Code code)
        {
            switch (code.Head)
            {
                case CodeHead.SEND_FILE:
                    //���·����
                    UpdateApp(sender, code);
                    break;
                case CodeHead .FILE_TRAN_END:
                    CloseMe(sender);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ��ȡϵͳ��ʱ�ļ���
        /// </summary>
        /// <returns></returns>
        private string GetSystemTempDir()
        {
            return "";// "%temp%\\";
        }

        /// <summary>
        ///��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void UpdateApp(BaseCommunication sender, Code code)
        {
            FileCode fileCode = code as FileCode;
            if (fileCode != null)
            {
                string fileName=fileCode.SavePath;
                tempFile=GetSystemTempDir() +fileName + ".tmp";
                System.Console.WriteLine("���ظ��°�:" + tempFile);
                fileCode.SavePath = tempFile;
                ICanSeeYou.Common.IO.SaveFile(sender, fileCode);
                int index = appName.IndexOf(".");
                //��ȡ������߳���
                string process=appName.Substring(0, index );
                System.Console.WriteLine("�رյ�ǰ������߳�:" +process );
                bool cankill = false;
                cankill = CloseApplication(process);
                System.Console.WriteLine("���ڹرշ�����߳�...");
                System.Threading.Thread.Sleep(500);
                if (cankill)
                {
                    string savedFile = Directory.GetCurrentDirectory() + "\\" + fileName;
                    System.Console.WriteLine("�����ļ�:" + savedFile);
                    Updatefile(tempFile, savedFile);
                    System.Console.WriteLine("������������˳���:" + Directory.GetCurrentDirectory() + "\\" + appName);
                    restart(Directory.GetCurrentDirectory() + "\\" + appName);
                }
                else
                    System.Console.WriteLine("�޷��رյͰ汾�ķ���˳���!");
                System.Console.WriteLine("�ر���������!");
                CloseMe(sender);
            }
        }

        /// <summary>
        /// �ر��������
        /// </summary>
        /// <param name="sender"></param>
        private void CloseMe(BaseCommunication sender)
        {
            BaseCode code = new BaseCode();
            code.Head = CodeHead.FILE_TRAN_END;
            sender.SendCode(code);
        }

        /// <summary>
        /// �ر�ָ������
        /// </summary>
        /// <param name="fileName">���������</param>
        /// <returns></returns>
        public static bool CloseApplication(string process)
        {
            try
            {
                Process[] localByName = Process.GetProcessesByName(process);
                //��ѭ���ķ�ʽ�ؽ���
                foreach (Process proc in localByName)
                {
                    proc.WaitForExit(100);
                    proc.Kill();
                }
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// ���³���,����Ĳ���Ϊ���������·����ԭ�������·��
        /// </summary>
        private void Updatefile(string tempfile, string Tofile)
        {
            if (File.Exists(Tofile))
                File.Delete(Tofile);
            File.Copy(tempfile, Tofile);
            if (File.Exists(tempfile))
                File.Delete(tempfile);
        }
        /// <summary>
        /// ��������������,����Ĳ���Ϊ���������
        /// </summary>
        private void restart(string excuteName)
        {
            try
            {
                System.Diagnostics.Process.Start(excuteName);
            }
            catch
            {
            }
        }
    }
}
