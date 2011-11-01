using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class ImagePreviewController : Controller
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AjaxSubmit(int? id)
        {
            Session["ContentLength"] = Request.Files[0].ContentLength;
            Session["ContentType"] = Request.Files[0].ContentType;
            var b = new byte[Request.Files[0].ContentLength];
            Request.Files[0].InputStream.Read(b, 0, Request.Files[0].ContentLength);
            Session["ContentStream"] = b;
            return Content(Request.Files[0].ContentType + ";" + Request.Files[0].ContentLength);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadSubmit(ProfilePictureModel model)
        {
            Upload(model);
            return RedirectToAction("Profile", "Profile");
        }

        private void Upload(ProfilePictureModel model)
        {
            var fileName = Path.GetFileName(model.imageLoad2.FileName);
            var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), Session["constituentId"].ToString());

            // The files are not actually saved in this demo
            model.imageLoad2.SaveAs(physicalPath + "_Full.jpg");

            var savedImage = Image.FromFile(physicalPath + "_Full.jpg");
            var resizedImage = resizeImage(savedImage, new Size(300, 350));

            var width = (model.x2 - model.x1) > resizedImage.Width ? resizedImage.Width : (model.x2 - model.x1);
            var height = (model.y2 - model.y1) > resizedImage.Width ? resizedImage.Width : (model.y2 - model.y1);

            if (width > 0 && height > 0)
            {
                var croppedImage = cropImage(resizedImage, new Rectangle(model.x1, model.y1, width, height));
                var resizedCropImage = resizeImage(croppedImage, new Size(115, 115));
                resizedCropImage.Save(physicalPath + "_thumb.jpg");
            }
            else
            {
                var resizedCropImage = resizeImage(resizedImage, new Size(115, 115));
                resizedCropImage.Save(physicalPath + "_thumb.jpg");
            }
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (size.Width/(float) sourceWidth);
            nPercentH = (size.Height/(float) sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int destWidth = (int) (sourceWidth*nPercent);
            int destHeight = (int) (sourceHeight*nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (bmpCrop);
        }

        public ActionResult ImageLoad(int? id)
        {
            byte[] b = (byte[]) Session["ContentStream"];
            int length = (int) Session["ContentLength"];
            string type = (string) Session["ContentType"];
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = type;
            Response.BinaryWrite(b);
            Response.Flush();
            Response.End();
            Session["ContentLength"] = null;
            Session["ContentType"] = null;
            Session["ContentStream"] = null;
            return Content("");
        }
    }

    public class CropData
    {
        public int X1;
        public int X2;
        public int Y1;
        public int Y2;
    }
}