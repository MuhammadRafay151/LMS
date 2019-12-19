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
            if (ds.Tables[0].Rows.Count > 0)
            {
                string Name = ds.Tables[0].Rows[0][0].ToString();
                string mail = ds.Tables[0].Rows[0][1].ToString();
                Body = string.Format("Dear {0} you are absent at {1} kindly for more information check your dashboard Thankyou.", Name, Date);
                if (AttendanceRecord(Body) > 0)
                {
                    Email e1 = new Email();
                    e1.Send(mail, Subject, Body);
                }

            }

        }
        public bool IsLeaveApplied()
        {
            string Querry = string.Format("");
            db database = new db();
            DataSet ds = database.Read(Querry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int AttendanceRecord(string message)
        {
            db DataBase = new db();
            string Querry = string.Format(
                @"IF not EXISTS(select * from  LeaveApplication  inner join statushistory
on StatusHistory.LeaveApplicationID=LeaveApplication.LeaveApplicationID 
where EmployeeID=(select EmployeeID from Employee where EmpNo='{1}') and '{0}' between FromDate and ToDate 
and LeaveApplication.ApplicationType=0
and StatusHistory.ApplicationStatusID='2'
)
                 BEGIN
                insert into Attendance(EmployeeId,AbsentDate,Message)
select EmployeeID, '{0}', '{2}' from Employee where Employee.EmpNo='{1}'
                 END
 select @@rowcount as Count"
                , Date, EmpNo, message);
            //retun int value=1 if employee leave is not found,pending or rejected so we can send emails to that employee and retun 0 if found...
            return Convert.ToInt32(DataBase.ExecuteScalar(Querry));
        }
        public void CloseNotification(int id,int EmployeeId)
        {
            string Querry = "update Attendance set IsClosed=1 where ID=" + id + "and EmployeeId="+EmployeeId;
            db database= new db();
            database.ExecuteQuerry(Querry);
        }
        
    }
}
