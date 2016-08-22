

using System;

namespace ICanSeeYou.Codes
{   
    /// <summary>
    /// "�ҵĵ���"ָ����(��Ϊ���л�ָ���������ϴ���)
    /// </summary>
    [Serializable]
    public class ExplorerCode:BaseCode
    {
        private string path;
        private bool available;
        private DirectoryStruct[] directorys;
        private FileStruct[] files;

        /// <summary>
        /// ��ǰ·����Ч
        /// </summary>
        public bool Available
        {
            get { return available; }
        }

        /// <summary>
        /// �ļ�������
        /// </summary>
        public DirectoryStruct[] Directorys
        {
            get { return directorys; }
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        public FileStruct[] Files
        {
            get { return files; }
        }

        /// <summary>
        /// ��ǰ·��
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// ����"�ҵĵ���"ָ�����ʵ��
        /// </summary>
        /// <param name="curPath"></param>
        public ExplorerCode()
        {
            base.Head = CodeHead.SEND_DIRECTORY_DETIAL;
        }
        /// <summary>
        /// ����ָ���ļ���
        /// </summary>
        /// <param name="path">�ļ���·��</param>
        public void Enter(string curPath)
        {
            path = curPath;
            try
            {
                if (!System.IO.Directory.Exists(curPath))
                { //��ǰ·����Ч
                    available = false;
                    return;
                }
                //��ȡ��ǰ·���������ļ���
                string[] directoryArray = System.IO.Directory.GetDirectories(curPath);
                directorys = new DirectoryStruct[directoryArray.Length]; 
                for(int i=0;i<directoryArray.Length;i++)
                    directorys[i] = new DirectoryStruct(directoryArray[i]);
                //��ȡ��ǰ·���������ļ�
                string[] fileArray = System.IO.Directory.GetFiles(curPath);
                files = new FileStruct[fileArray.Length];
                for (int i = 0; i < files.Length; i++)
                    files[i] = new FileStruct(fileArray[i]);
                available = true;
            }
            catch 
            {
                available = false;
            }
        }        
    }
}