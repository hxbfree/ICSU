

using System;
using System.Runtime.InteropServices;

using ICanSeeYou.API;

namespace ICanSeeYou.Hooks
{   
    /// <summary>
    /// ���̿���
    /// </summary>
    public class KeyBoardHook
    {
        /// <summary>
        /// ���°����Ĳ���
        /// </summary>
        private const int KEYEVENTF_KEYDOWN = 0x0001;

        /// <summary>
        /// �ͷŰ����Ĳ���
        /// </summary>
        private const int KEYEVENTF_KEYUP = 0x0002;

        /// <summary>
        /// ģ������¼�-���°���
        /// </summary>
        /// <param name="keyCode"></param>
        public static void KeyDown(System.Windows.Forms.Keys keyCode)
        {
            Api.keybd_event((byte)keyCode, 0, KEYEVENTF_KEYDOWN, 0);
        }

        /// <summary>
        /// ģ������¼�-�ͷŰ���
        /// </summary>
        /// <param name="keyCode"></param>
        public static void KeyUp(System.Windows.Forms.Keys keyCode)
        {
            Api.keybd_event((byte)keyCode, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
