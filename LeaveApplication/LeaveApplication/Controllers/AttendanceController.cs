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
        public ActionResult ViewAttandence(HttpPostedFileBase Attendance)
        {
            if (Session["Employee"] != null && ((Employee)Session["Employee"]).isAdmin == true)
            {
                Excels WorkBook = new Excels();
                List < Attendance > x= WorkBook.Read(Attendance.InputStream);
                //Get data for only absecent employees....
                Session["AttFile"] = x;
                return View("ViewAttandence",x);
            }
            return RedirectToAction("Index", "ApplyForLeave");
        
        }
        public ActionResult SendNotifications()
        {
            List<Attendance> x = (List<Attendance>)Session["AttFile"];
            foreach(Attendance i in x)
            {
                i.NotifyAbsentees();
            }
            Session.Remove("AttFile");
            return RedirectToAction("Index");
        }

    }
}