using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LeaveApplication.Models
{
    public class Acheivement
    {
        public int AcheivementId { get; set; }
        [Required]
        public string title { get; set; }
        public string Description { get; set; }
        public DateTime AcheivementDate { get; set; }
    }
}