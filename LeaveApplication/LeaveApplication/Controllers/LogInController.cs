using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using LeaveApplication.Models;
using System.Configuration;

namespace LeaveApplication.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
     
 
       EmployeeBusinessLayer e1 = new EmployeeBusinessLayer();
        public ActionResult Index()
        {
            ViewBag.Authentication =null;
            return View();
        }
        [HttpPost]
        public ActionResult Login(Employee e1)
        {

          
            if (this.e1.Authenticate(e1))
            {
                
                this.e1.ReadEmployee(e1.UserName);
                Session["EmpID"] = EmployeeBusinessLayer.Employee.EmployeeID;
                return RedirectToAction("Index", "ApplyForLeave");
               
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
            EmployeeBusinessLayer.Employee = null;
            return RedirectToAction("Index");
        }
    
       
    }
}