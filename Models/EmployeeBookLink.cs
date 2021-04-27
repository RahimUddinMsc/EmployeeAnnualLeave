using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Models
{
    public class EmployeeBookLink
    {

        public int EmployeeBookLinkId { get; set; }
        public ApplicationUser User { get; set; }
        public String UserId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }       

    }
}