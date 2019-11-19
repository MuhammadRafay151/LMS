using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace LeaveApplication.Models
{
    public class ManagerBusinessLayer
    {
        static db database = new db();
        public static bool IsUnderManagement(string ApplicationId,int UserId)
        {//this will check for leave applicant is under management of current login user...
            string Querry = "select Employee.Manager from LeaveApplication inner join Employee on LeaveApplication.EmployeeID=Employee.EmployeeID where LeaveApplication.LeaveApplicationID=" + ApplicationId;
            DataSet ds = database.Read(Querry);
            if(ds.Tables[0].Rows[0][0]== System.DBNull.Value)
            {//if some one has no manager then he may be the top manager so currently we are returning false...
                return false;
            }
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0])==UserId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}