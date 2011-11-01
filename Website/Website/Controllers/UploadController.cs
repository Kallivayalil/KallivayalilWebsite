using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kallivayalil.Client;
using Telerik.Web.Mvc;
using Website.Helpers;
using Website.Models;

namespace Website.Controllers
{
    [HandleError]
    public class UploadController : Controller
    {
        private const string SessionKey = "employees";

        private ICollection<UploadModel> Model{
            get
            {
              return GetFiles();
            }
 
            set
            {
                Session[SessionKey] = value;
            }
        }

        private AutoDataContractMapper mapper = new AutoDataContractMapper();
        private string serviceBaseUri = ConfigurationManager.AppSettings["serviceBaseUri"];

        public ActionResult Index()
        {
            return View(new GridModel(Model));
        }

        private UploadFilesModel GetFiles()
        {
            var uploadDatas = HttpHelper.Get<UploadDatas>(string.Format(serviceBaseUri + "/UploadFiles"));

            mapper = new AutoDataContractMapper();
            var files = new UploadFilesModel();
            mapper.MapList(uploadDatas, files, typeof(UploadModel));
            Model = files;
            return files;
        }

        [HttpPost]
        public ActionResult Save(string id, HttpPostedFileBase attachment)
        {
            var attachmentName = Path.GetFileName(attachment.FileName);
            var imagesPath = Server.MapPath("~/Content/UserFiles");
            attachment.SaveAs(Path.Combine(imagesPath, attachmentName));

            return Json(new { fileName = attachmentName }, "text/plain");
        }

        [GridAction]
        public ActionResult Select()
        {
            return View(new GridModel(Model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult Insert(string lastUpload)
        {
            var uploadModel = new UploadModel();
            TryUpdateModel(uploadModel);
            var constituentId = (int)Session["loggedInConstituentId"];
            uploadModel.Constituent = new Constituent { Id = constituentId };

            mapper = new AutoDataContractMapper();
            var uploadData = new UploadData();
            mapper.Map(uploadModel, uploadData);

            HttpHelper.Post(string.Format(serviceBaseUri + "/UploadFiles"), uploadData);

            return View(new GridModel(Model));
        
        }

        [HttpPost]
        [GridAction]
        public ActionResult Delete(int id)
        {
            var uploadFilesModel = GetFiles();
            var fileToBeDeleted = uploadFilesModel.Find(model => model.Id.Equals(id));

            var httpWebResponse = HttpHelper.DoHttpDelete(string.Format(serviceBaseUri + "/UploadFiles/{0}", id));
            if (httpWebResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                Remove(new[] {fileToBeDeleted.Name});
                uploadFilesModel.Remove(fileToBeDeleted);
            }
            return View(new GridModel(uploadFilesModel));
        } 
        
        
        [HttpPost]
        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/Content/UserFiles"), fileName);

                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                     System.IO.File.Delete(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }

    }
}
