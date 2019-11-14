using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveApplication.Exceptional_Classes
{
    public class DuplicateException:Exception
    {//1 Duplicate USername
        //2 Duplicate EmpNo
        int ExceptionId = 0;
        public DuplicateException(int ExceptionId):base("Primary Key Is Avilable")
        {
            this.ExceptionId = ExceptionId;
        }
        public int ExceptionID
        {
            get { return ExceptionId; }
        }
    }
}