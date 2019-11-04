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

            if (Convert.ToInt32(database.ExecuteScalar(Querry))==UserId)
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