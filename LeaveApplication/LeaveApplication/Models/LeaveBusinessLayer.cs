using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace LeaveApplication.Models
{
    public class LeaveBusinessLayer
    {
        db database = new db();
        SqlConnection con;
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds;
        static LeaveApplication leave;

        public int CalculateTotalLeaveDays(LeaveApplication l1)
        {

            string x = (DateTime.Parse(l1.ToDate) - DateTime.Parse(l1.FromDate)).TotalDays.ToString();

            return (int.Parse(x) + 1);
        }
        public void SaveApplication(LeaveApplication a1)
        {
            a1.TotalDays = CalculateTotalLeaveDays(a1);
            a1.ApplyDate = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
            con = new SqlConnection(connection);
            con.Open();
            string Querry = string.Format("insert into LeaveApplication (EmployeeID,LeaveTypeID,[ApplyDate],FromDate,ToDate,[TotalDays],Remarks,ReasonID) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]", a1.EmployeeID, a1.LeaveType, a1.ApplyDate, a1.FromDate,
               a1.ToDate, a1.TotalDays, a1.LeaveRemarks, a1.LeaveReason);
            cmd = new SqlCommand(Querry, con);
            //cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                a1.ApplicationId = reader[0].ToString();
            }
            reader.Close();
            Querry = string.Format("insert into StatusHistory (LeaveApplicationID,Date,ApplicationStatusID) values ('{0}','{1}','{2}')", a1.ApplicationId, a1.ApplyDate, Status.s1.ToString());
            cmd = new SqlCommand(Querry, con);
            cmd.ExecuteNonQuery(); cmd.Dispose();
            con.Close();
        }
        public LeaveTypes[] GetLeaveTypes()
        {
           
            string Querry = "select LeaveTypeID,LeaveType from LeaveType";
         
            ds = database.Read(Querry); ;
           
            LinkedList<LeaveTypes> list = new LinkedList<LeaveTypes>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                LeaveTypes type = new LeaveTypes();
                type.LeaveTypeID = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                type.LeaveType = ds.Tables[0].Rows[i][1].ToString();
                list.AddLast(type);
            }
            return list.ToArray();
        }
        /// <summary>
        /// return leave types in dataset
        /// </summary>
        /// <returns></returns>
        public  DataSet GetLeaveTypesDS()
        {

            string Querry = "select LeaveTypeID,LeaveType from LeaveType";

            ds = database.Read(Querry); ;

          
            return ds;
        }
        public List<LeaveApplication> GetAllApplications(string EmployeeID)
        {
          
            string Querry = string.Format("select LeaveApplication.LeaveApplicationID,LeaveApplication.EmployeeID,LeaveType.LeaveType,LeaveApplication.ApplyDate,LeaveApplication.FromDate,LeaveApplication.ToDate,LeaveApplication.TotalDays,LeaveApplication.Remarks,Reasons.LeaveReason from LeaveApplication inner join LeaveType on LeaveApplication.LeaveTypeID=LeaveType.LeaveTypeID inner join Reasons on LeaveApplication.ReasonID=Reasons.ReasonID where LeaveApplication.EmployeeID='{0}'", EmployeeID);
           
            DataSet ds = database.Read(Querry);
           
            List<Models.LeaveApplication> la = new List<Models.LeaveApplication>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                LeaveApplication v1 = new Models.LeaveApplication();
                v1.ApplicationId = ds.Tables[0].Rows[i][0].ToString();
                v1.EmployeeID = ds.Tables[0].Rows[i][1].ToString();
                v1.LeaveType = ds.Tables[0].Rows[i][2].ToString();
                v1.ApplyDate = DateTime.Parse(ds.Tables[0].Rows[i][3].ToString()).ToString();
                v1.FromDate = DateTime.Parse(ds.Tables[0].Rows[i][4].ToString()).ToString("dd-MM-yyyy");
                v1.ToDate = DateTime.Parse(ds.Tables[0].Rows[i][5].ToString()).ToString("dd-MM-yyyy");
                v1.TotalDays = int.Parse(ds.Tables[0].Rows[i][6].ToString());
                v1.LeaveRemarks = ds.Tables[0].Rows[i][7].ToString();
                v1.LeaveReason = ds.Tables[0].Rows[i][8].ToString();
                v1.ApplicationStatus = GetApplicationStatus(v1.ApplicationId);
                la.Add(v1);


                //leave reason is remaining
            }
            ds.Dispose();
            return la;
        }
        public List<LeaveApplication> GetPendingApplications(string EmployeeID)
        {
            List<LeaveApplication> l1 = GetAllApplications(EmployeeID);
            List<LeaveApplication> l2 = new List<LeaveApplication>();
            foreach (LeaveApplication x in l1)
            {
                if (x.ApplicationStatus == "Pending")
                {
                    l2.Add(x);
                }
            }
            l1 = null;
            return l2;
        }
        public List<LeaveApplication> GetApprovedApplications(string EmployeeID)
        {
            List<LeaveApplication> l1 = GetAllApplications(EmployeeID);
            List<LeaveApplication> l2 = new List<LeaveApplication>();
            foreach (LeaveApplication x in l1)
            {
                if (x.ApplicationStatus == "Approved")
                {
                    l2.Add(x);
                }
            }
            l1 = null;
            return l2;
        }
        public List<LeaveApplication> GetRejectedApplications(string EmployeeID)
        {
            List<LeaveApplication> l1 = GetAllApplications(EmployeeID);
            List<LeaveApplication> l2 = new List<LeaveApplication>();
            foreach (LeaveApplication x in l1)
            {
                if (x.ApplicationStatus == "Rejected")
                {
                    l2.Add(x);
                }
            }
            l1 = null;
            return l2;
        }
        public LeaveApplication GetApplication(string Application_Id)
        {
            //return application for edit application feature...
         
            string Querry = string.Format("select LeaveApplication.LeaveApplicationID,LeaveApplication.EmployeeID,LeaveType.LeaveTypeID,LeaveApplication.ApplyDate,LeaveApplication.FromDate,LeaveApplication.ToDate,LeaveApplication.TotalDays,LeaveApplication.Remarks,LeaveApplication.ReasonID from LeaveApplication inner join LeaveType on LeaveApplication.LeaveTypeID=LeaveType.LeaveTypeID  where LeaveApplication.LeaveApplicationID='{0}'", Application_Id);
           
            DataSet ds = database.Read(Querry);
         

            LeaveApplication v1 = new Models.LeaveApplication();
            v1.ApplicationId = ds.Tables[0].Rows[0][0].ToString();
            v1.EmployeeID = ds.Tables[0].Rows[0][1].ToString();
            v1.LeaveTypeID = ds.Tables[0].Rows[0][2].ToString();
            v1.LeaveType = ds.Tables[0].Rows[0][2].ToString();
            v1.ApplyDate = DateTime.Parse(ds.Tables[0].Rows[0][3].ToString()).ToString("yyyy-MM-dd");
            v1.FromDate = DateTime.Parse(ds.Tables[0].Rows[0][4].ToString()).ToString("yyyy-MM-dd");
            v1.ToDate = DateTime.Parse(ds.Tables[0].Rows[0][5].ToString()).ToString("yyyy-MM-dd");
            v1.TotalDays = int.Parse(ds.Tables[0].Rows[0][6].ToString());
            v1.LeaveRemarks = ds.Tables[0].Rows[0][7].ToString();
            v1.LeaveReason = ds.Tables[0].Rows[0][8].ToString();
            v1.ApplicationStatus = GetApplicationStatus(v1.ApplicationId);
          
            leave = v1;
            if (v1.ApplicationStatus == "Pending")
            {
                leave = v1;
                return v1;
            }
            else
                return null;

        }
        public LeaveApplication GetViewApplication(string Application_Id)
        {

           
            string Querry = string.Format("select LeaveApplication.LeaveApplicationID,LeaveApplication.EmployeeID,LeaveType.LeaveTypeID,LeaveApplication.ApplyDate,LeaveApplication.FromDate,LeaveApplication.ToDate,LeaveApplication.TotalDays,LeaveApplication.Remarks,LeaveApplication.ReasonID from LeaveApplication inner join LeaveType on LeaveApplication.LeaveTypeID=LeaveType.LeaveTypeID  where LeaveApplication.LeaveApplicationID='{0}'", Application_Id);
           
            DataSet ds = database.Read(Querry);
          
            LeaveApplication v1 = new Models.LeaveApplication();
            v1.ApplicationId = ds.Tables[0].Rows[0][0].ToString();
            v1.EmployeeID = ds.Tables[0].Rows[0][1].ToString();
            v1.LeaveType = ds.Tables[0].Rows[0][2].ToString();
            v1.ApplyDate = DateTime.Parse(ds.Tables[0].Rows[0][3].ToString()).ToString("dd-MM-yyyy");
            v1.FromDate = DateTime.Parse(ds.Tables[0].Rows[0][4].ToString()).ToString("dd-MM-yyyy");
            v1.ToDate = DateTime.Parse(ds.Tables[0].Rows[0][5].ToString()).ToString("dd-MM-yyyy");
            v1.TotalDays = int.Parse(ds.Tables[0].Rows[0][6].ToString());
            v1.LeaveRemarks = ds.Tables[0].Rows[0][7].ToString();
            v1.LeaveReason = ds.Tables[0].Rows[0][8].ToString();
            v1.ApplicationStatus = GetApplicationStatus(v1.ApplicationId);
           

            return v1;
        }
        public void SaveChanges(LeaveApplication l1)
        {
            l1.TotalDays = CalculateTotalLeaveDays(l1);
            l1.ApplicationId = leave.ApplicationId;
           
            string Querry = string.Format("update LeaveApplication set LeaveTypeID='{0}',FromDate='{1}',ToDate='{2}',TotalDays='{3}',Remarks='{4}',ReasonID='{5}' where LeaveApplicationID='{6}'", l1.LeaveType, l1.FromDate, l1.ToDate, l1.TotalDays, l1.LeaveRemarks, l1.LeaveReason, l1.ApplicationId);
            database.ExecuteQuerry(Querry);
        }
        private string GetApplicationStatus(string ApplicationId)
        {

            string Querry = string.Format("select ApplicationStatusID,Date from statushistory where LeaveApplicationID='{0}'", ApplicationId);

            DataSet ds1 = database.Read(Querry); ;

            DateTime d1, d2;
            string st;
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 1)
                {
                    d1 = DateTime.Parse(ds1.Tables[0].Rows[0][1].ToString());
                    d2 = DateTime.Parse(ds1.Tables[0].Rows[1][1].ToString());
                    if (DateTime.Compare(d1, d2) < 0)
                    {
                        st = ds1.Tables[0].Rows[1][0].ToString();
                    }
                    else
                    {
                        st = ds1.Tables[0].Rows[0][0].ToString();
                    }

                }
                else
                {
                    st = ds1.Tables[0].Rows[0][0].ToString();
                }
                Querry = string.Format("Select ApplicationStatus from ApplicationStatus where ApplicationStatusID ='{0}'", st);
                ds1 = database.Read(Querry); ;

                return ds1.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return null;
            }


        }
        public List<StatusHistory> GetStatusHistory(string ApplicationId)
        {
            List<StatusHistory> SH = new List<StatusHistory>();
            string Querry = string.Format("select * from statushistory where LeaveApplicationID='{0}' order by StatusHistory.Date asc", ApplicationId);
            DataSet ds1 = database.Read(Querry);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                StatusHistory sh = new StatusHistory();
                sh.Date = DateTime.Parse(ds1.Tables[0].Rows[i][2].ToString()).ToString("dd-MM-yyyy");
                string st = ds1.Tables[0].Rows[i][3].ToString();
                Querry = string.Format("Select ApplicationStatus from ApplicationStatus where ApplicationStatusID ='{0}'", st);
                DataSet ds2 = database.Read(Querry); ;
                sh.Status = ds2.Tables[0].Rows[0][0].ToString();
                SH.Add(sh);
            }
           

            return SH;

        }
        public void CancelApplication(String ApplicationId)
        {
            if (GetApplicationStatus(ApplicationId) == "Pending")
            {

                string Querry = string.Format("Delete from LeaveApplication where LeaveApplicationID='{0}'", ApplicationId);
                database.ExecuteQuerry(Querry);
            }

        }
        public List<LeaveApplication> GetFacultyAll()
        {

            string Querry = string.Format("select LeaveApplication.LeaveApplicationID,LeaveApplication.EmployeeID,Employee.EmployeeName,LeaveType.LeaveType,LeaveApplication.ApplyDate,LeaveApplication.FromDate,LeaveApplication.ToDate,LeaveApplication.TotalDays,LeaveApplication.Remarks,Reasons.LeaveReason  from LeaveApplication INNER JOIN Employee on Employee.EmployeeID=LeaveApplication.EmployeeID inner join LeaveType on LeaveApplication.LeaveTypeID=LeaveType.LeaveTypeID inner join Reasons on Reasons.ReasonID=LeaveApplication.ReasonID where Employee.Manager='{0}'", EmployeeBusinessLayer.Employee.EmployeeID);
            ds = database.Read(Querry);
            List<Models.LeaveApplication> la = new List<Models.LeaveApplication>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                LeaveApplication v1 = new Models.LeaveApplication();
                v1.ApplicationId = ds.Tables[0].Rows[i][0].ToString();
                v1.EmployeeID = ds.Tables[0].Rows[i][1].ToString();
                v1.EmployeeName = ds.Tables[0].Rows[i][2].ToString();
                v1.LeaveType = ds.Tables[0].Rows[i][3].ToString();
                v1.ApplyDate = DateTime.Parse(ds.Tables[0].Rows[i][4].ToString()).ToString("dd-MM-yyyy");
                v1.FromDate = DateTime.Parse(ds.Tables[0].Rows[i][5].ToString()).ToString("dd-MM-yyyy");
                v1.ToDate = DateTime.Parse(ds.Tables[0].Rows[i][6].ToString()).ToString("dd-MM-yyyy");
                v1.TotalDays = int.Parse(ds.Tables[0].Rows[i][7].ToString());
                v1.LeaveRemarks = ds.Tables[0].Rows[i][8].ToString();
                v1.LeaveReason = ds.Tables[0].Rows[i][9].ToString();
                v1.ApplicationStatus = GetApplicationStatus(v1.ApplicationId);
                la.Add(v1);


                //leave reason is remaining
            }
            ds.Dispose();
            return la;

        }
        public List<LeaveApplication> GetFacultyPending()
        {
            List<LeaveApplication> l1 = GetFacultyAll();
            List<LeaveApplication> l2 = new List<LeaveApplication>();
            foreach (LeaveApplication x in l1)
            {
                if (x.ApplicationStatus == "Pending")
                {
                    l2.Add(x);
                }
            }
            l1 = null;
            return l2;
        }
        public List<LeaveApplication> GetFacultyApproved()
        {
            List<LeaveApplication> l1 = GetFacultyAll();
            List<LeaveApplication> l2 = new List<LeaveApplication>();
            foreach (LeaveApplication x in l1)
            {
                if (x.ApplicationStatus == "Approved")
                {
                    l2.Add(x);
                }
            }
            l1 = null;
            return l2;
        }
        public List<LeaveApplication> GetFacultyReject()
        {
            List<LeaveApplication> l1 = GetFacultyAll();
            List<LeaveApplication> l2 = new List<LeaveApplication>();
            foreach (LeaveApplication x in l1)
            {
                if (x.ApplicationStatus == "Rejected")
                {
                    l2.Add(x);
                }
            }
            l1 = null;
            return l2;
        }
        public void AcceptApplication(string ApplicationID)
        {
            string Querry = string.Format("insert into StatusHistory(LeaveApplicationID,Date,ApplicationStatusID) select '{0}','{1}',ApplicationStatus.ApplicationStatusID from ApplicationStatus where ApplicationStatus.ApplicationStatus='Approved' ", ApplicationID, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss"));
            database.ExecuteQuerry(Querry);

        }
        public void RejectApplication(string ApplicationID)
        {
            string Querry = string.Format("insert into StatusHistory(LeaveApplicationID,Date,ApplicationStatusID) select '{0}','{1}',ApplicationStatus.ApplicationStatusID from ApplicationStatus where ApplicationStatus.ApplicationStatus='Rejected' ", ApplicationID, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss"));
            database.ExecuteQuerry(Querry);
        }
        private string GetApplicationId()
        {//sample id App1
            String id = "";
            con = new SqlConnection(connection);
            con.Open();
            string Querry = "select MAX([Application id]) from ApplyLeaveApplication";
            cmd = new SqlCommand(Querry, con);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                id = reader[0].ToString();
            }
            int x;
            if (id != string.Empty)
            {
                x = int.Parse(id[3].ToString());
            }
            else
            {
                x = 0;
            }

            x += 1;
            id = "App" + x;
            con.Close();
            return id;
        }
        public DataSet GetFaculty()
        {
            string Querry = string.Format("select EmployeeID ,EmployeeName from employee where manager='{0}'", EmployeeBusinessLayer.Employee.EmployeeID);
            return database.Read(Querry); ;
        }
        public DataSet GetReasons()
        {
            string Querry = string.Format("select * from Reasons");
            return database.Read(Querry);
        }
        public DataSet GetLeaveCount()
        {
            string Querry = string.Format(@"declare @a NVARCHAR(MAX) = '',@b NVARCHAR(MAX) = '';
declare @id NVARCHAR(MAX)='{0}',@st NVARCHAR(MAX)='s2';
select @a+=QUOTENAME(LeaveType.LeaveType) + ',' from LeaveType

SET @a = LEFT(@a, LEN(@a) - 1);
print @st
 SET @b='
select * from
 (
 select StatusHistory.ApplicationStatusID,LeaveType.LeaveType from Employee inner join LeaveApplication on Employee.EmployeeID=@id and Employee.EmployeeID=LeaveApplication.EmployeeID inner join StatusHistory on StatusHistory.LeaveApplicationID=LeaveApplication.LeaveApplicationID and StatusHistory.ApplicationStatusID=@st inner join LeaveType on LeaveType.LeaveTypeID=LeaveApplication.LeaveTypeID
 )t
 pivot(
count(ApplicationStatusID) for LeaveType in('+@a+')
)as ax';
EXECUTE sp_executesql @b, N'@id NVARCHAR(MAX),@st NVARCHAR(MAX)', @id = @id,@st=@st;", EmployeeBusinessLayer.Employee.EmployeeID);

            return database.Read(Querry);
        }
        public DataSet FacultyLeaveCount()
        {
            string Querry = string.Format(@"declare @a NVARCHAR(MAX) = '',@b NVARCHAR(MAX) = '';
declare @id NVARCHAR(MAX)='{0}',@st NVARCHAR(MAX)='s2';
select @a+=QUOTENAME(LeaveType.LeaveType) + ',' from LeaveType

SET @a = LEFT(@a, LEN(@a) - 1);
print @st
 SET @b='
select * from
 (
 select Employee.EmployeeName,StatusHistory.ApplicationStatusID,LeaveType.LeaveType from Employee inner join LeaveApplication on Employee.Manager=@id and Employee.EmployeeID=LeaveApplication.EmployeeID inner join StatusHistory on StatusHistory.LeaveApplicationID=LeaveApplication.LeaveApplicationID and StatusHistory.ApplicationStatusID=@st inner join LeaveType on LeaveType.LeaveTypeID=LeaveApplication.LeaveTypeID
 
 )t
 pivot(
count(ApplicationStatusID) for LeaveType in('+@a+')
)as ax';
EXECUTE sp_executesql @b, N'@id NVARCHAR(MAX),@st NVARCHAR(MAX)', @id = @id,@st=@st;", EmployeeBusinessLayer.Employee.EmployeeID);

            return database.Read(Querry);
        }


    }
}

