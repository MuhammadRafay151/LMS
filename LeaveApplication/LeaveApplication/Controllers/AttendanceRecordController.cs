using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeaveApplication.Models;
using System.Web.Mvc;

namespace LeaveApplication.Controllers
{
    public class AttendanceRecordController : Controller
    {
        EmployeeBusinessLayer eb = new EmployeeBusinessLayer();

        // GET: AttendanceRecord
        public ActionResult ViewAttendanceRecord()
        {

            if (Session["Employee"] != null )
            {
                EmployeeBusinessLayer eb = new EmployeeBusinessLayer();
                Employee e1 = (Employee)Session["Employee"];
               

                return View(eb.GetAttendance(e1.EmployeeID));

            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

            
        }

    }
}