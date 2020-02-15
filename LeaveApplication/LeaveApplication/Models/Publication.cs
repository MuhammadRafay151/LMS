using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
namespace LeaveApplication.Models
{
    public class Publication
    {
        public string Querry { get; private set; }
        db database = new db();
        public int PublishedId { get; set; }
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Description { get; set; }

        public List<string> Author;
        public HttpPostedFileBase File { get; set; }
        public Byte[] FileBytes { get; set; }
        public DataSet GetPublication()
        {
            Querry = "";
            return database.Read(Querry);
        }
        public void UpdatePublication()
        {
            Querry = "";
        }
        public void InsertPublication()
        {
            Querry = "";
            database.ExecuteQuerry(Querry);
        }
        public void DeletePublication()
        {
            Querry = "";
            database.ExecuteQuerry(Querry);
        }
    }
}