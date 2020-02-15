using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace LeaveApplication.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public int EmployeeId { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string Descipline { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        db database = new db();
        string Querry = "";
        public DataSet GetExperience()
        {
            Querry = "";
            return database.Read(Querry);
        }
        public void UpdateExp()
        {
            Querry = "";
        }
        public void Insert()
        {
            Querry = "";
            database.ExecuteQuerry(Querry);
        }
        public void DeleteExp()
        {
            Querry = "";
            database.ExecuteQuerry(Querry);
        }
    }
}