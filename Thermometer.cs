using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DotNetNuke.Services.GeneratedImage;

namespace Bitboxx.Services.GeneratedImage
{
    public class Thermometer : ImageTransform
    {
        public string Degree { get; set; }

        public string Areas { get; set; }
        
        public override string UniqueString
        {
            get { return base.UniqueString + this.Degree + this.Areas; }
        }

        public Thermometer()
        {
            InterpolationMode = InterpolationMode.HighQualityBicubic;
            SmoothingMode = SmoothingMode.HighQuality;
            PixelOffsetMode = PixelOffsetMode.HighQuality;
            CompositingQuality = CompositingQuality.HighQuality;
        }

        public override Image ProcessImage(Image image)
        {

            // create new "canvas" for the new image in this case 
            // exactly the size of the thermometer image in folder images
            Bitmap bitmap = new Bitmap(126,574);

            if (String.IsNullOrEmpty(Areas) || Areas.Length - Areas.Replace(",", "").Length != 3)
                Areas = "20,30,40,60";
            string[] areas = Areas.Split(',');
            int[] iAreas = new int[] {Convert.ToInt16(areas[0]), Convert.ToInt16(areas[1]), Convert.ToInt16(areas[2]), Convert.ToInt16(areas[3])};

            using (Graphics objGraphics = Graphics.FromImage(bitmap))
            {
                // Initialize graphics
                objGraphics.Clear(Color.White);

                // Convert Parameters from string to whats needed.
                decimal degree = Convert.ToDecimal(Degree);

                
                // Retrieve color for circle and bar
                Brush colorBrush = GetThermometerColor(degree,iAreas);

                // Draw the circle on a fixed position
                objGraphics.FillEllipse(colorBrush, new Rectangle(1, 485, 82, 82));

                // Do some math to determine the height of the thermometerbar and draw it
                int height = Convert.ToInt32((decimal) 450 / 100 * degree + 15);
                objGraphics.FillRectangle(colorBrush, 24, 490 - height, 40, height);
                
                // Load the image - which is included in the project with property:
                // Build Action = Embedded Resouce
                System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream file = thisExe.GetManifestResourceStream("Bitboxx.Services.GeneratedImage.Images.Thermometer.png");
                Bitmap bmp = (Bitmap)Image.FromStream(file);

                //.. and draw it to the canvas as a layer on top
                objGraphics.DrawImageUnscaled(bmp, new Point(0, 0));

                // we are done and save it and return the image
                objGraphics.Flush();
			}
			return (Image) bitmap;
        }

        private static Brush GetThermometerColor(decimal degree, int[] areas)
        {
            Color barColor = Color.RoyalBlue;

            if (degree >= areas[3])
                barColor = Color.Purple;
            else if (degree >= areas[2])
                barColor = Color.Red;
            else if (degree >= areas[1])
                barColor = Color.Orange;
            else if (degree >= areas[0])
                barColor = Color.LimeGreen;
            Brush barBrush = new SolidBrush(barColor);
            return barBrush;
        }
    }
}
