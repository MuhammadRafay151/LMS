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
        db DataBase = new db();

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
        public void AssignLeave(EmployeeLeaveCount EL)
        {
            string Querry = string.Format("insert into EmployeeLeaveCountHistory (EmployeeID,LeaveTypeID,Count,Date) values('{0}','{1}','{2}','{3}')", EL.EmployeeID, EL.LeaveTypeID, EL.Count, DateTime.Now);
            DataBase.ExecuteQuerry(Querry);

            EL.Count += GetLeaveCount(EL);
            Querry = string.Format("update  EmployeeLeaveCount set Count = '{0}' where EmployeeID = '{1}'and LeaveTypeID ='{2}'  " +
                "if @@ROWCOUNT = 0 " +
                "insert into EmployeeLeaveCount(Count,EmployeeID,LeaveTypeID) values('{0}', '{1}', '{2}')", EL.Count, EL.EmployeeID, EL.LeaveTypeID);
            DataBase.ExecuteQuerry(Querry);

        }
        /// <summary>
        /// It return count of employee's remaining leaves
        /// </summary>
        public int GetLeaveCount(EmployeeLeaveCount EL)
        {
            string Querry = string.Format("select EmployeeLeaveCount.Count  from EmployeeLeaveCount where EmployeeID='{0}'and LeaveTypeID='{1}'", EL.EmployeeID, EL.LeaveTypeID);
            return Convert.ToInt32(DataBase.ExecuteScalar(Querry));
        }


    }

}