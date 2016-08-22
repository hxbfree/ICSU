using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// �ļ��нṹ(��Ϊ���л�ָ���������ϴ���)
    /// </summary>
    [Serializable]
    public class DirectoryStruct : FileStruct
    {
        /// <summary>
        /// �ļ��б�־
        /// </summary>
        public override FileFlag Flag
        {
            get { return FileFlag.Directory; }
        }
        public DirectoryStruct(string name) : base(name) { }
    }
}
