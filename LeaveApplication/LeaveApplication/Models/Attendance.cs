using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveApplication.Models;
using System.Data;
namespace LeaveApplication.Models
{
    public class Attendance
    {
        db DataBase = new db();
        public int EmpNo { get; set; }
        public String EmployeeName { get; set; }
        public bool Abscent { get; set; }
        public DateTime Date { get; set; }
        public void NotifyAbsentees()
        {
            string Subject = System.Configuration.ConfigurationManager.AppSettings["SubjectAttandance"];
            string Body = "";
            string Querry = string.Format("select EmployeeName,Email from Employee where EmpNo='{0}'", EmpNo);
            db d1 = new db();
            DataSet ds = d1.Read(Querry);
            string Name = ds.Tables[0].Rows[0][0].ToString();
            string mail = ds.Tables[0].Rows[0][1].ToString();
            //Email e1 = new Email();
            //e1.Send(mail, subject, Body);

        }

        public void AttendanceRecord()
        {
            string Querry = string.Format("insert into Attendance(EmpNo,AbsentDate) values ('{0}','{1}')", EmpNo, Date);
            DataBase.ExecuteQuerry(Querry);
        }
    }
}
