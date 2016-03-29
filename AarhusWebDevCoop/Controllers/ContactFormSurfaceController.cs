using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: Default
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                //Perhaps you might want to add a custom message to the ViewBag
                //which will be available on the View when it renders (since we're not 
                //redirecting)          
                return CurrentUmbracoPage();
            }


            using (SmtpClient smtp = new SmtpClient())
            {
                TempData["success"] = true;
                // create message
                IContent emailMessage = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");

                emailMessage.SetValue("fullName", model.Name);
                emailMessage.SetValue("email", model.Email);
                emailMessage.SetValue("subject", model.Subject);
                emailMessage.SetValue("message", model.Message);

                //Save
                Services.ContentService.Save(emailMessage);

                //redirect to current page to clear the form
                return RedirectToCurrentUmbracoPage();
            }
        }
    }

}