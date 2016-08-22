/*----------------------------------------------------------------
        // Copyright (C) 2007 L3'Studio
        // ��Ȩ���С� 
        // �����ߣ�L3'Studio�Ŷ�
        // �ļ�����BaseServer.cs
        // �ļ��������������������ࣨ������ˣ������н������ӣ��Ͽ����Ӻͻ���ͨѶ����
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using ICanSeeYou.Bases;

namespace Server
{
    /// <summary>
    /// ����������
    /// </summary>
    public class BaseServer:BaseCommunication
    {
        /// <summary>
        /// ����
        /// </summary>
        private Socket connection;
        /// <summary>
        /// �������������
        /// </summary>
        private TcpListener listener;

        #region ��Ϊ�Ժ���չ������
        //private int startPort;

        /// <summary>
        /// �߳��Ƿ񱻹���
        /// </summary>
        //private bool threadSuspended = true;

        #endregion

        /// <summary>
        /// ����˹��캯��
        /// </summary>
        /// <param name="con"></param>
        /// <param name="poct"></param>
        public BaseServer(int port)
            : base()
        {
            base.port = port ;
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Run()
        {
            CloseConnections();
            listener = new TcpListener(System.Net.IPAddress.Any, base.port);
            listener.Start();//�ȴ�����...
            do
            {
                base.Disconnected = false;
                try
                {
                    connection = listener.AcceptSocket();
                    base.stream = new NetworkStream(connection);
                    base.SayHello();
                }
                catch //(Exception exp)
                {
                    //  MessageBox.Show(exp.ToString());
                }
                //��ʼ����ͨѶ
                base.communication();
                if (base.Disconnected)
                    CloseConnections();
            } while (!base.Exit);

        }

        /// <summary>
        /// �ر���������
        /// </summary>
        /// <returns></returns>
        public  void CloseConnections()
        {
            base.CloseStream();
            try
            {
                if (connection != null)
                    connection.Close();
            }
            catch
            {
            }
        }


    }
}
