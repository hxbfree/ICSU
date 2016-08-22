using System;

namespace ICanSeeYou.Configure
{
    /// <summary>
    /// 密码文件结构(保存已经经过加密后的密码)
    /// </summary>
    [Serializable]
    public class PassWordFile
    {
        private string passWord;
        /// <summary>
        /// 经过加密后的密码
        /// </summary>
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
    }
}
