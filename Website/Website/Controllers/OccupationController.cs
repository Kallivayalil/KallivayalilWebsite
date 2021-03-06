using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Kallivayalil.Client;
using Telerik.Web.Mvc;
using Website.Helpers;
using Website.Models;
using Website.Models.ReferenceData;

namespace Website.Controllers
{
    public class OccupationController : Controller
    {
        private AutoDataContractMapper mapper = new AutoDataContractMapper();
        private string serviceBaseUri = ConfigurationManager.AppSettings["serviceBaseUri"];

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["userName"] == null)
                FormsAuthentication.RedirectToLoginPage();
            PopulateOccupationTypes();
            return PartialView();
        }

        private void PopulateOccupationTypes()
        {
            var occupationTypesData = HttpHelper.Get<OccupationTypesData>(serviceBaseUri+"/OccupationTypes");

            var occupationTypes = new OccupationTypes();
            mapper.MapList(occupationTypesData, occupationTypes, typeof (OccupationType));
            ViewData["occupationTypes"] = occupationTypes;
        }

        [GridAction]
        public ActionResult AllOccupations()
        {
            return PartialView(new GridModel(GetOccupations()));
        }

        private Occupations GetOccupations()
        {
            var constituentId =  Convert.ToInt32(Session["constituentId"]);
            var OccupationsData = HttpHelper.Get<OccupationsData>(string.Format(serviceBaseUri+"/Occupations?ConstituentId={0}", constituentId));

            mapper = new AutoDataContractMapper();
            var Occupations = new Occupations();
            mapper.MapList(OccupationsData, Occupations, typeof (Occupation));
            return Occupations;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Create(int OccupationType)
        {
            var Occupation = new Occupation();
            TryUpdateModel(Occupation);

            var constituentId =  Convert.ToInt32(Session["constituentId"]);
            Occupation.Constituent = new Constituent {Id = constituentId};
            Occupation.Type = new OccupationType { Id = OccupationType };

            mapper = new AutoDataContractMapper();
            var OccupationData = new OccupationData();
            mapper.Map(Occupation, OccupationData);

            var newOccupation = HttpHelper.Post(string.Format(serviceBaseUri+"/Occupations?ConstituentId={0}", constituentId), OccupationData);

            return PartialView(new GridModel(GetOccupations()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Edit(int id, int OccupationType)
        {
            var Occupation = new Occupation();

            TryUpdateModel(Occupation);
            var constituentId =  Convert.ToInt32(Session["constituentId"]);
            Occupation.Type = new OccupationType {Id = OccupationType};
            Occupation.Address = new Address() {Id = 1};
            Occupation.Constituent = new Constituent {Id = constituentId};
            mapper = new AutoDataContractMapper();
            var OccupationData = new OccupationData();
            mapper.Map(Occupation, OccupationData);

            HttpHelper.Put(string.Format(serviceBaseUri+"/Occupations/{0}",id), OccupationData);
            return PartialView(new GridModel(GetOccupations()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Delete(int id)
        {
            HttpHelper.DoHttpDelete(string.Format(serviceBaseUri+"/Occupations/{0}", id));
            return PartialView(new GridModel(GetOccupations()));
        }
    }
}