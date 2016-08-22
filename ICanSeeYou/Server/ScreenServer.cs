using System;
using System.Drawing;

using ICanSeeYou.Bases;
using ICanSeeYou.Codes;
using ICanSeeYou.Windows;

namespace Server
{
    /// <summary>
    /// ��Ļ���Ͷ�
    /// </summary>
    public class ScreenServer:BaseServer
    {      
        /// <summary>
        /// ������Ļ���Ͷ�ʵ��
        /// </summary>
        /// <param name="port">��Ļ������ʹ�õĶ˿�</param>
        public ScreenServer(int port) : base(port)
        {
            base.Execute = new ExecuteCodeEvent(screenExecuteCode);
        }

        /// <summary>
        /// ִ����Ļָ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void screenExecuteCode(BaseCommunication sender, Code code)
        {
            switch (code.Head)
            {
                case CodeHead.CONNECT_OK:
                case CodeHead.SCREEN_GET:
                    //������Ļ�����ض�
                    SendScreen();
                    break;
                case CodeHead.SCREEN_CLOSE:
                    base.CloseConnections();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ��ȡ��Ļ������
        /// </summary>
        private void SendScreen()
        {
            SendScreenCode code = new SendScreenCode();
            code.ScreenImage = ScreenCapture.Capture();
            if(code.ScreenImage==null)
            {//���ܷ�����Ļ
                BaseCode failcode = new BaseCode();
                failcode.Head = CodeHead.FAIL;
                base.SendCode(failcode);
            }
            else
                base.SendCode(code);
        }
    }
}
