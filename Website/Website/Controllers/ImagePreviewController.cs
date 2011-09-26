using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;

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
            public ActionResult UploadSubmit(int? id)
            {
                Session["ContentLength"] = Request.Files[0].ContentLength;
                Session["ContentType"] = Request.Files[0].ContentType;
                byte[] b = new byte[Request.Files[0].ContentLength];
                Upload(Request.Files[0]);
                Request.Files[0].InputStream.Read(b, 0, Request.Files[0].ContentLength);
                Session["ContentStream"] = b;
                return Content(Request.Files[0].ContentType + ";" + Request.Files[0].ContentLength);
            }

        private void Upload(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), Session["ConstituentId"]+".jpg");

            // The files are not actually saved in this demo
             file.SaveAs(physicalPath);
        }

            public ActionResult ImageLoad(int? id)
            {
                byte[] b = (byte[])Session["ContentStream"];
                int length = (int)Session["ContentLength"];
                string type = (string)Session["ContentType"];
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
}