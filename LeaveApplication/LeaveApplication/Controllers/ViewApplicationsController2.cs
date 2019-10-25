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
        public ActionResult FacultyAll(int? PageNo)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyAll();
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


            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyPending();
               
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyApproved();
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager)
            {
                System.Data.DataSet ds = lb.GetFacultyReject();
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
                return Content("Access Denied");
            }
        }
        [HttpPost]
        public ActionResult AcceptApplication(String Application_Id,string ManagerRemarks)
        {
           
            lb.AcceptApplication(Application_Id,ManagerRemarks);
            return RedirectToAction("FacultyApplications");
        }
        public ActionResult RejectApplication(String Application_Id,string ManagerRemarks)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager == true)
            {
                
                lb.RejectApplication(Application_Id,ManagerRemarks);
                return RedirectToAction("FacultyApplications");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult FacultyLeaveCount()
        {

            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.IsManager == true)
            {
                ViewBag.LeaveCount = false;
                return View("LeaveCount", lb.FacultyLeaveCount());
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
                return PartialView("LeaveBalance", lb.GetBalance(id.Value.ToString()));
            }
            else
            {
                return Content("Some thing going wrong");
            }

        }

    }
}