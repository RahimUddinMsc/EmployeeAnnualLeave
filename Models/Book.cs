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
        public Approval Approval { get; set; }
        public int ApprovalId { get; set; }
        public Boolean CustomTimeSet { get; set; }
        public double MinutesAllocated { get; set; }
        public DateTime DateTimeRequested { get; set; }
    }
}