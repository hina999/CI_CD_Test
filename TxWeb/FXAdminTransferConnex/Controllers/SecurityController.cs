using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}