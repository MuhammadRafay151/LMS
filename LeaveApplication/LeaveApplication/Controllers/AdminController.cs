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
            ViewBag.leaves_t = lb.GetLeaveTypes();
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

            ad.updateDeparment(new Models.Department() { DepartmentId = Request.Form["id"].ToString(), department = Request.Form["edittxt"].ToString() });
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

            ad.updateDesignation(new Designation() { DesignationID = int.Parse(Request.Form["id"].ToString()), designation = Request.Form["edittxt"].ToString() });
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
            if (TempData["ValidationError"] == null)
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
        public ActionResult AffectedUsers()
        {
            AssignLeaves al = new AssignLeaves();
            AdminBusinessLayer.Al = null;
            System.Data.DataSet ds = null;
            string Querry = string.Empty;
            al.AssignType = Request.Form["customRadio"].ToString();
            try
            {
                if (al.AssignType == "Select Employee")
                {
                    al.EmployeeID = Request.Form["emp"].ToString();
                    al.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                    al.Count = int.Parse(Request.Form["count"].ToString());
                    Querry = string.Format("Select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}' where EmployeeID='{1}'",al.LeaveTypeID, al.EmployeeID);
                    ds = ad.ShowAffectedUsers(al, Querry);
                }
                else if (al.AssignType == "All(Select Department)")
                {
                    al.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                    al.Count = int.Parse(Request.Form["count"].ToString());
                    al.DepartmentID = Request.Form["dep"].ToString();
                  
                    Querry = string.Format("select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}' where Departments.DepartmentID='{1}'", al.LeaveTypeID,al.DepartmentID);
                   
                    ds = ad.ShowAffectedUsers(al, Querry);

                }
                else if (al.AssignType == "All Employess")
                {
                    al.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                    al.Count = int.Parse(Request.Form["count"].ToString());
                    Querry = string.Format("select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}'", al.LeaveTypeID);
                    ds = ad.ShowAffectedUsers(al, Querry);

                }
                
                ViewBag.Count = al.Count;
                return View(ds);
            }
            catch (NullReferenceException)
            {
                TempData["ValidationError"] = true;

                return RedirectToAction("AssignLeave");
            }


        }
        public ActionResult Submit()
        {


            if(AdminBusinessLayer.Al!=null)
            {
                if (AdminBusinessLayer.Al.AssignType == "Select Employee")
                {
                    ad.AssignLeave();
                }
                else if (AdminBusinessLayer.Al.AssignType == "All(Select Department)")
                {
                    ad.AssignAllDep();

                }
                else if (AdminBusinessLayer.Al.AssignType == "All Employess")
                {
                    ad.AssignAll();
                }
                ad.RemoveAssignLeaveRequest();

            }

            return RedirectToAction("AssignLeave");



        }
        public ActionResult CancelAssignLeave()
        {
            AdminBusinessLayer.Al = null;
            return RedirectToAction("AssignLeave");
        }
        //public ActionResult Submit()
        //{
        //    EmployeeLeaveCount el = new EmployeeLeaveCount();
        //    try
        //    {
        //        if (Request.Form["customRadio"].ToString() == "Select Employee")
        //        {
        //            el.EmployeeID = Request.Form["emp"].ToString();
        //            el.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
        //            el.Count = int.Parse(Request.Form["count"].ToString());
        //            ad.AssignLeave(el);
        //        }
        //        else if (Request.Form["customRadio"].ToString() == "All(Select Department)")
        //        {
        //            el.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
        //            el.Count = int.Parse(Request.Form["count"].ToString());
        //            ad.AssignAllDep(el, Request.Form["dep"].ToString());
        //        }
        //        else if (Request.Form["customRadio"].ToString() == "All Employess")
        //        {
        //            el.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
        //            el.Count = int.Parse(Request.Form["count"].ToString());
        //            ad.AssignAll(el);
        //        }


        //        return RedirectToAction("AssignLeave");
        //    }
        //    catch (NullReferenceException)
        //    {
        //        TempData["ValidationError"] = true;

        //        return RedirectToAction("AssignLeave");
        //    }


        //}
        public ActionResult Assign_Leave_History()
        {
            return View(ad.ShowAssignLeaveHistory());
        }

    }
}