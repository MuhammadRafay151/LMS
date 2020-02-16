using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;

namespace LeaveApplication.Models
{
    public class Education
    {
        private DataSet ds;
        private db database = new db();

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

        public void AddEducation(Education edu, int EmployeeID)
        {
            string Querry = string.Format("insert into Education(EmployeeId,DegreeId,DegreeTittle,Field,Institute,Year) values({0},{1},'{2}','{3}','{4}','{5}')", EmployeeID, edu.DegreeID, edu.DegreeTittle, edu.Field, edu.Institute, DateTime.ParseExact(edu.Year, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString());
            database.ExecuteQuerry(Querry);
        }

        public void UpdateEducation(Education edu, int EmployeeID)
        {
            string Querry = string.Format("UPDATE Education SET DegreeId = {1}, DegreeTittle = '{2}',Field = '{3}',Institute ='{4}' ,Year = {5} WHERE EmployeeId={0}", EmployeeID, edu.DegreeID, edu.DegreeTittle, edu.Field, edu.Institute, DateTime.ParseExact(edu.Year, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString());
            database.ExecuteQuerry(Querry);
        }

        public void DeleteEducation(int eduid)
        {
            string Querry = string.Format("Delete from education where id={0}", eduid);
            database.ExecuteQuerry(Querry);
        }
    }
}