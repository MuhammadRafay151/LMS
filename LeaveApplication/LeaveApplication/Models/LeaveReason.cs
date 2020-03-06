using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class LeaveReason
    {
        public int id { get; set; }
        public string _LeaveReason { get; set; }
        db DataBase = new db();
        public void UpdateLeaveReason()
        {
            string Querry = string.Format("update Reasons set LeaveReason=@lr where ReasonID={0}", id);
            SqlParameter p1 = new SqlParameter() { ParameterName = "lr", Value = _LeaveReason };
            DataBase.ExecuteQuerry(Querry, p1);
        }

        public void AddLeaveReason()
        {
            string Querry = "insert into Reasons(LeaveReason) values(@lr)";
            SqlParameter p1 = new SqlParameter() { ParameterName = "lr", Value = _LeaveReason };
            DataBase.ExecuteQuerry(Querry, p1);
        }

        public void DeleteLeaveReason()
        {
            string Querry = string.Format("delete from Reasons where ReasonID={0}", id);
            DataBase.ExecuteQuerry(Querry);
        }
    }
}