using System;

namespace ICanSeeYou.Configure
{
    /// <summary>
    /// 客户端的配置文件
    /// </summary>
    [Serializable]
    public class OptionFile : PassWordFile
    {
        private string updatedFile;
        /// <summary>
        /// 服务端更新文件
        /// </summary>
        public string UpdatedFile
        {
            get { return updatedFile; }
            set { updatedFile = value; }
        }
        private string updatedVersion;
        /// <summary>
        /// 服务端更新文件的版本
        /// </summary>
        public string UpdatedVersion
        {
            get { return updatedVersion; }
            set { updatedVersion = value; }
        }
    }
}
