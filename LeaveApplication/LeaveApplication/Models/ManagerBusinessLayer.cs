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
        public static bool IsUnderManagement(string ApplicationId,String UserId)
        {
            string Querry = "select Employee.Manager from LeaveApplication inner join Employee on LeaveApplication.EmployeeID=Employee.EmployeeID where LeaveApplication.LeaveApplicationID=" + ApplicationId;

            if (Convert.ToString(database.ExecuteScalar(Querry))==UserId)
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