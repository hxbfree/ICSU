using System;

namespace ICanSeeYou.Codes
{
    /// <summary>
    /// "所有磁盘"指令类(作为序列化指令在网络上传输)
    /// </summary>
    [Serializable]
    public class DisksCode : BaseCode
    {
        private DiskStruct[] disks;
        /// <summary>
        /// 磁盘数组
        /// </summary>
        public DiskStruct[] Disks
        {
            get { return disks; }
            set { disks = value; }
        }

        public DisksCode() { base.Head = CodeHead.SEND_DISKS; }
    }
}
