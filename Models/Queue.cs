using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Models
{
    public class Queue
    {

        public int QueueId { get; set; }
        public Calendar Calendar { get; set; }
        public int CalendarId { get; set; }
        public ApplicationUser User { get; set; }
        public String UserId { get; set; }
        public int position { get; set; }

        


    }
}