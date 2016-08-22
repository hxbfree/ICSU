using System;
namespace ICanSeeYou.Codes
{
    /// <summary>
    /// ·¢ËÍÆÁÄ»Ö¸Áî
    /// </summary>
    [Serializable]
    public class SendScreenCode : BaseCode
    {
        private System.Drawing.Image screenImage;
        /// <summary>
        /// ÆÁÄ»½ØÍ¼
        /// </summary>
        public System.Drawing.Image ScreenImage
        {
            get { return screenImage; }
            set { screenImage = value; }
        }
        public SendScreenCode()
        {
            base.Head = CodeHead.SCREEN_SUCCESS;
        }
    }
}
