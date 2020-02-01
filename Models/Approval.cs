using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AnnualLeave.Models
{
    public class Approval
    {
        public int ApprovalId { get; set; }
        [Required]
        public string Description { get; set; }


    }
}