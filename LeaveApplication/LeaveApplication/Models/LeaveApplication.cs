using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class LeaveApplication
    {

        public String ApplicationId { get; set; }
        public String EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public String LeaveType { get; set; }
        public String LeaveTypeID { get; set; }
        public String ApplyDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public double TotalDays { get; set; }
        public string LeaveRemarks { get; set; }
        public String LeaveReason { get; set; }
        public string ApplicationStatus { get; set; }

        public int ApplicationType { get; set; }   //full day leave or half day leave

    }
    public enum Status
    {
        [Description("Pending")] s1, [Description("Approved")] s2, [Description("Rejected")] s3

    };
}