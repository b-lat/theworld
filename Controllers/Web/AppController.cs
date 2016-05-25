using System;

namespace TheWorld.Controllers.Web
{
    using Microsoft.AspNet.Mvc;

    using TheWorld.Services;
    using TheWorld.ViewModels;

    public class AppController : Controller
    {
        private IMailService _mailService;

        public AppController(IMailService service)
        {
            this._mailService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem.");
                }

                if (this._mailService.SendMail(
                    email,
                    email,
                    $"Contact Page from {model.Name} ({model.Email})",
                    model.Message))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail Sent. Thanks!";
                }
            }
            return this.View();
        }
    }
}
