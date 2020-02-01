using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Models
{
    public class Book
    {
        public int BookId { get; set; }            
        public Calendar Calendar { get; set; }
        public int CalendarId { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public Approval Approval { get; set; }
        public int ApprovalId { get; set; }

       
    }
}