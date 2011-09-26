using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    [HandleError]
    public class UploadController : Controller
    {
        private const string SessionKey = "employees";

        private ICollection<UploadModel> Model
        {
            get
            {
                var employees = (ICollection<UploadModel>)Session[SessionKey];
                if (employees == null)
                {
                    employees = new List<UploadModel>
                    {
                        new UploadModel
                        {
                            Id = "1",
                            Name = "John Doe",
                            Picture = "johndoe.jpg"
                        }
                    };

                    Model = employees;
                }

                return employees;
            }

            set
            {
                Session[SessionKey] = value;
            }
        }

        public ActionResult Index()
        {
            return View(Model);
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
        public ActionResult Update(string id)
        {
            var employee = Model.First(e => e.Id == id);
            TryUpdateModel(employee);

            return View(new GridModel(Model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult Insert(string lastUpload)
        {
            var employee = new UploadModel();
            if (TryUpdateModel(employee))
            {
                employee.Id = Guid.NewGuid().ToString();
                Model.Add(employee);
            }

            return View(new GridModel(Model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult Delete(int id)
        {
            // The parameter of the Remove action must be called "fileNames"
//            foreach (var fullName in fileNames)
//            {
//                var fileName = Path.GetFileName(fullName);
//                var physicalPath = Path.Combine(Server.MapPath("~/Content/UserFiles"), fileName);
//
                // TODO: Verify user permissions
//                if (System.IO.File.Exists(physicalPath))
//                {
                    // The files are not actually removed in this demo
//                     System.IO.File.Delete(physicalPath);
//                }
//            }
            // Return an empty string to signify success
            return Content("");
        }

    }
}
