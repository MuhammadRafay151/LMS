using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using Newtonsoft.Json;

namespace LeaveApplication.Controllers
{
    public class PublicationController : Controller
    {
        // GET: Publication
        public ActionResult Index()
        {
            Publication p1 = new Publication() { EmployeeId = int.Parse(Session["EmpId"].ToString()) };
            ViewBag.data = p1.GetPublications();
            return View();
        }
        public ActionResult AddPub(Publication publication)
        {
            Validation_Classes.Validation v1 = new Validation_Classes.Validation();
            v1.ValidatePublication(publication,ModelState);
            if (ModelState.IsValid)
            {
                publication.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                publication.InsertPublication();
            }
            else
            {
                publication.EmployeeId = int.Parse(Session["EmpId"].ToString());
                ViewBag.data = publication.GetPublications();
                ViewBag.Openform = true;
                return View("Index",publication);
            }
            return RedirectToAction("Index");

        }
        public FileResult DownloadFile(int FileId, int PubId)
        {
            Publication p1 = new Publication();
            File f1 = p1.GetFile(FileId, Int32.Parse(Session["EmpId"].ToString()), PubId);
            return File(f1.Content, System.Web.MimeMapping.GetMimeMapping(f1.FileName), f1.FileName);
        }
        public ActionResult DeltePub(int id)
        {
            Publication p1 = new Publication() { PublishedId = id, EmployeeId = Convert.ToInt32(Session["EmpId"]) };
            p1.DeletePublication();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdatePub(Publication publication)
        {
            if (ModelState.IsValid)
            {
                //save 
                publication.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                publication.UpdatePublication();
            }
            else
            {
                
            }
            return RedirectToAction("Index");
        }
        
        public JsonResult GetPublication(int? id)
        {
            if (id != null)
            {
                Publication p1 = new Publication() { PublishedId = id.Value, EmployeeId = Convert.ToInt32(Session["EmpId"].ToString()) };
                string z = JsonConvert.SerializeObject(p1.GetPublication());
                return Json(z, JsonRequestBehavior.AllowGet);
            }
            else
                return Json("Some thing going wrong please try again later", JsonRequestBehavior.AllowGet);
           

        }

    //Reporting
        public ActionResult Report()
        {
            EmployeeBusinessLayer emp = new EmployeeBusinessLayer();
            ViewBag.Employees = emp.GetEmployees();
            ViewBag.List2 = emp.GetDepartments();
            Publication p1 = new Publication();
            return View(p1.GenrateReport(0));//0 means all dep
        }
        //filters
        public JsonResult DepartmentReport(int? depid)
        {
            Publication p1 = new Publication();
            string data=string.Empty;
            if(depid.HasValue)
            {
                data = JsonConvert.SerializeObject(p1.GenrateReport(depid.Value));
            }
            else
            {
                data = JsonConvert.SerializeObject(p1.GenrateReport(0));
            }
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmployeeReport(int? empid)
        {
            
            string data = string.Empty;
            Publication p1 = new Publication();
            if (empid.HasValue)
            {
                
                data = JsonConvert.SerializeObject(p1.GenrateReport_emp(empid.Value));
            }
            else
            {
                data = JsonConvert.SerializeObject(p1.GenrateReport_emp(0));
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public FileResult DownloadPub(int FileId, int PubId)
        {
            Publication p1 = new Publication();
            File f1 = p1.GetFile(FileId, PubId);
            return File(f1.Content, System.Web.MimeMapping.GetMimeMapping(f1.FileName), f1.FileName);
        }
    }
  
}