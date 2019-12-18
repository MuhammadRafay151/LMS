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
        public ActionResult Index()
        {
            
            return ViewDashboard();
        }
        // GET: Dashboard
        public ActionResult ViewDashboard()
        {
          
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
               
                return View("ViewDashboard", eb.GetAbsentees(e1.EmployeeID));
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult CloseAbsentNotification()
        {
            return ViewDashboard();
        }
    }
}