using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Trikal_Website.Models;
using Trikal_Website.Models.ViewModels;

namespace Trikal_Website.Controllers
{
    public class HomeController : Controller
    {
        UserDetailsModel UDMObj = null;

        public HomeController()
        {
            UDMObj = new UserDetailsModel();
        }

        [HttpGet]
        public ActionResult HomePage()
        {
            return View();
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendMailWithBrochure(String Name, String Email)
        {
            String Message;
            Int32 SendFlag=0;
            Boolean IsSendMail = false;
            try
            {
                String BrochureFile = Convert.ToString(Server.MapPath("~/Content/Brochure/SOURAV BAGCHI.pdf"));
                String EmailTemplatepath = Convert.ToString(Server.MapPath("~/Content/EmailTemplates/BrochureMailTemplate.html"));
                String EmailTemplate = String.Empty;
                EmailTemplate = CommonMethod.ReadHtmlFile(EmailTemplatepath);

                //Email Template Data Insert
                
                EmailTemplate = EmailTemplate.Replace("@@UserName@@", Name);
                //EmailTemplate = EmailTemplate.Replace("@@UserAddreess@@", Obj.);
                
               

                EmailTemplate = EmailTemplate.Replace("@@LastFooterText@@", "© " + DateTime.Now.Year.ToString() + " Trikal Technology. All rights reserved");

                IsSendMail = CommonMethod.SendMail(EmailTemplate, Email, "Trikal Tech Brochure", BrochureFile, "Trikal Tech Brochure.pdf");
                if (IsSendMail)
                {
                    SendFlag = 1;
                    Message = "Mail sent to your email successfully";
                }
                   
                else
                {
                    SendFlag = 0;
                    Message = "Email cannot be sent.Please try again later";
                }
                    
            }
            catch (Exception ex)
            {
                IsSendMail = false;
                SendFlag = 0;
                Message = ex.Message.ToString();
            }

            return Json(new { Success = SendFlag, Message = Message });
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AdminLogin(UserDetailsViewModel UDVMObj)
        {
            try
            {
                UDVMObj = UDMObj.AdminLogin(UDVMObj);
                if(!String.IsNullOrEmpty(UDVMObj.AdminId))
                {
                    Session["Admin"] = UDVMObj.AdminId;
                }
                if(!String.IsNullOrEmpty(UDVMObj.UserId))
                {
                    Session["UserId"] = UDVMObj.UserId;
                }
                if(!String.IsNullOrEmpty(UDVMObj.Username))
                {
                    Session["Username"] = UDVMObj.Username;
                }
            }
            catch(Exception Ex)
            {

            }

            return Json(UDVMObj);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            Session["UserId"] = null;
            Session["Username"] = null;

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult GetAdminDropdownOption()
        {
            if(Session["Admin"]!=null)
            {
                Regex initials = new Regex(@"(\b[a-zA-Z])[a-zA-Z]* ?");
                String init = initials.Replace(Convert.ToString(Session["Username"]), "$1");
                ViewBag.Name = init;
            }
            return PartialView("_AdminDropdownOption");
        }

        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TCDV()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Tumio()
        {
            return View();
        }
    }
}