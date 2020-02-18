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
        public int FileId { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime AcheivementDate { get; set; }

        [Required]
        public HttpPostedFileBase File { get; set; }

        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }

        private db database = new db();
        private string Querry;

        public DataSet GetAcheivement(int EmployeeID)
        {
            Querry = string.Format("select Acheivement.id as 'AcheivementID',AcheivementAttachments.FileId as 'FileID',AcheivementAttachments.id as 'AcheivementAttachID' ,Title,Date,Description,FileName,Content from Acheivement inner join AcheivementAttachments on Acheivement.id=AcheivementId inner join Files on Files.FileId=AcheivementAttachments.FileId where EmployeeID={0}", EmployeeID);
            return database.Read(Querry);
        }

        public void UpdateAcheivement()
        {
            Querry = "";
        }

        public void InsertAcheivement(int EmployeeID, Acheivement ach)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "title", Value = ach.title });
            sqlParameters.Add(new SqlParameter() { ParameterName = "AcheivementDate", Value = ach.AcheivementDate });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Description", Value = ach.Description });
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });

            Querry = string.Format(@"DECLARE @achid AS INT, @fileid int
            insert into Acheivement(Title,Date,Description,EmployeeID) values(@title,@AcheivementDate,@Description,@EmployeeID);
            set @achid = SCOPE_IDENTITY();

            Insert into Files(Content) values(@content);
            set @fileid = SCOPE_IDENTITY();

            Insert into AcheivementAttachments(FileId,AcheivementId,FileName) values(@fileid,@achid,@FileName);
            ");
            sqlParameters.Add(new SqlParameter() { ParameterName = "FileName", Value = ach.FileName });
            Stream s1 = ach.File.InputStream;
            BinaryReader b1 = new BinaryReader(s1);
            FileBytes = b1.ReadBytes((int)s1.Length);
            sqlParameters.Add(new SqlParameter() { ParameterName = "content", Value = FileBytes });

            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void DeleteAcheivement()
        {
            Querry = "";
            database.ExecuteQuerry(Querry);
        }
    }
}