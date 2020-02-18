using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using System.IO;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace LeaveApplication.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        private EmployeeBusinessLayer eb = new EmployeeBusinessLayer();

        private Education edu = new Education();

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

        public ActionResult Education()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                ViewBag.List = edu.GetDegrees();
                System.Data.DataSet x = edu.GetEducation(e1.EmployeeID);

                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult AddEducation(Education eu)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                edu.AddEducation(eu, e1.EmployeeID);
                return RedirectToAction("Education", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult DeleteEducation(int EduID)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                edu.DeleteEducation(e1.EmployeeID, EduID);
                return RedirectToAction("Education", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult UpdateEducation(Education eu)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                edu.UpdateEducation(eu, e1.EmployeeID);
                return RedirectToAction("Education", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public JsonResult getedu(int? EduID)
        {
            Employee e1 = (Employee)Session["Employee"];
            System.Data.DataSet x = edu.GetEdu(e1.EmployeeID, Convert.ToInt32(EduID));
            //x.Tables[0].
            string z = JsonConvert.SerializeObject(x);

            return Json(z, JsonRequestBehavior.AllowGet);
        }
    }
}