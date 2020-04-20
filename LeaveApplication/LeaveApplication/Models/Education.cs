using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace LeaveApplication.Models
{
    public class Education
    {
        private DataSet ds;
        private db database = new db();

        public int EmployeeID { get; set; }
        public int EduID { get; set; }
        public int DegreeID { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+")]
        public int DegreeType { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+")]
        public string DegreeTittle { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+")]
        public string Field { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+")]
        public string Institute { get; set; }

        [Required]
        public string Year { get; set; }

        public DataSet GetDegrees()
        {
            string Querry = "select id,degree from Degree";

            ds = database.Read(Querry);

            return ds;
        }

        public DataSet GetEdu()
        {
            string Querry = string.Format("select Education.ID,Degree.ID, DegreeTittle,Field,Institute,year(Year) as 'Year' from Education inner join Degree on Education.DegreeId=Degree.id where EmployeeId={0} and Education.ID={1} ", EmployeeID, EduID);
            ds = database.Read(Querry);
            return ds;
        }

        public DataSet GetEducation()
        {
            string Querry = string.Format("select Education.ID,Degree,DegreeTittle,Field,Institute,year(Year) from Education inner join Degree on Education.DegreeId=Degree.id where EmployeeId={0}", EmployeeID);
            ds = database.Read(Querry);
            return ds;
        }

        public void AddEducation()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeID", Value = DegreeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeTittle", Value = DegreeTittle });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Field", Value = Field });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Institute", Value = Institute });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Year", Value = DateTime.ParseExact(Year, "yyyy", CultureInfo.InvariantCulture).ToString() });

            string Querry = string.Format("insert into Education(EmployeeId,DegreeId,DegreeTittle,Field,Institute,Year) values(@EmployeeID,@DegreeID,@DegreeTittle,@Field,@Institute,@Year)");
            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void UpdateEducation()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "EduID", Value = EduID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeID", Value = DegreeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeTittle", Value = DegreeTittle });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Field", Value = Field });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Institute", Value = Institute });
            sqlParameters.Add(new SqlParameter() { ParameterName = "Year", Value = DateTime.ParseExact(Year, "yyyy", CultureInfo.InvariantCulture).ToString() });

            string Querry = string.Format("UPDATE Education SET DegreeId = @DegreeID, DegreeTittle = @DegreeTittle,Field = @Field,Institute =@Institute ,Year = @Year WHERE EmployeeId=@EmployeeID and id=@EduID");
            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void DeleteEducation()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter() { ParameterName = "EmployeeID", Value = EmployeeID });
            sqlParameters.Add(new SqlParameter() { ParameterName = "eduid", Value = EduID });
            string Querry = string.Format("Delete from education where id=@eduid and EmployeeID=@EmployeeID");
            database.ExecuteQuerry(Querry, sqlParameters);
        }
    }
}