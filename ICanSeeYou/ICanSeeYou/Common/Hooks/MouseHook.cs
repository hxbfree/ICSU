using System;
using System.Text;
using System.Runtime.InteropServices;

using ICanSeeYou.API;
using ICanSeeYou.Codes;

namespace ICanSeeYou.Hooks
{  
    /// <summary>
    /// ���Hook��
    /// </summary>
    public class MouseHook
    {   
        /// <summary>
        /// ����¼�ö��
        /// </summary>
        public enum MouseEventFlag
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }
        /// <summary>
        /// ί��-��갴������
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        public delegate void DoMouseButtons(int flags, int dx, int dy, int dwData, int dwExtraInfo);
        /// <summary>
        /// ί��-����ƶ�����
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public delegate bool DoMouseMove(int X, int Y);
        /// <summary>
        /// ģ����갴ť���µ��¼�
        /// </summary>
        private event DoMouseButtons MouseButton;
        /// <summary>
        /// ģ������ƶ����¼�
        /// </summary>
        private event DoMouseMove MouseMove;

        /// <summary>
        /// ������깳�ӵ�ʵ��
        /// </summary>
        public MouseHook()
        {
            MouseButton += new DoMouseButtons(Api.mouse_event);
            MouseMove += new DoMouseMove(Api.SetCursorPos);
        }

        /// <summary>
        /// �������ִ����Ӧ����
        /// </summary>
        /// <param name="MEvent">ָ��������¼�</param>
        public void MouseWork(MouseEvent MEvent)
        {

            switch (MEvent.Type)
            {
                case MouseEventType.MouseMove:
                    MouseMove(MEvent.X, MEvent.Y);
                    break;
                case MouseEventType.MouseLeftDown:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseLeftUp:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseRightDown:
                    MouseButton((int)MouseEventFlag.RightDown, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseRightUp:
                    MouseButton((int)MouseEventFlag.RightUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseClick:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseDoubleClick:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
            }
        }
    }
}
