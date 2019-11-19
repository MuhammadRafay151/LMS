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
            if (Session["EmpID"] != null && ((Employee)Session["Employee"]).IsManager == true)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }
        public ActionResult FacultyAll(int? PageNo)
        {
            if (Session["EmpID"] != null && ((Employee)Session["Employee"]).IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyAll(((Employee)Session["Employee"]).EmployeeID);
                PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();
                ViewBag.Manager = true;
                if (PageNo.HasValue && PageNo.Value > 0)
                {

                    ViewBag.PageNo = PageNo.Value;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, PageNo);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("All", ds1);
                }
                else
                {
                    ViewBag.PageNo = 1;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, 1);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("All", ds1);
                }
            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyPending(int? PageNo)
        {


            if (Session["EmpID"] != null && ((Employee)Session["Employee"]).IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyPending(((Employee)Session["Employee"]).EmployeeID);

                PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();
                ViewBag.Manager = true;
                if (PageNo.HasValue && PageNo.Value > 0)
                {

                    ViewBag.PageNo = PageNo.Value;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, PageNo);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("Pending", ds1);
                }
                else
                {
                    ViewBag.PageNo = 1;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, 1);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("Pending", ds1);
                }



            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyApproved(int? PageNo)
        {
            if (Session["EmpID"] != null && ((Employee)Session["Employee"]).IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyApproved(((Employee)Session["Employee"]).EmployeeID);
                PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();
                ViewBag.Manager = true;
                if (PageNo.HasValue && PageNo.Value > 0)
                {

                    ViewBag.PageNo = PageNo.Value;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, PageNo);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("Approved", ds1);
                }
                else
                {
                    ViewBag.PageNo = 1;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, 1);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("Approved", ds1);
                }


            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyRejected(int? PageNo)
        {
            if (Session["EmpID"] != null && ((Employee)Session["Employee"]).IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyReject(((Employee)Session["Employee"]).EmployeeID);
                PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();
                ViewBag.Manager = true;
                if (PageNo.HasValue && PageNo.Value > 0)
                {

                    ViewBag.PageNo = PageNo.Value;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, PageNo);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("Rejected", ds1);
                }
                else
                {
                    ViewBag.PageNo = 1;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 5, 1);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return PartialView("Rejected", ds1);
                }

            }
            else
            {
                return Content("Access Denied");
            }

        }
        public ActionResult FacultyDetiledView(string Application_Id)
        {
            if (Application_Id != null && ((Employee)Session["Employee"]).IsManager)
            {
                LeaveApplication.Models.LeaveApplication x = lb.GetViewApplication(Application_Id);
                List<StatusHistory> a = lb.GetStatusHistory(Application_Id);

                ViewBag.SH = lb.GetStatusHistory(Application_Id);
                ViewBag.Manager = true;
                return View("ViewFullApplication", x);
            }
            else
            {
                return Content("Access Denied");
            }
        }
        [HttpPost]
        public ActionResult AcceptApplication(String Application_Id, string ManagerRemarks)
        {
            if (Session["EmpID"] == null)
            {
                return RedirectToAction("Index", "LogIn");
            }
            Application_Id = LeaveApplication.Models.Encryption.Base64Decode(Application_Id);
           
            try
            {

                int.Parse(Application_Id);
                if (((Employee)Session["Employee"]).IsManager && ManagerBusinessLayer.IsUnderManagement(Application_Id, ((Employee)Session["Employee"]).EmployeeID))
                {

                    lb.AcceptApplication(Application_Id, ManagerRemarks);
                    LeaveApplication.Models.LeaveApplication l1 = new Models.LeaveApplication();
                    l1.ApplicationId = Application_Id;
                    l1.NotifyAcceptedLeave();
                }

            }
            catch (FormatException)
            {
                return RedirectToAction("FacultyApplications");
            }

            return RedirectToAction("FacultyApplications");
        }
        public ActionResult RejectApplication(String Application_Id, string ManagerRemarks)
        {
            if (Session["EmpID"] == null)
            {
                return RedirectToAction("Index", "LogIn");
            }

            try
            {
                Application_Id = LeaveApplication.Models.Encryption.Base64Decode(Application_Id);
                int.Parse(Application_Id);
                if (((Employee)Session["Employee"]).IsManager && ManagerBusinessLayer.IsUnderManagement(Application_Id, ((Employee)Session["Employee"]).EmployeeID))
                {

                    lb.RejectApplication(Application_Id, ManagerRemarks);
                    LeaveApplication.Models.LeaveApplication l1 = new Models.LeaveApplication();
                    l1.ApplicationId = Application_Id;
                    l1.NotifyRejectedLeave();
                }

            }
            catch (FormatException)
            {
                return RedirectToAction("FacultyApplications");
            }

            return RedirectToAction("FacultyApplications");

        }

        public ActionResult FacultyLeaveCount()
        {

            if (Session["EmpID"] != null && ((Employee)Session["Employee"]).IsManager)
            {
                ViewBag.LeaveCount = false;
                return View("LeaveCount", lb.FacultyLeaveCount(((Employee)Session["Employee"]).EmployeeID));
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }
        public ActionResult FacultyLeaveBalance(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.HideCloseBtn = true;
                return PartialView("LeaveBalance", lb.GetBalance(id.Value, ((Employee)Session["Employee"]).EmployeeID));
            }
            else
            {
                return Content("Some thing going wrong");
            }

        }

    }
}