﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Kallivayalil.Client;
using Website.Helpers;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly AutoDataContractMapper mapper = new AutoDataContractMapper();

        public ActionResult Index()
        {
            PopulateEvents();
            return View();
        }

        private void PopulateEvents()
        {
            var uri = string.Format("{0}?isApproved={1}&startDate={2}&endDate={3}&includeBirthdays={4}"
                                          , "http://localhost/kallivayalilService/KallivayalilService.svc/Events", true, DateTime.Today, DateTime.Today, true);
            var eventsData = HttpHelper.Get<EventsData>(uri);

            var events = new Events();
            mapper.MapList(eventsData, events, typeof(Event));
            ViewData["events"] = events;
        }

        [HttpPost]
        public ActionResult Submit(string name, string email, string comment)
        {
            var contactUs = new ContactUs();

            contactUs.Name = name;
            contactUs.Email = email;
            contactUs.Comments = HttpUtility.HtmlDecode(comment);

            var contactUsData = new ContactUsData();
            mapper.Map(contactUs, contactUsData);

            HttpHelper.Post(string.Format(@"http://localhost/kallivayalilService/KallivayalilService.svc/ContactUs"), contactUsData);
            return View("Index");
        }


        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var userName = collection["userName"];
            var password = collection["password"];

            var authenticated = HttpHelper.Get<bool>(string.Format(@"http://localhost/kallivayalilService/KallivayalilService.svc/Authenticate?username={0}&password={1}",userName,password));

            if (authenticated)
            {
                var constituentData = HttpHelper.Get<ConstituentData>(string.Format(@"http://localhost/kallivayalilService/KallivayalilService.svc/Find?emailId={0}", userName));
                Session["userName"] = constituentData.Name.FirstName;
                Session["password"] = password;
                Session["constituentId"] = constituentData.Id;
                FormsAuthentication.RedirectFromLoginPage(userName,false);
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session["password"] = null;

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();

            return View("Index");
        }
    }
}