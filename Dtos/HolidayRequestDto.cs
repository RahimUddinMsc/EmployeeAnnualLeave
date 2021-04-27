using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Dtos
{
    public class HolidayRequestDto
    {
        public int CalendarId { get; set; }  
        public double MinutesRequested { get; set; }
        public Boolean CustomTime { get; set; }
        public string Start { get; set; }
        public string End { get; set; }


    }

}