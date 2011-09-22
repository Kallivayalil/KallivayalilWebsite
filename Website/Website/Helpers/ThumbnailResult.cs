using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;

namespace Website.Helpers
{
    public class ThumbnailResult : ActionResult
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Filename { get; set; }

        public ThumbnailResult(int width, int height, string filename)
        {
            Width = width;
            Height = height;
            Filename = filename;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (string.IsNullOrEmpty(Filename))
            {
                throw new NullReferenceException("parameter Image cannot be null or empty");
            }

            var filePath = context.HttpContext.Server.MapPath(context.HttpContext.Request.ApplicationPath)
                           + @"Content\images\Constituents" + Filename;

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Image does not exist at " + filePath);
            }

            var bitmap = new Bitmap(filePath);


            try
            {
                if (bitmap.Width < Width && bitmap.Height < Height)
                {
                    context.HttpContext.Response.ContentType = "image/gif";
                    bitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                    return;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                bitmap.Dispose();
            }


            Bitmap finalBitmap = null;

            try
            {
                bitmap = new Bitmap(filePath);

                int bitmapNewWidth;
                decimal ratio;
                int bitmapNewHeight;

                if (bitmap.Width > bitmap.Height)
                {
                    ratio = (decimal) Width/bitmap.Height;
                    bitmapNewWidth = Width;

                    decimal temp = bitmap.Height*ratio;
                    bitmapNewHeight = (int) temp;
                }
                else
                {
                    ratio = (decimal) Height/bitmap.Height;
                    bitmapNewHeight = Height;
                    decimal temp = bitmap.Width*ratio;
                    bitmapNewWidth = (int) temp;
                }


                finalBitmap = new Bitmap(bitmapNewWidth, bitmapNewHeight);
                Graphics graphics = Graphics.FromImage(finalBitmap);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, bitmapNewWidth, bitmapNewHeight);
                graphics.DrawImage(bitmap, 0, 0, bitmapNewWidth, bitmapNewHeight);

                context.HttpContext.Response.ContentType = "image/gif";
                finalBitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (finalBitmap != null)
                {
                    finalBitmap.Dispose();
                }
            }
        }
    }
}