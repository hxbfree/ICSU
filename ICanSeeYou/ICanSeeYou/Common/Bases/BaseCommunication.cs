/*----------------------------------------------------------------
        // Copyright (C) 2007 L3'Studio
        // ��Ȩ���С� 
        // �����ߣ�L3'Studio�Ŷ�
        // �ļ�����BaseCommunication.cs
        // �ļ���������������ͨѶ�࣬������ֱ�ӵ�������ͨѶʹ�ã���BaseServer��BaseControler�̳С�
//----------------------------------------------------------------*/

using System;
using System.Text;

using System.Net.Sockets;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Forms;

using ICanSeeYou.Common;
using ICanSeeYou.Codes;

namespace ICanSeeYou.Bases
{
    /// <summary>
    /// ִ��ָ���ί��
    /// </summary>
    /// <param name="code">ָ��</param>
    public delegate void ExecuteCodeEvent(BaseCommunication sender, Code code);

    /// <summary>
    /// ����ͨѶ��
    /// </summary>
    public class BaseCommunication
    {
        /// <summary>
        /// ���л�����������
        /// </summary>
        private BinaryFormatter formatterReader;
        /// <summary>
        /// ���л�������д��
        /// </summary>
        private BinaryFormatter formatterWriter;

        /// <summary>
        /// ������
        /// </summary>
        protected NetworkStream stream;
        /// <summary>
        /// �˿�
        /// </summary>
        protected int port;

        /// <summary>
        ///��ȡ�ȴ�����ʱ��
        /// </summary>
        protected virtual int sleepTime
        {
            get { return Constant.SleepTime; }//Ĭ��ֵ
        }

        /// <summary>
        ///�Ƿ�Ͽ�����
        /// </summary>
        public bool Disconnected;
        /// <summary>
        /// �Ƿ��˳�(��������)
        /// </summary>
        public bool Exit;
        /// <summary>
        /// ί��(ִ��ָ��)
        /// </summary>
        public ExecuteCodeEvent Execute;

        /// <summary>
        /// �ͻ��˻��๹�캯��
        /// </summary>
        public BaseCommunication()
        {
            Disconnected =true;
            Exit = false;
            formatterReader = new BinaryFormatter();
            formatterWriter = new BinaryFormatter();
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        protected void communication()
        {
            try
            {
                Disconnected = false;
                do
                {
                    //��ȡ������
                    Code code = (Code)formatterReader.Deserialize(stream);
                    if (code != null)
                    {
                        if (code.Head == CodeHead.CONNECT_CLOSE)
                            Disconnected = true;
                        else if (code.Head == CodeHead.EXIT)
                        {
                            Disconnected = true;
                            Exit = true;
                        }
                        else
                            Execute(this, code);
                    }

                } while (!Disconnected);
            }
            catch// (Exception e)
            {
               // MessageBox.Show(e.ToString());
            }
            Disconnected = true;
        }       

        /// <summary>
        /// ���߶Է����ӳɹ�
        /// </summary>
        protected void SayHello()
        {
            try
            {
                Disconnected = false;
                BaseCode ConnectOK = new BaseCode();
                ConnectOK.Head = CodeHead.CONNECT_OK;
                SendCode(ConnectOK);
            }
            catch
            {
                Disconnected = true;
            }
        }

        /// <summary>
        /// �ر���������
        /// </summary>
        /// <returns></returns>
        public  void CloseStream()
        {
            try
            {
                Disconnected = true;
                if (stream != null)
                    stream.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="code">ָ��</param>
        public void SendCode(Code code)
        {
            try
            {
                formatterWriter.Serialize(stream, code);
            }
            catch// (Exception exp)
            {
               // MessageBox.Show("Error:" + exp.ToString());
            }
        }
    }
}
