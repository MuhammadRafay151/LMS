using System;
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
            BirthDay = e1.Birthday;
        }
        public BasicInfo()
        { }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string Cnic { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
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
            sqlParameters.Add(new SqlParameter() { ParameterName = "BirthDay", Value = BirthDay.ToString("yyyy/MM/dd") });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Cnic", Value = Cnic });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Email", Value = Email });
            sqlParameters.Add(new SqlParameter() { ParameterName = "PhoneNumber", Value = PhoneNumber });
            if (Image != null)
            {
                Querry =string.Format( @"
                update employee set EmployeeName = @Name, Address = @Address, PhoneNumber = @PhoneNumber, CNIC = @Cnic, Email =@Email, dob=@BirthDay where EmployeeID ={0};
                update Picture set Picture = @Image where EmployeeID={0}",EmployeeId);
                Stream s1 = this.Image.InputStream;
                BinaryReader b1 = new BinaryReader(s1);
                ImagesBytes = b1.ReadBytes((int)s1.Length);
                sqlParameters.Add(new SqlParameter() { ParameterName = "Image", Value = ImagesBytes });
            }
            else
            {
                Querry = @"
                update employee set EmployeeName = @Name, Address = @Address, PhoneNumber = @PhoneNumber, CNIC = @Cnic, Email =@Email, dob=@BirthDay
                where EmployeeID =" + EmployeeId;
            }

            db.ExecuteQuerry(Querry, sqlParameters);
        }
    }
}