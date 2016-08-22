
using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// ������Ϣ�ṹ
    /// </summary>
    [Serializable]
    public class HostCode : BaseCode
    {
        private string ip;
        private string name;

        /// <summary>
        /// ������IP��ַ
        /// </summary>
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ����ToString�������
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return IP + "(" + Name + ")";
        }
    }
}