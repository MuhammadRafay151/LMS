using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using Newtonsoft.Json;

namespace LeaveApplication.Controllers
{
    public class ExperienceController : Controller
    {
        // GET: Experience
        public ActionResult Index()
        {
            if (Session["EmpID"] != null)
            {
                Experience exp = new Experience() { EmployeeId = Convert.ToInt32(Session["EmpId"]) };
                ViewBag.data = exp.GetExperiences();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult AddExp(Experience exp)
        {
            if (Session["EmpID"] != null)
            {
                Validation_Classes.Validation v1 = new Validation_Classes.Validation();
                v1.ValidateExp(exp, ModelState);
                if (ModelState.IsValid)
                {
                    exp.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                    exp.Insert();
                }
                else
                {
                    exp.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                    ViewBag.data = exp.GetExperiences();
                    ViewBag.OpenModel = true;
                    return View("Index", exp);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult UpdateExp(Experience exp, int? id)
        {
            if (Session["EmpID"] != null)
            {
                Validation_Classes.Validation v1 = new Validation_Classes.Validation();
                v1.ValidateExp(exp, ModelState);
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
                    if (id.HasValue)
                    {
                        exp.ExperienceId = id.Value;
                        exp.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                        ViewBag.data = exp.GetExperiences();
                        ViewBag.OpenModel = true;
                        return View("Index", exp);
                    }

                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult DelExp(int id)
        {
            if (Session["EmpID"] != null)
            {
                Experience exp = new Experience() { ExperienceId = id, EmployeeId = Convert.ToInt32(Session["EmpId"]) };
                exp.DeleteExp();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult Report()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                EmployeeBusinessLayer eb = new EmployeeBusinessLayer();
                ViewBag.Department = eb.GetDepartmentsDS();
                Experience exp = new Experience();
                System.Data.DataSet x = exp.GetExperiencesReport();

                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public JsonResult GetDepExp(int DepID)
        {
            Experience exp = new Experience();
            System.Data.DataSet x = exp.GetDepExperiencesReport(DepID);
            string z = JsonConvert.SerializeObject(x);
            return Json(z, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllExp()
        {
            Experience exp = new Experience();
            System.Data.DataSet x = exp.GetExperiencesReport();
            string z = JsonConvert.SerializeObject(x);
            return Json(z, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExp(int id)
        {
            Experience exp = new Experience() { ExperienceId = id, EmployeeId = Convert.ToInt32(Session["EmpId"]) };
            return Json(exp.GetExperience(), JsonRequestBehavior.AllowGet);
        }
    }
}