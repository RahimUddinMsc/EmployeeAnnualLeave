using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnnualLeave.Models;

namespace AnnualLeave.ViewModel
{
    public class CalendarAllocation
    {
        public IEnumerable<Calendar> CalendarDays { get; set; }

    }
}