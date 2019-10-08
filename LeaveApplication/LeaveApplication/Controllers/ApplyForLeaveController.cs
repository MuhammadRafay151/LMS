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
        public ActionResult Index()
        {
            if (Session["EmpID"] != null)
            {
               
                ViewBag.Reasons = lb.GetReasons();
                ViewBag.Leavetypes = lb.GetLeaveTypes();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult Submit(LeaveApplication.Models.LeaveApplication l1)
        {

            if (l1.IsHalfDay == 0 || l1.IsHalfDay == 1)
            {
                if (l1.IsHalfDay == 1)
                {
                    l1.FromDate = Request.Form["halfday"].ToString();
                    l1.ToDate = Request.Form["halfday"].ToString();
                }
                l1.EmployeeID = Session["EmpID"].ToString();
                l1.ApplicationType = false;//that's means this is type application
                lb.SaveApplication(l1);
            }

            return RedirectToAction("Index", "ApplyForLeave");
        }
        [HttpPost]
        public ActionResult Calculateadays(LeaveApplication.Models.LeaveApplication l1)
        {
            return Json(lb.CalculateTotalLeaveDays(l1));
        }





    }
}