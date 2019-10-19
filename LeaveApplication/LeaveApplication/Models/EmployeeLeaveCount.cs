using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class EmployeeLeaveCount
    {
        public int EmployeeID { get; set; }
        public int LeaveTypeID { get; set; }
        public double Count { get; set; }
    }
}