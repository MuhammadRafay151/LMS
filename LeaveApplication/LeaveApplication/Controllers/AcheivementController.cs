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

        public ActionResult UpdateAcheivement(int AcheivementID)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public ActionResult DeleteAcheivement(int AcheivementId)
        {
            Employee e1 = (Employee)Session["Employee"];
            if (Session["EmpID"] != null)
            {
                ac.DeleteAcheivement(AcheivementId, e1.EmployeeID);
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
                System.Data.DataSet ds = ac.DownloadFile(FileID, Convert.ToInt32(Session["EmpID"]));
                return File((Byte[])ds.Tables[0].Rows[0][0], System.Web.MimeMapping.GetMimeMapping(ds.Tables[0].Rows[0][1].ToString()), ds.Tables[0].Rows[0][1].ToString());
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        public JsonResult GetAcheivement(int AcheivementID)
        {
            System.Data.DataSet x = ac.GetAcheivement((int)Session["EmpID"], AcheivementID);
            string z = JsonConvert.SerializeObject(x);
            return Json(z, JsonRequestBehavior.AllowGet);
        }
    }
}