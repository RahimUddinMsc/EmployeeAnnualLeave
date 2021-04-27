using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Models
{
    public class UserHolidayInfo
    {
        public int UserHolidayInfoID { get; set; }
        public ApplicationUser User { get; set; }
        public String UserId { get; set; }
        public double FullDay { get; set; }
    }
}