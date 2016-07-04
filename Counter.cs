using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using DotNetNuke.Services.GeneratedImage;

namespace Bitboxx.Services.GeneratedImage
{
    public class Counter : ImageTransform
    {
        /// <summary>
        /// Sets the counter value. Defaultvalue is 0
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Sets the number of digits. Defaultvalue is 5
        /// </summary>
        public int Digits { get; set; }

        /// <summary>
        /// Path to Counter.gif file
        /// </summary>
        public string Filename { get; set; }

        public override string UniqueString
        {
            get
            {
                return base.UniqueString + "-" +
                       this.Value.ToString() + "-" +
                       this.Digits.ToString() + "-";
            }
        }

        public Counter()
        {
            InterpolationMode = InterpolationMode.HighQualityBicubic;
            SmoothingMode = SmoothingMode.Default;
            PixelOffsetMode = PixelOffsetMode.Default;
            CompositingQuality = CompositingQuality.HighSpeed;

            this.Value = 0;
            this.Digits = 5;
        }

        public override Image ProcessImage(Image image)
        {
            if (!File.Exists(Filename))
            {
                Filename = Path.GetFullPath(HttpContext.Current.Request.PhysicalApplicationPath + Filename);
                if (!File.Exists(Filename))
                    throw new FileNotFoundException();
            }
            image = Image.FromFile(Filename);

            
            //Get measurements of a digit 
            int digitWidth = image.Width / 10;
            int digitHeight = image.Height;

            // Create output grahics
            Bitmap imgOutput = new Bitmap(digitWidth * this.Digits, digitHeight, PixelFormat.Format24bppRgb);
            Graphics graphics = Graphics.FromImage(imgOutput);

            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality;
            graphics.InterpolationMode = InterpolationMode;
            graphics.SmoothingMode = SmoothingMode;
            graphics.PixelOffsetMode = PixelOffsetMode;


            // Sampling the output together
            string strCountVal = this.Value.ToString().PadLeft(this.Digits, '0');
            for (int i = 0; i < this.Digits; i++)
            {
                // Extract digit from countVal
                int digit = Convert.ToInt32(strCountVal.Substring(i, 1));

                // Add digit to output graphics
                Rectangle targetRect = new Rectangle(i * digitWidth, 0, digitWidth, digitHeight);
                Rectangle sourceRect = new Rectangle(digit * digitWidth, 0, digitWidth, digitHeight);
                graphics.DrawImage(image, targetRect, sourceRect, GraphicsUnit.Pixel);
            }
            return imgOutput;
        }
    }
}