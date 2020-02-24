using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;

namespace LeaveApplication.Models
{
    public class Education
    {
        private DataSet ds;
        private db database = new db();

        public int EduID { get; set; }
        public int DegreeID { get; set; }
        public int DegreeType { get; set; }
        public string DegreeTittle { get; set; }
        public string Field { get; set; }
        public string Institute { get; set; }
        public string Year { get; set; }

        public DataSet GetDegrees()
        {
            string Querry = "select id,degree from Degree";

            ds = database.Read(Querry);

            return ds;
        }

        public DataSet GetEdu(int EmployeeID, int EduID)
        {
            string Querry = string.Format("select Education.ID,Degree.ID, DegreeTittle,Field,Institute,year(Year) as 'Year' from Education inner join Degree on Education.DegreeId=Degree.id where EmployeeId={0} and Education.ID={1} ", EmployeeID, EduID);
            ds = database.Read(Querry);
            return ds;
        }

        public DataSet GetEducation(int EmployeeID)
        {
            string Querry = string.Format("select Education.ID,Degree,DegreeTittle,Field,Institute,year(Year) from Education inner join Degree on Education.DegreeId=Degree.id where EmployeeId={0}", EmployeeID);
            ds = database.Read(Querry);
            return ds;
        }

        public void AddEducation(Education edu, int EmployeeID)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeID", Value = edu.DegreeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeTittle", Value = edu.DegreeTittle });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Field", Value = edu.Field });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Institute", Value = edu.Institute });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Year", Value = DateTime.ParseExact(edu.Year, "yyyy", CultureInfo.InvariantCulture).ToString() });

            string Querry = string.Format("insert into Education(EmployeeId,DegreeId,DegreeTittle,Field,Institute,Year) values(@EmployeeID,@DegreeID,@DegreeTittle,@Field,@Institute,@Year)");
            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void UpdateEducation(Education edu, int EmployeeID)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeID", Value = edu.DegreeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeTittle", Value = edu.DegreeTittle });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Field", Value = edu.Field });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Institute", Value = edu.Institute });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Year", Value = DateTime.ParseExact(edu.Year, "yyyy", CultureInfo.InvariantCulture).ToString() });

            string Querry = string.Format("UPDATE Education SET DegreeId = {1}, DegreeTittle = '{2}',Field = '{3}',Institute ='{4}' ,Year = '{5}' WHERE EmployeeId={0} and id={6}", EmployeeID, edu.DegreeID, edu.DegreeTittle, edu.Field, edu.Institute, DateTime.ParseExact(edu.Year, "yyyy", CultureInfo.InvariantCulture).ToString(), edu.EduID);
            database.ExecuteQuerry(Querry);
        }

        public void DeleteEducation(int EmployeeID, int eduid)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "eduid", Value = eduid });
            string Querry = string.Format("Delete from education where id=@eduid and EmployeeID=@EmployeeID");
            database.ExecuteQuerry(Querry);
        }
    }
}