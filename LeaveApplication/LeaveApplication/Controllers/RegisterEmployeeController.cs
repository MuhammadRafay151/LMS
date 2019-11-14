using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.SqlClient;
using LeaveApplication.Models;
using System.Drawing;
using System.Web.UI;
using System.Configuration;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace LeaveApplication.Controllers
{
    public class RegisterEmployeeController : Controller
    {

        EmployeeBusinessLayer emp = new EmployeeBusinessLayer();
        // GET: RegisterEmployee
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Registeration()
        {
            ViewBag.Employees = emp.GetEmployees();
            ViewBag.List = emp.GetDesignation();
            ViewBag.List2 = emp.GetDepartments();
            return View();
        }

        public JsonResult data()
        {
            List<Employee> e1 = new List<Employee>();
            e1.Add
                (new Employee());
            e1.Add
               (new Employee());
            var x = JsonConvert.SerializeObject(e1);
            return Json(new Employee(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(Employee e1)
        {
            
            //   string text = e1.EmployeeID + "<br />" + e1.EmployeeID + "<br />" + "<br />" + e1.Address + "<br />" + e1.CNIC + "<br />" + e1.DateOfJoining + "<br />" + e1.Password + "<br />" + e1.Picture;
            //return RedirectToAction("Registeration");
            if (ModelState.IsValid)
            {
                try
                {
                    emp.Register(e1);
                    //   string text = e1.EmployeeID + "<br />" + e1.EmployeeID + "<br />" + "<br />" + e1.Address + "<br />" + e1.CNIC + "<br />" + e1.DateOfJoining + "<br />" + e1.Password + "<br />" + e1.Picture;
                    return RedirectToAction("Registeration");
                }
                catch (SqlException e)
                {
                   return Content(e.Message);
                   
                }
                catch(LeaveApplication.Exceptional_Classes.DuplicateException e )
                {if(e.ExceptionID==1)
                    ModelState.AddModelError("UserName", "User Name is Not Available");
                else if (e.ExceptionID == 2)
                    ModelState.AddModelError("EmpNo", "Employee Number is already in use");
                    ViewBag.Employees = emp.GetEmployees();
                    ViewBag.List = emp.GetDesignation();
                    ViewBag.List2 = emp.GetDepartments();
                    return View("Registeration");
                }

            }
            else
            {
                ViewBag.Employees = emp.GetEmployees();
                ViewBag.List = emp.GetDesignation();
                ViewBag.List2 = emp.GetDepartments();
                return View("Registeration");
            }

        }









    }
}