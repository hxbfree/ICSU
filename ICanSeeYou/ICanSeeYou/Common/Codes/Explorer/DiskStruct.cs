using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// ���̽ṹ(��Ϊ���л�ָ���������ϴ���)
    /// </summary>
    [Serializable]
    public class DiskStruct : FileStruct
    {
        /// <summary>
        /// ���̱�־
        /// </summary>
        public override FileFlag Flag
        {
            get { return FileFlag.Disk; }
        }
        public DiskStruct(string name) : base(name) { }
    }
}