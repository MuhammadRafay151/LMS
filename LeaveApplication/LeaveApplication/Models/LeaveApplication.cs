using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Data;

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
        
        public string FromTime { get; set; }
       
        public string ToTime { get; set; }
        public double TotalDays { get; set; }
        public string LeaveRemarks { get; set; }

        public String LeaveReason { get; set; }
        public string ApplicationStatus { get; set; }
        public int IsHalfDay { get; set; }//full day leave or half day leave

        public bool ApplicationType { get; set; }   //leaverequest or leaveapplication...
        public HttpPostedFileBase Attachment { get; set; }
        public string FileId { get; set; }//use when displaying leave in detail view
        public string FileName { get; set; }
        public string ManagerRemarks { get; set; }
        public void NotifyManager(Employee Manager, Employee FacultyMember)
        {//notify manger about new leave...
            if(Manager!=null)
            {
                string Body = string.Format("Dear {0} your received a leave application from {1}",Manager.EmployeeName,FacultyMember.EmployeeName);
                string Subject = System.Configuration.ConfigurationManager.AppSettings["SubjectNewLeaveReceive"];
                Email e1 = new Email();
                e1.Send(Manager.Email, Subject, Body);
                
            }

        }
        public void NotifyAcceptedLeave()
        {// call when leave has been accpted
            string Querry = string.Format("select Employee.EmployeeName,Employee.Email from LeaveApplication inner join Employee on LeaveApplication.EmployeeID=Employee.EmployeeID where LeaveApplication.LeaveApplicationID={0}", ApplicationId);
            db d1 = new db();
           DataSet ds= d1.Read(Querry);
            string Subject= System.Configuration.ConfigurationManager.AppSettings["SubjectAccept"]; ;
            string Content = "";
            Email e1 = new Email();
            //e1.Send(ds.Tables[0].Rows[0][1].ToString(), Subject, Content);
            e1 = null;
            d1 = null;
            ds = null;
        }
        public void NotifyRejectedLeave()
        {// call when leave has been rejected
            string Querry = string.Format("select Employee.EmployeeName,Employee.Email from LeaveApplication inner join Employee on LeaveApplication.EmployeeID=Employee.EmployeeID where LeaveApplication.LeaveApplicationID={0}", ApplicationId);
            db d1 = new db();
            DataSet ds = d1.Read(Querry);
            string Subject = System.Configuration.ConfigurationManager.AppSettings["SubjectAccept"]; ;
            string Content = "";
             Email e1 = new Email();
            //e1.Send(ds.Tables[0].Rows[0][1].ToString(), Subject, Content);
            e1 = null;
            d1 = null;
            ds = null;
        }

    }

}