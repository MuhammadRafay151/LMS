﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using LeaveApplication.Exceptional_Classes;

namespace LeaveApplication.Models
{
    public class AdminBusinessLayer
    {
        private db DataBase = new db();

        private EmployeeBusinessLayer eb = new EmployeeBusinessLayer();


        //public void AssignLeave(AssignLeaves Al)
        //{
        //    string Querry = string.Format("insert into EmployeeLeaveCountHistory (EmployeeID,LeaveTypeID,Count,Date) values('{0}','{1}','{2}','{3}')", Al.EmployeeID, Al.LeaveTypeID, Al.Count, DateTime.Now);
        //    DataBase.ExecuteQuerry(Querry);

        //    Al.Count += GetLeaveCount(Al);
        //    Querry = string.Format("update  EmployeeLeaveCount set Count = '{0}' where EmployeeID = '{1}'and LeaveTypeID ='{2}'  " +
        //        "if @@ROWCOUNT = 0 " +
        //        "insert into EmployeeLeaveCount(Count,EmployeeID,LeaveTypeID) values('{0}', '{1}', '{2}')", Al.Count, Al.EmployeeID, Al.LeaveTypeID);
        //    DataBase.ExecuteQuerry(Querry);

        //}
        public void AssignLeave(AssignLeaves al)
        {//this function is used by assign all or assign_all_dep or single assign employee
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
        public void AssignAll(AssignLeaves Al)
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
        public void AssignAllDep(AssignLeaves Al)
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
        public double GetLeaveCount(AssignLeaves al)
        {//this table has employee leave balance...
            string Querry = string.Format("select EmployeeLeaveCount.Count  from EmployeeLeaveCount where EmployeeID='{0}'and LeaveTypeID='{1}'", al.EmployeeID, al.LeaveTypeID);
            return Convert.ToDouble(DataBase.ExecuteScalar(Querry));
        }

        public DataSet ShowAssignLeaveHistory()
        {
            string Querry = "select EmployeeLeaveCountHistory.Date,Employee.EmployeeName,LeaveType.LeaveType,EmployeeLeaveCountHistory.Count from EmployeeLeaveCountHistory inner join Employee on Employee.EmployeeID=EmployeeLeaveCountHistory.EmployeeID inner join LeaveType on LeaveType.LeaveTypeID=EmployeeLeaveCountHistory.LeaveTypeID order by EmployeeLeaveCountHistory.Date desc";
            DataSet ds = DataBase.Read(Querry);
            return ds;
        }

        public DataSet ShowAffectedUsers(AssignLeaves al, String Querry)
        {//this method store the data temporarily from incoming assign leave request for further processing and return data for users who are getting affected by this request
            DataSet ds = DataBase.Read(Querry);
            return ds;
        }

        public void RequestableStateChange(string LeaveTypeID, bool IsRequestable)
        {
            //change enable/disable isrequestable for leave type based on user input
            string Querry = string.Format("update LeaveType set IsRequestable='{0}' where LeaveTypeID='{1}'", IsRequestable, LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);
        }

        public void EmployeeStateChange(string EmployeeID, bool IsActive)
        {
            //change enable/disable isactive for Employee based on user input
            string Querry = string.Format("update Employee set IsActive='{0}' where EmployeeID='{1}'", IsActive, EmployeeID);
            DataBase.ExecuteQuerry(Querry);
        }

        public void UpdateEmployee(Employee Emp)
        {
            if (!eb.IsUserAvailable(Emp.EmployeeID, Emp.UserName))
                throw new DuplicateException(1);
            if (!eb.IsEmpNoAvailable(Emp.EmployeeID, Emp.EmpNo))
                throw new DuplicateException(2);
            string Querry;
            string UserName = eb.GetUserName(Emp.EmployeeID);
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
                if (Emp.Image == null)
                {
                    Querry = string.Format(@"UPDATE Users Set UserName='{0}' where Users.UserName='{2}'
update employee set  employeename = '{3}', address = '{4}', PhoneNumber = '{5}', cnic = '{6}', DesignationID = '{7}', DepartmentID = '{8}',Email='{10}',EmpNo='{11}',IsAdmin='{12}' where Employee.EmployeeID = '{9}'",
Emp.UserName, Emp.Password, UserName, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DesignationID, Emp.DepartmentID, Emp.EmployeeID, Emp.Email, Emp.EmpNo, Emp.isAdmin);
                    DataBase.ExecuteQuerry(Querry);
                }
                else
                {
                    Querry = string.Format(@"UPDATE Users Set UserName='{0}' where Users.UserName='{2}'
update employee set  employeename = '{3}', address = '{4}', PhoneNumber = '{5}', cnic = '{6}', DesignationID = '{7}', DepartmentID = '{8}',Email='{10}',EmpNo='{11}',IsAdmin='{12}'  where Employee.EmployeeID = '{9}'
Update Picture set Picture.Picture = @img where Picture.EmployeeID = '{9}'",
  Emp.UserName, Emp.Password, UserName, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DesignationID, Emp.DepartmentID, Emp.EmployeeID, Emp.Email, Emp.EmpNo, Emp.isAdmin);
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "img";
                    p1.Value = bytes;
                    DataBase.ExecuteQuerry(Querry, p1);
                }
            }
            else
            {
                if (Emp.Image == null)
                {
                    Querry = string.Format(@"UPDATE Users Set UserName='{0}' where Users.UserName='{2}'
update employee set  employeename = '{3}', address = '{4}', PhoneNumber = '{5}', cnic = '{6}', DesignationID = '{7}', DepartmentID = '{8}',Manager='{10}',Email='{11}',EmpNo='{12}',IsAdmin='{13}'  where Employee.EmployeeID = '{9}'",
Emp.UserName, Emp.Password, UserName, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DesignationID, Emp.DepartmentID, Emp.EmployeeID, Emp.Manager, Emp.Email, Emp.EmpNo, Emp.isAdmin);

                    DataBase.ExecuteQuerry(Querry);
                }
                else
                {
                    Querry = string.Format(@"UPDATE Users Set UserName='{0}' where Users.UserName='{2}'
update employee set  employeename = '{3}', address = '{4}', PhoneNumber = '{5}', cnic = '{6}', DesignationID = '{7}', DepartmentID = '{8}',Manager='{10}',Email='{11}',EmpNo='{12}',IsAdmin='{13}'  where Employee.EmployeeID = '{9}'
Update Picture set Picture.Picture = @img where Picture.EmployeeID = '{9}'",
Emp.UserName, Emp.Password, UserName, Emp.EmployeeName, Emp.Address, Emp.PhoneNumber, Emp.CNIC, Emp.DesignationID, Emp.DepartmentID, Emp.EmployeeID, Emp.Manager, Emp.Email, Emp.EmpNo, Emp.isAdmin);
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "img";
                    p1.Value = bytes;
                    DataBase.ExecuteQuerry(Querry, p1);
                }
            }
        }

       
    }
}