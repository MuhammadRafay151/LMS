﻿using System;
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
    public class AdminController : Controller
    {
        // GET: Admin

        private LeaveBusinessLayer lb = new LeaveBusinessLayer();
        private EmployeeBusinessLayer eb = new EmployeeBusinessLayer();
        private AdminBusinessLayer ad = new AdminBusinessLayer();

        public ActionResult Index()
        {
            return Content(Session["EmpID"].ToString());
        }

        public ActionResult LeaveType()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                return View(lb.GetLeaveTypesDS());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult Designation(int? PageNo)
        {
            Employee e1 = (Employee)Session["Employee"];
            Pagination.Pagination p1 = new Pagination.Pagination();
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                if (PageNo == null)
                {
                    PageNo = 1;
                }
                List<Designation> x = eb.GetDesignation(p1, PageNo.Value);
                if (x == null)
                {
                    return RedirectToAction("Designation");
                }
                else
                {
                    ViewBag.PageNo = PageNo.Value;
                    ViewBag.TotalPages = p1.TotalPages;
                }
                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult Department()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                ViewBag.Departments = eb.GetDepartments();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult Employees(int? PageNo)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                if (PageNo == null)
                {
                    PageNo = 1;
                }
                System.Data.DataSet x = eb.GetEmployeesDs(PageNo.Value);

                if (x == null)
                {
                    return RedirectToAction("Employees");
                    //x = eb.GetEmployeesDs(1);
                    //ViewBag.PageNo = 1;
                    //ViewBag.TotalPages = Convert.ToInt32(x.Tables[1].Rows[0][0]);
                }
                else
                {
                    ViewBag.PageNo = PageNo.Value;
                    ViewBag.TotalPages = Convert.ToInt32(x.Tables[1].Rows[0][0]);
                }

                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult EditEmployees(int EmployeeID)
        {
            Employee e2 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e2.isAdmin == true)
            {
                Employee e1 = eb.GetEmployeeData(EmployeeID);
                ViewBag.Employees = eb.GetEmployees();
                ViewBag.List = eb.GetDesignation();
                ViewBag.List2 = eb.GetDepartments();

                TempData["Image"] = e1.ImageBytes;
                return View(e1);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public void EmployeeStateChange(string EmployeeID, bool IsActive)
        {
            ad.EmployeeStateChange(EmployeeID, IsActive);
        }

        public ActionResult UpdateEmployee(Employee e1)
        {
            //select EmpNo from Employee where EmpNo = 10 and EmployeeID != 6
            //use above querry

            EmployeeBusinessLayer emp = new EmployeeBusinessLayer();
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                try
                {
                    ad.UpdateEmployee(e1);
                    return RedirectToAction("Employees");
                }
                catch (SqlException e)
                {
                    return Content(e.Message);
                }
                catch (LeaveApplication.Exceptional_Classes.DuplicateException e)
                {
                    if (e.ExceptionID == 1)
                        ModelState.AddModelError("UserName", "User Name is Not Available");
                    else if (e.ExceptionID == 2)
                        ModelState.AddModelError("EmpNo", "Employee Number is already in use");
                    ViewBag.Employees = emp.GetEmployees();
                    ViewBag.List = emp.GetDesignation();
                    ViewBag.List2 = emp.GetDepartments();
                    return View("EditEmployees", eb.GetEmployeeData(e1.EmployeeID));
                }
            }
            else
            {
                int count = 0;
                foreach (string i in ModelState.Keys)
                {
                    if (!ModelState.IsValidField(i))
                    {
                        ++count;
                    }
                }
                if (count == 1 && e1.Image == null)
                {
                    try
                    {
                      
                        ad.UpdateEmployee(e1);
                        return RedirectToAction("Employees");
                    }
                    catch (SqlException e)
                    {
                        return Content(e.Message);
                    }
                    catch (LeaveApplication.Exceptional_Classes.DuplicateException e)
                    {
                        if (e.ExceptionID == 1)
                            ModelState.AddModelError("UserName", "User Name is Not Available");
                        else if (e.ExceptionID == 2)
                            ModelState.AddModelError("EmpNo", "Employee Number is already in use");

                        ViewBag.Employees = emp.GetEmployees();
                        ViewBag.List = emp.GetDesignation();
                        ViewBag.List2 = emp.GetDepartments();
                        return View("EditEmployees", eb.GetEmployeeData(e1.EmployeeID));
                    }
                }
                ViewBag.Employees = emp.GetEmployees();
                ViewBag.List = emp.GetDesignation();
                ViewBag.List2 = emp.GetDepartments();
                Employee e2 = eb.GetEmployeeData(e1.EmployeeID);
                return View("EditEmployees", e2);
            }
        }

        [HttpPost]
        public ActionResult AddDepartment(Department dp)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                dp.AddDeparment();
                return RedirectToAction("Department");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult DeleteDepartment(Department dp)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                dp.DeleteDeparment();
                return RedirectToAction("Department");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult UpdateDepartment()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                Department d1 = new Models.Department() { DepartmentId = Request.Form["id"].ToString(), department = Request.Form["edittxt"].ToString() };
                d1.updateDeparment();
                return RedirectToAction("Department");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult AddDesignation(Designation ds)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                ds.AddDesignation();
                return RedirectToAction("Designation");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult DeleteDesignation(Designation ds)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {

                ds.DeleteDesignation();
                return RedirectToAction("Designation");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult UpdateDesignation()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
               Designation ds =new Designation() { DesignationID = int.Parse(Request.Form["id"].ToString()), designation = Request.Form["edittxt"].ToString() };
                ds.updateDesignation();
                return RedirectToAction("Designation");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult AddLeaveType(LeaveTypes lt)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                lt.AddLeaveType();
                return RedirectToAction("LeaveType");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult DeleteLeaveType(LeaveTypes lt)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                lt.DeleteLeaveType();
                return RedirectToAction("LeaveType");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult UpdateLeaveType()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                LeaveTypes lt = new LeaveTypes() { LeaveTypeID = int.Parse(Request.Form["id"].ToString()), LeaveType = Request.Form["edittxt"].ToString() };
                lt.updateLeaveType();
                return RedirectToAction("LeaveType");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult AssignLeave()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
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
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public JsonResult GetEmployees(Department d1)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                return Json(eb.GetEmployeesList(d1.DepartmentId));
            }
            else
            {
                return Json(null);
            }
        }

        public JsonResult GetAllEmployees()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                string data = JsonConvert.SerializeObject(eb.GetEmployees());
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AffectedUsers()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                AssignLeaves al = new AssignLeaves();

                System.Data.DataSet ds = null;
                string Querry = string.Empty;
                al.AssignType = Request.Form["customRadio"].ToString();
                try
                {
                    if (al.AssignType == "Select Employee")
                    {
                        al.EmployeeID = Request.Form["emp"].ToString();
                        al.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                        al.Count = double.Parse(Request.Form["count"].ToString());
                        Querry = string.Format("Select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}' where EmployeeID='{1}'", al.LeaveTypeID, al.EmployeeID);
                        ds = ad.ShowAffectedUsers(al, Querry);
                    }
                    else if (al.AssignType == "All(Select Department)")
                    {
                        al.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                        al.Count = double.Parse(Request.Form["count"].ToString());
                        al.DepartmentID = Request.Form["dep"].ToString();

                        Querry = string.Format("select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}' where Departments.DepartmentID='{1}'", al.LeaveTypeID, al.DepartmentID);

                        ds = ad.ShowAffectedUsers(al, Querry);
                    }
                    else if (al.AssignType == "All Employess")
                    {
                        al.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                        al.Count = double.Parse(Request.Form["count"].ToString());
                        Querry = string.Format("select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}'", al.LeaveTypeID);
                        ds = ad.ShowAffectedUsers(al, Querry);
                    }
                    Session["AffectedEmp"] = al;
                    ViewBag.Count = al.Count;
                    return View(ds);
                }
                catch (NullReferenceException)
                {
                    TempData["ValidationError"] = true;

                    return RedirectToAction("AssignLeave");
                }
                catch (FormatException)
                {
                    TempData["ValidationError"] = true;

                    return RedirectToAction("AssignLeave");
                }
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult Submit()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                if (Session["AffectedEmp"] != null)
                {
                    if (((AssignLeaves)Session["AffectedEmp"]).AssignType == "Select Employee")
                    {
                        ad.AssignLeave((AssignLeaves)Session["AffectedEmp"]);
                    }
                    else if (((AssignLeaves)Session["AffectedEmp"]).AssignType == "All(Select Department)")
                    {
                        ad.AssignAllDep((AssignLeaves)Session["AffectedEmp"]);
                    }
                    else if (((AssignLeaves)Session["AffectedEmp"]).AssignType == "All Employess")
                    {
                        ad.AssignAll((AssignLeaves)Session["AffectedEmp"]);
                    }

                    Session.Remove("AffectedEmp");
                }

                return RedirectToAction("AssignLeave");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult CancelAssignLeave()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true && Session["AffectedEmp"] != null)
            {
                Session.Remove("AffectedEmp");
                return RedirectToAction("AssignLeave");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult Assign_Leave_History(int? PageNo)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                System.Data.DataSet ds = ad.ShowAssignLeaveHistory();
                PagedDataSet.PagedDataSet p1 = new PagedDataSet.PagedDataSet();

                if (PageNo.HasValue && PageNo.Value > 0)
                {
                    ViewBag.PageNo = PageNo.Value;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 12, PageNo);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return View(ds1);
                }
                else
                {
                    ViewBag.PageNo = 1;

                    System.Data.DataSet ds1 = p1.GetPage(ds, 12, 1);
                    ViewBag.TotalPages = p1.GetTotalPages();
                    return View(ds1);
                }
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public void RequestableStateChange(string LeaveTypeID, bool IsRequestable)
        {
            ad.RequestableStateChange(LeaveTypeID, IsRequestable);
        }

        public ActionResult LeaveReason()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                System.Data.DataSet x = lb.GetReasons();
                ViewBag.data = x;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult AddLeaveReason(LeaveReason lr)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                lr.AddLeaveReason();
                return RedirectToAction("LeaveReason");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult DeleteLeaveReason(LeaveReason lr)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                lr.DeleteLeaveReason();
                return RedirectToAction("LeaveReason");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        [HttpPost]
        public ActionResult UpdateLeaveReason()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                LeaveReason lr = new LeaveReason() { id = int.Parse(Request.Form["id"].ToString()), _LeaveReason = Request.Form["edittxt"].ToString() };
                lr.UpdateLeaveReason();
                return RedirectToAction("LeaveReason");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult Image()
        {
            //this controller return image when edit is called...
            return File((byte[])TempData["Image"], "image/jpeg", "ds.jpg");
        }

        public ActionResult Degrees()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                Degrees Deg = new Degrees();
                ViewBag.Degrees = Deg.GetDegrees();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult AddDegree(Degrees Deg)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                if (ModelState.IsValid)
                {
                    Deg.AddDegree();
                }
                return RedirectToAction("Degrees");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult DeleteDegree(int DegreeID)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                Degrees Deg = new Degrees { DegreeID = DegreeID };
                Deg.DeleteDegree();
                return RedirectToAction("Degrees");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult UpdateDegree(Degrees Deg)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null && e1.isAdmin == true)
            {
                if (ModelState.IsValid)
                {
                    Deg.UpdateDegree();
                }
                return RedirectToAction("Degrees");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
    }
}