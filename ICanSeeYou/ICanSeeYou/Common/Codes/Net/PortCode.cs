
using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// 用来获取服务端和客户端文件/屏幕的通讯端口
    /// </summary>
    [Serializable]
    public class PortCode : BaseCode
    {
        private int port;
        /// <summary>
        /// 通讯端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
