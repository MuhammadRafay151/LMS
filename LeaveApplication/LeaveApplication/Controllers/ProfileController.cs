using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using System.IO;
using System.Data.SqlClient;
using LeaveApplication.Validation_Classes;
using System.Net;

namespace LeaveApplication.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        private EmployeeBusinessLayer eb = new EmployeeBusinessLayer();
        [SessionLive]
        public ActionResult Index()
        {
            return View();
        }
        [SessionLive]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword, string ComfirmPassword)
        {
            Employee e1 = (Employee)Session["Employee"];

            if (eb.UserPassword(e1.UserName) == eb.MD5Hash(CurrentPassword))
            {
                if (NewPassword == ComfirmPassword)
                {
                    eb.ResetPassword(NewPassword, e1.UserName);
                }
                else
                {
                    ModelState.AddModelError("ComfirmPassword", "Confirm Password not matched");
                    return View("ResetPassword");
                }
            }
            else
            {
                ModelState.AddModelError("CurrentPassword", "Current Password is not Correct");
                return View("ResetPassword");
            }
            TempData["Notify"] = true;
            return RedirectToAction("ResetPassword", "Profile");


        }

        public ActionResult Basicinfo()
        {
            if (Session["EmpID"] == null)
            {
                return RedirectToAction("Index", "LogIn");
            }
            BasicInfo b1 = new BasicInfo((LeaveApplication.Models.Employee)Session["Employee"]);
            return View(b1);
        }
        [SessionLive]
        public ActionResult UploadImage(string fname, HttpPostedFileBase img)
        {

            if (Request.HttpMethod == "POST")
            {
                Validation_Classes.Validation v1 = new Validation();
                if (img != null)
                {
                    if (v1.IsImageFormat(fname))
                    {
                        LeaveBusinessLayer lb = new LeaveBusinessLayer();
                        Employee e1 = (LeaveApplication.Models.Employee)Session["Employee"];
                        e1.ImageBytes = lb.GetFileBytes(img);
                        ProfilePicture p1 = new ProfilePicture() { Image = e1.ImageBytes };
                        p1.Update(int.Parse(Session["EmpId"].ToString()));
                        return Content("Successfully Uploaded");
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,"Invalid image format");
                    }
                 
                }
            }
            return View();
        }
        [SessionLive]
        public ActionResult Edit_Info_Submit(BasicInfo b1)
        {
            if (Session["EmpID"] != null)
            {
                if (ModelState.IsValid)
                {
                    Validation_Classes.Validation v1 = new Validation_Classes.Validation();
                    b1.SaveChanges(int.Parse(Session["EmpId"].ToString()));
                    Employee e2 = (Employee)Session["Employee"];
                    e2.EmployeeName = b1.Name;
                    e2.Birthday = DateTime.ParseExact(b1.BirthDay, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    e2.Address = b1.Address;
                    e2.CNIC = b1.Cnic;
                    e2.Email = b1.Email;
                    e2.PhoneNumber = b1.PhoneNumber;
                }
                else
                {
                    b1 = null;
                    ViewBag.OpenModel = true;
                    b1 = new BasicInfo((LeaveApplication.Models.Employee)Session["Employee"]);
                    return View("Basicinfo", b1);
                }
                return RedirectToAction("Basicinfo");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        [SessionLive]
        public ActionResult Experience()
        {
            return View();
        }
        [SessionLive]
        public ActionResult Publications()
        {
            return View();
        }
        [SessionLive]
        public ActionResult Acheivements()
        {
            return View();
        }
    }

}