using AnnualLeave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.ViewModel
{
    public class EmployeeCalendarData
    {
        public int CalendarId { get; set; }
        public int ApprovalId { get; set; }
        public double MinutesUsed { get; set; }
        public bool CustomTimeSet { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

    public class EmployeeHolidayData
    {
        public IEnumerable<EmployeeCalendarData> EmployeeHoliday { get; set; }
        public IEnumerable<Calendar> CalendarData { get; set; }
    }
}