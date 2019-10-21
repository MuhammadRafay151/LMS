using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class AdminBusinessLayer
    {
        SqlConnection con;
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlDataReader reader;
        SqlCommand cmd;
        public static Employee Employee;
        db DataBase = new db();
        public static AssignLeaves Al;

        public void updateDeparment(Department dp)
        {
            string Querry = string.Format("update Departments set Department='{0}' where DepartmentID='{1}'", dp.department, dp.DepartmentId);
            DataBase.ExecuteQuerry(Querry);
        }

        public void AddDeparment(string Deparment)
        {
            string Querry = string.Format("insert into Departments(Department) values('{0}')", Deparment);
            DataBase.ExecuteQuerry(Querry);
        }

        public void DeleteDeparment(string DeparmentID)
        {
            string Querry = string.Format("delete from Departments where DepartmentID='{0}'", DeparmentID);
            DataBase.ExecuteQuerry(Querry);
        }

        public void updateDesignation(Designation ds)
        {
            string Querry = string.Format("update Designations set Designation='{0}' where DesignationID='{1}'", ds.designation, ds.DesignationID);
            DataBase.ExecuteQuerry(Querry);
        }

        public void AddDesignation(Designation ds)
        {
            string Querry = string.Format("insert into Designations(Designation) values('{0}')", ds.designation);
            DataBase.ExecuteQuerry(Querry);
        }

        public void DeleteDesignation(Designation ds)
        {
            string Querry = string.Format("delete from Designations where DesignationID='{0}'", ds.DesignationID);
            DataBase.ExecuteQuerry(Querry);
        }

        public void updateLeaveType(LeaveTypes lt)
        {
            string Querry = string.Format("update LeaveType set LeaveType='{0}' where LeaveTypeID='{1}'", lt.LeaveType, lt.LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);
        }

        public void AddLeaveType(LeaveTypes lt)
        {
            string Querry = string.Format("insert into LeaveType(LeaveType) values('{0}')", lt.LeaveType);
            DataBase.ExecuteQuerry(Querry);
        }

        public void DeleteLeaveType(LeaveTypes lt)
        {
            string Querry = string.Format("delete from LeaveType where LeaveTypeID='{0}'", lt.LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);
        }
        public void AssignLeave()
        {
            string Querry = string.Format("insert into EmployeeLeaveCountHistory (EmployeeID,LeaveTypeID,Count,Date) values('{0}','{1}','{2}','{3}')", Al.EmployeeID, Al.LeaveTypeID, Al.Count, DateTime.Now);
            DataBase.ExecuteQuerry(Querry);

            Al.Count += GetLeaveCount(Al);
            Querry = string.Format("update  EmployeeLeaveCount set Count = '{0}' where EmployeeID = '{1}'and LeaveTypeID ='{2}'  " +
                "if @@ROWCOUNT = 0 " +
                "insert into EmployeeLeaveCount(Count,EmployeeID,LeaveTypeID) values('{0}', '{1}', '{2}')", Al.Count, Al.EmployeeID, Al.LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);

        }
        public void AssignLeave(AssignLeaves al)
        {//this function is used by assign all or assign_all_dep
            string Querry = string.Format("insert into EmployeeLeaveCountHistory (EmployeeID,LeaveTypeID,Count,Date) values('{0}','{1}','{2}','{3}')", al.EmployeeID, al.LeaveTypeID, al.Count, DateTime.Now);
            DataBase.ExecuteQuerry(Querry);

            al.Count += GetLeaveCount(al);
            Querry = string.Format("update  EmployeeLeaveCount set Count = '{0}' where EmployeeID = '{1}'and LeaveTypeID ='{2}'  " +
                "if @@ROWCOUNT = 0 " +
                "insert into EmployeeLeaveCount(Count,EmployeeID,LeaveTypeID) values('{0}', '{1}', '{2}')", al.Count, al.EmployeeID, al.LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);

        }
        /// <summary>
        /// Assign Leave to all employees
        /// </summary>
        public void AssignAll()
        {
            string Querry = "select EmployeeID from Employee";
            DataSet ds = DataBase.Read(Querry);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                AssignLeave(new AssignLeaves() { EmployeeID = ds.Tables[0].Rows[i][0].ToString(), LeaveTypeID = Al.LeaveTypeID, Count = Al.Count });
            }
        }/// <summary>
         /// Assign Leaves to all department employees
         /// </summary>
        public void AssignAllDep()
        {
            string Querry = string.Format("select * from Employee where DepartmentID='{0}'", Al.DepartmentID);
            DataSet ds = DataBase.Read(Querry);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                AssignLeave(new AssignLeaves() { EmployeeID = ds.Tables[0].Rows[i][0].ToString(), LeaveTypeID = Al.LeaveTypeID, Count = Al.Count });
            }
        }
        /// <summary>
        /// It return count of employee's remaining leaves
        /// </summary>
        public int GetLeaveCount(AssignLeaves al)
        {
            string Querry = string.Format("select EmployeeLeaveCount.Count  from EmployeeLeaveCount where EmployeeID='{0}'and LeaveTypeID='{1}'", al.EmployeeID, al.LeaveTypeID);
            return Convert.ToInt32(DataBase.ExecuteScalar(Querry));
        }
        public DataSet ShowAssignLeaveHistory()
        {
            string Querry = "select EmployeeLeaveCountHistory.Date,Employee.EmployeeName,LeaveType.LeaveType,EmployeeLeaveCountHistory.Count from EmployeeLeaveCountHistory inner join Employee on Employee.EmployeeID=EmployeeLeaveCountHistory.EmployeeID inner join LeaveType on LeaveType.LeaveTypeID=EmployeeLeaveCountHistory.LeaveTypeID order by EmployeeLeaveCountHistory.Date desc";
            DataSet ds = DataBase.Read(Querry);
            return ds;
        }
        public DataSet ShowAffectedUsers(AssignLeaves al,String Querry)
        {//this method store the data temporarily from incoming assign leave request for further processing and return data for users who are getting affected by this request
            DataSet ds = DataBase.Read(Querry);
            Al = al;
            return ds;
        }
        public void RemoveAssignLeaveRequest()
        {//this method remove the request data from temporary method after recording in the database or if user cancel the request
           Al = null;

        }
        public void RequestableStateChange(string LeaveTypeID,bool IsRequestable)
        {
            //change enable/disable isrequestable for leave type based on user input
            string Querry= string.Format("update LeaveType set IsRequestable='{0}' where LeaveTypeID='{1}'", IsRequestable, LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);

        }
        public Employee ReadEmployee(int EmployeeID)
        {

            string Querry = string.Format("select Employee.EmployeeID,Employee.UserName,Employee.EmployeeName,Employee.Address,Employee.PhoneNumber,Employee.CNIC,Employee.JoiningDate,Designations.Designation,Departments.Department,Picture.Picture,Employee.IsAdmin  from Employee inner join Picture on Employee.EmployeeID=Picture.EmployeeID inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join Designations on Employee.DesignationID=Designations.DesignationID where Employee.EmployeeID='{0}'", EmployeeID);

            DataSet d1 = DataBase.Read(Querry);

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
            return Employee;

        }
        public string GetBase64Image(Byte[] Image)
        {
            string temp = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(Image));
            return temp;
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
    }

}