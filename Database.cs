using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Web;
using DotNetNuke.Services.GeneratedImage;
using Microsoft.ApplicationBlocks.Data;

namespace Bitboxx.Services.GeneratedImage
{
    public class DbImage : ImageTransform
    {
        /// <summary>
        /// Sets the Connectionstring Descriptor from web.config. Defaultvalue is empty
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Sets the Table to select from. Defaultvalue is empty
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Sets the ID Field name to select from. Defaultvalue is empty
        /// </summary>
        public string IdField { get; set; }

        /// <summary>
        /// Sets the ID Field value to select from. Defaultvalue is empty
        /// </summary>
        public string IdValue { get; set; }

        /// <summary>
        /// Sets the Image Field name to select from. Defaultvalue is empty
        /// </summary>
        public string ImageField { get; set; }


        public override string UniqueString
        {
            get
            {
                return base.UniqueString + "-" + this.Connection + "-" + this.Table + "-" +
                       this.IdField + "-" + this.IdValue + "-" + this.ImageField ;
            }
        }

        public DbImage()
        {
            InterpolationMode = InterpolationMode.HighQualityBicubic;
            SmoothingMode = SmoothingMode.Default;
            PixelOffsetMode = PixelOffsetMode.Default;
            CompositingQuality = CompositingQuality.HighSpeed;
        }

        public override Image ProcessImage(Image image)
        {
            Bitmap emptyBmp = new Bitmap(1, 1, PixelFormat.Format1bppIndexed);
            emptyBmp.MakeTransparent();

            ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings[Connection];

            if (conn == null || string.IsNullOrEmpty(Table) || string.IsNullOrEmpty(IdField) ||
                string.IsNullOrEmpty(IdValue) || string.IsNullOrEmpty(ImageField))
            {
                return emptyBmp;
            }

            string sqlCmd = "SELECT " + this.ImageField + " FROM " +
                            this.Table + " WHERE " + this.IdField + " = @Value";

            object result = SqlHelper.ExecuteScalar(conn.ConnectionString, CommandType.Text, sqlCmd, new SqlParameter("Value", Convert.ToInt32(this.IdValue)));
            if (result != null)
            {
                MemoryStream ms = new MemoryStream((byte[])result);
                return Image.FromStream(ms);
            }

            
            return emptyBmp;
        }


    }
}
