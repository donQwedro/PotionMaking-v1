using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Services;

namespace ZV_Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IUserAuthenticationService userAuthenticationService;

        public HomeController(IUserAuthenticationService userAuthenticationService)
        {
            this.userAuthenticationService = userAuthenticationService;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
