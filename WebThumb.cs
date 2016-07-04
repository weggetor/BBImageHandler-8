using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using Bitboxx.Web.GeneratedImage.Transform;
using DotNetNuke.Services.GeneratedImage;

namespace Bitboxx.Services.GeneratedImage
{
    public class WebThumb : ImageTransform
    {
        /// <summary>
        /// Sets the Url. Defaultvalue is empty
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Sets the resulting ratio. (Full,Screen,Cinema) 
        /// </summary>
        public string Ratio { get; set; }

        public override string UniqueString
        {
            get
            {
                return base.UniqueString + "-" + this.Url + "-" + this.Ratio.ToString();
            }
        }

        public WebThumb()
        {
            InterpolationMode = InterpolationMode.HighQualityBicubic;
            SmoothingMode = SmoothingMode.Default;
            PixelOffsetMode = PixelOffsetMode.Default;
            CompositingQuality = CompositingQuality.HighSpeed;

        }

        public override Image ProcessImage(Image image)
        {
            IEBrowser.UrlRatioMode ratio ;
            if (!Enum.TryParse(Ratio, true, out ratio))
                ratio = IEBrowser.UrlRatioMode.Screen;

            AutoResetEvent resultEvent = new AutoResetEvent(false);
            IEBrowser browser = new IEBrowser(Url, ratio, resultEvent);
            WaitHandle.WaitAll(new[] { resultEvent });
            return browser.Thumb;
        }
    }
}
