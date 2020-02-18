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
            string Querry = string.Format("insert into Education(EmployeeId,DegreeId,DegreeTittle,Field,Institute,Year) values({0},{1},'{2}','{3}','{4}','{5}')", EmployeeID, edu.DegreeID, edu.DegreeTittle, edu.Field, edu.Institute, DateTime.ParseExact(edu.Year, "yyyy", CultureInfo.InvariantCulture).ToString());
            database.ExecuteQuerry(Querry);
        }

        public void UpdateEducation(Education edu, int EmployeeID)
        {
            string Querry = string.Format("UPDATE Education SET DegreeId = {1}, DegreeTittle = '{2}',Field = '{3}',Institute ='{4}' ,Year = '{5}' WHERE EmployeeId={0} and id={6}", EmployeeID, edu.DegreeID, edu.DegreeTittle, edu.Field, edu.Institute, DateTime.ParseExact(edu.Year, "yyyy", CultureInfo.InvariantCulture).ToString(), edu.EduID);
            database.ExecuteQuerry(Querry);
        }

        public void DeleteEducation(int EmployeeID, int eduid)
        {
            string Querry = string.Format("Delete from education where id={0} and EmployeeID={1}", eduid, EmployeeID);
            database.ExecuteQuerry(Querry);
        }
    }
}