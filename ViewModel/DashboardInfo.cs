using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.ViewModel
{
    public class DashboardInfo
    {
        //data that needs to be pulled for dashboard

        //user name
        // profile img
        // time used
        // time remaining
        // requests 
        // upcoming leave 


        public String Name { get; set; }
        public string profileImg { get; set; }
        public double MinutesUsed { get; set; }
        public double MinutesAvailable { get; set; }        
        public int NumRequest { get; set; }
        public List<String> upcomingLeave { get; set; }




    }
}