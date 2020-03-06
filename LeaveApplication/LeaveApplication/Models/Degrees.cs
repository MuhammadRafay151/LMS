using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Degrees
    {
        private DataSet ds;
        private db database = new db();
        public int DegreeID { get; set; }

        [Required]
        public string Degree { get; set; }

        public DataSet GetDegrees()
        {
            string Querry = string.Format("select * from degree");
            return database.Read(Querry);
        }

        public void AddDegree()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter() { ParameterName = "Degree", Value = Degree });

            string Querry = string.Format("insert into Degree(Degree) values(@Degree)");

            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void UpdateDegree()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter() { ParameterName = "Degree", Value = Degree });
            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeID", Value = DegreeID });

            string Querry = string.Format("update Degree set Degree=@Degree where id=@DegreeID");

            database.ExecuteQuerry(Querry, sqlParameters);
        }

        public void DeleteDegree()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter() { ParameterName = "DegreeID", Value = DegreeID });

            string Querry = string.Format("delete from Degree where id=@DegreeID");

            database.ExecuteQuerry(Querry, sqlParameters);
        }
    }
}