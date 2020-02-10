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
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword, string ComfirmPassword)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                if (eb.UserPassword(e1.UserName) == eb.MD5Hash(CurrentPassword))
                {
                    if (NewPassword == ComfirmPassword)
                    {
                        eb.ResetPassword(NewPassword, e1.UserName);
                    }
                    else
                    {
                        ModelState.AddModelError("ComfirmPassword", "Confirm Password not matched");
                        return View("ResetPassword");
                    }
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "Current Password is not Correct");
                    return View("ResetPassword");
                }
                TempData["Notify"] = true;
                return RedirectToAction("ResetPassword", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }

        public ActionResult Basicinfo()
        {

            if (Session["EmpID"] == null)
            {
                return RedirectToAction("Index", "LogIn");
            }



            BasicInfo b1 = new BasicInfo((LeaveApplication.Models.Employee)Session["Employee"]);

            return View(b1);
        }
        public ActionResult Edit_Info_Submit(BasicInfo b1)
        {
            if (ModelState.IsValid)
            {

            }
            return RedirectToAction("Basicinfo");
        }


    }
}