using System;
using System.Windows.Forms;
using System.Text;
using System.Threading;

using ICanSeeYou.Bases;
using ICanSeeYou.Codes;

namespace Client
{
    /// <summary>
    /// ��Ļ���ն�
    /// </summary>
    public class ScreenControler:BaseControler
    {
        /// <summary>
        /// ��Ļ���ն��߳�
        /// </summary>
        private Thread screenThread;

        /// <summary>
        /// ��ʾ��Ļ��ͼƬ��
        /// </summary>
        private PictureBox pic_screen;

        /// <summary>
        /// ��ʾ��Ϣ�ı�ǩ
        /// </summary>
        private ToolStripStatusLabel lbl_message;

        /// <summary>
        /// ��ʾ��Ϣ�ı�ǩ
        /// </summary>
        public PictureBox pic_Screen
        {
            get { return pic_screen; }
            set { pic_screen = value; }
        }

        /// <summary>
        /// ��ʾ��Ϣ�ı�ǩ
        /// </summary>
        public ToolStripStatusLabel lbl_Message
        {
            get { return lbl_message; }
            set { lbl_message = value; }
        }

        /// <summary>
        /// ����һ����Ļ���ն˵�ʵ��
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="port"></param>
        public ScreenControler(System.Net.IPAddress serverAddress,int port):base(serverAddress,port)
        {
            base.Execute = new ExecuteCodeEvent(screenExecuteCode);
        }

        /// <summary>
        /// ִ��ָ��
        /// </summary>
        /// <param name="code"></param>
        private void screenExecuteCode(BaseCommunication sender, Code code)
        {
            switch (code.Head)
            {
                case CodeHead.CONNECT_OK:
                    DisplayMessage("��Ļ���ӳɹ�!");
                    break;
                //case CodeHead.SCREEN_READY:
                //    DisplayMessage("��Ļ��ȡ׼�����!");
                //    break;
                case CodeHead.SCREEN_SUCCESS:
                    //������Ļ�ɹ�
                    screenShow((SendScreenCode)code);
                    break;
                case CodeHead.SCREEN_FAIL:
                    //����˽�����������
                    MessageBox.Show("�޷���ȡ��Ļ!");
                    CloseConnections();
                    break;
                case CodeHead.CONNECT_CLOSE:
                    //�ر�����
                    CloseConnections();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ��ʾ��Ļ
        /// </summary>
        /// <param name="code"></param>
        private void screenShow(SendScreenCode code)
        {
            pic_screen.Image = code.ScreenImage;
        }

        /// <summary>
        ///  ����Ļ���ն�
        /// </summary>
        public void Open()
        {
            if (screenThread != null && screenThread.IsAlive)
            {
                DialogResult result = MessageBox.Show("��ǰ�ļ��߳�û�ر�!�Ƿ�ر�?", "�ر��߳�", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;
            }
            CloseConnections();
            ThreadStart threadStart = new ThreadStart(base.Connecting);
            threadStart += new ThreadStart(base.Run);
            screenThread = new Thread(threadStart);
            screenThread.Start();
        }


        /// <summary>
        /// ���������˷��ͽ�������
        /// </summary>
        public bool GetScreen()
        {
            if (!base.Disconnected)
            {
                BaseCode code = new BaseCode();
                code.Head = CodeHead.SCREEN_GET;
                base.SendCode(code);
                return true;
            }
            else
                return false;
        }       

        /// <summary>
        /// �ر�����
        /// </summary>
        public override void CloseConnections()
        {
            base.CloseConnections();
            if (screenThread != null && screenThread.IsAlive)
                screenThread.Abort();
        }

        /// <summary>
        /// ��ʾ���յ���Ϣ
        /// </summary>
        /// <param name="msg"></param>
        public void DisplayMessage(string msg)
        {
            lbl_message.Text = msg;
        }
    }
}
