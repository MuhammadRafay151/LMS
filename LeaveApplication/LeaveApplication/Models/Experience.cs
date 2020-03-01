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

        private db database = new db();
        private string Querry = "";

        public DataSet GetExperiences()
        {
            Querry = string.Format("select * from experience where Employeeid='{0}'", EmployeeId);
            return database.Read(Querry);
        }

        public DataSet GetExperiencesReport()
        {
            Querry = string.Format("select EmployeeName,Department,Designations.Designation,JoiningDate,Sum(DATEDIFF(YEAR,Fromdate,Todate)) as 'years' from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join Designations on Employee.DesignationID=Designations.DesignationID inner join Experience on Employee.EmployeeID=Experience.Employeeid group by EmployeeName,Department,Designations.Designation,JoiningDate");
            return database.Read(Querry);
        }

        public DataSet GetDepExperiencesReport(int DepID)
        {
            Querry = string.Format("select EmployeeName,Department,Designations.Designation,JoiningDate,Sum(DATEDIFF(YEAR,Fromdate,Todate)) as 'years' from Employee inner join Departments on Employee.DepartmentID=Departments.DepartmentID inner join Designations on Employee.DesignationID=Designations.DesignationID inner join Experience on Employee.EmployeeID=Experience.Employeeid where Departments.DepartmentID={0} group by EmployeeName,Department,Designations.Designation,JoiningDate", DepID);
            return database.Read(Querry);
        }

        public Experience GetExperience()
        {
            Querry = string.Format("select * from experience where Employeeid='{0}' and id='{1}'", EmployeeId, ExperienceId);
            DataSet x = database.Read(Querry);
            ExperienceId = Convert.ToInt32(x.Tables[0].Rows[0][0]);
            EmployeeId = Convert.ToInt32(x.Tables[0].Rows[0][1]);
            Organization = x.Tables[0].Rows[0][2].ToString();
            Designation = x.Tables[0].Rows[0][3].ToString();
            Descipline = x.Tables[0].Rows[0][4].ToString();
            Fromdate = DateTime.Parse(x.Tables[0].Rows[0][5].ToString()).ToString("dd/MM/yyyy");
            Todate = DateTime.Parse(x.Tables[0].Rows[0][6].ToString()).ToString("dd/MM/yyyy");
            return this;
        }

        public void UpdateExp()
        {
            Querry = string.Format(@"update Experience set Organization=@org,Designation=@dsg
                     ,Descipline = @des
                     ,Fromdate = @fd
                     ,Todate = @td where Employeeid={0} and id={1}", EmployeeId, ExperienceId);
            List<SqlParameter> pm = new List<SqlParameter>();
            pm.Add(new SqlParameter() { ParameterName = "org", Value = Organization });
            pm.Add(new SqlParameter() { ParameterName = "dsg", Value = Designation });
            pm.Add(new SqlParameter() { ParameterName = "des", Value = Descipline });
            pm.Add(new SqlParameter() { ParameterName = "fd", Value = Fromdate });
            pm.Add(new SqlParameter() { ParameterName = "td", Value = Todate });
            database.ExecuteQuerry(Querry, pm);
        }

        public void Insert()
        {
            Querry = "insert into Experience(Employeeid,Organization,Designation,Descipline,Fromdate,Todate)values(@empid,@org,@dsg,@des,@fd,@td)";
            List<SqlParameter> pm = new List<SqlParameter>();
            pm.Add(new SqlParameter() { ParameterName = "empid", Value = EmployeeId });
            pm.Add(new SqlParameter() { ParameterName = "org", Value = Organization });
            pm.Add(new SqlParameter() { ParameterName = "dsg", Value = Designation });
            pm.Add(new SqlParameter() { ParameterName = "des", Value = Descipline });
            pm.Add(new SqlParameter() { ParameterName = "fd", Value = Fromdate });
            pm.Add(new SqlParameter() { ParameterName = "td", Value = Todate });
            database.ExecuteQuerry(Querry, pm);
        }

        public void DeleteExp()
        {
            Querry = string.Format("delete from Experience where id={0} and Employeeid={1}", ExperienceId, EmployeeId);
            database.ExecuteQuerry(Querry);
        }
    }
}