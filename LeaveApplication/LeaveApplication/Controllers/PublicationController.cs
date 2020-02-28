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
            if (ModelState.IsValid)
            {
                publication.EmployeeId = Convert.ToInt32(Session["EmpId"]);
                publication.InsertPublication();
            }
            else
            {
                return View("Index");
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
                return Content(publication.PublishedId.ToString());
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
    }
  
}