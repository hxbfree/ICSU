using System;
using System.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace ICanSeeYou.Windows
{
    /// <summary>
    /// ��Ļ������
    /// </summary>
    public class ScreenCapture
    {
        /// <summary>
        /// �ѵ�ǰ��Ļ����λͼ������
        /// </summary>
        /// <param name="hdcDest">Ŀ���豸�ľ��</param>
        /// <param name="nXDest">Ŀ���������Ͻǵ�X����</param>
        /// <param name="nYDest">Ŀ���������Ͻǵ�X����</param>
        /// <param name="nWidth">Ŀ�����ľ��εĿ��</param>
        /// <param name="nHeight">Ŀ�����ľ��εĳ���</param>
        /// <param name="hdcSrc">Դ�豸�ľ��</param>
        /// <param name="nXSrc">Դ��������Ͻǵ�X����</param>
        /// <param name="nYSrc">Դ��������Ͻǵ�X����</param>
        /// <param name="dwRop">��դ�Ĳ���ֵ</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
        IntPtr hdcDest,
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        int dwRop
        );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern IntPtr CreateDC(
        string lpszDriver, // ��������
        string lpszDevice, // �豸����
        string lpszOutput, // ���ã������趨λ"NULL"
        IntPtr lpInitData // ����Ĵ�ӡ������
        );

        /// <summary>
        /// ��Ļ����λͼ������
        /// </summary>
        /// <returns></returns>
        public static Image Capture()
        {
            //������ʾ����DC
            IntPtr dc1 = CreateDC("DISPLAY", null, null, (IntPtr)null);
            //��һ��ָ���豸�ľ������һ���µ�Graphics����
            Graphics g1 = Graphics.FromHdc(dc1);
            //������Ļ��С����һ����֮��ͬ��С��Bitmap����
            Bitmap ScreenImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, g1);

            Graphics g2 = Graphics.FromImage(ScreenImage);
            //�����Ļ�ľ��
            IntPtr dc3 = g1.GetHdc();
            //���λͼ�ľ��
            IntPtr dc2 = g2.GetHdc();
            //�ѵ�ǰ��Ļ����λͼ������
            BitBlt(dc2, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc3, 0, 0, 13369376);
            //�ͷ���Ļ���
            g1.ReleaseHdc(dc3);
            //�ͷ�λͼ���
            g2.ReleaseHdc(dc2);

            //ѹ��ͼƬ
            Image bmp = MakeThumbnail(ScreenImage, ScreenImage.Width * 3 / 4, ScreenImage.Height * 3 / 4);
            //ScreenImage.SetResolution(800,600);
            return bmp;
        }

        /// <summary>
        /// ѹ��ͼƬ
        /// </summary>
        /// <param name="originalImage"></param>
        public static Image  MakeThumbnail(Image originalImage, int towidth,int toheight)
        {
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;       
     
            //�½�һ��bmpͼƬ
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //�½�һ������
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //���ø�������ֵ��
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //���õ�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //��ջ�������͸������ɫ���
            g.Clear(System.Drawing.Color.Transparent);

            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            return bitmap;
        }
    }
}
