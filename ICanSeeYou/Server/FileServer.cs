using System;
using System.Collections.Generic;
using System.Text;

using ICanSeeYou.Bases;
using ICanSeeYou.Codes;

namespace Server
{
    /// <summary>
    /// �ļ������
    /// </summary>
    public class FileServer:BaseServer
    {
        /// <summary>
        /// ����һ���ļ������ʵ��
        /// </summary>
        /// <param name="port">�ļ�����˿�</param>
        public FileServer(int port)
            : base(port)
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
                    //displayMessage("׼�������ļ�...");
                    // ICanSeeYou.Common.IO.UploadFile(sender,uploadFile);                    
                    break;
                case CodeHead.SEND_FILE:
                   // displayMessage("�����ļ�.");
                    ICanSeeYou.Common.IO.SaveFile(sender, (FileCode)code);
                    ICanSeeYou.Common.IO.EndTranFile(sender);
                    break;
                case CodeHead.GET_FILE:
                   // displayMessage("�ϴ��ļ�...");
                    ICanSeeYou.Common.IO.UploadFile(sender, (FileCode)code);
                    break;
                case CodeHead.FILE_TRAN_END:
                    ICanSeeYou.Common.IO.EndTranFile(sender);
                   // displayMessage("�����ļ����.");
                    break;
                default:
                    break;
            }
        }
    }
}
