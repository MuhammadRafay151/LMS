﻿using System;
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
        public ActionResult ALL(int? PageNo)
        {
            System.Data.DataSet ds = lb.GetAllApplications(GetEmpID());
            PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();

            if (PageNo.HasValue && PageNo.Value > 0)
            {

                ViewBag.PageNo = PageNo.Value;
               
                System.Data.DataSet ds1 = p1.GetPage(ds, 2, PageNo);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("ALL", ds1);
            }
            else
            {
                ViewBag.PageNo = 1;

                System.Data.DataSet ds1 = p1.GetPage(ds, 2, 1);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("ALL", ds1);
            }

        }
        public PartialViewResult Pending(int? PageNo)
        {
            System.Data.DataSet ds = lb.GetPendingApplications(GetEmpID());
            PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();

            if (PageNo.HasValue && PageNo.Value > 0)
            {

                ViewBag.PageNo = PageNo.Value;

                System.Data.DataSet ds1 = p1.GetPage(ds, 2, PageNo);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("Pending", ds1);
            }
            else
            {
                ViewBag.PageNo = 1;

                System.Data.DataSet ds1 = p1.GetPage(ds, 2, 1);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("Pending", ds1);
            }
           
        }
        public PartialViewResult Approved(int? PageNo)
        {
            System.Data.DataSet ds = lb.GetApprovedApplications(GetEmpID());
            PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();

            if (PageNo.HasValue && PageNo.Value > 0)
            {

                ViewBag.PageNo = PageNo.Value;

                System.Data.DataSet ds1 = p1.GetPage(ds, 2, PageNo);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("Approved", ds1);
            }
            else
            {
                ViewBag.PageNo = 1;

                System.Data.DataSet ds1 = p1.GetPage(ds, 2, 1);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("Approved", ds1);
            }

            
        }
        public PartialViewResult Rejected(int? PageNo)
        {
            System.Data.DataSet ds = lb.GetRejectedApplications(GetEmpID());
            PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();

            if (PageNo.HasValue && PageNo.Value > 0)
            {

                ViewBag.PageNo = PageNo.Value;

                System.Data.DataSet ds1 = p1.GetPage(ds, 2, PageNo);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("Rejected", ds1);
            }
            else
            {
                ViewBag.PageNo = 1;

                System.Data.DataSet ds1 = p1.GetPage(ds, 2, 1);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("Rejected", ds1);
            }
            
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

                return View("ViewFullApplication", x);
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