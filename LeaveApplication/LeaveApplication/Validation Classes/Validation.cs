using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using System.Text.RegularExpressions;
namespace LeaveApplication.Validation_Classes
{
    public class Validation
    {
        public void ValidateFullDay_L(LeaveApplication.Models.LeaveApplication l1, ModelStateDictionary x)
        {//for leave application
            if(l1.Attachment!=null&&!IsValidFileFormat(l1.Attachment.FileName))
            {
                x.AddModelError("Attachment", "Invalid Format");
            }
            if (string.IsNullOrWhiteSpace(l1.LeaveType))
            {
                x.AddModelError("LeaveType", "Required");
            }
            if (string.IsNullOrWhiteSpace(l1.FromDate))
            {
                x.AddModelError("FromDate", "Required");
            }
            else
            {
                try
                {
                    DateTime.ParseExact(l1.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    x.AddModelError("FromDate", "Date is not in correct format");
                }
            }
            if (string.IsNullOrWhiteSpace(l1.ToDate))
            {
                x.AddModelError("ToDate", "Required");
            }
            else
            {
                try
                {
                    DateTime.ParseExact(l1.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    x.AddModelError("ToDate", "Date is not in correct format");
                }
            }
            if (string.IsNullOrWhiteSpace(l1.LeaveReason))
            {
                x.AddModelError("LeaveReason", "Required");
            }
         
        }
        public void ValidateHalfDay_L(LeaveApplication.Models.LeaveApplication l1, ModelStateDictionary x)
        {
            if (l1.Attachment != null && !IsValidFileFormat(l1.Attachment.FileName))
            {
                x.AddModelError("Attachment", "Invalid Format");
            }
            if (string.IsNullOrWhiteSpace(l1.LeaveType))
            {
                x.AddModelError("LeaveType", "Required");
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(l1.FromDate))
            {
                x.AddModelError("FromDate", "Required");
            }
            else
            {
                try
                {
                    DateTime.ParseExact(l1.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    x.AddModelError("FromDate", "Date is not in correct format");
                }
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(l1.FromTime))
            {
                x.AddModelError("FromTime", "Required");
            }
            else
            {
                try
                {
                    DateTime.Parse(l1.FromTime);
                }
                catch (FormatException)
                {
                    x.AddModelError("FromTime", "Time is not in correct format");
                }
            }
            if (l1.IsHalfDay == 1 && string.IsNullOrWhiteSpace(l1.ToTime))
            {
                x.AddModelError("ToTime", "Required");
            }
            else
            {
                try
                {
                    DateTime.Parse(l1.ToTime);
                }
                catch (FormatException)
                {
                    x.AddModelError("ToTime", "Time is not in correct format");
                }
            }

            if (string.IsNullOrWhiteSpace(l1.LeaveReason))
            {
                x.AddModelError("LeaveReason", "Required");
            }

        }
        public bool IsValidFileFormat(string FileName)
        {//for attachment on leave application form
            string ContentType = string.Empty;
            List<string> MimeType = null;

            ContentType = System.Web.MimeMapping.GetMimeMapping(FileName);
            MimeType = new List<string>() { "image/jpeg", "application/pdf","image/png",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"};
            if (MimeType.Contains(ContentType))
            {
                return true;
               
            }
            else
            {
                return false;
            }

        }
        public bool IsImageFormat(string FileName)
        {
            string ContentType = string.Empty;
            List<string> MimeType = null;

            ContentType = System.Web.MimeMapping.GetMimeMapping(FileName);
            MimeType = new List<string>() { "image/jpeg"};
            if (MimeType.Contains(ContentType))
            {
                return true;

            }
            else
            {
                return false;
            }

        }
        public bool IsNegativeDifference(string From,string To)
        {
            if((DateTime.Parse(To)-DateTime.Parse(From)).Days<0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ValidateExp(Experience exp, ModelStateDictionary ModelState)
        {
            try
            {
                exp.Fromdate = DateTimeHelper.yyyy_mm_dd(exp.Fromdate);
                if (DateTime.Parse(exp.Fromdate) > DateTime.Now.Date)
                {
                   ModelState.AddModelError("Fromdate", "Invalid date");
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Fromdate", "Invalid Format");
            }
            try
            {
                exp.Todate = DateTimeHelper.yyyy_mm_dd(exp.Todate);
                if (DateTime.Parse(exp.Todate) > DateTime.Now.Date)
                {
                    ModelState.AddModelError("Todate", "Invalid date");
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Todate", "Invalid Format");
            }
            if (IsNegativeDifference(exp.Fromdate, exp.Todate))
            {
                ModelState.AddModelError("Todate", "Todate cannot be older than fromdate");
            }
        }
        public void ValidatePublication(Publication Pub,ModelStateDictionary ModelState)
        {
            Regex r1 = new Regex("[a-zA-Z][a-zA-Z ]+");
            Pub.PublishedDate = DateTimeHelper.yyyy_mm_dd(Pub.PublishedDate);
            for (int i=0;i<Pub.Author.Count;i++)
            {
                if (string.IsNullOrWhiteSpace(Pub.Author[i]))
                {
                    ModelState.AddModelError(string.Format("Author[{0}]", i), "Required");
                }
                else if(!r1.IsMatch(Pub.Author[i]))
                {
                    ModelState.AddModelError(string.Format("Author[{0}]", i), "Invalid Input");
                }
            }
            if(!IsValidFileFormat(Pub.File.FileName))
            {
                ModelState.AddModelError("File", "Invalid File");
            }
            if (DateTime.Parse(Pub.PublishedDate) > DateTimeHelper.GetDate())
            {
                ModelState.AddModelError("PublishedDate", "Invalid Time");
            }
        }
    }
}