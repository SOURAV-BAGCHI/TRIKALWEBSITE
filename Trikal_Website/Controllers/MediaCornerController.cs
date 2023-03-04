using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trikal_Website.Models.ViewModels;

namespace Trikal_Website.Controllers
{
    public class MediaCornerController : Controller
    {
        // GET: MediaCorner
        [HttpGet]
        public ActionResult TumioFeedback()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTumioFeedbackList(PaginationViewModel PVMObj)
        {
            List<FeedbackViewModel> FVMObjList = new List<FeedbackViewModel>();

            FeedbackViewModel FVMObj = new FeedbackViewModel();
            FVMObj.Name="Sudip Nandi";
            FVMObj.CreateDate = "Jan 16, 2020 at 11:24 AM";
            FVMObj.Ratings = 4.6;
            FVMObj.FeedbackText = "Convenient and useful";

            FVMObjList.Add(FVMObj);
            FVMObjList.Add(FVMObj);

            return Json(FVMObjList, JsonRequestBehavior.AllowGet);
        }
    }
}