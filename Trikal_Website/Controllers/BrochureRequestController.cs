using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Trikal_Website.Models;
using Trikal_Website.Models.ViewModels;

namespace Trikal_Website.Controllers
{
    public class BrochureRequestController : Controller
    {
        BrochureRequestDetailsModel BRDMObj = null;
        public BrochureRequestController()
        {
            BRDMObj = new BrochureRequestDetailsModel();
        }
        // GET: BrochureRequest
        [HttpPost]
        public JsonResult BrochureRequestInsert(BrochureRequestDetailsViewModel BRDVMObj)
        {
            //BRDVMObj.MarkAsSpam = false;
            //BRDVMObj.SendBit = false;
            //BRDVMObj.LastUpdateByAdminId = String.Empty;
            Thread GenAndSentMail;
            if (Session["Admin"]!=null)
            {
                BRDVMObj.LastUpdateByAdminId = Convert.ToString(Session["Admin"]);
            }

            BRDVMObj = BRDMObj.BrochureRequestDetailsInsertUpdate(BRDVMObj);

            if (Session["Admin"]!=null && BRDVMObj.SendBit==true && BRDVMObj.Success==1)
            {
                var json = new JavaScriptSerializer().Serialize(BRDVMObj);
                GenAndSentMail = new Thread(new ParameterizedThreadStart(SendMailWithBrochure));
                //      GeneratePasswordAndSendMail(json);
                GenAndSentMail.Start(json);
            }

            return Json(new { Message = BRDVMObj.Message, Success = BRDVMObj.Success });
        }

        [HttpGet]
        public ActionResult BrochureRequestReceived()
        {
            if(Session["Admin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpGet]
        public ActionResult BrochureRequestProcessed()
        {
            if (Session["Admin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetBrochureRequest(BrochureRequestDetailsViewModel BRDVMObj)
        {
            List<BrochureRequestDetailsViewModel> BRDVMObjList = new List<BrochureRequestDetailsViewModel>();
            if (Session["Admin"]!=null)
            {
                try
                {
                    BRDVMObjList = BRDMObj.GetBrochureRequestDetails(BRDVMObj);
                }
                catch (Exception Ex)
                {

                }
            }
            return PartialView("_GetBrochureRequest", BRDVMObjList);


        }

        public ActionResult GetBrochureRequestProcessed(BrochureRequestDetailsViewModel BRDVMObj)
        {
            List<BrochureRequestDetailsViewModel> BRDVMObjList = new List<BrochureRequestDetailsViewModel>();
            if (Session["Admin"] != null)
            {
                try
                {
                    BRDVMObjList = BRDMObj.GetBrochureRequestDetails(BRDVMObj);
                }
                catch (Exception Ex)
                {

                }
            }
            return PartialView("_GetBrochureRequestProcessed", BRDVMObjList);


        }

        public Int32 GetRequestCount()
        {
            Int32 RequestCount = 0;
            if (Session["Admin"] != null)
            {
                try
                {
                    RequestCount = BRDMObj.GetBrochureRequestCount();
                }
                catch (Exception Ex)
                {
                    RequestCount = 0;
                }
            }
            return RequestCount;
        }

        public void SendMailWithBrochure(Object Obj)
        {
            BrochureRequestDetailsViewModel BRDVMObj = new JavaScriptSerializer().Deserialize<BrochureRequestDetailsViewModel>(Convert.ToString(Obj));
            Boolean IsSendMail = false;
            try
            {
                String BrochureFile = Convert.ToString(Server.MapPath("~/Content/Brochure/Tumio_Brochure.pdf"));
                String EmailTemplatepath = Convert.ToString(Server.MapPath("~/Content/EmailTemplates/BrochureMailTemplate.html"));
                String EmailTemplate = String.Empty;
                EmailTemplate = CommonMethod.ReadHtmlFile(EmailTemplatepath);

                //Email Template Data Insert

                EmailTemplate = EmailTemplate.Replace("@@UserName@@", BRDVMObj.Name);
                //EmailTemplate = EmailTemplate.Replace("@@UserAddreess@@", Obj.);



                EmailTemplate = EmailTemplate.Replace("@@LastFooterText@@", "© " + DateTime.Now.Year.ToString() + " Trikal Technology. All rights reserved");

                IsSendMail = CommonMethod.SendMail(EmailTemplate, BRDVMObj.Email, "Tumio Brochure", BrochureFile, "TumioBrochure.pdf");
             
                if(!IsSendMail)
                {
                    BRDVMObj.SendBit = false;
                    BRDVMObj = BRDMObj.BrochureRequestDetailsInsertUpdate(BRDVMObj);
                }

            }
            catch (Exception ex)
            {
                IsSendMail = false;              
            }
        }
    }
}