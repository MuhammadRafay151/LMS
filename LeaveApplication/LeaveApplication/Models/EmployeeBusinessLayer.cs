﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Http;
using Pagination;
using LeaveApplication.Exceptional_Classes;
using System.Security.Cryptography;
using System.Text;

namespace LeaveApplication.Models
{
    public class EmployeeBusinessLayer
    {
        /// <summary>
        /// Business layer...
        /// </summary>
        SqlConnection con;
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlDataReader reader;
        SqlCommand cmd;
        DataSet ds;
        db database = new db();
        //public static Employee Employee;

        /// <summary>
        /// Pass An employee object to register in Db...
        /// </summary>
        /// <param name="Emp">Emp Type Object</param>
        public void Register(Employee Emp)
        {
            if (!IsUserAvailable(Emp.UserName))
                throw new DuplicateException(1);
            if (!IsEmpNoAvailable(Emp.EmpNo))
                throw new DuplicateException(2);
            Emp.DateOfJoining = DateTime.Now.ToString("yyyy-MM-dd");
            Byte[] bytes = null;
            if (Emp.Image != null)
            {
                Stream s1 = Emp.Image.InputStream;
                BinaryReader b1 = new BinaryReader(s1);
                bytes = b1.ReadBytes((int)s1.Length);

            }
            Emp.Password = MD5Hash(Emp.Password);

            if (string.IsNullOrWhiteSpace(Emp.Manager))
            {
                string Querry = string.Format("insert into Users(UserName,Password) values('{0}','{1}')" +
                 "insert into employee(UserName, employeename, address, PhoneNumber, cnic, JoiningDate, DesignationID, DepartmentID, IsActive,Email,EmpNo) values('{0}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9},{10},{11})" +
                 "declare @id int = SCOPE_IDENTITY()" +
                 "insert  into Picture(EmployeeID, Picture) values(@id, @img)", Emp.UserName, Emp.Password, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DateOfJoining, Emp.DesignationID, Emp.DepartmentID, 1, Emp.Email, Emp.EmpNo);
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "img";
                p1.Value = bytes;
                database.ExecuteQuerry(Querry, p1);

            }
            else
            {
                string Querry = string.Format("insert into Users(UserName,Password) values('{0}','{1}')" +
                "insert into employee(UserName, employeename, address, PhoneNumber, cnic, JoiningDate, DesignationID, DepartmentID,Manager, IsActive,Email,EmpNo) values('{0}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}','{10}','{11}','{12}')" +
                "declare @id int = SCOPE_IDENTITY()" +
                "insert  into Picture(EmployeeID, Picture) values(@id, @img)", Emp.UserName, Emp.Password, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DateOfJoining, Emp.DesignationID, Emp.DepartmentID, Emp.Manager, 1, Emp.Email, Emp.EmpNo);
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "img";
                p1.Value = bytes;
                database.ExecuteQuerry(Querry, p1);
            }
            //Email Email = new Email();
            // string Subject = System.Configuration.ConfigurationManager.AppSettings["SubjectNewEmpAccInfo"];
            //string Body = string.Format("Your User name is: {0} \nYour Password is: {1} \nKindly login your account and change your password \n ©Reckon Force", Emp.UserName,Emp.Password);
            //Email.Send(Emp.Email,Subject,Body);

        }
        public List<Department> GetDepartments()
        {
            List<Department> Departments = new List<Department>();

            string command = "Select * from departments";

            DataSet ds = database.Read(command);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Department d1 = new Department();
                d1.DepartmentId = ds.Tables[0].Rows[i][0].ToString();
                d1.department = ds.Tables[0].Rows[i][1].ToString();
                Departments.Add(d1);
            }

            return Departments;
        }
        public DataSet GetDepartmentsDS()
        {

            string command = "Select * from departments";
            ds = database.Read(command);


            return ds;
        }
        public List<Designation> GetDesignation(Pagination.Pagination p1, int PageNo)
        {
            String Querry = "select count(*) from Designations";

            int RowPerPage = 5;
            p1.CalculateRanges(Convert.ToInt32(database.ExecuteScalar(Querry)), PageNo, RowPerPage);
            List<Designation> Designations = new List<Designation>();
            string command = string.Format("select * from Designations order by DesignationID offset {0} rows fetch next {1} rows only", p1.OffsetRows, RowPerPage);

            DataSet ds = database.Read(command);

            if (PageNo > p1.TotalPages)
                return null;
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Designation d1 = new Designation();
                    d1.DesignationID = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                    d1.designation = ds.Tables[0].Rows[i][1].ToString();
                    Designations.Add(d1);
                }

