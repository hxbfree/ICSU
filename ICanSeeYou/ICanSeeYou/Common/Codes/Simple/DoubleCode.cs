
using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// 双指令
    /// </summary>
    [Serializable]
    public class DoubleCode : BaseCode
    {
        private string body;
        /// <summary>
        /// 指令身体
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
