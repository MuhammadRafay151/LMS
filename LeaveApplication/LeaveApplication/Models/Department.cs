using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Department
    {
        public string department { get; set; }
        public string DepartmentId { get; set; }
        db DataBase = new db();

        public void AddDeparment()
        {
            string Querry = "insert into Departments(Department) values(@dp)";
            SqlParameter p1 = new SqlParameter() { ParameterName = "@dp", Value = department };
            DataBase.ExecuteQuerry(Querry, p1);
        }
        public void updateDeparment()
        {
            string Querry = string.Format("update Departments set Department=@dp where DepartmentID={0}", DepartmentId);
            SqlParameter p1 = new SqlParameter() { ParameterName = "dp", Value = department };
            DataBase.ExecuteQuerry(Querry, p1);
        }
        public void DeleteDeparment()
        {
            string Querry = string.Format("delete from Departments where DepartmentID={0}", DepartmentId);
            DataBase.ExecuteQuerry(Querry);
        }
    }
}