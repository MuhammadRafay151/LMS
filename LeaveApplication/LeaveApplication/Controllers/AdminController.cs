﻿using System;
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
            return RedirectToAction("Designation");
        }
        [HttpPost]
        public ActionResult DeleteDesignation(Designation ds)
        {
           
            ad.DeleteDesignation(ds);
            return RedirectToAction("Designation");
        }
        [HttpPost]
        public ActionResult UpdateDesignation(Designation ds)
        {

            ad.updateDesignation(new Designation() { DesignationID =int.Parse( Request.Form["id"].ToString()), designation = Request.Form["edittxt"].ToString() });
            return RedirectToAction("Designation");
        }
        public ActionResult AddLeaveType(LeaveTypes lt)
        {
           
            ad.AddLeaveType(lt);
            return RedirectToAction("LeaveType");
        }
        [HttpPost]
        public ActionResult DeleteLeaveType(LeaveTypes lt)
        {

            ad.DeleteLeaveType(lt);
            return RedirectToAction("LeaveType");
        }
        [HttpPost]
        public ActionResult UpdateLeaveType(LeaveTypes lt)
        {

            ad.updateLeaveType(new LeaveTypes() { LeaveTypeID = int.Parse(Request.Form["id"].ToString()), LeaveType = Request.Form["edittxt"].ToString() });
            return RedirectToAction("LeaveType");
        }
     
        public ActionResult AssignLeave()
        {
            List<System.Data.DataSet> d1 = new List<System.Data.DataSet>();
            d1.Add(lb.GetLeaveTypesDS());
            d1.Add(eb.GetDepartmentsDS());
            if(TempData["ValidationError"]==null)
            {
                ViewBag.ValidationError = false;
            }
            else
            {
                ViewBag.ValidationError = true;
            }
            return View(d1);
        }
        [HttpPost]
        public JsonResult GetEmployees(Department d1)
        {
            return Json(eb.GetEmployeesList(d1.DepartmentId));
        }
        [HttpPost]
        public ActionResult Submit(EmployeeLeaveCount el)
        {
            try
            {
                el.EmployeeID = Request.Form["emp"].ToString();
                el.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                el.Count = int.Parse(Request.Form["count"].ToString());
                ad.AssignLeave(el);
                
                return RedirectToAction("AssignLeave");
            }
            catch(NullReferenceException)
            {
                TempData["ValidationError"] = true;
               
                return RedirectToAction("AssignLeave");
            }
          
           
        }
    }
}