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
        public ActionResult ResetPassword()
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

        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ComfirmPassword)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && OldPassword == e1.Password)
            {

                if (NewPassword == ComfirmPassword)
                {
                    eb.ResetPassword(NewPassword, e1.UserName);
                }


                return RedirectToAction("ResetPassword", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }

    }
}