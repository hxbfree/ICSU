

using System;

using System.Net;
using System.Net.Sockets;

namespace ICanSeeYou.Common
{
    /// <summary>
    /// ��ȡ����ĳЩ��Ϣ����
    /// </summary>
    public class Network
    {
        /// <summary>
        /// ��ȡIP��ַ
        /// </summary>
        /// <param name="hostname">������</param>
        /// <returns></returns>
        public static string GetIpAdrress(string hostname)
        {
            string ip;
            try
            {
                IPHostEntry iphe = Dns.GetHostEntry(hostname);
                foreach (IPAddress address in iphe.AddressList)
                {
                    ip = address.ToString();
                    if (ip != "") return ip;
                }
            }
            catch
            {
            }
            return "";
        }

        /// <summary>
        /// ��ȡ���ؼ������
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// �ַ���ʽ��IP��ַת��ΪIPAddressʵ��
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(string IP)
        {
            return  IPAddress.Parse(IP);
        }

        /// <summary>
        /// byte������ʽ��IP��ַת��ΪIPAddressʵ��
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(byte[] IP)
        {
            return new IPAddress(IP);
        }

        /// <summary>
        /// �ֿ�IP��ַΪbyte������ʽ
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static byte[] SplitIP(string ip)
        {
            byte[] IP=new byte[4];
            string []splitIp=ip.Split(new char[]{'.'});
            if(splitIp.Length!=4)return null;
            try
            {
                for (int i = 0; i < 4; i++)
                    IP[i] =byte.Parse(splitIp[i]);
            }
            catch
            {
                return null;
            }
            return IP;
        }
    }
}
