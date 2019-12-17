using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using LeaveApplication.Models;
using System.Configuration;
using System.Web;
using System.IO;
namespace LeaveApplication.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn


        EmployeeBusinessLayer e1 = new EmployeeBusinessLayer();
        public ActionResult Index()
        {
            ViewBag.Authentication = null;
            return View();
        }
        [HttpPost]
        public ActionResult Login(Employee e1)
        {


            if (this.e1.Authenticate(e1))
            {
                if(!this.e1.IsactiveEmployee(e1.UserName))
                {
                    return RedirectToAction("inactive");
                }
                this.e1.ReadEmployee(e1.UserName);

                Session["Employee"] = this.e1.ReadEmployee(e1.UserName);
                Employee e2 = (Employee)Session["Employee"];
                Session["EmpID"] = e2.EmployeeID;
                return RedirectToAction("Index", "Dashboard");

            }
            else
            {
                ModelState.AddModelError("EmployeeID", "Invalid Username Or Password");


            }
            return View("Index");
        }
        public ActionResult LogOff()
        {
            Session["EmpID"] = null;
            Session["Employee"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
        public FileResult ProfileImg()
        {//MimeMapping.GetMimeMapping()get content type from file name
            return File(((Employee)Session["Employee"]).ImageBytes, "image/jpeg");
            //MemoryStream m1 = new MemoryStream(((Employee)Session["Employee"]).ImageBytes);
            //return new FileStreamResult(m1, "image/jpeg");
        }
        public ActionResult inactive()
        {
            return Content("The account has been disabled try to contact HR in case of any misconception");
        }

    }
}