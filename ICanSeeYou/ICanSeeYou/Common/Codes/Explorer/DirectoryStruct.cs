using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// 文件夹结构(作为序列化指令在网络上传输)
    /// </summary>
    [Serializable]
    public class DirectoryStruct : FileStruct
    {
        /// <summary>
        /// 文件夹标志
        /// </summary>
        public override FileFlag Flag
        {
            get { return FileFlag.Directory; }
        }
        public DirectoryStruct(string name) : base(name) { }
    }
}
