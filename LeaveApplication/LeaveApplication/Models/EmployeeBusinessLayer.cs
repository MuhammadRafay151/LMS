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
        SqlDataAdapter da;
        DataSet ds;
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

            con = new SqlConnection(connection);
            con.Open();
            cmd = new SqlCommand();
            cmd.Connection = con;
            if (string.IsNullOrWhiteSpace(Emp.Manager))
            {
                cmd.CommandText = string.Format("insert into employee (employeeid,employeename,address,PhoneNumber,cnic,JoiningDate,DesignationID,DepartmentID,password) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", Emp.EmployeeID, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DateOfJoining, Emp.DesignationID, Emp.DepartmentID, Emp.Password);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("insert into picture (employeeid,picture) values ('{0}',@img)", Emp.EmployeeID);
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "img";
                p1.Value = bytes;
                cmd.Parameters.Add(p1);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                cmd.CommandText = string.Format("insert into employee (employeeid,employeename,address,PhoneNumber,cnic,JoiningDate,DesignationID,DepartmentID,password,manager) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", Emp.EmployeeID, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DateOfJoining, Emp.DesignationID, Emp.DepartmentID, Emp.Password, Emp.Manager);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("insert into picture (employeeid,picture) values ('{0}',@img)", Emp.EmployeeID);
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "img";
                p1.Value = bytes;
                cmd.Parameters.Add(p1);
                cmd.ExecuteNonQuery();
                con.Close();
            }


        }
        public List<Department> GetDepartments()
        {
            List<Department> Departments = new List<Department>();

            con = new SqlConnection(connection);
            con.Open();
            SqlDataAdapter ad;
            DataSet ds = new DataSet();
            string command = "Select * from departments";
            ad = new SqlDataAdapter(command, con);
            ad.Fill(ds);
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
            cmd.CommandText = string.Format("Select * from Employee where EmployeeID='{0}' and Password='{1}'", e1.EmployeeID, e1.Password);
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
        public void ReadEmployee(string EmployeeID)
        {
            con = new SqlConnection(connection);
            con.Open();
            string Querry = string.Format("select Employee.EmployeeID,Employee.EmployeeName,Employee.Address,Employee.PhoneNumber,Employee.CNIC,Employee.JoiningDate,Designations.Designation,Departments.Department,Picture.Picture  from Employee inner join Picture on Employee.EmployeeID=Picture.EmployeeID inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join Designations on Employee.DesignationID=Designations.DesignationID where Employee.EmployeeID='{0}'", EmployeeID);
            SqlDataAdapter da = new SqlDataAdapter(Querry, con);
            DataSet d1 = new DataSet();
            da.Fill(d1);
            Employee e1 = new Employee();
            e1.EmployeeID = d1.Tables[0].Rows[0][0].ToString();
            e1.EmployeeName = d1.Tables[0].Rows[0][1].ToString();
            e1.Department = d1.Tables[0].Rows[0][7].ToString();
            e1.Designation = d1.Tables[0].Rows[0][6].ToString();
            e1.ImageBase64 = GetBase64Image((Byte[])d1.Tables[0].Rows[0][8]);
            con.Close();
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
            con = new SqlConnection(connection);
            string Querry = "select EmployeeID,EmployeeName from Employee";
            da = new SqlDataAdapter(Querry, con);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();
            da.Dispose();
            return ds;
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

    }
}