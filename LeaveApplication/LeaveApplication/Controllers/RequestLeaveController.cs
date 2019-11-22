using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
namespace LeaveApplication.Controllers
{
    public class RequestLeaveController : Controller
    {
        // GET: RequestLeave
        LeaveBusinessLayer lb = new LeaveBusinessLayer();
        public ActionResult Index(int? ViewId)
        {
            if (Session["EmpID"] != null)
            {
                ViewBag.Reasons = lb.GetReasons();
                ViewBag.Leavetypes = lb.GetRequestableLeaves();
                if (ViewId == null || ViewId == 0)
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
                if (TempData["HrsError"] != null && Convert.ToBoolean(TempData["HrsError"]) == true)
                {
                    ViewBag.HrsError = true;
                }
                else
                {
                    ViewBag.HrsError = false;
                }
                return View(lb.GetRequestableLeaves());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult Submit(LeaveApplication.Models.LeaveApplication l1)
        {
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
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(Request.Form["Date"]))
            {
                ModelState.AddModelError("Date", "Required");
            }
            else if(l1.IsHalfDay == 1)
            {
                try
                {
                    DateTime.ParseExact(Request.Form["Date"], "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    ModelState.AddModelError("Date", "Date is not in correct format");
                }
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(Request.Form["halfday_from"]))
            {
                ModelState.AddModelError("halfday_from", "Required");
            }
            else if (l1.IsHalfDay == 1)
            {
                try
                {
                    DateTime.Parse(Request.Form["halfday_from"].ToString());
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("halfday_from", "Time is not in correct format");
                }
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(Request.Form["halfday_to"]))
            {
                ModelState.AddModelError("halfday_to", "Required");
            }
            else if (l1.IsHalfDay == 1)
            {
                try
                {
                    DateTime.Parse(Request.Form["halfday_to"].ToString());
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("halfday_to", "Time is not in correct format");
                }
            }

            if (string.IsNullOrWhiteSpace(l1.LeaveReason))
            {
                ModelState.AddModelError("LeaveReason", "Required");
            }
            if (ModelState.IsValid)
            {
                if (l1.IsHalfDay == 0 || l1.IsHalfDay == 1)
                {
                    if (l1.IsHalfDay == 1)
                    {

                        l1.FromDate = Request.Form["Date"].ToString() + " " + Request.Form["halfday_from"].ToString();

                        l1.ToDate = Request.Form["Date"].ToString() + " " + Request.Form["halfday_to"].ToString();
                        Double hrs = lb.CalculateLeaveHours(l1);
                        if (hrs <= 0)
                        {
                            TempData["HrsError"] = true;
                            return RedirectToAction("Index", "RequestLeave", new { ViewId = 1 });
                        }

                    }
                    else
                    {
                        if (lb.CalculateTotalLeaveDays(l1) <= 0)
                        {
                            TempData["HrsError"] = true;
                            return RedirectToAction("Index", "RequestLeave", new { ViewId = 0 });
                        }
                    }
                    l1.ApplicationType = true;//that's means this is type request...
                    l1.EmployeeID = Session["EmpID"].ToString();
                   
                    lb.SaveApplication(l1);

                }
            }
            else
            {
                ViewBag.Reasons = lb.GetReasons();

                ViewBag.ViewID = l1.IsHalfDay;
                return View("Index", lb.GetRequestableLeaves());
            }


            return RedirectToAction("Index", "ViewApplications");
        }
        public ActionResult GetView(int? ViewId)
        {
            if (ViewId == null || ViewId == 0)
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

            return PartialView("LeaveForm", lb.GetRequestableLeaves());
        }
    }
}