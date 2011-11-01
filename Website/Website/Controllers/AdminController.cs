using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Kallivayalil.Client;
using Telerik.Web.Mvc;
using Website.Helpers;
using Website.Models;
using System.Linq;

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
            Session["approvalConstituentId"] = 0;
            Session["matchedConstituentId"] = 0;
            return View();
        }

        [HttpPost]
        public JsonResult Matches(string constId)
        {
            Session["approvalConstituentId"] = constId;
            Session["matchedConstituentId"] = 0;
            var constituents = PopulateSearchResults(constId);
            return this.Json(constituents);
         }  
        
        [HttpPost]
        public JsonResult RegisterNew(AdminInput input)
        {
            var approvalConstituentId = Convert.ToInt32(Session["approvalConstituentId"]);
            var registerationData = new ConfirmRegisterationData { ConstituentToRegister = approvalConstituentId, IsAdmin = input.IsAdmin, IsApproved = true, AdminEmail = Session["email"].ToString() };

            HttpHelper.Post(serviceBaseUri + "/Registration/RegisterConstituent",registerationData);

            var expandoObject = new ExpandoObject();
            return this.Json(expandoObject);
        }

        public JsonResult RegisterAndLink(AdminInput input)
        {
            var approvalConstituentId = Convert.ToInt32(Session["approvalConstituentId"]);
            var matchConstituentId = Convert.ToInt32(Session["matchedConstituentId"]);
            var registerationData = new ConfirmRegisterationData { ConstituentToRegister = approvalConstituentId, Constituent = matchConstituentId, IsAdmin = input.IsAdmin, IsApproved = true, AdminEmail = Session["email"].ToString() };

            HttpHelper.Post(serviceBaseUri + "/Registration/RegisterConstituent",registerationData);

            var expandoObject = new ExpandoObject();
            return this.Json(expandoObject);
        }  
        
        public ActionResult Cancel()
        {
            return RedirectToAction("Registrations");
        } 
        
        public ActionResult Reject()
        {
            var approvalConstituentId = Convert.ToInt32(Session["approvalConstituentId"]);
            var registerationData = new ConfirmRegisterationData { ConstituentToRegister = approvalConstituentId, IsApproved = false, AdminEmail = Session["email"].ToString(),RejectReason = "Some random reason"};

            HttpHelper.Post(serviceBaseUri + "/Registration/RegisterConstituent", registerationData);

            var expandoObject = new ExpandoObject();
            return this.Json(expandoObject);
        }

        public JsonResult SelectMatch(string constId)
        {
            Session["matchedConstituentId"] = constId;

            var expandoObject = new ExpandoObject();
            return this.Json(expandoObject);
         } 
        
        public ActionResult Confirm()
        {
            var approvalConstituentId = Session["approvalConstituentId"];
            var matchConstituentId = Session["matchedConstituentId"];

            if (Convert.ToInt32(approvalConstituentId) > 0 && Convert.ToInt32(matchConstituentId) > 0)
            {

                var approvalConstituent = GetConstituent((string) approvalConstituentId);
                var matchConstituent = GetConstituent((string) matchConstituentId);
                ViewData["ApprovalConstituentName"] = approvalConstituent.Name.NameString;
                ViewData["ApprovalConstituentDOB"] = approvalConstituent.BornOn.ToString("d");
                ViewData["ApprovalConstituentFamily"] = string.Format("{0} - {1}", approvalConstituent.BranchName.Description, approvalConstituent.HouseName);
                ViewData["ApprovalConstituentAddress"] = GetAddress(approvalConstituent.Id).First().ToString();
                ViewData["ApprovalConstituentPhone"] = GetPhones(approvalConstituent.Id).First().ToString();
                ViewData["ApprovalConstituentEmail"] = GetEmails(approvalConstituent.Id).First().ToString();

                ViewData["MatchConstituentName"] = matchConstituent.Name.NameString;
                ViewData["MatchConstituentDOB"] = matchConstituent.BornOn.ToString("d");
                ViewData["MatchConstituentFamily"] = string.Format("{0} - {1}", matchConstituent.BranchName.Description, matchConstituent.HouseName);
                var addresses = GetAddress(matchConstituent.Id);
                var allAddress = addresses.Select(address => address.ToString()).ToList();
                ViewData["MatchConstituentAddress"] = allAddress;
                var phones = GetPhones(matchConstituent.Id);
                var allPhones = phones.Select(phone => phone.ToString()).ToList();
                ViewData["MatchConstituentPhone"] = allPhones;
                var emails = GetEmails(matchConstituent.Id);
                var allEmails = emails.Select(email => email.ToString()).ToList();
                ViewData["MatchConstituentEmail"] = allEmails;
            }

            return PartialView();
        }

        private Constituent GetConstituent(string id)
        {
            var data = HttpHelper.Get<ConstituentData>(serviceBaseUri + "/Constituents/"+id);
            var constituent = new Constituent();
            mapper.Map(data, constituent);
            return constituent;
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