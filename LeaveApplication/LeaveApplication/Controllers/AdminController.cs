using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using System.IO;
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

            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
              
                return View(lb.GetLeaveTypesDS());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }
        public ActionResult Designation()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {

                return View(eb.GetDesignation());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }
        public ActionResult Department()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ViewBag.Departments = eb.GetDepartments();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }
        public ActionResult Employees()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {

                System.Data.DataSet x = eb.EmployeeDS();

                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }
        public ActionResult EditEmployees(int EmployeeID)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {//empty dataset...
                Employee e1 = ad.ReadEmployee(EmployeeID);

                //System.Data.DataSet x = new System.Data.DataSet();
                //x.Tables.Add(new System.Data.DataTable());
                ////here you pass your filled dataset in place of x
                return View(e1);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }
        
        [HttpPost]
        public ActionResult AddDepartment(Department dp)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.AddDeparment(dp.department);
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.DeleteDeparment(dp.DepartmentId);
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.updateDeparment(new Models.Department() { DepartmentId = Request.Form["id"].ToString(), department = Request.Form["edittxt"].ToString() });
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.AddDesignation(ds);
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {

                ad.DeleteDesignation(ds);
                return RedirectToAction("Designation");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        [HttpPost]
        public ActionResult UpdateDesignation(Designation ds)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.updateDesignation(new Designation() { DesignationID = int.Parse(Request.Form["id"].ToString()), designation = Request.Form["edittxt"].ToString() });
                return RedirectToAction("Designation");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }
        public ActionResult AddLeaveType(LeaveTypes lt)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.AddLeaveType(lt);
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.DeleteLeaveType(lt);
                return RedirectToAction("LeaveType");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }
        [HttpPost]
        public ActionResult UpdateLeaveType(LeaveTypes lt)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                ad.updateLeaveType(new LeaveTypes() { LeaveTypeID = int.Parse(Request.Form["id"].ToString()), LeaveType = Request.Form["edittxt"].ToString() });
                return RedirectToAction("LeaveType");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

        }

        public ActionResult AssignLeave()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
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
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            { return Json(eb.GetEmployeesList(d1.DepartmentId)); }
            else
            {
                return Json(null);
            }

        }
        [HttpPost]
        public ActionResult AffectedUsers()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
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
                        Querry = string.Format("Select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}' where EmployeeID='{1}'", al.LeaveTypeID, al.EmployeeID);
                        ds = ad.ShowAffectedUsers(al, Querry);
                    }
                    else if (al.AssignType == "All(Select Department)")
                    {
                        al.LeaveTypeID = int.Parse(Request.Form["lev"].ToString());
                        al.Count = int.Parse(Request.Form["count"].ToString());
                        al.DepartmentID = Request.Form["dep"].ToString();

                        Querry = string.Format("select EmployeeName,Departments.Department,LeaveType.LeaveType from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join LeaveType on LeaveType.LeaveTypeID='{0}' where Departments.DepartmentID='{1}'", al.LeaveTypeID, al.DepartmentID);

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
            else
            {
                return RedirectToAction("Index", "LogIn");
            }


        }
        public ActionResult Submit()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                if (AdminBusinessLayer.Al != null)
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
            else
            {
                return RedirectToAction("Index", "LogIn");
            }





        }
        public ActionResult CancelAssignLeave()
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
            {
                AdminBusinessLayer.Al = null;
                return RedirectToAction("AssignLeave");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }

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
        public ActionResult Assign_Leave_History(int? PageNo)
        {
            if (Session["EmpID"] != null && EmployeeBusinessLayer.Employee.isAdmin == true)
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

        public void EmployeeStateChange(string EmployeeID, bool IsActive)
        {
            ad.EmployeeStateChange(EmployeeID, IsActive);
        }
    }
}