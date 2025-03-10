﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;

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
            BirthDay = e1.Birthday.ToString("dd/MM/yyyy");
        }
        public BasicInfo()
        { }
        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+")]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string BirthDay { get; set; }
        [Required]
        [RegularExpression("^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "Invalid CNIC")]
        public string Cnic { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$", ErrorMessage = "Required format(####-#######)")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase Image { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }
        public byte[] ImagesBytes;

        public void SaveChanges(int EmployeeId)
        {

            string Querry;
            db db = new db();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "Name", Value = Name });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Address", Value = Address });
            sqlParameters.Add(new SqlParameter() { ParameterName = "BirthDay", Value = DateTimeHelper.yyyy_mm_dd(BirthDay) });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Cnic", Value = Cnic });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Email", Value = Email });
            sqlParameters.Add(new SqlParameter() { ParameterName = "PhoneNumber", Value = PhoneNumber });
            Querry = @"
                update employee set EmployeeName = @Name, Address = @Address, PhoneNumber = @PhoneNumber, CNIC = @Cnic, Email =@Email, dob=@BirthDay
                where EmployeeID =" + EmployeeId;
            db.ExecuteQuerry(Querry, sqlParameters);
        }
    }
}