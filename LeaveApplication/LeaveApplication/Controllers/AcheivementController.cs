﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;

namespace LeaveApplication.Controllers
{
    public class AcheivementController : Controller
    {
        private Acheivement ac = new Acheivement();

        // GET: Acheivement
        public ActionResult Index()
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                System.Data.DataSet x = ac.GetAcheivement(e1.EmployeeID);
                return View(x);
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult AddAcheivement(Acheivement ach)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                ac.InsertAcheivement(e1.EmployeeID, ach);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult DeleteAcheivement(Acheivement ach)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                ac.DeleteAcheivement(ach, e1.EmployeeID);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
    }
}