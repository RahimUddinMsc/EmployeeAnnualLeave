using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Models
{
    public class Pending
    {
        public int PendingId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string Reason { get; set; }
        public string Response { get; set; }
        public int approved { get; set; }


    }
}