using System;

namespace ICanSeeYou.Configure
{
    /// <summary>
    /// �����ļ��ṹ(�����Ѿ��������ܺ������)
    /// </summary>
    [Serializable]
    public class PassWordFile
    {
        private string passWord;
        /// <summary>
        /// �������ܺ������
        /// </summary>
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
    }
}
