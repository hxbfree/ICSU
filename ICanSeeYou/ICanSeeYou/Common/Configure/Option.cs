using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ICanSeeYou.Configure
{
    /// <summary>
    /// ������
    /// </summary>
    public class Option
    {
        private OptionFile file;
        /// <summary>
        /// �ͻ��˵������ļ�
        /// </summary>
        public OptionFile OptFile
        {
            get { return file; }
            set { file = value; }
        }

        private string fileName;
        public Option() : this(ICanSeeYou.Common.Constant.OptionFilename) { }
        public Option(string fileName)
        {
            this.fileName = fileName;
            file = new OptionFile();
            Read();
        }

        public Option(string fileName, string updatedFile, string updatedVersion)
            : this(fileName)
        {
            file.UpdatedFile = updatedFile;
            file.UpdatedVersion = updatedVersion;

        }
        public Option(string fileName, string password)
            : this(fileName)
        {
            file.PassWord = password;
        }
        public Option(string fileName, string password, string updatedFile, string updatedVersion)
        {
            file = new OptionFile();
            file.PassWord = password;
            file.UpdatedFile = updatedFile;
            file.UpdatedVersion = updatedVersion;

        }
        /// <summary>
        /// д�������ļ�
        /// </summary>
        public bool Write()
        {
            FileStream stream = null;
            try
            {

                if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, file);
                if (stream != null)
                    stream.Close();
            }
            catch
            {
                if (stream != null)
                    stream.Close();
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ȡ�����ļ�
        /// </summary>
        public bool Read()
        {
            if (!System.IO.File.Exists(fileName)) return false;
            FileStream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                file = (OptionFile)formatter.Deserialize(stream);
                if (stream != null)
                    stream.Close();
            }
            catch
            {
                if (stream != null)
                    stream.Close();
                return false;
            }
            return true;
        }
    }

  
}
