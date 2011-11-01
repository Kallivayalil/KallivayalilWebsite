using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Kallivayalil.Client;
using Website.Helpers;
using Website.Models;
using Website.Models.ViewModels;

namespace Website.Controllers
{
    public class AccountDetailsController :Controller
    {
        private AutoDataContractMapper mapper = new AutoDataContractMapper();
        private string serviceBaseUri = ConfigurationManager.AppSettings["serviceBaseUri"];
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["userName"] == null)
                FormsAuthentication.RedirectToLoginPage();
            return PartialView(GetUser());
        }
        

        private Login GetUser()
        {
            var email = Session["email"];
            var loginData = HttpHelper.Get<LoginData>(string.Format(serviceBaseUri + "/Login?username={0}", email));

            mapper = new AutoDataContractMapper();
            var login = new Login();
            mapper.Map(loginData, login);
            return login;
        }

        [HttpPost]
        public ActionResult Save(LoginInputModel loginInputModel)
        {
            var loginToUpdate = new Login();
            loginToUpdate.CreatedDateTime = loginInputModel.CreatedDateTime;
            loginToUpdate.CreatedBy = loginInputModel.CreatedBy;
            loginToUpdate.Password = loginInputModel.Password;
            loginToUpdate.IsAdmin = loginInputModel.IsAdmin;
            loginToUpdate.Id = loginInputModel.Id;
            loginToUpdate.Email = new Email { Address = (string)Session["email"] };

            var mapper = new AutoDataContractMapper();
            var loginData = new LoginData();
            mapper.Map(loginToUpdate, loginData);

            var data = HttpHelper.Put(string.Format(serviceBaseUri + "/Login/{0}", loginInputModel.Id), loginData);
            var savedLogin = new Login();
            mapper.Map(data, savedLogin);
            return PartialView(GetUser());
        }

    }
}