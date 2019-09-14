using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
namespace LeaveApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        LeaveBusinessLayer lb = new LeaveBusinessLayer();
        EmployeeBusinessLayer eb = new EmployeeBusinessLayer();
        AdminBusinessLayer ad = new AdminBusinessLayer();
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult LeaveType()
        {
           ViewBag.leaves_t= lb.GetLeaveTypes();
            return View();
        }
        public ActionResult Designation()
        {
            return View(eb.GetDesignation());
        }
        public ActionResult Department()
        {
            ViewBag.Departments = eb.GetDepartments();
            return View();
        }
        [HttpPost]
        public ActionResult AddDepartment(Department dp)
        {
            ad.AddDeparment(dp.department);
            return RedirectToAction("Department");
        }
        [HttpPost]
        public ActionResult DeleteDepartment(Department dp)
        {
            ad.DeleteDeparment(dp.DepartmentId);
            return RedirectToAction("Department");
        }
        [HttpPost]
        public ActionResult UpdateDepartment()
        {
           
            ad.updateDeparment(new Models.Department() {DepartmentId=Request.Form["id"].ToString(), department = Request.Form["edittxt"].ToString() });
            return RedirectToAction("Department");
        }
        [HttpPost]
        public ActionResult AddDesignation(Designation ds)
        {
            ad.AddDesignation(ds);
            return RedirectToAction("Department");
        }
        [HttpPost]
        public ActionResult DeleteDesignation(Designation ds)
        {
           
            ad.DeleteDesignation(ds);
            return RedirectToAction("Department");
        }
        [HttpPost]
        public ActionResult UpdateDesignation(Designation ds)
        {

            ad.updateDeparment(new Models.Department() { DepartmentId = Request.Form["id"].ToString(), department = Request.Form["edittxt"].ToString() });
            return RedirectToAction("Department");
        }
    }
}