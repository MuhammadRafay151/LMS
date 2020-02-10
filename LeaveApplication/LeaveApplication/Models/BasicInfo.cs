using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace LeaveApplication.Models
{
    public class BasicInfo
    {
        public BasicInfo(Employee e1)
        {
            Name = e1.EmployeeName;
            Address = e1.Address;
            //BirthDay=
            Cnic = e1.CNIC;
            Email = e1.Email;
            PhoneNumber = e1.PhoneNumber;
            Department = e1.Department;
            Designation = e1.Designation;
        }
        public BasicInfo()
        { }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime BirthDay { get; set; }
        public string Cnic { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase Image{ get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }
        public void SaveChanges()
        {
            string Querry = "";
            db db = new db();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() {ParameterName="Name",Value=Name});
            sqlParameters.Add(new SqlParameter() {ParameterName="Address",Value=Address});
            sqlParameters.Add(new SqlParameter() { ParameterName = "BirthDay", Value = Address });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Cnic", Value = Address });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Email", Value = Address });
            sqlParameters.Add(new SqlParameter() { ParameterName = "PhoneNumber", Value = Address });
            if(Image!=null)
            sqlParameters.Add(new SqlParameter() { ParameterName = "Image", Value = Address });
            db.ExecuteQuerry(Querry,sqlParameters);
        }
    }
}