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
                Session["FileName"] = string.Empty;
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

                System.Data.DataSet ds1 = p1.GetPage(ds, 5, PageNo);
                ViewBag.TotalPages = p1.GetTotalPages();
                return PartialView("ALL", ds1);
            }
            else
            {
                ViewBag.PageNo = 1;

                System.Data.DataSet ds1 = p1.GetPage(ds, 5, 1);
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
        public PartialViewResult Approved(int? PageNo)
        {
            System.Data.DataSet ds = lb.GetApprovedApplications(GetEmpID());
            PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();

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
        public PartialViewResult Rejected(int? PageNo)
        {
            System.Data.DataSet ds = lb.GetRejectedApplications(GetEmpID());
            PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();

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
        public ActionResult EditDetails(string Application_Id)
        {

            if (Application_Id != null && Session["EmpID"] != null)
            {
                Application_Id = LeaveApplication.Models.Encryption.Base64Decode(Application_Id);
                try
                {

                    int.Parse(Application_Id);
                    LeaveApplication.Models.LeaveApplication x = lb.GetApplication(Application_Id);
                    Session["EditLeave"] = x;
                    if (x == null)
                    {
                        return RedirectToAction("Index", "ViewApplications");
                    }
                    else
                    {
                        ViewBag.Reasons = lb.GetReasons();
                        ViewBag.Leavetypes = lb.GetLeaveTypes();

                        if (TempData["HrsError"] != null && Convert.ToBoolean(TempData["HrsError"]) == true)
                        {
                            ViewBag.HrsError = true;
                        }
                        else
                        {
                            ViewBag.HrsError = false;
                        }
                        return View(x);
                    }

                }
                catch (FormatException)
                {
                    return RedirectToAction("FacultyApplications");
                }

               
            }
            else
            {
                return RedirectToAction("Index", "ViewApplications");
            }
        }
        public ActionResult DetiledView(string Application_Id)
        {

            if (Session["EmpID"] == null)
            {
                return RedirectToAction("Index", "LogIn");
            }
            if (Application_Id != null && Session["EmpID"] != null)
            {
                Application_Id = LeaveApplication.Models.Encryption.Base64Decode(Application_Id);

                try
                {
                    int.Parse(Application_Id);
                }
                catch (FormatException)
                {
                    return RedirectToAction("Index", "ViewApplications");
                }
                LeaveApplication.Models.LeaveApplication x = lb.GetViewApplication(Application_Id);
                List<StatusHistory> a = lb.GetStatusHistory(Application_Id);

                ViewBag.SH = a; // lb.GetStatusHistory(Application_Id);
                // Session["FileName"] = x.FileName;
                return View("ViewFullApplication", x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        public ActionResult CancelApplication(string Application_Id)
        {
            try
            {
                Application_Id = LeaveApplication.Models.Encryption.Base64Decode(Application_Id);
                int.Parse(Application_Id);
                lb.CancelApplication(Application_Id);
                return RedirectToAction("Index");
            }
            catch (FormatException)
            {
                return RedirectToAction("Index");
            }
           
        }
        [HttpPost]
        public ActionResult SaveChanges(LeaveApplication.Models.LeaveApplication l1)
        {

            if (Session["EmpID"] == null)
            {
                return RedirectToAction("Index", "LogIn");
            }
            bool IsDeletedFile = Convert.ToBoolean(Request.Form["IsDeleted"]);
            l1.ApplicationId = ((LeaveApplication.Models.LeaveApplication)Session["EditLeave"]).ApplicationId;
            l1.TotalDays = ((LeaveApplication.Models.LeaveApplication)Session["EditLeave"]).TotalDays;
            l1.FileId = ((LeaveApplication.Models.LeaveApplication)Session["EditLeave"]).FileId;

            if (((LeaveApplication.Models.LeaveApplication)Session["EditLeave"]).TotalDays == 0.5)
            {
                l1.FromDate = Request.Form["halfday_from"].ToString();
                string[] temp = l1.FromDate.Split(' ');
                l1.ToDate = temp[0] + " " + Request.Form["halfday_to"].ToString();

                Double hrs = lb.CalculateLeaveHours(l1);

                if (hrs > 5 || hrs <= 0)
                {

                    TempData["HrsError"] = true;
                    return RedirectToAction("EditDetails", "ViewApplications", new { Application_Id = l1.ApplicationId });
                }
                lb.SaveChanges(l1, IsDeletedFile);
            }
            else
            {
                lb.SaveChanges(l1, IsDeletedFile);
            }

            Session.Remove("EditLeave");
            return RedirectToAction("Index", "ViewApplications");
        }
        public ActionResult LeaveCount()
        {
            if (Session["EmpID"] != null)
            {
                ViewBag.LeaveCount = true;
                return View(lb.GetLeaveCount(((Employee)Session["Employee"]).EmployeeID));
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }
        public ActionResult LeaveBalance()
        {
            ViewBag.HideCloseBtn = false;
            return PartialView(lb.GetLeaveBalance(((Employee)Session["Employee"]).EmployeeID));


        }

        [NonAction]
        public string GetEmpID()
        {
            return Session["EmpID"].ToString();
        }
        public ActionResult DownLoadFile(string Fileid)
        {

            if (Session["EmpID"] != null)
            {

                DataSet ds = lb.DownloadFile(Convert.ToInt32(Encryption.Base64Decode(Fileid)));
                return File((Byte[])ds.Tables[0].Rows[0][0], System.Web.MimeMapping.GetMimeMapping(ds.Tables[0].Rows[0][1].ToString()), ds.Tables[0].Rows[0][1].ToString());

            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }


    }
}