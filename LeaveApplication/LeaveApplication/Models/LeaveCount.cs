using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class LeaveCount
    {
        public int CountPending { get; set; }
        public int CountRejected { get; set; }
        public int CountApproved { get; set; }
        public int TotalLeave { get; set; }
        public int TotalLeaveDays { get; set; }
        public int Leaveleft { get; set; }
        public int SickLeaveCount { get; set; }
        public int CasualLeaveCount { get; set; }
        public int AnnualLeaveCount { get; set; }
    }
}