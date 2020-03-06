using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class LeaveTypes
    {
        public int LeaveTypeID { get; set; }
        public string LeaveType { get; set; }
        db DataBase = new db();

        public void updateLeaveType()
        {
            string Querry = string.Format("update LeaveType set LeaveType=@lt where LeaveTypeID={0}", LeaveTypeID);
            SqlParameter p1 = new SqlParameter() { ParameterName = "lt", Value = LeaveType };
            DataBase.ExecuteQuerry(Querry, p1);
        }

        public void AddLeaveType()
        {
            string Querry = "insert into LeaveType(LeaveType) values(@lt)";
            SqlParameter p1 = new SqlParameter() { ParameterName = "lt", Value = LeaveType };
            DataBase.ExecuteQuerry(Querry, p1);
        }

        public void DeleteLeaveType()
        {
            string Querry = string.Format("delete from LeaveType where LeaveTypeID={0}", LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);
        }

    }
}