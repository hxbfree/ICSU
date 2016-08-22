using System;
using System.Text;

namespace ICanSeeYou.Common
{
    /// <summary>
    /// ������(����Ĭ��ֵ)
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// �ȴ�����ʱ��
        /// </summary>
        public const int SleepTime = 1000;
        /// <summary>
        /// ������Դ���
        /// </summary>
        public const int MaxTimes = 100;
        /// <summary>
        /// Ĭ����ҪͨѶ�˿�
        /// </summary>
        public const int Port_Main = 5566;
        /// <summary>
        /// Ĭ���ļ����Ͷ˿�
        /// </summary>
        public const int Port_File = 6566;
        /// <summary>
        /// Ĭ����Ļ���Ͷ˿�
        /// </summary>
        public const int Port_Screen = 7566;
        /// <summary>
        /// ��������Ķ˿�
        /// </summary>
        public const int Port_Update = 8566;
        /// <summary>
        /// Ĭ����Ļ��С
        /// </summary>
        public static System.Drawing.Size ScreenSize = new System.Drawing.Size(1024, 768);
        /// <summary>
        /// ��һ��
        /// </summary>
        public const string ParentPath = "��һ��";
        /// <summary>
        /// �����ܵ��ļ���չ��
        /// </summary>
        public const string UnknowFileType = "/";
        /// <summary>
        /// ����˵������ļ���
        /// </summary>
        public static string PassWordFilename = System.IO.Directory.GetCurrentDirectory()+"\\INCUpwd.lll";
        /// <summary>
        /// �ͻ��˵������ļ���
        /// </summary>
        public static string OptionFilename = System.IO.Directory.GetCurrentDirectory() + "\\INCUopt.lll";
        /// <summary>
        /// �ͻ��˵İ����ļ���
        /// </summary>
        public static string HelpFilename = System.IO.Directory.GetCurrentDirectory() + "\\INCU.chm";
    }
}
