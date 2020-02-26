using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;

namespace LeaveApplication.Models
{
    public class Publication
    {
        public string Querry { get; private set; }
        db database = new db();
        public int EmployeeId { get; set; }
        public int PublishedId { get; set; }
        public int FileId { get; set; }
        public int PublicationAtcchmentId { get; set; }
        public Byte[] FileBytes { get; set; }
        public string Title { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase File { get; set; }

        public List<string> Author { get; set; }


        public DataSet GetPublication()
        {
            Querry = @"select x.Title,x.PublishDate,x.Description,x.Authors,y.FileName,y.Fileid,x.id from Publications as x inner join PublicationAttachment as y on
x.id=y.PublicationId where x.Employeeid=" + EmployeeId;
            DataSet d1 = database.Read(Querry);
            return d1;
        }
        public void UpdatePublication()
        {
            Querry = "";
        }
        public void InsertPublication()
        {
            List<SqlParameter> parm = new List<SqlParameter>();
            LeaveBusinessLayer lb = new LeaveBusinessLayer();
            FileBytes = lb.GetFileBytes(File);
            Querry = @"DECLARE @pid AS INT, @fileid int
insert into Publications(Employeeid,Title,PublishDate,Description,Authors) values(@Employeeid,@Title,@PublishDate,@Description,@Authors)
set @pid = SCOPE_IDENTITY()

Insert into Files(Content) values(@Content)
set @fileid = SCOPE_IDENTITY()

Insert into PublicationAttachment(FileId,PublicationId,FileName) values(@fileid,@pid,@FileName)";
            HelperClasses.SqlParm sq = new HelperClasses.SqlParm();
            sq.Add("Employeeid", EmployeeId);
            sq.Add("Title", Title);
            sq.Add("PublishDate", DateTimeHelper.yyyy_mm_dd(PublishedDate));
            sq.Add("Description", Description);
            sq.Add("Authors", MakeAuthorsString());
            sq.Add("Content", FileBytes);
            sq.Add("FileName", File.FileName);
            database.ExecuteQuerry(Querry, sq.GetParmList());
        }
        public void DeletePublication()
        {
            Querry = @"DECLARE @fileid AS INT
            set @fileid =(select FileId from PublicationAttachment inner join Publications on Publications.id=PublicationAttachment.PublicationId where Publications.id=@PublicationId and Publications.Employeeid=@EmployeeID)
if(@fileid is not null)
begin
delete from Publications where id=@PublicationId and Employeeid=@EmployeeID;
delete  from Files where FileId=@fileid 
end 
";
            HelperClasses.SqlParm p1 = new HelperClasses.SqlParm();
            p1.Add("EmployeeID", EmployeeId);
            p1.Add("PublicationId", PublishedId);
            database.ExecuteQuerry(Querry, p1.GetParmList());
        }
        public string MakeAuthorsString()
        {
            string temp = "";
            Debug.WriteLine(Author.Count);
            for (int i = 0; i < Author.Count; i++)
            {
                if (i + 1 != Author.Count)
                {
                    temp += Author[i] + ",";
                }
                else
                {
                    temp += Author[i];
                }

            }
            return temp;
        }
        public List<string> SplitAuthors(string Authors)
        {

            return Authors.Split(',').ToList<string>();

        }
        public File GetFile(int Fileid, int EmployeeId,int PublicationId)
        {
            Querry = string.Format(@" select PublicationAttachment.FileName,Files.Content from PublicationAttachment 
   inner join Publications on PublicationAttachment.PublicationId=Publications.id
  inner join
  Files on PublicationAttachment.Fileid=Files.FileId where Files.FileId={0} and Publications.Employeeid={1} and Publications.id={2}", Fileid,EmployeeId,PublicationId);
            DataSet d1 = database.Read(Querry);
            File f1 = new File();
            f1.FileName = d1.Tables[0].Rows[0][0].ToString();
            f1.Content = (Byte[])d1.Tables[0].Rows[0][1];
            return f1;
        }

    }
}
