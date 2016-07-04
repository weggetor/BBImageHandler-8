using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DotNetNuke.Services.GeneratedImage;

namespace Bitboxx.Services.GeneratedImage
{
    public class Percent:ImageTransform
    {
        /// <summary>
		/// Sets the percentage value for the radial indicator
		/// </summary>
		public int Percentage { get; set; }

		/// <summary>
		/// Sets the Color of the indicator element
		/// </summary>
		public Color Color { get; set; }

        /// <summary>
        /// Sets the type of the Percentage image (circle,bar)
        /// </summary>
        public string Type { get; set; }

        private enum PercentageType
        {
            Bar,
            Circle
        }

		public override string UniqueString
		{
			get { return base.UniqueString + this.Percentage.ToString() + "-" + this.Color.ToString() + "-" +this.Type.ToString(); }
		}

		public Percent()
		{
			InterpolationMode = InterpolationMode.HighQualityBicubic;
			SmoothingMode = SmoothingMode.HighQuality;
			PixelOffsetMode = PixelOffsetMode.HighQuality;
			CompositingQuality = CompositingQuality.HighQuality;
		}

		public override Image ProcessImage(Image image)
		{
            PercentageType pType;
		    Bitmap bitmap;
            if (!PercentageType.TryParse(Type,true,out pType))
                pType = PercentageType.Bar;

		    if (pType == PercentageType.Bar)
		    {
                bitmap = new Bitmap(500, 50);
		        using (Graphics objGraphics = Graphics.FromImage(bitmap))
		        {
		            // Initialize graphics
		            objGraphics.Clear(Color.White);
		            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
		            objGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

		            Pen borderPen = new Pen(Color.DarkGray, 6f);

                    
                    Brush colorBrush = new SolidBrush(Color);
                    Rectangle barRectangle = new Rectangle(3, 3, Percentage *5 - 6, 44);
                    objGraphics.FillRectangle(colorBrush, barRectangle);

                    Rectangle borderRectangle = new Rectangle(3, 3, 494, 44);
                    objGraphics.DrawRectangle(borderPen, borderRectangle);

                    // Draw text on image
                    // Use rectangle for text and align text to center of rectangle
                    var font = new Font("Arial", 34, FontStyle.Bold);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
		            Brush textBrush = new SolidBrush(MostDifferent(Color));
                    objGraphics.DrawString(Percentage + "%", font, textBrush, borderRectangle, stringFormat);

                    // Save indicator to file
                    objGraphics.Flush();
                }
		    }
		    else
		    {
		        bitmap = new Bitmap(1000, 1000);
		        using (Graphics objGraphics = Graphics.FromImage(bitmap))
		        {
		            // Initialize graphics
		            objGraphics.Clear(Color.White);
		            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
		            objGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

		            Pen borderPen = new Pen(Color.DarkGray, 10f);

		            // Fill pie
		            // Degrees are taken clockwise, 0 is parallel with x
		            // For sweep angle we must convert percent to degrees (90/25 = 18/5)
		            float startAngle = -90.0F;
		            float sweepAngle = (18.0F/5)*Percentage;

		            Rectangle rectangle = new Rectangle(50, 50, 900, 900);
		            Brush colorBrush = new SolidBrush(Color);
		            objGraphics.FillPie(colorBrush, rectangle, startAngle, sweepAngle);

		            // Fill inner circle with white
		            rectangle = new Rectangle(200, 200, 600, 600);
		            objGraphics.FillEllipse(Brushes.White, rectangle);

		            // Draw circles
		            rectangle = new Rectangle(50, 50, 900, 900);
		            objGraphics.DrawEllipse(borderPen, rectangle);
		            rectangle = new Rectangle(200, 200, 600, 600);
		            objGraphics.DrawEllipse(borderPen, rectangle);

		            // Draw text on image
		            // Use rectangle for text and align text to center of rectangle
		            var font = new Font("Arial", 130, FontStyle.Bold);
		            StringFormat stringFormat = new StringFormat();
		            stringFormat.Alignment = StringAlignment.Center;
		            stringFormat.LineAlignment = StringAlignment.Center;

		            rectangle = new Rectangle(200, 400, 620, 200);
		            objGraphics.DrawString(Percentage + "%", font, Brushes.DarkGray, rectangle, stringFormat);

		            // Save indicator to file
		            objGraphics.Flush();
		        }
		    }
		    return (Image) bitmap;
		}

        public Color MostDifferent(Color original)
        {
            if (original.R + original.G + original.B <= 192 && Percentage > 50)
                return System.Drawing.Color.FromArgb(255, 210, 210, 210);
            else
                return System.Drawing.Color.FromArgb(255, 80, 80, 80);

        }
    }
}
