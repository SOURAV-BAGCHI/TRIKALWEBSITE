using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trikal_Website.Controllers
{
    public class LegalController : Controller
    {
        // GET: Legal
        public ActionResult Disclaimer()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}