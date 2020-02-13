using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public int EmployeeId { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string Descipline { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
    }
}