

using System;
using System.Runtime.InteropServices;

namespace ICanSeeYou.API
{
    /// <summary>
    /// API��
    /// </summary>
    public class Api
    {
        /// <summary>
        /// ģ������¼��ĺ���ģ��
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void mouse_event(int flags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// ���ù�굽ָ��λ�õĺ���ģ��
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetCursorPos(int X, int Y);

        /// <summary>
        /// ģ������¼��ĺ���ģ��
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "keybd_event")]
        public static extern void keybd_event(
            byte bVk,
            byte bScan,
            int dwFlags,
            int dwExtraInfo
        );
    }
}
