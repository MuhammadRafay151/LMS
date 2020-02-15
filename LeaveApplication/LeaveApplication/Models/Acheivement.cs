using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace LeaveApplication.Models
{
    public class Acheivement
    {
        public int AcheivementId { get; set; }
        [Required]
        public string title { get; set; }
        public string Description { get; set; }
        public DateTime AcheivementDate { get; set; }
        db database = new db();
        string Querry;
        public DataSet GetAcheivement()
        {
            Querry = "";
            return database.Read(Querry);
        }
        public void UpdateAcheivement()
        {
            Querry = "";
        }
        public void InsertAcheivement()
        {
            Querry = "";
            database.ExecuteQuerry(Querry);
        }
        public void DeleteAcheivement()
        {
            Querry = "";
            database.ExecuteQuerry(Querry);
        }
    }
}