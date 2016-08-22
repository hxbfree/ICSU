using System;

namespace ICanSeeYou.Configure
{
    /// <summary>
    /// �ͻ��˵������ļ�
    /// </summary>
    [Serializable]
    public class OptionFile : PassWordFile
    {
        private string updatedFile;
        /// <summary>
        /// ����˸����ļ�
        /// </summary>
        public string UpdatedFile
        {
            get { return updatedFile; }
            set { updatedFile = value; }
        }
        private string updatedVersion;
        /// <summary>
        /// ����˸����ļ��İ汾
        /// </summary>
        public string UpdatedVersion
        {
            get { return updatedVersion; }
            set { updatedVersion = value; }
        }
    }
}
