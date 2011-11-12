using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Kallivayalil.Client;
using Website.Helpers;

namespace Website.Controllers
{
    public class FamilyTreeController : Controller
    {

        private AutoDataContractMapper mapper = new AutoDataContractMapper();
        private string serviceBaseUri = ConfigurationManager.AppSettings["serviceBaseUri"];

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["userName"] == null)
                FormsAuthentication.RedirectToLoginPage();
            return View();
        }

        [HttpGet]
        public JsonResult Relationships(int id)
        {
            if (Session["userName"] == null)
                FormsAuthentication.RedirectToLoginPage();

            var uri = serviceBaseUri + "/Relationships?constituentId=" + id;
            var relationshipData = HttpHelper.Get<RelationshipData>(uri);

            return new JsonResult { Data = relationshipData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}