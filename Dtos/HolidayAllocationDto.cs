using AnnualLeave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Dtos
{
    public class HolidayAllocationDto
    {

        public int EmpLinkId { get; set; }
        public int  BookId { get; set; }
        public int CalendarId { get; set; }
        public Approval Approval { get; set; }
        public Pending Pending { get; set; }        
        public int NumQueue { get; set; }
        public Boolean CustomTimeSet { get; set; }        
        public CustomTime CustomTime { get; set; }
    }
}