/*----------------------------------------------------------------
        // Copyright (C) 2007 L3'Studio
        // ��Ȩ���С� 
        // �����ߣ�L3'Studio�Ŷ�
        // �ļ�����BaseControler.cs
        // �ļ��������������������ࣨ���ͻ��ˣ������н������ӣ��Ͽ����Ӻͻ���ͨѶ����
//----------------------------------------------------------------*/

using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Forms;

using ICanSeeYou.Bases;
using ICanSeeYou.Common;

namespace Client
{
    /// <summary>
    /// ����������
    /// </summary>
    public class BaseControler : BaseCommunication
    {
        /// <summary>
        /// Tcp�ͻ���
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// �����IP
        /// </summary>
        private IPAddress serverAddress;
        /// <summary>
        /// �����IP
        /// </summary>
        public IPAddress ServerAddress
        {
            get { return serverAddress; }
            set { serverAddress = value; }
        }

        private bool haveConnected;
        /// <summary>
        /// �Ƿ��Ѿ���������
        /// </summary>
        public bool HaveConnected
        {
            get
            {
                return haveConnected;
            }

        }

        //startPort��Ϊ�Ժ���չ������
        // private int startPort;

        private int maxTimes;
        /// <summary>
        /// ������Դ���
        /// </summary>
        public int MaxTimes
        {
            get { return maxTimes; }
            set { maxTimes = value; }
        }
        /// <summary>
        ///�ȴ�����ʱ��
        /// </summary>
        protected override int sleepTime
        {
            get
            {
                return base.sleepTime;
            }
        }

        /// <summary>
        /// ���ƶ˵Ĺ��캯��(Ĭ�����Դ���Ϊ����:Constant.MaxTimes)
        /// </summary>
        /// <param name="serverAddress">�����IP��ַ</param>
        /// <param name="port">ͨѶ�˿�</param>
        public BaseControler(IPAddress serverAddress, int port)
            : this(serverAddress, port, Constant.MaxTimes)
        {
        }
        /// <summary>
        /// ���ƶ˵Ĺ��캯��
        /// </summary>
        /// <param name="serverAddress">�����IP��ַ</param>
        /// <param name="port">ͨѶ�˿�</param>
        ///<param name="maxTimes">���Դ���</param>
        public BaseControler(IPAddress serverAddress, int port, int maxTimes)
            : base()
        {
            this.serverAddress = serverAddress;
            base.port = port;
          //  this.startPort = port;
            this.maxTimes = maxTimes;
            haveConnected = false;
        }

        /// <summary>
        /// �������ӷ���� 
        /// </summary>
        /// <returns>�ܷ�����</returns>
        public void Connecting()
        {
            haveConnected = false;
            base.Disconnected = true;
            //base.port = startPort;
            int timeNum = 0;
            while (!connectSucceed())
            {
                Thread.Sleep(sleepTime);
                if (++timeNum > maxTimes)
                {
                    //if (++base.port > IPEndPoint.MaxPort)
                    //{
                    MessageBox.Show("�޷�����" + serverAddress + "!");
                    CloseConnections();
                    return;
                    //}
                }
            }
            try
            {
                stream = client.GetStream();
                haveConnected = true;
            }
            catch
            {
                MessageBox.Show("�޷���ȡ������!");
                haveConnected = false;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Run()
        {
            try
            {
                Disconnected = false;
                if (haveConnected)
                    base.communication();

            }
            catch //(Exception exp)
            {
                CloseConnections();
               // MessageBox.Show( exp.ToString());
            }
            Disconnected = true;
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        public virtual  void CloseConnections()
        {
            try
            {
                base.CloseStream();
                haveConnected = false;
                if (client != null)
                    client.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        ///�����Ƿ�ɹ�(�����Դ��������涨�������ͱ�ʾ����ʧ��)
        /// </summary>
        /// <returns></returns>
        private bool connectSucceed()
        {
            try
            {
                client = new TcpClient();
                client.Connect(serverAddress,base.port);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
