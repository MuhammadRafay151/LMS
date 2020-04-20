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

        public ActionResult Index()
        {
            if (Session["EmpID"] != null)
            {
                Education edu = new Education() { EmployeeID = Convert.ToInt32(Session["EmpId"]) };
                ViewBag.List = edu.GetDegrees();
                System.Data.DataSet x = edu.GetEducation();

                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult AddEducation(Education edu)
        {
            if (Session["EmpID"] != null)
            {
                if (ModelState.IsValid)
                {
                    edu.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    edu.AddEducation();
                }
                else
                {
                    edu.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    ViewBag.List = edu.GetDegrees();
                    System.Data.DataSet x = edu.GetEducation();
                    return View("Index", x);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult DeleteEducation(int EduID)
        {
            if (Session["EmpID"] != null)
            {
                Education edu = new Education() { EmployeeID = Convert.ToInt32(Session["EmpId"]), EduID = EduID };
                edu.DeleteEducation();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult UpdateEducation(Education edu)
        {
            if (Session["EmpID"] != null)
            {
                if (ModelState.IsValid)
                {
                    edu.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    edu.UpdateEducation();
                }
                else
                {
                    edu.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    ViewBag.List = edu.GetDegrees();
                    System.Data.DataSet x = edu.GetEducation();
                    return View("Index", x);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public JsonResult getedu(int EduID)
        {
            Education edu = new Education() { EmployeeID = Convert.ToInt32(Session["EmpId"]), EduID = EduID };
            System.Data.DataSet x = edu.GetEdu();
            string z = JsonConvert.SerializeObject(x);

            return Json(z, JsonRequestBehavior.AllowGet);
        }
    }
}