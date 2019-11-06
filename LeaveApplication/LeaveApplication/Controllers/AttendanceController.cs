using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
namespace LeaveApplication.Controllers
{
    public class AttandenceController : Controller
    {
        // GET: Attendance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAttandence()
        {
            return View();
        }
        public ActionResult SendNotifications(HttpPostedFileBase Attendance)
        {
            if (Session["Employee"] != null && ((Employee)Session["Employee"]).isAdmin == true)
            {
                Excels WorkBook = new Excels();
                return View("NotifiedList", WorkBook.Read(Attendance.InputStream));
            }
            return View("NotifiedList");
        }

    }
}