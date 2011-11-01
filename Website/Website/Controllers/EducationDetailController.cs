using System;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Web.Mvc;
using System.Web.Security;
using Kallivayalil.Client;
using Telerik.Web.Mvc;
using Website.Helpers;
using Website.Models;
using Website.Models.ReferenceData;

namespace Website.Controllers
{
    public class EducationDetailController : Controller
    {
        private AutoDataContractMapper mapper = new AutoDataContractMapper();
        private string serviceBaseUri = ConfigurationManager.AppSettings["serviceBaseUri"];

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["userName"] == null)
                FormsAuthentication.RedirectToLoginPage();
            PopulateEducationTypes();
            return PartialView();
        }

        private void PopulateEducationTypes()
        {
            var educationTypesData = HttpHelper.Get<EducationTypesData>(serviceBaseUri+"/EducationTypes");

            var educationTypes = new EducationTypes();
            mapper.MapList(educationTypesData, educationTypes, typeof(EducationType));
            ViewData["educationTypes"] = educationTypes;
        }

        [GridAction]
        public ActionResult AllEducationDetails()
        {
            return PartialView(new GridModel(GetEducations()));
        }

        private EducationDetails GetEducations()
        {
            var constitinetId =  Convert.ToInt32(Session["constituentId"]);
            var educationDetailsData = HttpHelper.Get<EducationDetailsData>(string.Format(serviceBaseUri+"/EducationDetails?ConstituentId={0}", constitinetId));

            mapper = new AutoDataContractMapper();
            var educations = new EducationDetails();
            mapper.MapList(educationDetailsData, educations, typeof(EducationDetail));
            return educations;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Create(int educationType)
        {
            var educationDetail = new EducationDetail();
            TryUpdateModel(educationDetail);

            var constituentId =  Convert.ToInt32(Session["constituentId"]);
            educationDetail.Constituent = new Constituent { Id = constituentId};
            educationDetail.Type = new EducationType() { Id = educationType };

            mapper = new AutoDataContractMapper();
            var educationDetailData = new EducationDetailData();
            mapper.Map(educationDetail, educationDetailData);

            HttpHelper.Post(string.Format(serviceBaseUri+"/EducationDetails?ConstituentId={0}", constituentId), educationDetailData);

            return PartialView(new GridModel(GetEducations()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Edit(int id, int educationType)
        {
            var educationDetail = new EducationDetail();
            var constituentId =  Convert.ToInt32(Session["constituentId"]);

            TryUpdateModel(educationDetail);
            educationDetail.Type = new EducationType() { Id = educationType };
            educationDetail.Constituent = new Constituent { Id = constituentId };
            mapper = new AutoDataContractMapper();
            var educationDetailData = new EducationDetailData();
            mapper.Map(educationDetail, educationDetailData);

            HttpHelper.Put(string.Format(serviceBaseUri+"/EducationDetails/{0}",id), educationDetailData);
            return PartialView(new GridModel(GetEducations()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Delete(int id)
        {
            HttpHelper.DoHttpDelete(string.Format(serviceBaseUri+"/EducationDetails/{0}", id));
            return PartialView(new GridModel(GetEducations()));
        }

    }
}