using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using LeaveApplication.Models;
using System.Data;

namespace LeaveApplication.Controllers
{
    public class ApplyForLeaveController : Controller
    {
        // GET: ApplyForLeave

        EmployeeBusinessLayer e1 = new EmployeeBusinessLayer();
        LeaveBusinessLayer lb = new LeaveBusinessLayer();
        public ActionResult Index(int? ViewId)
        {

            if (Session["EmpID"] != null)
            {
                
                ViewBag.Reasons = lb.GetReasons();
                ViewBag.Leavetypes = lb.GetLeaveTypes();

                if (ViewId == null || ViewId == 0)
                {
                    ViewBag.ViewID = 0;
                }
                else if (ViewId == 1)
                {
                    ViewBag.ViewID = 1;
                    if (TempData["HrsError"] != null && Convert.ToBoolean(TempData["HrsError"]) == true)
                    {
                        ViewBag.HrsError = true;
                    }
                    else
                    {
                        ViewBag.HrsError = false;
                    }
                }
                else
                {
                    ViewBag.ViewID = 0;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult Submit(LeaveApplication.Models.LeaveApplication l1)
        {//data validation
            if (string.IsNullOrWhiteSpace(l1.LeaveType))
            {
                ModelState.AddModelError("LeaveType", "Required");
            }
            if (l1.IsHalfDay == 0 && string.IsNullOrWhiteSpace(l1.FromDate))
            {
                ModelState.AddModelError("FromDate", "Required");
            }
            if (l1.IsHalfDay == 0 && string.IsNullOrWhiteSpace(l1.ToDate))
            {
                ModelState.AddModelError("ToDate", "Required");
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(Request.Form["halfday_from"]))
            {
                ModelState.AddModelError("halfday_from", "Required");
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(Request.Form["halfday_to"]))
            {
                ModelState.AddModelError("halfday_to", "Required");
            }

            if (string.IsNullOrWhiteSpace(l1.LeaveReason))
            {
                ModelState.AddModelError("LeaveReason", "Required");
            }
            //end...
            if (ModelState.IsValid)
            {
                if (l1.IsHalfDay == 0 || l1.IsHalfDay == 1)
                {
                    if (l1.IsHalfDay == 1)
                    {
                        l1.FromDate = Request.Form["halfday_from"].ToString();
                        string[] temp = l1.FromDate.Split(' ');
                        l1.ToDate = temp[0] + " " + Request.Form["halfday_to"].ToString();
                        Double hrs = lb.CalculateLeaveHours(l1);

                        if (hrs > 5 || hrs <= 0)
                        {
                            TempData["HrsError"] = true;
                            return RedirectToAction("Index", "ApplyForLeave", new { ViewId = 1 });
                        }

                    }
                    l1.EmployeeID = Session["EmpID"].ToString();
                    l1.ApplicationType = false;//that's means this is type application
                    lb.SaveApplication(l1);
                    l1.NotifyManager(((Employee)Session["Employee"]).GetManager(),(Employee)Session["Employee"]);
                }

            }
            else
            {
                ViewBag.Reasons = lb.GetReasons();
                ViewBag.Leavetypes = lb.GetLeaveTypes();
                ViewBag.ViewID = l1.IsHalfDay;
                return View("Index");
            }

            return RedirectToAction("Index", "ApplyForLeave");
        }
        [HttpPost]
        public ActionResult Calculateadays(LeaveApplication.Models.LeaveApplication l1)
        {
            return Json(lb.CalculateTotalLeaveDays(l1));
        }
        public ActionResult GetView(int? ViewId)
        {
            if (ViewId == null && ViewId == 0)
            {
                ViewBag.ViewID = 0;
            }
            else if (ViewId == 1)
            {
                ViewBag.ViewID = 1;
            }
            else
            {
                ViewBag.ViewID = 0;
            }
            ViewBag.Reasons = lb.GetReasons();
            ViewBag.Leavetypes = lb.GetLeaveTypes();
            return PartialView("LeaveForm");
        }
        public ContentResult mgr()
        {
            return Content(((Employee)Session["Employee"]).GetManager().Manager);
        }





    }
}