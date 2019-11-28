using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;

namespace LeaveApplication.Controllers
{
    public class DashboardController : Controller
    {
        EmployeeBusinessLayer eb = new EmployeeBusinessLayer();
        // GET: Dashboard
        public ActionResult ViewDashboard()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                return View(eb.GetAbsentees(e1.EmpNo));
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
    }
}