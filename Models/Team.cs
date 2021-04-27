using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnnualLeave.Models
{
    public class Team
    {
        [Required]
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        [Required]        
        public double ManagerID { get; set; }
        
        
    }
}