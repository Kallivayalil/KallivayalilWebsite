using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Kallivayalil.Client;
using Telerik.Web.Mvc;
using Website.Helpers;
using Website.Models;

namespace Website.Controllers
{
    public class AdminController : Controller
    {
        private AutoDataContractMapper mapper = new AutoDataContractMapper();
        private readonly string serviceBaseUri = ConfigurationManager.AppSettings["serviceBaseUri"];

        [HttpGet]
        public ActionResult Registrations()
        {
            if (Session["userName"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            ViewData["Constituents"] = GetRegistrations();
            return View();
        }

        [HttpPost]
        public JsonResult Matches(string id)
         {
            var constituents = PopulateSearchResults(id);
            return this.Json(constituents);
         }

        private Constituents PopulateSearchResults(string id)
        {
            var uriString = serviceBaseUri + "/SearchUnRegistered?constituentId=" + id;
            var constituentsData = HttpHelper.Get<ConstituentsData>(uriString);

            mapper = new AutoDataContractMapper();
            var constituents = new Constituents();
            mapper.MapList(constituentsData, constituents, typeof (Constituent));

            return constituents;
        }

        private Constituents GetRegistrations()
        {
            var constituentsData = HttpHelper.Get<ConstituentsData>(serviceBaseUri + "/Registrations");

            mapper = new AutoDataContractMapper();
            var constituents = new Constituents();
            mapper.MapList(constituentsData, constituents, typeof (Constituent));
            return constituents;
        }

        [GridAction]
        public ActionResult AllEmails(int constituentId)
        {
            return View(new GridModel(GetEmails(constituentId)));
        }

        private Emails GetEmails(int constituentId)
        {
            var emailsData = HttpHelper.Get<EmailsData>(serviceBaseUri + "/Emails?ConstituentId=" + constituentId);

            mapper = new AutoDataContractMapper();
            var emails = new Emails();
            mapper.MapList(emailsData, emails, typeof(Email));
            return emails;
        }

        [HttpGet]
        [GridAction]
        public ActionResult AllPhones(int id,int constituentId)
        {
            return View(new GridModel(GetPhones(constituentId)));
        }

        private Phones GetPhones(int constituentId)
        {
            var phonesData = HttpHelper.Get<PhonesData>(string.Format(serviceBaseUri + "/Phones?ConstituentId={0}", constituentId));

            mapper = new AutoDataContractMapper();
            var phones = new Phones();
            mapper.MapList(phonesData, phones, typeof(Phone));
            return phones;
        }

        private Addresses GetAddress(int constituentId)
        {
            var addressesData = HttpHelper.Get<AddressesData>(string.Format(serviceBaseUri + "/Addresses?ConstituentId={0}", constituentId));

            mapper = new AutoDataContractMapper();
            var addresses = new Addresses();
            mapper.MapList(addressesData, addresses, typeof(Address));
            return addresses;
        }

        [GridAction]
        public ActionResult AllAddresses(int constituentId)
        {
            return View(new GridModel(GetAddress(constituentId)));
        }

        [GridAction]
        public ActionResult AllEducationDetails(int constituentId)
        {
            return View(new GridModel(GetEducations(constituentId)));
        }

        private EducationDetails GetEducations(int constituentId)
        {
            var educationDetailsData = HttpHelper.Get<EducationDetailsData>(string.Format(serviceBaseUri + "/EducationDetails?ConstituentId={0}", constituentId));

            mapper = new AutoDataContractMapper();
            var educations = new EducationDetails();
            mapper.MapList(educationDetailsData, educations, typeof(EducationDetail));
            return educations;
        }


        [GridAction]
        public ActionResult AllOccupations(int constituentId)
        {
            return PartialView(new GridModel(GetOccupations(constituentId)));
        }

        private Occupations GetOccupations(int constituentId)
        {
            var OccupationsData = HttpHelper.Get<OccupationsData>(string.Format(serviceBaseUri + "/Occupations?ConstituentId={0}", constituentId));

            mapper = new AutoDataContractMapper();
            var Occupations = new Occupations();
            mapper.MapList(OccupationsData, Occupations, typeof(Occupation));
            return Occupations;
        }

        [GridAction]
        public ActionResult AllAssociations(int constituentId)
        {
            return View(new GridModel(GetAssociations(constituentId)));
        }

        private Associations GetAssociations(int constituentId)
        {
            var associationsData = HttpHelper.Get<AssociationsData>(string.Format(serviceBaseUri + "/Associations?ConstituentId={0}", constituentId));

            mapper = new AutoDataContractMapper();
            var associations = new Associations();
            mapper.MapList(associationsData, associations, typeof(Association));
            return associations;
        }
    }
}