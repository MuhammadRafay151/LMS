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
        string Querry;
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


        public DataSet GetPublications()
        {
            Querry = @"select x.Title,x.PublishDate,x.Description,x.Authors,y.FileName,y.Fileid,x.id from Publications as x inner join PublicationAttachment as y on
x.id=y.PublicationId where x.Employeeid=" + EmployeeId;
            DataSet d1 = database.Read(Querry);
            return d1;
        }
        public DataSet GetPublication()
        {
            Querry = string.Format(@"select x.Title,x.PublishDate,x.Description,x.Authors,y.FileName,y.Fileid,x.id from Publications as x inner join PublicationAttachment as y on
x.id=y.PublicationId where x.Employeeid={0} and x.id={1}", EmployeeId, PublishedId);
            return database.Read(Querry);
        }
        public void UpdatePublication()
        {
            Querry = "";
            HelperClasses.SqlParm s1 = new HelperClasses.SqlParm();
            s1.Add("Title", Title);
            s1.Add("pd", DateTimeHelper.yyyy_mm_dd(PublishedDate));
            s1.Add("desc", Description);
            s1.Add("Author", MakeAuthorsString());
            s1.Add("pid", PublishedId);
            s1.Add("eid", EmployeeId);
            if (File == null)
            {
                Querry = "update Publications set Authors=@Author,Description=@desc,PublishDate=@pd,Title=@Title where id=@pid and Employeeid=@eid";
            }
            else
            {
                LeaveBusinessLayer lb = new LeaveBusinessLayer();
                FileBytes = lb.GetFileBytes(File);
                s1.Add("file", FileBytes);
                s1.Add("fn", File.FileName);
                Querry = @"update Publications set Authors=@Author,Description=@desc,PublishDate=@pd,Title=@Title where id=@pid and Employeeid=@eid
declare @fileid as int
select @fileid=Fileid from PublicationAttachment inner join Publications 
on
Publications.id=PublicationAttachment.PublicationId
where PublicationId=@pid and Employeeid=@eid
if(@fileid is not null)
begin
update Files set Content=@file where FileId=@fileid
update PublicationAttachment set FileName=@fn where Fileid=@fileid and PublicationId=@pid
end";
            }
            database.ExecuteQuerry(Querry, s1.GetParmList());
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
        public File GetFile(int Fileid, int EmployeeId, int PublicationId)
        {
            Querry = string.Format(@" select PublicationAttachment.FileName,Files.Content from PublicationAttachment 
   inner join Publications on PublicationAttachment.PublicationId=Publications.id
  inner join
  Files on PublicationAttachment.Fileid=Files.FileId where Files.FileId={0} and Publications.Employeeid={1} and Publications.id={2}", Fileid, EmployeeId, PublicationId);
            DataSet d1 = database.Read(Querry);
            File f1 = new File();
            f1.FileName = d1.Tables[0].Rows[0][0].ToString();
            f1.Content = (Byte[])d1.Tables[0].Rows[0][1];
            return f1;
        }
        public File GetFile(int Fileid,  int PublicationId)
        {
            Querry = string.Format(@" select PublicationAttachment.FileName,Files.Content from PublicationAttachment 
  inner join
  Files on PublicationAttachment.Fileid=Files.FileId where Files.FileId={0}  and PublicationAttachment.PublicationId={1}", Fileid,  PublicationId);
            DataSet d1 = database.Read(Querry);
            File f1 = new File();
            f1.FileName = d1.Tables[0].Rows[0][0].ToString();
            f1.Content = (Byte[])d1.Tables[0].Rows[0][1];
            return f1;
        }
        public DataSet GenrateReport(int deptid)
        {
            Querry = "";
            if(deptid==0)
            {
                Querry = @"select Employee.EmployeeName,Publications.Title,Publications.PublishDate,Publications.Authors
,PublicationAttachment.FileName,PublicationAttachment.Fileid,Publications.id from Employee inner join Publications 
on
Employee.EmployeeID=Publications.Employeeid
inner join PublicationAttachment on Publications.id=PublicationAttachment.PublicationId
order by Employee.EmployeeID";
            }else
            {
                Querry = string.Format(@"select Employee.EmployeeName,Publications.Title,Publications.PublishDate,Publications.Authors
,PublicationAttachment.FileName,PublicationAttachment.Fileid,Publications.id from Employee inner join Publications 
on
Employee.EmployeeID=Publications.Employeeid
inner join PublicationAttachment on Publications.id=PublicationAttachment.PublicationId
where DepartmentID={0} order by Employee.EmployeeID", deptid);
            }
            //switch (filterid)
            //{
            //    case 1:
                   
            //        break;
            //    case 2:
            //        Querry = "";
            //        break;
            //}

            
            return database.Read(Querry);
        }

    }
}
