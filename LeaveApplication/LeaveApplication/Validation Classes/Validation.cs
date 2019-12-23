using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveApplication.Validation_Classes
{
    public class Validation
    {
        public void ValidateFullDay_L(LeaveApplication.Models.LeaveApplication l1, ModelStateDictionary x)
        {//for leave application
            if(l1.Attachment!=null&&!IsValidFileFormat(l1))
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
            if (l1.Attachment != null && !IsValidFileFormat(l1))
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
        public bool IsValidFileFormat(LeaveApplication.Models.LeaveApplication l1)
        {//for attachment on leave application form
            string ContentType = string.Empty;
            List<string> MimeType = null;

            ContentType = System.Web.MimeMapping.GetMimeMapping(l1.Attachment.FileName);
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
        //public bool ValidateFullDay_R()
        //{

        //}
        //public bool ValidateHalfDay_R()
        //{

        //}
    }
}