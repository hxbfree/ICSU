
using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// ˫ָ��
    /// </summary>
    [Serializable]
    public class DoubleCode : BaseCode
    {
        private string body;
        /// <summary>
        /// ָ������
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public override string ToString()
        {
            return body;
        }
    }
}
