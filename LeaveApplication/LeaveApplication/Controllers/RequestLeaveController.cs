using System;
using System.Collections.Generic;
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

                return View(lb.GetRequestableLeaves());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult Submit(LeaveApplication.Models.LeaveApplication l1)
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
                        return RedirectToAction("Index", "RequestLeave", new { ViewId = 1 });
                    }

                }
                l1.ApplicationType = true;//that's means this is type request...
                l1.EmployeeID = Session["EmpID"].ToString();
                lb.SaveApplication(l1);

            }

           
            return RedirectToAction("Index");
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
    }
}