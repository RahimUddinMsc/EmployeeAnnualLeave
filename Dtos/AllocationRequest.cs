using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Dtos
{
    public class AllocationRequest
    {
        public int CalendarId { get; set; }
        public double TimeSet { get; set; }

    }
}