using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;

namespace LeaveApplication.Controllers
{
    public class ExperienceController : Controller
    {
        // GET: Experience
        public ActionResult Index()
        {
            Experience exp = new Experience() { EmployeeId = Convert.ToInt32(Session["EmpId"]) };
            ViewBag.data = exp.GetExperiences();
            return View();
        }

        public ActionResult AddExp(Experience exp)
        {
            try
            {
                exp.Fromdate = DateTimeHelper.yyyy_mm_dd(exp.Fromdate);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Fromdate", "Invalid Format");
            }
            try
            {
                exp.Todate = DateTimeHelper.yyyy_mm_dd(exp.Todate);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Todate", "Invalid Format");
            }
            if (ModelState.IsValid)
            {
                exp.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                exp.Insert();
            }
            else
            {
                return View("Index", exp);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdateExp(Experience exp, int? id)
        {
            try
            {
                exp.Fromdate = DateTimeHelper.yyyy_mm_dd(exp.Fromdate);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Fromdate", "Invalid Format");
            }
            try
            {
                exp.Todate = DateTimeHelper.yyyy_mm_dd(exp.Todate);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Todate", "Invalid Format");
            }
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    exp.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                    exp.ExperienceId = id.Value;
                    exp.UpdateExp();
                }
            }
            else
            {
                return View("Index", exp);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DelExp(int id)
        {
            Experience exp = new Experience() { ExperienceId = id, EmployeeId = Convert.ToInt32(Session["EmpId"]) };
            exp.DeleteExp();
            return RedirectToAction("Index");
        }

        public ActionResult Report()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                Experience exp = new Experience();
                System.Data.DataSet x = exp.GetExperiencesReport();

                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public JsonResult GetExp(int id)
        {
            Experience exp = new Experience() { ExperienceId = id, EmployeeId = Convert.ToInt32(Session["EmpId"]) };
            return Json(exp.GetExperience(), JsonRequestBehavior.AllowGet);
        }
    }
}