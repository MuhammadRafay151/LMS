using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
namespace LeaveApplication.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public int EmployeeId { get; set; }
        [Required]
        public string Organization { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public string Descipline { get; set; }
        [Required]
      
        public string Fromdate { get; set; }
        [Required]
        
        public string Todate { get; set; }
        db database = new db();
        string Querry = "";
        public DataSet GetExperiences()
        {
            Querry = string.Format("select * from experience where Employeeid='{0}'", EmployeeId);
            return database.Read(Querry);
        }
        public Experience GetExperience()
        {
            Querry = string.Format("select * from experience where Employeeid='{0}'",EmployeeId);
            DataSet x = database.Read(Querry);
            ExperienceId =Convert.ToInt32( x.Tables[0].Rows[0][0]);
            EmployeeId = Convert.ToInt32(x.Tables[0].Rows[0][1]);
            Organization = x.Tables[0].Rows[0][2].ToString();
            Designation = x.Tables[0].Rows[0][3].ToString();
            Descipline = x.Tables[0].Rows[0][4].ToString();
            Fromdate = x.Tables[0].Rows[0][5].ToString();
            Todate = x.Tables[0].Rows[0][6].ToString();
            return this;
        }
        public void UpdateExp()
        {
            //Querry = "insert into Experience(Employeeid,Organization,Designation,Descipline,Fromdate,Todate)values(@empid,@org,@dsg,@des,@fd,@td)";
            //SqlParameter s1 = new SqlParameter() { ParameterName = "empid", Value = EmployeeId };
            //SqlParameter s2 = new SqlParameter() { ParameterName = "org", Value = Organization };
            //SqlParameter s3 = new SqlParameter() { ParameterName = "dsg", Value = Designation };
            //SqlParameter s4 = new SqlParameter() { ParameterName = "des", Value = Descipline };
            //SqlParameter s5 = new SqlParameter() { ParameterName = "fd", Value = Fromdate };
            //SqlParameter s6 = new SqlParameter() { ParameterName = "td", Value = Todate };
            //database.ExecuteQuerry(Querry);
        }
        public void Insert()
        {
            Querry = "insert into Experience(Employeeid,Organization,Designation,Descipline,Fromdate,Todate)values(@empid,@org,@dsg,@des,@fd,@td)";
            List<SqlParameter> pm = new List<SqlParameter>();
            pm.Add( new SqlParameter() { ParameterName = "empid", Value = EmployeeId });
            pm.Add(new SqlParameter() { ParameterName = "org", Value = Organization });
            pm.Add(new SqlParameter() { ParameterName = "dsg", Value = Designation });
            pm.Add(new SqlParameter() { ParameterName = "des", Value = Descipline });
            pm.Add(new SqlParameter() { ParameterName = "fd", Value = Fromdate });
            pm.Add(new SqlParameter() { ParameterName = "td", Value =Todate});
            database.ExecuteQuerry(Querry,pm);

        }
        public void DeleteExp()
        {
            Querry = string.Format("delete from Experience where id={0} and Employeeid={1}",ExperienceId,EmployeeId);
            database.ExecuteQuerry(Querry);
        }
    }
}