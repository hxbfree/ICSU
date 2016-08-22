
using ICanSeeYou.Common;

namespace ICanSeeYou.Configure
{
    /// <summary>
    /// �����ļ�������
    /// </summary>
    public class OptionManager
    {
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="pwd">����</param>
        /// <returns></returns>
        public static bool ChangePassWord(string pwd)
        {
            Option option = new Option(Constant.OptionFilename, pwd);
            return option.Write();
        }
        /// <summary>
        /// �޸ĸ����ļ���·��
        /// </summary>
        /// <param name="fileName">�ļ�</param>
        /// <param name="version">�汾</param>
        /// <returns></returns>
        public static bool ChangeUpdatedFile(string fileName, string version)
        {
            Option option = new Option(Constant.OptionFilename, fileName, version);
            return option.Write();
        }
        /// <summary>
        /// �޸������ļ�
        /// </summary>
        /// <param name="pwd">����</param>
        /// <param name="fileName">�ļ�</param>
        /// <param name="version">�汾</param>
        /// <returns></returns>
        public static bool Change(string pwd, string fileName, string version)
        {
            Option option = new Option(Constant.OptionFilename, pwd, fileName, version);
            return option.Write();
        }
    }
}
