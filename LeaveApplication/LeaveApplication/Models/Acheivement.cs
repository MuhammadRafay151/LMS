using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace LeaveApplication.Models
{
    public class Acheivement
    {
        public int AcheivementId { get; set; }
        public int AcheivementAttachmentsID { get; set; }
        public int EmployeeID { get; set; }
        public int FileId { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string AcheivementDate { get; set; }

        //[Required]
        public HttpPostedFileBase File { get; set; }

        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }

        private db database = new db();
        private string Querry;

        public DataSet GetAcheivement()
        {
            Querry = string.Format("select Acheivement.id as 'AcheivementID',AcheivementAttachments.FileId as 'FileID',AcheivementAttachments.id as 'AcheivementAttachID' ,Title,Date,Description,FileName,Content from Acheivement inner join AcheivementAttachments on Acheivement.id=AcheivementId inner join Files on Files.FileId=AcheivementAttachments.FileId where EmployeeID={0}", EmployeeID);
            return database.Read(Querry);
        }

        public DataSet GetJsonAcheivement()
        {
            Querry = string.Format("select Acheivement.id as 'AcheivementID',AcheivementAttachments.FileId as 'FileID',AcheivementAttachments.id as 'AcheivementAttachID' ,Title,Date,Description,FileName,Content from Acheivement inner join AcheivementAttachments on Acheivement.id=AcheivementId inner join Files on Files.FileId=AcheivementAttachments.FileId where EmployeeID={0} and Acheivement.id={1}", EmployeeID, AcheivementId);
            return database.Read(Querry);
        }

        public void UpdateAcheivement()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "AcheivementId", Value = AcheivementId });
            sqlParameters.Add(new SqlParameter() { ParameterName = "title", Value = title });
            sqlParameters.Add(new SqlParameter() { ParameterName = "AcheivementDate", Value = AcheivementDate });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Description", Value = Description });
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            if (File == null)
            {
                Querry = string.Format(@"UPDATE Acheivement SET Title = @title, Date = @AcheivementDate,Description = @Description WHERE EmployeeID=@EmployeeID and id=@AcheivementId");
            }
            else
            {
                Querry = string.Format(@"Declare @FID int ,@AchAttchID int
                select @FID= Files.FileId,@AchAttchID=AcheivementAttachments.id from Acheivement inner join AcheivementAttachments on Acheivement.id=AcheivementAttachments.AcheivementId inner join Files on Files.FileId=AcheivementAttachments.FileId where Acheivement.id=@AcheivementId and EmployeeID=@EmployeeID;
                UPDATE Acheivement SET Title = @title, Date = @AcheivementDate,Description = @Description WHERE EmployeeID=@EmployeeID and id=@AcheivementId;
                update AcheivementAttachments set FileName=@FileName where id=@AchAttchID;
                update Files set Content=@content where FileId=@FID;
                ");

                sqlParameters.Add(new SqlParameter() { ParameterName = "FileName", Value = FileName });
                Stream s1 = File.InputStream;
                BinaryReader b1 = new BinaryReader(s1);
                FileBytes = b1.ReadBytes((int)s1.Length);
                sqlParameters.Add(new SqlParameter() { ParameterName = "content", Value = FileBytes });
            }

            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void InsertAcheivement()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "title", Value = title });
            sqlParameters.Add(new SqlParameter() { ParameterName = "AcheivementDate", Value = AcheivementDate });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Description", Value = Description });
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });

            Querry = string.Format(@"DECLARE @achid AS INT, @fileid int
            insert into Acheivement(Title,Date,Description,EmployeeID) values(@title,@AcheivementDate,@Description,@EmployeeID);
            set @achid = SCOPE_IDENTITY();

            Insert into Files(Content) values(@content);
            set @fileid = SCOPE_IDENTITY();

            Insert into AcheivementAttachments(FileId,AcheivementId,FileName) values(@fileid,@achid,@FileName);
            ");
            sqlParameters.Add(new SqlParameter() { ParameterName = "FileName", Value = FileName });
            Stream s1 = File.InputStream;
            BinaryReader b1 = new BinaryReader(s1);
            FileBytes = b1.ReadBytes((int)s1.Length);
            sqlParameters.Add(new SqlParameter() { ParameterName = "content", Value = FileBytes });

            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void DeleteAcheivement()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "AcheivementId", Value = AcheivementId });
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            Querry = string.Format(@"DECLARE @fileid AS INT
            set @fileid =(select FileId from AcheivementAttachments inner join Acheivement on Acheivement.id=AcheivementAttachments.AcheivementId where AcheivementId=@AcheivementId and EmployeeID=@EmployeeID)
            delete Acheivement where id=@AcheivementId and EmployeeID=@EmployeeID
            delete Files where FileId=@fileid
            ");
            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public DataSet DownloadFile()
        {
            string Querry = string.Format("select Files.Content,FileName from Acheivement inner join AcheivementAttachments on Acheivement.id=AcheivementAttachments.AcheivementId inner join Files on Files.FileId=AcheivementAttachments.FileId where EmployeeID={0} and Files.FileId={1}", EmployeeID, FileId);
            return database.Read(Querry);
        }
    }
}