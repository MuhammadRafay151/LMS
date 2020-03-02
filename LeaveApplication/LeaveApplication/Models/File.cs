using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class File
    {
        public string FileName { get; set; }
        public int FileID { get; set; }

        public byte[] Content { get; set; }
    }
}