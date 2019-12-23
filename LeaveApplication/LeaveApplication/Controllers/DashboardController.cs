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
                LeaveBusinessLayer lb = new LeaveBusinessLayer();
                System.Data.DataSet ds = eb.GetAbsents(e1.EmployeeID);
                ds.Tables[0].TableName = "asd";
                if (e1.isAdmin)
                {
                    ds.Tables.Add(lb.ManagersPendings().Tables[0].Copy());
                }

                if (e1.IsManager)
                {
                    ViewBag.PendingCount = lb.FacultyPendingApplications_Count(e1.EmployeeID);
                }

                return View("ViewDashboard", ds);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult CloseAbsentNotification(int? id)
        {
            if (Session["EmpID"] == null)
            {
                return Content("Access denied");
            }
            if (id != null && id.Value > 0)
            {
                Attendance a1 = new Attendance();
                a1.CloseNotification(id.Value, Convert.ToInt32(Session["EmpID"]));
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Content("Invalid Argument");

        }
    }
}