using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

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
        public static Employee Employee;
        /// <summary>
        /// Pass An employee object to register in Db...
        /// </summary>
        /// <param name="Emp">Emp Type Object</param>
        public void Register(Employee Emp)
        {

            Emp.DateOfJoining = DateTime.Now.ToString("yyyy-MM-dd");
            Byte[] bytes = null;
            if (Emp.Image != null)
            {
                Stream s1 = Emp.Image.InputStream;
                BinaryReader b1 = new BinaryReader(s1);
                bytes = b1.ReadBytes((int)s1.Length);

            }


            if (string.IsNullOrWhiteSpace(Emp.Manager))
            {
                string Querry = string.Format("insert into Users(UserName,Password) values('{0}','{1}')" +
                 "insert into employee(UserName, employeename, address, PhoneNumber, cnic, JoiningDate, DesignationID, DepartmentID, IsActive) values('{0}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9})" +
                 "declare @id int = SCOPE_IDENTITY()" +
                 "insert  into Picture(EmployeeID, Picture) values(@id, @img)", Emp.UserName, Emp.Password, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DateOfJoining, Emp.DesignationID, Emp.DepartmentID, 1);
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "img";
                p1.Value = bytes;
                database.ExecuteQuerry(Querry, p1);

            }
            else
            {
                string Querry = string.Format("insert into Users(UserName,Password) values('{0}','{1}')" +
                "insert into employee(UserName, employeename, address, PhoneNumber, cnic, JoiningDate, DesignationID, DepartmentID,Manager, IsActive) values('{0}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}','{10}')" +
                "declare @id int = SCOPE_IDENTITY()" +
                "insert  into Picture(EmployeeID, Picture) values(@id, @img)", Emp.UserName, Emp.Password, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DateOfJoining, Emp.DesignationID, Emp.DepartmentID, Emp.Manager, 1);
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "img";
                p1.Value = bytes;
                database.ExecuteQuerry(Querry, p1);
            }


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
            con.Close();
            return Departments;
        }
        public DataSet GetDepartmentsDS()
        {

            string command = "Select * from departments";
            ds = database.Read(command);


            return ds;
        }
        public List<Designation> GetDesignation()
        {
            List<Designation> Designations = new List<Designation>();
            string command = string.Format("select * from designations");
            con = new SqlConnection(connection);
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter(command, con);
            DataSet ds = new DataSet();
            ad.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Designation d1 = new Designation();
                d1.DesignationID = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                d1.designation = ds.Tables[0].Rows[i][1].ToString();
                Designations.Add(d1);
            }
            con.Close();
            return Designations;
        }


        public bool Authenticate(Employee e1)
        {
            con = new SqlConnection();
            cmd = new SqlCommand();
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
        public void ReadEmployee(string UserName)
        {
          
            string Querry = string.Format("select Employee.EmployeeID,Employee.UserName,Employee.EmployeeName,Employee.Address,Employee.PhoneNumber,Employee.CNIC,Employee.JoiningDate,Designations.Designation,Departments.Department,Picture.Picture,Employee.IsAdmin  from Employee inner join Picture on Employee.EmployeeID=Picture.EmployeeID inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join Designations on Employee.DesignationID=Designations.DesignationID where Employee.UserName='{0}'", UserName);
            
            DataSet d1 = database.Read(Querry);
           
            Employee e1 = new Employee();
            e1.EmployeeID = int.Parse(d1.Tables[0].Rows[0][0].ToString());
            e1.UserName = d1.Tables[0].Rows[0][1].ToString();
            e1.EmployeeName = d1.Tables[0].Rows[0][2].ToString();
            e1.Department = d1.Tables[0].Rows[0][8].ToString();
            e1.Designation = d1.Tables[0].Rows[0][7].ToString();
            e1.ImageBase64 = GetBase64Image((Byte[])d1.Tables[0].Rows[0][9]);
            e1.isAdmin = bool.Parse(d1.Tables[0].Rows[0][10].ToString());
           
            Employee = e1;
            Employee.IsManager = IsManager();

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

            return e1;
        }
        private bool IsManager()
        {
            con = new SqlConnection(connection);
            string Querry = string.Format("select COUNT(*) from Employee where Manager='{0}' group by Manager", Employee.EmployeeID);
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
        //My changes

        public DataSet EmployeeDS()
        {
            string Querry = "select Employee.EmployeeID,Employee.EmployeeName,Departments.Department,Designations.Designation,IsActive from Employee inner join Designations on Designations.DesignationID=Employee.DesignationID inner join Departments on Employee.DepartmentID=Departments.DepartmentID order by EmployeeName Asc";
            DataSet ds = database.Read(Querry);
            return ds;
        }

    }
}