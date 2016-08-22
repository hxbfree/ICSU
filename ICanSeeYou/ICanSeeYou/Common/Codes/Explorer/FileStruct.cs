using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// 文件结构(作为序列化指令在网络上传输)
    /// </summary>
    [Serializable]
    public class FileStruct : BaseFile
    {
        private string name;
        /// <summary>
        /// 文件标志
        /// </summary>
        public override FileFlag Flag
        {
            get
            {
                return FileFlag.File;
            }
        }
        /// <summary>
        /// 全名
        /// </summary>
        public override string Name
        {
            get { return name; }
        }
        public FileStruct(string name) { this.name = name; }

        public override string ToString()
        {
            return name;
        }

    }
}
