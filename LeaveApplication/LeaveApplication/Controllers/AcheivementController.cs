﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using Newtonsoft.Json;

namespace LeaveApplication.Controllers
{
    public class AcheivementController : Controller
    {
        // GET: Acheivement
        public ActionResult Index()
        {
            if (Session["EmpID"] != null)
            {
                Acheivement ac = new Acheivement() { EmployeeID = Convert.ToInt32(Session["EmpId"]) };
                ViewBag.data = ac.GetAcheivement();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult AddAcheivement(Acheivement ach)
        {
            if (Session["EmpID"] != null)
            {
                try
                {
                    ach.AcheivementDate = DateTimeHelper.yyyy_mm_dd(ach.AcheivementDate);
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("AcheivementDate", "Invalid Format");
                }
                if (ModelState.IsValid)
                {
                    ach.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    ach.InsertAcheivement();
                }
                else
                {
                    ach.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    ViewBag.data = ach.GetAcheivement();
                    return View("Index", ach);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult UpdateAcheivement(Acheivement ach)
        {
            if (Session["EmpID"] != null)
            {
                try
                {
                    ach.AcheivementDate = DateTimeHelper.yyyy_mm_dd(ach.AcheivementDate);
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("AcheivementDate", "Invalid Format");
                }
                if (ModelState.IsValid)
                {
                    ach.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    ach.UpdateAcheivement();
                }
                else
                {
                    ach.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                    ViewBag.data = ach.GetAcheivement();
                    return View("Index", ach);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult DeleteAcheivement(int AcheivementId)
        {
            if (Session["EmpID"] != null)
            {
                Acheivement ac = new Acheivement() { EmployeeID = Convert.ToInt32(Session["EmpId"]), AcheivementId = AcheivementId };
                ac.DeleteAcheivement();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult DownloadFile(int FileID)
        {
            if (Session["EmpID"] != null)
            {
                Acheivement ac = new Acheivement() { EmployeeID = Convert.ToInt32(Session["EmpId"]), FileId = FileID };
                System.Data.DataSet ds = ac.DownloadFile();
                return File((Byte[])ds.Tables[0].Rows[0][0], System.Web.MimeMapping.GetMimeMapping(ds.Tables[0].Rows[0][1].ToString()), ds.Tables[0].Rows[0][1].ToString());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public JsonResult GetAcheivement(int AcheivementID)
        {
            Acheivement ac = new Acheivement() { EmployeeID = Convert.ToInt32(Session["EmpId"]), AcheivementId = AcheivementID };
            System.Data.DataSet x = ac.GetJsonAcheivement();
            string z = JsonConvert.SerializeObject(x);
            return Json(z, JsonRequestBehavior.AllowGet);
        }
    }
}