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
        public int FileId { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime AcheivementDate { get; set; }
        [Required]
        public HttpPostedFileBase File { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }


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