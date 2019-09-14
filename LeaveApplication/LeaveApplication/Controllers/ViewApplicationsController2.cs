using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
namespace LeaveApplication.Controllers
{
    public partial class ViewApplicationsController : Controller
    {

        //partial class for hod level features
      
        public ActionResult FacultyApplications()
        {
            if (Session["EmpID"] != null && Session["Manager"] == null)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }
        public ActionResult FacultyAll()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                ViewBag.Manager = true;
                return PartialView("All", lb.GetFacultyAll());
            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyPending()
        {


            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                ViewBag.Manager = true;
                return PartialView("Pending", lb.GetFacultyPending());


            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyApproved()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                ViewBag.Manager = true;
                return PartialView("Approved", lb.GetFacultyApproved());
            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyRejected()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                ViewBag.Manager = true;
                return PartialView("Rejected", lb.GetFacultyReject());
            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyDetiledView(string Application_Id)
        {
            if (Application_Id != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                LeaveApplication.Models.LeaveApplication x = lb.GetViewApplication(Application_Id);
                List<StatusHistory> a = lb.GetStatusHistory(Application_Id);

                ViewBag.SH = lb.GetStatusHistory(Application_Id);
                ViewBag.Manager = true;
                return View("ViewFullApplication", x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult AcceptApplication(String Application_Id)
        {
            lb.AcceptApplication(Application_Id);
            return RedirectToAction("FacultyApplications");
        }
        public ActionResult RejectApplication(String Application_Id)
        {

            lb.RejectApplication(Application_Id);
            return RedirectToAction("FacultyApplications");
        }
      
        public ActionResult FacultyLeaveCount()
        {

            if (Session["EmpID"] != null)
            {

                ViewBag.LeaveCount = lb.FacultyLeaveCount();
                
                return View("LeaveCount");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }
        //private ActionResult Faculty()
        //{
        //    if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
        //    {

        //        ViewBag.FacultyList = lb.GetFaculty();
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "LogIn");
        //    }

        //}
    }
}