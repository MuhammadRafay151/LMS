using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class EmployeeLeaveCountHistory
    {
        public string EmployeeID { get; set; }
        public int LeaveTypeID { get; set; }
        public int Count { get; set; }
        public string Date { get; set; }
    }
}