using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Models
{
    public class CustomTime
    {

        public int CustomTimeId { get; set; }
        public Calendar Calendar { get; set; }
        public int CalendarId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

    }
}