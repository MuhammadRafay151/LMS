using System;
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
                System.Data.DataSet x = ac.GetAcheivement();
                return View(x);
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
                ach.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                ach.InsertAcheivement();

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
                ach.EmployeeID = Convert.ToInt32(Session["EmpId"]);
                ach.UpdateAcheivement();
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