                return Designations;
            }

        }
        public List<Designation> GetDesignation()
        {
            String Querry = "select count(*) from Designations";
            List<Designation> Designations = new List<Designation>();
            string command = string.Format("select * from Designations order by DesignationID");

            DataSet ds = database.Read(command);


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Designation d1 = new Designation();
                d1.DesignationID = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                d1.designation = ds.Tables[0].Rows[i][1].ToString();
                Designations.Add(d1);
            }

            return Designations;
        }


        public bool Authenticate(Employee e1)
        {
            con = new SqlConnection();
            cmd = new SqlCommand();
            if (string.IsNullOrWhiteSpace(e1.Password))
            {
                return false;
            }
            e1.Password = MD5Hash(e1.Password);
            cmd.CommandText = string.Format("Select * from Users where UserName='{0}' and Password='{1}'", e1.UserName, e1.Password);
            con.ConnectionString = connection;
            con.Open();
            cmd.Connection = con;
            cmd.ExecuteNonQuery();

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Close();
                reader.Dispose();
                con.Close();
                return true;
            }
            else
            {
                reader.Close();
                reader.Dispose();
                con.Close();
                return false;

            }

        }
        public bool IsactiveEmployee(string Username)
        {
            string Querry = string.Format("select isactive from employee where username='{0}'", Username);
            return Convert.ToBoolean(database.ExecuteScalar(Querry));
        }
        public Employee ReadEmployee(string UserName)
        {
            //load user data on login
            string Querry = string.Format("select Employee.EmployeeID,Employee.UserName,Employee.EmployeeName,Employee.Address,Employee.PhoneNumber,Employee.CNIC,Employee.JoiningDate,Designations.Designation,Departments.Department,Picture.Picture,Employee.IsAdmin,Employee.Email,Employee.Address,Employee.CNIC,Employee.PhoneNumber,Employee.Dob  from Employee inner join Picture on Employee.EmployeeID=Picture.EmployeeID inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join Designations on Employee.DesignationID=Designations.DesignationID where Employee.UserName='{0}'", UserName);
            DataSet d1 = database.Read(Querry);
            Employee e1 = new Employee();
            e1.EmployeeID = int.Parse(d1.Tables[0].Rows[0][0].ToString());
            e1.UserName = d1.Tables[0].Rows[0][1].ToString();
            e1.EmployeeName = d1.Tables[0].Rows[0][2].ToString();
            e1.Department = d1.Tables[0].Rows[0][8].ToString();
            e1.Designation = d1.Tables[0].Rows[0][7].ToString();
            e1.ImageBytes = (Byte[])d1.Tables[0].Rows[0][9];
            e1.isAdmin = bool.Parse(d1.Tables[0].Rows[0][10].ToString());
            e1.IsManager = IsManager(e1.EmployeeID);
            e1.Email = d1.Tables[0].Rows[0][11].ToString();
            e1.Address = d1.Tables[0].Rows[0][12].ToString();
            e1.CNIC = d1.Tables[0].Rows[0][13].ToString();
            e1.PhoneNumber = d1.Tables[0].Rows[0][14].ToString();
            if (d1.Tables[0].Rows[0][15]==System.DBNull.Value)
            {
               
            }
            else
            {
                e1.Birthday = DateTime.Parse(d1.Tables[0].Rows[0][15].ToString());
            }

            return e1;

        }
        public DataSet GetEmployeesDs(int PageNumber)
        {// send employees for manage employees forms...
            string Querry1 = "select COUNT(*)as TotalEmp  from Employee;";

            Pagination.Pagination p1 = new Pagination.Pagination();
            int RowsPerPage = 6;
            p1.CalculateRanges(Convert.ToInt32(database.ExecuteScalar(Querry1)), PageNumber, RowsPerPage);
            if (PageNumber > p1.TotalPages)
            {
                return null;
            }
            else
            {
                string Querry = string.Format(@"select Employee.EmployeeID,Employee.EmployeeName,Departments.Department,Designations.Designation,IsActive from Employee inner join Designations on Designations.DesignationID=Employee.DesignationID inner join Departments on Employee.DepartmentID=Departments.DepartmentID order by EmployeeName Asc offset {0} rows fetch next {1} rows only;
", p1.OffsetRows, RowsPerPage);
                DataSet ds = database.Read(Querry);

                DataTable dt = new DataTable();
                dt.Columns.Add("Count");
                dt.Rows.Add(p1.TotalPages);
                ds.Tables.Add(dt);
                return ds;
            }



        }
        public Employee GetEmployeeData(int Empid)
        {

            string Querry = string.Format("select Employee.EmployeeID,Employee.UserName,Employee.EmployeeName,Employee.Address,Employee.PhoneNumber,Employee.CNIC,Employee.JoiningDate,Employee.DesignationID,Employee.DepartmentID,Picture.Picture,Employee.IsAdmin,Employee.Manager,Users.Password,Employee.Email,Employee.EmpNo  from Employee inner join Picture on Employee.EmployeeID=Picture.EmployeeID inner join Users on Employee.UserName=Users.UserName where Employee.EmployeeID='{0}'", Empid);

            DataSet d1 = database.Read(Querry);

            Employee e1 = new Employee();
            e1.EmployeeID = int.Parse(d1.Tables[0].Rows[0][0].ToString());
            e1.UserName = d1.Tables[0].Rows[0][1].ToString();
            e1.EmployeeName = d1.Tables[0].Rows[0][2].ToString();
            e1.Address = d1.Tables[0].Rows[0][3].ToString();
            e1.PhoneNumber = d1.Tables[0].Rows[0][4].ToString();
            e1.CNIC = d1.Tables[0].Rows[0][5].ToString();
            e1.DepartmentID = d1.Tables[0].Rows[0][8].ToString();
            e1.DesignationID = Convert.ToInt32(d1.Tables[0].Rows[0][7]);
            e1.ImageBytes = (Byte[])d1.Tables[0].Rows[0][9];
            e1.isAdmin = bool.Parse(d1.Tables[0].Rows[0][10].ToString());
            e1.Manager = d1.Tables[0].Rows[0][11].ToString();
            //e1.Password = d1.Tables[0].Rows[0][12].ToString();
            e1.Email = d1.Tables[0].Rows[0][13].ToString();
            e1.EmpNo = Convert.ToInt32(d1.Tables[0].Rows[0][14]);
            return e1;
        }
        public string GetBase64Image(Byte[] Image)
        {
            string temp = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(Image));
            return temp;
        }
        public DataSet GetEmployees()
        {

            string Querry = "select EmployeeID,EmployeeName from Employee";

            ds = database.Read(Querry);

            return ds;
        }
        public List<Employee> GetEmployeesList(string DepartmentID)
        {
            List<Employee> e1 = new List<Employee>();

            string Querry = string.Format("select EmployeeID,EmployeeName from Employee where Employee.DepartmentID='{0}'", DepartmentID);

            ds = database.Read(Querry);

            foreach (DataRow x in ds.Tables[0].Rows)
            {
                e1.Add(new Employee() { EmployeeID = int.Parse(x[0].ToString()), EmployeeName = x[1].ToString() });
            }

            if (e1.Count > 0)
                return e1;
            else
                return null;
        }
        private bool IsManager(int EmployeeID)
        {//should modify with top1 record
            con = new SqlConnection(connection);
            string Querry = string.Format("select COUNT(*) from Employee where Manager='{0}' group by Manager", EmployeeID);
            cmd = new SqlCommand(Querry, con);
            con.Open();
            if (cmd.ExecuteScalar() != null && int.Parse(cmd.ExecuteScalar().ToString()) > 0)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }
        public bool IsManager(string EmployeeID)
        {
            con = new SqlConnection(connection);
            string Querry = string.Format("select COUNT(*)  from Employee where Manager='{0}' group by Manager", EmployeeID);
            con.Open();
            cmd = new SqlCommand(Querry, con);
            if (cmd.ExecuteScalar() != null && int.Parse(cmd.ExecuteScalar().ToString()) > 0)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }
        public string GetUserName(int Empid)
        {
            string Querry = " select Employee.UserName from Employee where Employee.EmployeeID =" + Empid;
            return database.ExecuteScalar(Querry).ToString();
        }
        public bool IsUserAvailable(string UserName)
        {
            string Querry = string.Format("select UserName from Users where UserName='{0}'", UserName);
            DataSet ds = database.Read(Querry);
            if (ds.Tables[0].Rows.Count == 0)
                return true;
            else
                return false;
        }
        public bool IsEmpNoAvailable(int EmpNo)
        {
            string Querry = string.Format("select EmpNo from Employee where EmpNo={0}", EmpNo);
            DataSet ds = database.Read(Querry);
            if (ds.Tables[0].Rows.Count == 0)
                return true;
            else
                return false;
        }
        public bool IsEmpNoAvailable(int EmpId, int EmpNo)
        {//this code will be called during employee details updation
            string Querry = string.Format("select EmpNo from Employee where EmpNo ={0} and EmployeeID != {1}", EmpNo, EmpId);
            DataSet ds = database.Read(Querry);
            if (ds.Tables[0].Rows.Count == 0)
                return true;
            else
                return false;
        }
        public bool IsUserAvailable(int EmpId, string UserName)
        {
            string Querry = string.Format("select UserName from Employee where UserName='{0}' and EmployeeID!='{1}'", UserName, EmpId);
            DataSet ds = database.Read(Querry);
            if (ds.Tables[0].Rows.Count == 0)
                return true;
            else
                return false;
        }

        public void ResetPassword(string NewPassword, string UserName)
        {
            NewPassword = MD5Hash(NewPassword);
            string Querry = string.Format("update Users set Password='{0}' where UserName='{1}'", NewPassword, UserName);
            database.ExecuteQuerry(Querry);

        }

        public string UserPassword(string UserName)
        {

            string Querry = string.Format("select Password from Users where UserName='{0}'", UserName);
            return Convert.ToString(database.ExecuteScalar(Querry));

        }

        public string MD5Hash(string plaintext)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();

            }
        }

        public DataSet GetAbsents(int EmployeeId)
        {

            string Querry = string.Format(@"select * from Attendance where EmployeeId={0} and IsClosed=0", EmployeeId);

            ds = database.Read(Querry);

            return ds;
        }


    }
}