using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
namespace LeaveApplication.Models
{
    public class Employee
    {

        //[Required(ErrorMessage = "please bhai dalo value")]

        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "The information is required")]
        public int EmpNo { get; set; }
        [Required(ErrorMessage = "The information is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The information is required")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "The information is required")]
        public string Address { get; set; }
        public string Manager { get; set; }
        [Required(ErrorMessage = "The information is required")]
        [Phone(ErrorMessage = "Please Enter Valid Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "The information is required")]
        public string CNIC { get; set; }
        [Required(ErrorMessage = "Employee image is required")]
        public HttpPostedFileBase Image { get; set; }

        public byte[] ImageBytes { get; set; }
        [Required(ErrorMessage = "The information is required")]
        public int DesignationID { get; set; }

        public string Designation { get; set; }

        public string DateOfJoining { get; set; }
        [Required(ErrorMessage = "The information is required")]
        public String DepartmentID { get; set; }
        public String Department { get; set; }
        [Required(ErrorMessage = "The information is required")]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        //public bool IsEmployeeEmpty()
        //{
        //    if(string.IsNullOrWhiteSpace(EmployeeID))
        //}
        //to get check wheather the user was manager or not...
        public bool IsManager { get; set; }

        public Employee GetManager()
        {
            string Querry = string.Format("select d.EmployeeName as ManagerName,d.Email from Employee e inner join Employee d on e.Manager = d.EmployeeID where e.EmployeeID ={0}", EmployeeID);
            Employee e1 = new Employee();
            db database = new db();
            DataSet ds = database.Read(Querry);
            if(ds.Tables[0].Rows.Count==0)
            {
                return null;
            }
            e1.EmployeeName = ds.Tables[0].Rows[0][0].ToString();
            e1.Email= ds.Tables[0].Rows[0][1].ToString();
            return e1;
        }
    }
}