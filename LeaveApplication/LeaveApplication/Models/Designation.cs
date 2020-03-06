using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Designation
    {
        public int DesignationID { get; set; }
        public string designation { get; set; }
        db DataBase = new db();
        public void updateDesignation()
        {
            string Querry = string.Format("update Designations set Designation=@ds where DesignationID={0}", DesignationID);
            SqlParameter p1 = new SqlParameter() { ParameterName = "ds", Value = designation };
            DataBase.ExecuteQuerry(Querry, p1);
        }

        public void AddDesignation()
        {
            string Querry = "insert into Designations(Designation) values(@ds)";
            SqlParameter p1 = new SqlParameter() { ParameterName = "ds", Value = designation };
            DataBase.ExecuteQuerry(Querry, p1);
        }

        public void DeleteDesignation()
        {
            string Querry = string.Format("delete from Designations where DesignationID='{0}'", DesignationID);
            DataBase.ExecuteQuerry(Querry);
        }
    }
}