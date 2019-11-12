using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using System.IO;
using System.Data.SqlClient;


namespace LeaveApplication.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        EmployeeBusinessLayer eb = new EmployeeBusinessLayer();
        public ActionResult ResetPassword(string OldPassword,string NewPassword,string ComfirmPassword)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
    }
}