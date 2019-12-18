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
                int Empno = eb.GetEmpNo(e1.EmployeeID);
                return View(eb.GetAbsentees(Empno));
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
    }
}