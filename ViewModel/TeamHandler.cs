using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.ViewModel
{
    public class TeamHandler
    {
        public String Name { get; set; }        
        public double MinutesUsed { get; set; }
        public double MinutesAvailable { get; set; }
        public bool OnHoliday { get; set; }
        public string profileImg { get; set; }

        public List<TeamUserHolidayInfo> UserHolidayInfo { get; set; }
    }


    public class TeamUserHolidayInfo
    {

        public string Date { get; set; }
        public Boolean CustomTime { get; set; }        

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}