
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class ProfilePicture
    {
       
        public byte[] Image;
        db database = new db();
        public void Update(int EmpId)
        {
            string querry = "update Picture set Picture=@img where EmployeeID=@emp";
            HelperClasses.SqlParm sq = new HelperClasses.SqlParm();
            sq.Add("img", Image);
            sq.Add("emp", EmpId);
            database.ExecuteQuerry(querry, sq.GetParmList());

        }
        public void insert()
        {

        }
        public void Delete()
        {

        }

    }
}