using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Infrastructure.Services;
using ZV_Web.Models;

namespace ZV_Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserAuthenticationService authenticationService;

        public AccountController(IUserAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                authenticationService.Authenticate(model.UserName);
                return Redirect(returnUrl ?? FormsAuthentication.DefaultUrl);
            }
            else
            {
                return View("Login", model);
            }
           
        }
    }
}
