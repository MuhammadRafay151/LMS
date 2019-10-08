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
        public ActionResult Index()
        {
            if (Session["EmpID"] != null)
            {
                ViewBag.Reasons = lb.GetReasons();
                ViewBag.Leavetypes = lb.GetLeaveTypes();
                return View(lb.GetRequestableLeaves());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult Submit(LeaveApplication.Models.LeaveApplication l1)
        {
            l1.ApplicationType = true;//that's means this is type request...
            l1.EmployeeID = Session["EmpID"].ToString();
            lb.SaveApplication(l1);
            return RedirectToAction("Index");
        }
    }
}