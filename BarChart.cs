using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DotNetNuke.Services.GeneratedImage;

namespace Bitboxx.Services.GeneratedImage
{
    public class BarChart : ImageTransform
    {
        public string xaxis { get; set; }
        public string yaxis { get; set; }
        public Color color { get; set; }

        public override string UniqueString
        {
            get { return base.UniqueString + this.xaxis.ToString() + "-" + this.yaxis.ToString() + "-" + this.color.ToString(); }
        }

        public BarChart()
        {
            InterpolationMode = InterpolationMode.HighQualityBicubic;
            SmoothingMode = SmoothingMode.HighQuality;
            PixelOffsetMode = PixelOffsetMode.HighQuality;
            CompositingQuality = CompositingQuality.HighQuality;
        }

        public override Image ProcessImage(Image image)
        {
            Graphics graphDummy = Graphics.FromImage(new Bitmap(1, 1));

            int spacing = 10;
            int barWidth = 20;
            int barScale = 10;
            int maxHeight = 0;
            int maxWidth = 0;

            // 1.) Get an array of each axis value and description
            string[] arrXAxis = xaxis.Split(',');
            string[] arrYAxis = yaxis.Split(',');


            // 2) Get the maximum height for the chart
            for (int height = 0; height < arrXAxis.Length; height++)
            {
                if (Convert.ToInt32(arrYAxis[height]) > maxHeight)
                    maxHeight = Convert.ToInt32(arrYAxis[height]);
            }

            // 3) Add 40 pixels to it to compensate for the labels  
            maxHeight = maxHeight*barScale + 40;

            // 4) Get the maximum width of any x or y axis string to work out the bar chart width  
            for (int width = 0; width < arrXAxis.Length; width++)
            {
                int stringWidth = Convert.ToInt32(graphDummy.MeasureString(arrXAxis[width], new Font("Courier New", 10, FontStyle.Italic)).Width);
                if (stringWidth > maxWidth)
                    maxWidth = stringWidth;
            }

            for (int width = 0; width < arrYAxis.Length; width++)
            {
                int stringWidth = Convert.ToInt32(graphDummy.MeasureString(arrYAxis[width], new Font("Courier New", 10, FontStyle.Italic)).Width);
                if (stringWidth > maxWidth)
                    maxWidth = stringWidth;
            }
            barWidth = maxWidth;

            //5) Create a new bitmap and graphics object based on the size of the number of arrary elements
            Bitmap bitmap = new Bitmap(Convert.ToInt32((arrXAxis.Length*barWidth) + (arrXAxis.Length*spacing) + (spacing/2)), maxHeight);
            using (Graphics objGraphics = Graphics.FromImage(bitmap))
            {
                // 6) Set the background colour  
                objGraphics.Clear(Color.White);

                // 7) Draw each bar
                for (int i = 0; i < arrYAxis.Length; i++)
                {
                    // Add a border for the coloured bar 
                    objGraphics.DrawRectangle(Pens.Black, (i*spacing) + (i*barWidth) + 5, maxHeight - (Convert.ToInt32(arrYAxis[i])*barScale), barWidth, Convert.ToInt32(arrYAxis[i])*barScale);

                    //Add the coloured bar  
                    objGraphics.FillRectangle(new SolidBrush(color), (i*spacing) + (i*barWidth) + 5, maxHeight - (Convert.ToInt32(arrYAxis[i])*barScale), barWidth, (Convert.ToInt32(arrYAxis[i])*barScale));

                    // Add the x axis values to the bar chart  
                    objGraphics.DrawString(arrXAxis[i], new Font("Courier New", 10, FontStyle.Italic), Brushes.Black, new PointF((i*spacing) + (i*barWidth) + 5, maxHeight - 25 - Convert.ToInt32(arrYAxis[i])*barScale));

                    // Add the y axis values to the bar chart  
                    objGraphics.DrawString(arrYAxis[i], new Font("Courier New", 10, FontStyle.Italic), Brushes.White, new PointF((i*spacing) + (i*barWidth) + 5, maxHeight - Convert.ToInt32(arrYAxis[i])*barScale));
                }

                // 8) Add a border to the chart  
                objGraphics.DrawRectangle(Pens.Black, 1, 1, Convert.ToInt32((arrXAxis.Length*barWidth) + (arrXAxis.Length*spacing) + (spacing/2)) - 2, maxHeight - 2);

                objGraphics.Flush();
			}
			return (Image) bitmap;
        }
    }
}
