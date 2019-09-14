using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using LeaveApplication.Models;

namespace LeaveApplication.Controllers
{
    public partial class ViewApplicationsController : Controller
    {
        // GET: ViewApplications
        //partial class for user(faculty) level features
        LeaveBusinessLayer lb = new LeaveBusinessLayer();

        public ActionResult Index()
        {

            if (Session["EmpID"] != null)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult ALL()
        {

            return PartialView("ALL", lb.GetAllApplications(GetEmpID()));
        }
        public PartialViewResult Pending()
        {

            return PartialView("Pending", lb.GetPendingApplications(GetEmpID()));
        }
        public ActionResult Approved()
        {
            return PartialView("Approved", lb.GetApprovedApplications(GetEmpID()));
        }
        public PartialViewResult Rejected()
        {
            return PartialView("Rejected", lb.GetRejectedApplications(GetEmpID()));
        }
        public ActionResult EditDetails(string Application_Id)
        {
            if (Application_Id != null && Session["EmpID"] != null)
            {
                LeaveApplication.Models.LeaveApplication x = lb.GetApplication(Application_Id);
                if (x == null)
                {
                    return RedirectToAction("Index", "ViewApplications");
                }
                else
                {
                    ViewBag.Reasons = lb.GetReasons();
                    ViewBag.Leavetypes = lb.GetLeaveTypes();

                    return View(x);
                }
            }
            else
            {
                return RedirectToAction("Index", "ViewApplications");
            }
        }
        public ActionResult DetiledView(string Application_Id)
        {
            if (Application_Id != null && Session["EmpID"] != null)
            {
                LeaveApplication.Models.LeaveApplication x = lb.GetViewApplication(Application_Id);
                List<StatusHistory> a = lb.GetStatusHistory(Application_Id);

                ViewBag.SH = lb.GetStatusHistory(Application_Id);

                return View("ViewFullApplication",x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult CancelApplication(string Application_Id)
        {
            lb.CancelApplication(Application_Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SaveChanges(LeaveApplication.Models.LeaveApplication l1)
        {
            lb.SaveChanges(l1);

            return RedirectToAction("Index", "ViewApplications");
        }

        public ActionResult LeaveCount()
        {
            if (Session["EmpID"] != null)
            {
                ViewBag.LeaveCount = lb.GetLeaveCount();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }

        [NonAction]
        public string GetEmpID()
        {
            return Session["EmpID"].ToString();
        }


    }
}