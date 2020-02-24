using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using Newtonsoft.Json;

namespace LeaveApplication.Controllers
{
    public class EducationController : Controller
    {
        // GET: Education
        private EmployeeBusinessLayer eb = new EmployeeBusinessLayer();

        private Education edu = new Education();

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