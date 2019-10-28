using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace LeaveApplication.Models
{
    public class Employee
    {
     
        //[Required(ErrorMessage = "please bhai dalo value")]
        
        public int EmployeeID { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }
       
        public string PhoneNumber { get; set; }
        public string CNIC { get; set; }
        [Required(ErrorMessage ="Employee image is required")]
        public HttpPostedFileBase Image { get; set; }
        public string ImageBase64 { get; set; }
        public int DesignationID { get; set; }
        public string Designation { get; set; }

        public string DateOfJoining { get; set; }
        public String DepartmentID { get; set; }
        public String Department { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }

        //public bool IsEmployeeEmpty()
        //{
        //    if(string.IsNullOrWhiteSpace(EmployeeID))
        //}
        //to get check wheather the user was manager or not...
        public bool IsManager { get; set; }
    }
}