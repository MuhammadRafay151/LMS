using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApplication.Models
{
    public class Attendance
    {
        public int EmpId { get; set; }
        public bool Abscent { get; set; }
        public DateTime Date { get; set; }
    }
}
