﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;

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

        public int CalculateTotalLeaveDays(LeaveApplication l1)
        {

            string x = (DateTime.ParseExact(l1.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) - DateTime.ParseExact(l1.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).TotalDays.ToString();
            return (int.Parse(x) + 1);
        }
        public Double CalculateLeaveHours(LeaveApplication l1)
        {
            DateTime d1 = DateTime.Parse(DateTimeHelper.ToDateTime(l1.FromDate));
            DateTime d2 = DateTime.Parse(DateTimeHelper.ToDateTime(l1.ToDate));
            double Hrs = (d2 - d1).TotalHours;
            return Hrs;
        }
        public void SaveApplication(LeaveApplication a1)
        {
            string Querry;
            con = new SqlConnection(connection);
            con.Open();
            cmd = new SqlCommand();
            cmd.Connection = con;
            if (a1.IsHalfDay == 0)
            {
                a1.TotalDays = CalculateTotalLeaveDays(a1);
                a1.ToDate = DateTime.ParseExact(a1.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
                a1.FromDate = DateTime.ParseExact(a1.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
            }
            else if (a1.IsHalfDay == 1)
            {
                a1.TotalDays = 0.5;
                a1.FromDate = DateTimeHelper.ToDateTime(a1.FromDate);
                a1.ToDate = DateTimeHelper.ToDateTime(a1.ToDate);
            }

            a1.ApplyDate = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");


            if (string.IsNullOrWhiteSpace(a1.LeaveRemarks))
            {
                Querry = string.Format("insert into LeaveApplication (EmployeeID,LeaveTypeID,[ApplyDate],FromDate,ToDate,[TotalDays],ReasonID,ApplicationType) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]", a1.EmployeeID, a1.LeaveType, a1.ApplyDate, a1.FromDate,
                a1.ToDate, a1.TotalDays, a1.LeaveReason, a1.ApplicationType);

            }
            else
            {
                Querry = string.Format("insert into LeaveApplication (EmployeeID,LeaveTypeID,[ApplyDate],FromDate,ToDate,[TotalDays],Remarks,ReasonID,ApplicationType) values('{0}','{1}','{2}','{3}','{4}','{5}',@Remarks,'{6}','{7}') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]", a1.EmployeeID, a1.LeaveType, a1.ApplyDate, a1.FromDate,
               a1.ToDate, a1.TotalDays, a1.LeaveReason, a1.ApplicationType);
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "Remarks", Value = a1.LeaveRemarks });
            }
            cmd.CommandText = Querry;

            //cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                a1.ApplicationId = reader[0].ToString();
            }
            reader.Close();
            con.Close();
            if (a1.Attachment != null)
            {
                InsertAttachement(a1.Attachment, int.Parse(a1.ApplicationId));
            }

            Querry = string.Format("insert into StatusHistory (LeaveApplicationID,Date,ApplicationStatusID) values ('{0}','{1}','{2}')", a1.ApplicationId, a1.ApplyDate, 1);//1 means pending...
            database.ExecuteQuerry(Querry);



        }
        public byte[] GetFileBytes(HttpPostedFileBase file)
        {
            Byte[] bytes = null;
            if (file != null)
            {
                Stream s1 = file.InputStream;
                BinaryReader b1 = new BinaryReader(s1);
                bytes = b1.ReadBytes((int)s1.Length);

            }
            return bytes;
        }
        public void InsertAttachement(HttpPostedFileBase file, int ApplicationId)
        {
            string Name = file.FileName;
            string Querry = string.Format(@"declare @id int=''
            insert into Files([Content])values(@File)
            select @id = SCOPE_IDENTITY()
            insert into Attachments(FileId, FileName, LeaveApplicationId) values(@id,'{0}','{1}')", Name, ApplicationId);
            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "File";
            p1.Value = GetFileBytes(file);
            database.ExecuteQuerry(Querry, p1);
        }
        public DataSet DownloadFile(int FileId)
        {
            string Querry = string.Format("select Files.Content,Attachments.FileName from Files inner join Attachments on Attachments.FileId = Files.FileId where Files.FileId ={0}", FileId);
            return database.Read(Querry);
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
        public DataSet GetLeaveTypesDS()
        {

            string Querry = "select LeaveTypeID,LeaveType,IsRequestable from LeaveType";

            ds = database.Read(Querry); ;


            return ds;
        }
        public DataSet GetRequestableLeaves()
        {//return leave types those are requestable
            string Querry = String.Format("select LeaveTypeID,LeaveType,IsRequestable from LeaveType where IsRequestable=1");
            return database.Read(Querry);
        }
        public DataSet GetAllApplications(string EmployeeID)
        {
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,
a.TotalDays,Reasons.LeaveReason,ApplicationStatus.ApplicationStatus,a.ApplicationType 
from LeaveApplication a inner join
( select  LeaveApplication.LeaveApplicationID,Max(StatusHistory.ApplicationStatusID) as StatusId 
from StatusHistory inner join LeaveApplication on 
LeaveApplication.LeaveApplicationID=StatusHistory.LeaveApplicationID inner join 
ApplicationStatus on StatusHistory.ApplicationStatusID=ApplicationStatus.ApplicationStatusID 
where LeaveApplication.EmployeeID={0}
group by LeaveApplication.LeaveApplicationID)bc on
a.LeaveApplicationID = bc.LeaveApplicationID inner join 
LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID inner join 
Reasons on a.ReasonID = Reasons.ReasonID
inner join ApplicationStatus on 
bc.StatusId = ApplicationStatus.ApplicationStatusID  where a.EmployeeID = {0}
order by a.ApplyDate desc", EmployeeID);
            return database.Read(Querry);
        }

        public DataSet GetPendingApplications(string EmployeeID)
        {
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,
a.TotalDays,Reasons.LeaveReason,ApplicationStatus.ApplicationStatus,a.ApplicationType 
from LeaveApplication a inner join
( select  LeaveApplication.LeaveApplicationID,Max(StatusHistory.ApplicationStatusID) as StatusId 
from StatusHistory inner join LeaveApplication on 
LeaveApplication.LeaveApplicationID=StatusHistory.LeaveApplicationID inner join 
ApplicationStatus on StatusHistory.ApplicationStatusID=ApplicationStatus.ApplicationStatusID 
where LeaveApplication.EmployeeID={0}
group by LeaveApplication.LeaveApplicationID)bc on
a.LeaveApplicationID = bc.LeaveApplicationID inner join 
LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID inner join 
Reasons on a.ReasonID = Reasons.ReasonID
inner join ApplicationStatus on 
bc.StatusId = ApplicationStatus.ApplicationStatusID and ApplicationStatus.ApplicationStatusID=1 where a.EmployeeID = {0}
order by a.ApplyDate desc", EmployeeID);

            return database.Read(Querry);
        }
        public DataSet GetApprovedApplications(string EmployeeID)
        {
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,
a.TotalDays,Reasons.LeaveReason,ApplicationStatus.ApplicationStatus,a.ApplicationType 
from LeaveApplication a inner join
( select  LeaveApplication.LeaveApplicationID,Max(StatusHistory.ApplicationStatusID) as StatusId 
from StatusHistory inner join LeaveApplication on 
LeaveApplication.LeaveApplicationID=StatusHistory.LeaveApplicationID inner join 
ApplicationStatus on StatusHistory.ApplicationStatusID=ApplicationStatus.ApplicationStatusID 
where LeaveApplication.EmployeeID={0}
group by LeaveApplication.LeaveApplicationID)bc on
a.LeaveApplicationID = bc.LeaveApplicationID inner join 
LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID inner join 
Reasons on a.ReasonID = Reasons.ReasonID
inner join ApplicationStatus on 
bc.StatusId = ApplicationStatus.ApplicationStatusID and ApplicationStatus.ApplicationStatusID=2 where a.EmployeeID = {0}
order by a.ApplyDate desc", EmployeeID);
            return database.Read(Querry);
        }
        public DataSet GetRejectedApplications(string EmployeeID)
        {
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,
a.TotalDays,Reasons.LeaveReason,ApplicationStatus.ApplicationStatus,a.ApplicationType 
from LeaveApplication a inner join
( select  LeaveApplication.LeaveApplicationID,Max(StatusHistory.ApplicationStatusID) as StatusId 
from StatusHistory inner join LeaveApplication on 
LeaveApplication.LeaveApplicationID=StatusHistory.LeaveApplicationID inner join 
ApplicationStatus on StatusHistory.ApplicationStatusID=ApplicationStatus.ApplicationStatusID 
where LeaveApplication.EmployeeID={0}
group by LeaveApplication.LeaveApplicationID)bc on
a.LeaveApplicationID = bc.LeaveApplicationID inner join 
LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID inner join 
Reasons on a.ReasonID = Reasons.ReasonID
inner join ApplicationStatus on 
bc.StatusId = ApplicationStatus.ApplicationStatusID and ApplicationStatus.ApplicationStatusID=3 where a.EmployeeID = {0}
order by a.ApplyDate desc", EmployeeID);
            return database.Read(Querry);
        }
        public LeaveApplication GetApplication(string Application_Id)
        {
            //return application for edit application feature...

            string Querry = string.Format(@"select LeaveApplication.LeaveApplicationID,LeaveApplication.EmployeeID,LeaveType.LeaveTypeID,
                LeaveApplication.ApplyDate,LeaveApplication.FromDate,LeaveApplication.ToDate,LeaveApplication.TotalDays,
LeaveApplication.Remarks,LeaveApplication.ReasonID,Attachments.AttachmentId,Attachments.FileName,LeaveApplication.ApplicationType from 
LeaveApplication inner join LeaveType on LeaveApplication.LeaveTypeID=LeaveType.LeaveTypeID 
left join Attachments on LeaveApplication.LeaveApplicationID=Attachments.LeaveApplicationId 
where LeaveApplication.LeaveApplicationID='{0}'", Application_Id);

            DataSet ds = database.Read(Querry);


            LeaveApplication v1 = new Models.LeaveApplication();
            v1.ApplicationId = ds.Tables[0].Rows[0][0].ToString();
            v1.EmployeeID = ds.Tables[0].Rows[0][1].ToString();
            v1.LeaveTypeID = ds.Tables[0].Rows[0][2].ToString();
            v1.LeaveType = ds.Tables[0].Rows[0][2].ToString();
            v1.ApplyDate = DateTimeHelper.dd_MM_yyyy(ds.Tables[0].Rows[0][3].ToString());

            v1.TotalDays = double.Parse(ds.Tables[0].Rows[0][6].ToString());
            if (v1.TotalDays == 0.5)
            {
                v1.FromDate = DateTimeHelper.dd_MM_yyyy_HH_mm_tt(ds.Tables[0].Rows[0][4].ToString());
                v1.FromTime = DateTimeHelper.ToTime(ds.Tables[0].Rows[0][4].ToString());
                v1.ToTime = DateTimeHelper.ToTime(ds.Tables[0].Rows[0][5].ToString());
            }
            else
            {
                v1.FromDate = DateTimeHelper.dd_MM_yyyy(ds.Tables[0].Rows[0][4].ToString());
                v1.ToDate = DateTimeHelper.dd_MM_yyyy(ds.Tables[0].Rows[0][5].ToString());
            }
            v1.LeaveRemarks = ds.Tables[0].Rows[0][7].ToString();
            v1.LeaveReason = ds.Tables[0].Rows[0][8].ToString();

            v1.ApplicationStatus = GetApplicationStatus(v1.ApplicationId);
            if (ds.Tables[0].Rows[0][9] != DBNull.Value)
            {

                v1.FileId = Encryption.Base64Encode(Convert.ToString(ds.Tables[0].Rows[0][9]));
                v1.FileName = ds.Tables[0].Rows[0][10].ToString();
            }
            v1.ApplicationType = Convert.ToBoolean(ds.Tables[0].Rows[0][11]);

            if (v1.ApplicationStatus == "Pending")
            {

                return v1;
            }
            else
                return null;

        }
        public LeaveApplication GetViewApplication(string Application_Id)
        {


            string Querry = string.Format(@"select LeaveApplication.LeaveApplicationID,LeaveApplication.EmployeeID,LeaveType.LeaveType,LeaveApplication.ApplyDate,
LeaveApplication.FromDate, LeaveApplication.ToDate, LeaveApplication.TotalDays, LeaveApplication.Remarks,
Reasons.LeaveReason, Attachments.FileId, Attachments.FileName,LeaveApplication.ManagerRemarks from LeaveApplication inner join LeaveType on LeaveApplication.LeaveTypeID = LeaveType.LeaveTypeID
  inner join Reasons on LeaveApplication.ReasonID = Reasons.ReasonID  left join Attachments on LeaveApplication.LeaveApplicationID = Attachments.LeaveApplicationId
where LeaveApplication.LeaveApplicationID = '{0}'", Application_Id);

            DataSet ds = database.Read(Querry);

            LeaveApplication v1 = new Models.LeaveApplication();
            v1.ApplicationId = ds.Tables[0].Rows[0][0].ToString();
            v1.EmployeeID = ds.Tables[0].Rows[0][1].ToString();
            v1.LeaveType = ds.Tables[0].Rows[0][2].ToString();
            v1.ApplyDate = DateTime.Parse(ds.Tables[0].Rows[0][3].ToString()).ToString();
            v1.FromDate = DateTime.Parse(ds.Tables[0].Rows[0][4].ToString()).ToString("dd/MM/yyyy h:mm tt");
            v1.ToDate = DateTime.Parse(ds.Tables[0].Rows[0][5].ToString()).ToString("dd/MM/yyyy h:mm tt");
            v1.TotalDays = double.Parse(ds.Tables[0].Rows[0][6].ToString());
            v1.LeaveRemarks = ds.Tables[0].Rows[0][7].ToString();
            v1.LeaveReason = ds.Tables[0].Rows[0][8].ToString();
            v1.ApplicationStatus = GetApplicationStatus(v1.ApplicationId);
            if (ds.Tables[0].Rows[0][9] != DBNull.Value)
            {
                v1.FileId = Encryption.Base64Encode(Convert.ToString(ds.Tables[0].Rows[0][9]));
                v1.FileName = ds.Tables[0].Rows[0][10].ToString();
            }
            v1.ManagerRemarks = ds.Tables[0].Rows[0][11].ToString();


            return v1;
        }
        public void SaveChanges(LeaveApplication l1, bool IsDeletedFile)
        {//half day functionality should be added ....
            string Querry = string.Empty;
            if (l1.TotalDays == 0.5)
            {
                l1.TotalDays = 0.5;
                l1.FromDate = DateTimeHelper.ToDateTime(l1.FromDate);
                l1.ToDate = DateTimeHelper.ToDateTime(l1.ToDate);
                l1.ApplicationId = l1.ApplicationId;
            }
            else
            {
                l1.TotalDays = CalculateTotalLeaveDays(l1);
                l1.ApplicationId = l1.ApplicationId;
                l1.ToDate = DateTime.ParseExact(l1.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
                l1.FromDate = DateTime.ParseExact(l1.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
            }
            if (IsDeletedFile == false && l1.Attachment == null)
            {
                Querry = string.Format(@"update LeaveApplication set LeaveTypeID='{0}',FromDate='{1}',ToDate='{2}',TotalDays='{3}',Remarks='{4}',ReasonID='{5}' where LeaveApplicationID='{6}'", l1.LeaveType, l1.FromDate, l1.ToDate, l1.TotalDays, l1.LeaveRemarks, l1.LeaveReason, l1.ApplicationId);
                database.ExecuteQuerry(Querry);

            }
            else if (l1.Attachment != null)
            {
                Querry = string.Format(@"update LeaveApplication set LeaveTypeID='{0}',FromDate='{1}',ToDate='{2}',TotalDays='{3}',Remarks='{4}',ReasonID='{5}' where LeaveApplicationID='{6}'", l1.LeaveType, l1.FromDate, l1.ToDate, l1.TotalDays, l1.LeaveRemarks, l1.LeaveReason, l1.ApplicationId);
                database.ExecuteQuerry(Querry);
                if (string.IsNullOrEmpty(l1.FileId))
                    InsertAttachement(l1.Attachment, Convert.ToInt32(l1.ApplicationId));

                else
                    UpdateAttachment(l1.Attachment, int.Parse(Encryption.Base64Decode(l1.FileId)));

            }
            else if (IsDeletedFile == true)
            {
                Querry = string.Format(@"update LeaveApplication set LeaveTypeID='{0}',FromDate='{1}',ToDate='{2}',TotalDays='{3}',Remarks='{4}',ReasonID='{5}' where LeaveApplicationID='{6}'", l1.LeaveType, l1.FromDate, l1.ToDate, l1.TotalDays, l1.LeaveRemarks, l1.LeaveReason, l1.ApplicationId, int.Parse(Encryption.Base64Decode(l1.FileId)));
                database.ExecuteQuerry(Querry);
                DeleteAttachement(int.Parse(Encryption.Base64Decode(l1.FileId)));

            }

        }
        public void UpdateAttachment(HttpPostedFileBase Attachment, int FileId)
        {
            String Querry = string.Format("update Attachments set FileName='{1}' where FileId='{0}' update Files set Content=@File where FileId={0}", FileId, Attachment.FileName);
            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "File";
            p1.Value = GetFileBytes(Attachment);
            database.ExecuteQuerry(Querry, p1);

        }
        public void DeleteAttachement(int FileId)
        {
            String Querry = string.Format("delete from Attachments where FileId = '{0}' delete from Files where FileId='{0}' ", FileId);
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
                Querry = string.Format("Select ApplicationStatus from ApplicationStatus where ApplicationStatusID ={0}", st);
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
        public void CancelApplication(int ApplicationId, int EmployeeId)
        {


            string Querry = string.Format(@"delete from LeaveApplication where LeaveApplication.LeaveApplicationID={0} and LeaveApplication.EmployeeID={1}
and(select MAX(a.ApplicationStatusID) from StatusHistory a where LeaveApplicationID = {0}) = 1", ApplicationId, EmployeeId);
            database.ExecuteQuerry(Querry);


        }
        public DataSet GetFacultyAll(int EmployeeId)
        {
            //if need manager name apply group by on manager as well...
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,a.TotalDays,Reasons.LeaveReason,
ApplicationStatus.ApplicationStatus, a.ApplicationType, bc.EmployeeName from LeaveApplication a  inner
                                                                        join
( select LeaveApplication.LeaveApplicationID, MAX(StatusHistory.ApplicationStatusID) as statusId, Employee.EmployeeName
from StatusHistory inner join LeaveApplication on LeaveApplication.LeaveApplicationID = StatusHistory.LeaveApplicationID
inner
join ApplicationStatus on StatusHistory.ApplicationStatusID = ApplicationStatus.ApplicationStatusID
inner
join Employee on Employee.EmployeeID = LeaveApplication.EmployeeID and Employee.Manager = {0}
group by LeaveApplication.LeaveApplicationID, Employee.EmployeeName)bc
on a.LeaveApplicationID = bc.LeaveApplicationID inner join LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID
inner join Reasons on a.ReasonID = Reasons.ReasonID  inner join ApplicationStatus on bc.statusId = ApplicationStatus.ApplicationStatusID order by a.ApplyDate desc", EmployeeId);

            return database.Read(Querry);

        }
        public DataSet GetFacultyPending(int EmployeeId)
        {
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,a.TotalDays,Reasons.LeaveReason,
ApplicationStatus.ApplicationStatus, a.ApplicationType, bc.EmployeeName from LeaveApplication a  inner join
( select LeaveApplication.LeaveApplicationID, MAX(StatusHistory.ApplicationStatusID) as statusId, Employee.EmployeeName
from StatusHistory inner join LeaveApplication on LeaveApplication.LeaveApplicationID = StatusHistory.LeaveApplicationID
inner
join ApplicationStatus on StatusHistory.ApplicationStatusID = ApplicationStatus.ApplicationStatusID
inner
join Employee on Employee.EmployeeID = LeaveApplication.EmployeeID and Employee.Manager = {0}
group by LeaveApplication.LeaveApplicationID, Employee.EmployeeName)bc
  on a.LeaveApplicationID = bc.LeaveApplicationID inner join LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID
inner join Reasons on a.ReasonID = Reasons.ReasonID  inner join ApplicationStatus on bc.statusId = ApplicationStatus.ApplicationStatusID and ApplicationStatus.ApplicationStatusID = 1
 order by a.ApplyDate desc", EmployeeId);
            return database.Read(Querry);
        }
        public DataSet GetFacultyApproved(int EmployeeId)
        {
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,a.TotalDays,Reasons.LeaveReason,
ApplicationStatus.ApplicationStatus, a.ApplicationType, bc.EmployeeName from LeaveApplication a  inner
                                                                        join
( select LeaveApplication.LeaveApplicationID, MAX(StatusHistory.ApplicationStatusID) as statusId, Employee.EmployeeName
from StatusHistory inner join LeaveApplication on LeaveApplication.LeaveApplicationID = StatusHistory.LeaveApplicationID
inner
join ApplicationStatus on StatusHistory.ApplicationStatusID = ApplicationStatus.ApplicationStatusID
inner
join Employee on Employee.EmployeeID = LeaveApplication.EmployeeID and Employee.Manager = {0}
group by LeaveApplication.LeaveApplicationID,Employee.EmployeeName)bc
  on a.LeaveApplicationID = bc.LeaveApplicationID inner join LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID
inner join Reasons on a.ReasonID = Reasons.ReasonID  inner join ApplicationStatus on bc.statusId = ApplicationStatus.ApplicationStatusID and ApplicationStatus.ApplicationStatusID = 2
 order by a.ApplyDate desc
", EmployeeId);
            return database.Read(Querry);
        }
        public DataSet GetFacultyReject(int EmployeeId)
        {
            string Querry = string.Format(@"select a.LeaveApplicationID,LeaveType.LeaveType,a.ApplyDate,a.FromDate,a.ToDate,a.TotalDays,Reasons.LeaveReason,
ApplicationStatus.ApplicationStatus, a.ApplicationType, bc.EmployeeName from LeaveApplication a  inner
                                                                        join
( select LeaveApplication.LeaveApplicationID, MAX(StatusHistory.ApplicationStatusID) as statusId, Employee.EmployeeName
from StatusHistory inner join LeaveApplication on LeaveApplication.LeaveApplicationID = StatusHistory.LeaveApplicationID
inner
join ApplicationStatus on StatusHistory.ApplicationStatusID = ApplicationStatus.ApplicationStatusID
inner
join Employee on Employee.EmployeeID = LeaveApplication.EmployeeID and Employee.Manager = {0}
group by LeaveApplication.LeaveApplicationID, Employee.EmployeeName)bc
  on a.LeaveApplicationID = bc.LeaveApplicationID inner join LeaveType on a.LeaveTypeID = LeaveType.LeaveTypeID
inner join Reasons on a.ReasonID = Reasons.ReasonID  inner join ApplicationStatus on bc.statusId = ApplicationStatus.ApplicationStatusID and ApplicationStatus.ApplicationStatusID = 3
 order by a.ApplyDate desc", EmployeeId);
            return database.Read(Querry);
        }
        public void AcceptApplication(string ApplicationID, string ManagerRemarks)
        {

            string Querry = string.Format(@"select LeaveApplication.EmployeeID, LeaveApplication.LeaveTypeID,LeaveApplication.ApplicationType,LeaveApplication.TotalDays,EmployeeLeaveCount.Count 
as CurrentBalance from LeaveApplication  left join EmployeeLeaveCount on LeaveApplication.EmployeeID=EmployeeLeaveCount.EmployeeID and LeaveApplication.LeaveTypeID=
EmployeeLeaveCount.LeaveTypeID
where LeaveApplication.LeaveApplicationID='{0}'", ApplicationID);
            DataSet ds = database.Read(Querry);
            EmployeeLeaveCount e1 = new EmployeeLeaveCount();
            e1.EmployeeID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            e1.LeaveTypeID = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0][4].ToString()))
            {
                e1.Count = Convert.ToDouble(ds.Tables[0].Rows[0][4]);
            }

            LeaveApplication l1 = new LeaveApplication();
            l1.ApplicationType = Convert.ToBoolean(ds.Tables[0].Rows[0][2]);
            l1.TotalDays = Convert.ToDouble(ds.Tables[0].Rows[0][3]);


            if (l1.ApplicationType == false)
            {

                e1.Count -= l1.TotalDays;



            }
            else if (l1.ApplicationType == true)
            {
                e1.Count += l1.TotalDays;

            }
            Querry = string.Format("insert into StatusHistory(LeaveApplicationID,Date,ApplicationStatusID) select '{0}','{1}',ApplicationStatus.ApplicationStatusID from ApplicationStatus where ApplicationStatus.ApplicationStatus='Approved' ", ApplicationID, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss"));
            database.ExecuteQuerry(Querry);
            UpdateManagerRemarks(ApplicationID, ManagerRemarks);
            Querry = string.Format("update  EmployeeLeaveCount set Count = '{0}' where EmployeeID = '{1}'and LeaveTypeID ='{2}'  " +
                "if @@ROWCOUNT = 0 " +
                "insert into EmployeeLeaveCount(Count,EmployeeID,LeaveTypeID) values('{0}', '{1}', '{2}')", e1.Count, e1.EmployeeID, e1.LeaveTypeID);
            database.ExecuteQuerry(Querry);





        }
        public void RejectApplication(string ApplicationID, string ManagerRemarks)
        {

            string Querry = string.Format("insert into StatusHistory(LeaveApplicationID,Date,ApplicationStatusID) select '{0}','{1}',ApplicationStatus.ApplicationStatusID from ApplicationStatus where ApplicationStatus.ApplicationStatus='Rejected' ", ApplicationID, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss"));
            database.ExecuteQuerry(Querry);
            UpdateManagerRemarks(ApplicationID, ManagerRemarks);
        }
        public void UpdateManagerRemarks(string ApplicationID, string ManagerRemarks)
        {//update remarks on apprve or reject leave...
            string Querry = string.Format("update LeaveApplication set ManagerRemarks='{0}' where LeaveApplicationID='{1}'", ManagerRemarks, ApplicationID);
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
        public DataSet GetReasons()
        {
            string Querry = string.Format("select * from Reasons");
            return database.Read(Querry);
        }
        public DataSet GetLeaveCount(int EmployeeId)
        {

            string Querry = string.Format(@"declare @a NVARCHAR(MAX) = '',@b NVARCHAR(MAX) = '';
declare @id NVARCHAR(MAX)='{0}',@st int=2;
select @a+=QUOTENAME(LeaveType.LeaveType) + ',' from LeaveType

SET @a = LEFT(@a, LEN(@a) - 1);
print @st
 SET @b='
select * from
 (
select LeaveApplication.TotalDays,LeaveType.LeaveType from LeaveApplication inner join StatusHistory on StatusHistory.LeaveApplicationID=LeaveApplication.LeaveApplicationID
inner join LeaveType on LeaveApplication.LeaveTypeID=LeaveType.LeaveTypeID
where LeaveApplication.EmployeeID=@id and LeaveApplication.ApplicationType=0 and StatusHistory.ApplicationStatusID=@st
 
 )t
 pivot(
sum(TotalDays) for LeaveType in('+@a+')
)as ax';
EXECUTE sp_executesql @b, N'@id NVARCHAR(MAX),@st NVARCHAR(MAX)', @id = @id,@st=@st;", EmployeeId);

            return database.Read(Querry);
        }
        public DataSet FacultyLeaveCount(int EmployeeId)
        {
            string Querry = string.Format(@"declare @a NVARCHAR(MAX) = '',@b NVARCHAR(MAX) = '';
declare @id NVARCHAR(MAX)={0},@st int='2';
select @a+=QUOTENAME(LeaveType.LeaveType) + ',' from LeaveType

SET @a = LEFT(@a, LEN(@a) - 1);
print @st
SET @b='
select * from
(
select x.EmployeeID,x.EmployeeName,ax.LeaveType,ax.TotalDays from Employee x left join (
select Employee.EmployeeID,Employee.EmployeeName,StatusHistory.ApplicationStatusID,LeaveApplication.TotalDays,LeaveType.LeaveType from Employee inner join LeaveApplication on Employee.EmployeeID=LeaveApplication.EmployeeID inner join StatusHistory on StatusHistory.LeaveApplicationID=LeaveApplication.LeaveApplicationID and StatusHistory.ApplicationStatusID=@st inner join LeaveType on LeaveType.LeaveTypeID=LeaveApplication.LeaveTypeID where Employee.Manager=@id and LeaveApplication.ApplicationType=0
)ax on x.EmployeeID=ax.EmployeeID where x.Manager=@id
)t
 pivot(
sum(TotalDays) for LeaveType in('+@a+')
)as ax';
EXECUTE sp_executesql @b, N'@id NVARCHAR(MAX),@st NVARCHAR(MAX)', @id = @id,@st=@st;", EmployeeId);

            return database.Read(Querry);
        }
        public DataSet GetBalance(int FacultyId, int EmployeeId)
        {//for faculty
            string Querry = string.Format(@"select LeaveType.LeaveType,COALESCE( EmployeeLeaveCount.Count,0) from LeaveType left join EmployeeLeaveCount on EmployeeLeaveCount.EmployeeID={0} and LeaveType.LeaveTypeID=EmployeeLeaveCount.LeaveTypeID 
inner join Employee on Employee.EmployeeID = {0} where  Employee.Manager = {1}", FacultyId, EmployeeId);
            return database.Read(Querry);
        }
        public DataSet GetLeaveBalance(int EmployeeId)
        {//for user itself
            string Querry = string.Format(@"select LeaveType.LeaveType, COALESCE( EmployeeLeaveCount.Count,0) 
from EmployeeLeaveCount right join LeaveType on 
EmployeeLeaveCount.LeaveTypeID=LeaveType.LeaveTypeID and EmployeeLeaveCount.EmployeeID={0}", EmployeeId);
            return database.Read(Querry);
        }
        public bool IsPending(int ApplicationId)
        {//select LeaveApplicationID,MAX(ApplicationStatusID) as st from StatusHistory 
            //where StatusHistory.LeaveApplicationID ={ 0}
            //group by LeaveApplicationID
            string Querry = string.Format(@"select MAX(ApplicationStatusID) as st from StatusHistory 
where StatusHistory.LeaveApplicationID={0} group by LeaveApplicationID", ApplicationId);
            int status = Convert.ToInt32(database.ExecuteScalar(Querry));
            if (status == 1)
                return true;
            else
                return false;
        }
        public int FacultyPendingApplications_Count(int EmployeeId)
        {
            string Querry = string.Format(@"
select count(*)PendingCount from(select LeaveApplication.LeaveApplicationID,Employee.EmployeeID from Employee inner join LeaveApplication on LeaveApplication.EmployeeID=Employee.EmployeeID
 inner join StatusHistory on StatusHistory.LeaveApplicationID=LeaveApplication.LeaveApplicationID
 where Employee.Manager={0} group by LeaveApplication.LeaveApplicationID, Employee.EmployeeID
 having  MAX(StatusHistory.ApplicationStatusID)=1)as x", EmployeeId);
            return Convert.ToInt32(database.ExecuteScalar(Querry));
        }
        public string[,] ManagersPendings()
        {
            string Querry = string.Format(@"select Employee.EmployeeName as Manager,count(*)PendingCount from(select LeaveApplication.LeaveApplicationID
, Employee.Manager from Employee
 inner join LeaveApplication on LeaveApplication.EmployeeID = Employee.EmployeeID
 
  inner
                   join StatusHistory on StatusHistory.LeaveApplicationID = LeaveApplication.LeaveApplicationID
 where Employee.Manager is not null group by LeaveApplication.LeaveApplicationID, Employee.Manager
 having  MAX(StatusHistory.ApplicationStatusID) = 1) as x inner join Employee on x.Manager = Employee.EmployeeID group by x.Manager,Employee.EmployeeName
");
            System.Data.DataSet dset = database.Read(Querry);
            string[,] ds = new string[dset.Tables[0].Rows.Count, dset.Tables[0].Columns.Count];
            for (int i = 0; i < ds.GetLength(0); i++)
            {
                for (int j = 0; j < ds.GetLength(1); j++)
                {
                    ds[i, j] = dset.Tables[0].Rows[i][j].ToString();
                }
            }
            return ds;
        }
        /// <summary>
        /// Return total pending applications of particular user
        /// </summary>
        /// <param name="Empid"></param>
        /// <returns></returns>
        public int GetTotalPendings(int Empid)
        {
            string Querry = string.Format(@"select count(*)from(select LeaveApplication.LeaveApplicationID from LeaveApplication
inner join StatusHistory on StatusHistory.LeaveApplicationID = LeaveApplication.LeaveApplicationID
where EmployeeID = {0} group by LeaveApplication.LeaveApplicationID
  having MAX(StatusHistory.ApplicationStatusID) = 1)as t1", Empid);
            return Convert.ToInt32(database.ExecuteScalar(Querry));
        }

    }
}

