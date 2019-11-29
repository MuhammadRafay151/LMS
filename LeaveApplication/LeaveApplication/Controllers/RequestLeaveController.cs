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
                return View("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult Submit(LeaveApplication.Models.LeaveApplication l1)
        {
            //data validation
            if (string.IsNullOrWhiteSpace(l1.LeaveType))
            {
                ModelState.AddModelError("LeaveType", "Required");
            }
            if (l1.IsHalfDay == 0 && string.IsNullOrWhiteSpace(l1.FromDate))
            {
                ModelState.AddModelError("FromDate", "Required");
            }
            else if (l1.IsHalfDay == 0)
            {
                try
                {
                    DateTime.ParseExact(l1.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    ModelState.AddModelError("FromDate", "Date is not in correct format");
                }
            }
            if (l1.IsHalfDay == 0 && string.IsNullOrWhiteSpace(l1.ToDate))
            {
                ModelState.AddModelError("ToDate", "Required");
            }
            else if (l1.IsHalfDay == 0)
            {
                try
                {
                    DateTime.ParseExact(l1.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    ModelState.AddModelError("ToDate", "Date is not in correct format");
                }
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(l1.FromDate))
            {
                ModelState.AddModelError("FromDate", "Required");
            }
            else if (l1.IsHalfDay == 1)
            {
                try
                {
                    DateTime.ParseExact(l1.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    ModelState.AddModelError("FromDate", "Date is not in correct format");
                }
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(l1.FromTime))
            {
                ModelState.AddModelError("FromTime", "Required");
            }
            else if (l1.IsHalfDay == 1)
            {
                try
                {
                    DateTime.Parse(l1.FromTime);
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("FromTime", "Time is not in correct format");
                }
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(l1.ToTime))
            {
                ModelState.AddModelError("ToTime", "Required");
            }
            else if (l1.IsHalfDay == 1)
            {
                try
                {
                    DateTime.Parse(l1.ToTime);
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("ToTime", "Time is not in correct format");
                }
            }

            if (string.IsNullOrWhiteSpace(l1.LeaveReason))
            {
                ModelState.AddModelError("LeaveReason", "Required");
            }


            //end...

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
                            return Index(1);
                        }

                    }
                    else
                    {
                        if (lb.CalculateTotalLeaveDays(l1) <= 0)
                        {
                            TempData["HrsError"] = true;
                            return Index(0);
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

            TempData["Notify"] = true;
            return RedirectToAction("Index", "RequestLeave");
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
            ViewBag.Leavetypes = lb.GetRequestableLeaves();
            return PartialView("LeaveForm");
        }
    }
}